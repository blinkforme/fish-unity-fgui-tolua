//
// Requests.cs
//
// Author:
//       fjy <jiyuan.feng@live.com>
//
// Copyright (c) 2020 fjy
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation bundles (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using Debug = UnityEngine.Debug;
using Object = UnityEngine.Object;

namespace libx
{
    public enum LoadState
    {
        Init,
        LoadAssetBundle,
        LoadAsset,
        Loaded,
        Unload,
    }

    public class AssetRequest : Reference, IEnumerator
    {
        public Type assetType;
        public string url;
        public string extensionName;

        public LoadState loadState { get; protected set; }

        public AssetRequest()
        {
            asset = null;
            loadState = LoadState.Init;
        }

        public virtual bool isDone
        {
            get { return true; }
        }

        public virtual float progress
        {
            get { return 1; }
        }

        public virtual string error { get; protected set; }

        public string text { get; protected set; }

        public byte[] bytes { get; protected set; }

        public Object asset { get; internal set; }

        internal virtual void Load()
        {
            if (!File.Exists(url))
            {
                return;
            }

            if (Assets.development && Assets.assetLoader != null)
                asset = Assets.assetLoader(url, assetType);
            if (asset == null)
            {
                error = "error! file not exist:" + url;
            }
        }

        internal virtual void Unload()
        {
            if (asset == null)
                return;

            if (!Assets.development)
            {
                if (!(asset is GameObject))
                    Resources.UnloadAsset(asset);
            }

            asset = null;
        }

        internal bool Update()
        {
            if (!isDone)
                return true;
            if (completed == null)
                return false;
            try
            {
                completed.Invoke(this);
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }

            completed = null;
            return false;
        }

        public Action<AssetRequest> completed;

        #region IEnumerator implementation

        public bool MoveNext()
        {
            return !isDone;
        }

        public void Reset()
        {
        }

        public object Current
        {
            get { return null; }
        }

        #endregion
    }

    public class BundleAssetRequest : AssetRequest
    {
        protected readonly string assetBundleName;
        protected BundleRequest bundle;

        public BundleAssetRequest(string bundle)
        {
            assetBundleName = bundle;
        }

        internal override void Load()
        {
            bundle = Assets.LoadBundle(assetBundleName);
            var assetName = Path.GetFileName(url);
            asset = bundle.assetBundle.LoadAsset(assetName, assetType);
        }

        internal override void Unload()
        {
            if (bundle != null)
            {
                bundle.Release();
                bundle = null;
            }

            asset = null;
        }
    }

    public class BundleAssetAsyncRequest : BundleAssetRequest
    {
        private AssetBundleRequest _request;

        public BundleAssetAsyncRequest(string bundle)
            : base(bundle)
        {
        }

        public override bool isDone
        {
            get
            {
                if (error != null || bundle.error != null)
                    return true;

                for (int i = 0, max = bundle.children.Count; i < max; i++)
                {
                    var item = bundle.children[i];
                    if (item.error != null)
                        return true;
                }

                switch (loadState)
                {
                    case LoadState.Init:
                        return false;
                    case LoadState.Loaded:
                        return true;
                    case LoadState.LoadAssetBundle:
                    {
                        if (!bundle.isDone)
                            return false;

                        for (int i = 0, max = bundle.children.Count; i < max; i++)
                        {
                            var item = bundle.children[i];
                            if (!item.isDone)
                                return false;
                        }

                        if (bundle.assetBundle == null)
                        {
                            error = "assetBundle == null";
                            return true;
                        }

                        var assetName = Path.GetFileName(url);
                        _request = bundle.assetBundle.LoadAssetAsync(assetName, assetType);
                        loadState = LoadState.LoadAsset;
                        break;
                    }
                    case LoadState.Unload:
                        break;
                    case LoadState.LoadAsset:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                if (loadState != LoadState.LoadAsset)
                    return false;
                if (!_request.isDone)
                    return false;
                asset = _request.asset;
                loadState = LoadState.Loaded;
                return true;
            }
        }

        public override float progress
        {
            get
            {
                var bundleProgress = bundle.progress;
                if (bundle.children.Count <= 0)
                    return bundleProgress * 0.3f + (_request != null ? _request.progress * 0.7f : 0);
                for (int i = 0, max = bundle.children.Count; i < max; i++)
                {
                    var item = bundle.children[i];
                    bundleProgress += item.progress;
                }

                return bundleProgress / (bundle.children.Count + 1) * 0.3f +
                       (_request != null ? _request.progress * 0.7f : 0);
            }
        }

        internal override void Load()
        {
            bundle = Assets.LoadBundleAsync(assetBundleName);
            loadState = LoadState.LoadAssetBundle;
        }

        internal override void Unload()
        {
            _request = null;
            loadState = LoadState.Unload;
            base.Unload();
        }
    }

    public class SceneAssetRequest : AssetRequest
    {
        public readonly LoadSceneMode loadSceneMode;
        protected readonly string sceneName;
        public string assetBundleName { get; set; }
        protected BundleRequest bundle;
        public List<SceneAssetRequest> additives { get; set; }
        
        public SceneAssetRequest(string path, bool addictive)
        {
            url = path;
            additives = new List<SceneAssetRequest>(); 
            
            sceneName = Path.GetFileNameWithoutExtension(url);
            loadSceneMode = addictive ? LoadSceneMode.Additive : LoadSceneMode.Single;
        }

        public override float progress
        {
            get { return 1; }
        }

        internal override void Load()
        {
            if (!string.IsNullOrEmpty(assetBundleName))
            {
                bundle = Assets.LoadBundle(assetBundleName);
                if (bundle != null)
                    SceneManager.LoadScene(sceneName, loadSceneMode);
            }
            else
            {
                try
                {
                    SceneManager.LoadScene(sceneName, loadSceneMode);
                    loadState = LoadState.LoadAsset;
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                    error = e.ToString();
                    loadState = LoadState.Loaded;
                }
            }
        }

        internal override void Unload()
        {
            if (bundle != null)
                bundle.Release();

            if (additives.Count > 0)
            {
                for (var i = 0; i < additives.Count; i++)
                {
                    var additive = additives[i];
                    if (!additive.IsUnused())
                    {
                        additive.Release();
                    }
                } 
                additives.Clear();
            } 

            if (loadSceneMode == LoadSceneMode.Additive)
            {
                if (SceneManager.GetSceneByName(sceneName).isLoaded)
                    SceneManager.UnloadSceneAsync(sceneName);
            }

            bundle = null;
        }
    }

    public class SceneAssetAsyncRequest : SceneAssetRequest
    {
        private AsyncOperation _request;

        public SceneAssetAsyncRequest(string path, bool addictive)
            : base(path, addictive)
        {
        }

        public override float progress
        {
            get
            {
                if (bundle == null)
                    return _request == null ? 0 : _request.progress;

                var bundleProgress = bundle.progress;
                if (bundle.children.Count <= 0)
                    return bundleProgress * 0.3f + (_request != null ? _request.progress * 0.7f : 0);
                for (int i = 0, max = bundle.children.Count; i < max; i++)
                {
                    var item = bundle.children[i];
                    bundleProgress += item.progress;
                }

                return bundleProgress / (bundle.children.Count + 1) * 0.3f +
                       (_request != null ? _request.progress * 0.7f : 0);
            }
        }

        public override bool isDone
        {
            get
            {
                switch (loadState)
                {
                    case LoadState.Loaded:
                        return true;
                    case LoadState.LoadAssetBundle:
                    {
                        if (bundle == null || bundle.error != null)
                            return true;

                        for (int i = 0, max = bundle.children.Count; i < max; i++)
                        {
                            var item = bundle.children[i];
                            if (item.error != null)
                                return true;
                        }

                        if (!bundle.isDone)
                            return false;

                        for (int i = 0, max = bundle.children.Count; i < max; i++)
                        {
                            var item = bundle.children[i];
                            if (!item.isDone)
                                return false;
                        }

                        LoadSceneAsync();

                        break;
                    }
                    case LoadState.Unload:
                        break;
                    case LoadState.LoadAsset:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                if (loadState != LoadState.LoadAsset)
                    return false;
                if (_request != null && !_request.isDone)
                    return false;
                loadState = LoadState.Loaded;
                return true;
            }
        }

        private void LoadSceneAsync()
        {
            try
            {
                _request = SceneManager.LoadSceneAsync(sceneName, loadSceneMode);
                loadState = LoadState.LoadAsset;
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                error = e.ToString();
                loadState = LoadState.Loaded;
            }
        }

        internal override void Load()
        {
            if (!string.IsNullOrEmpty(assetBundleName))
            {
                bundle = Assets.LoadBundleAsync(assetBundleName);
                loadState = LoadState.LoadAssetBundle;
            }
            else
            {
                LoadSceneAsync();
            }
        }

        internal override void Unload()
        {
            base.Unload();
            _request = null;
        }
    }

    public class WebAssetRequest : AssetRequest
    {
        private UnityWebRequest _www;

        public override bool isDone
        {
            get
            {
                if (loadState == LoadState.Init)
                    return false;
                if (loadState == LoadState.Loaded)
                    return true;
                if (loadState == LoadState.LoadAsset)
                {
                    if (_www == null || !string.IsNullOrEmpty(_www.error))
                        return true;

                    if (_www.isDone)
                    {
                        GetAsset();
                        loadState = LoadState.Loaded;
                        return true;
                    }

                    return false;
                }

                return true;
            }
        }

        private void GetAsset()
        {
            if (assetType == typeof(Texture2D))
            {
                asset = DownloadHandlerTexture.GetContent(_www);
            }
            else if (assetType == typeof(AudioClip))
            {
                asset = DownloadHandlerAudioClip.GetContent(_www);
            }
            else if (assetType == typeof(TextAsset))
            {
                text = _www.downloadHandler.text;
            }
            else
            {
                bytes = _www.downloadHandler.data;
            }
        }

        public override string error
        {
            get { return _www.error; }
        }

        public override float progress
        {
            get { return _www.downloadProgress; }
        }

        internal override void Load()
        {
            if (assetType == typeof(AudioClip))
            {
                _www = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.WAV);
            }
            else if (assetType == typeof(Texture2D))
            {
                _www = UnityWebRequestTexture.GetTexture(url);
            }
            else
            {
                _www = UnityWebRequest.Get(url);
                _www.downloadHandler = new DownloadHandlerBuffer();
            }

            _www.SendWebRequest();
            loadState = LoadState.LoadAsset;
        }

        internal override void Unload()
        {
            if (asset != null)
            {
                Object.Destroy(asset);
                asset = null;
            }

            if (_www != null)
                _www.Dispose();

            bytes = null;
            text = null;
        }
    }

    public class BundleRequest : AssetRequest
    {
        public string parent { get; set; }

        public readonly List<BundleRequest> children = new List<BundleRequest>();

        public AssetBundle assetBundle
        {
            get { return asset as AssetBundle; }
            internal set { asset = value; }
        }

        internal override void Load()
        {
            asset = AssetBundle.LoadFromFile(url);
            if (assetBundle == null)
                error = url + " LoadFromFile failed.";
        }

        internal override void Unload()
        {
            children.Clear();
            if (assetBundle == null)
                return;
            assetBundle.Unload(true);
            assetBundle = null;
        }

        void ReleaseByBundle(BundleRequest bundle)
        {
            if (bundle.name.Equals(name))
            {
                base.Release();
                return;
            } 
            if (_parents.Contains(bundle))
            {
                return;
            }
            _parents.Add(bundle); 
            foreach (var child in bundle.children)
            {
                ReleaseByBundle(child);
            }
        }

        private readonly List<BundleRequest> _parents = new List<BundleRequest>();

        public override void Release()
        {
            base.Release();
            foreach (var child in children)
            {
                ReleaseByBundle(child);
            }
            _parents.Clear();
        }
    }

    public class BundleAsyncRequest : BundleRequest
    {
        private AssetBundleCreateRequest _request;

        public override bool isDone
        {
            get
            {
                if (loadState == LoadState.Init)
                    return false;

                if (loadState == LoadState.Loaded)
                    return true;

                if (loadState == LoadState.LoadAssetBundle && _request.isDone)
                {
                    asset = _request.assetBundle;
                    if (_request.assetBundle == null)
                    {
                        error = string.Format("unable to load assetBundle:{0}", url);
                    }

                    loadState = LoadState.Loaded;
                }

                return _request == null || _request.isDone;
            }
        }

        public override float progress
        {
            get { return _request != null ? _request.progress : 0f; }
        }

        internal override void Load()
        {
            _request = AssetBundle.LoadFromFileAsync(url);
            if (_request == null)
            {
                error = url + " LoadFromFile failed.";
                return;
            }

            loadState = LoadState.LoadAssetBundle;
        }

        internal override void Unload()
        {
            _request = null;
            loadState = LoadState.Unload;
            base.Unload();
        }
    }

    public class WebBundleRequest : BundleRequest
    {
        private UnityWebRequest _request;

        public override string error
        {
            get { return _request != null ? _request.error : null; }
        }

        public override bool isDone
        {
            get
            {
                if (loadState == LoadState.Init)
                    return false;

                if (_request == null || loadState == LoadState.Loaded)
                    return true;

                if (_request.isDone)
                {
                    assetBundle = DownloadHandlerAssetBundle.GetContent(_request);
                    loadState = LoadState.Loaded;
                }

                return _request.isDone;
            }
        }

        public override float progress
        {
            get { return _request != null ? _request.downloadProgress : 0f; }
        }

        internal override void Load()
        {
            _request = UnityWebRequestAssetBundle.GetAssetBundle(url);
            _request.SendWebRequest();
            loadState = LoadState.LoadAssetBundle;
        }

        internal override void Unload()
        {
            if (_request != null)
            {
                _request.Dispose();
                _request = null;
            }

            loadState = LoadState.Unload;
            base.Unload();
        }
    }
}