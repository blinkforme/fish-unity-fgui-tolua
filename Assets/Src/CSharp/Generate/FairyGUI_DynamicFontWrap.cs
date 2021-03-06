﻿//this source code was auto-generated by tolua#, do not modify it
using System;
using LuaInterface;

public class FairyGUI_DynamicFontWrap
{
	public static void Register(LuaState L)
	{
		L.BeginClass(typeof(FairyGUI.DynamicFont), typeof(FairyGUI.BaseFont));
		L.RegFunction("Dispose", Dispose);
		L.RegFunction("SetFormat", SetFormat);
		L.RegFunction("PrepareCharacters", PrepareCharacters);
		L.RegFunction("GetGlyph", GetGlyph);
		L.RegFunction("DrawGlyph", DrawGlyph);
		L.RegFunction("DrawLine", DrawLine);
		L.RegFunction("HasCharacter", HasCharacter);
		L.RegFunction("GetLineHeight", GetLineHeight);
		L.RegFunction("New", _CreateFairyGUI_DynamicFont);
		L.RegFunction("__tostring", ToLua.op_ToString);
		L.RegVar("nativeFont", get_nativeFont, set_nativeFont);
		L.EndClass();
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateFairyGUI_DynamicFont(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 0)
			{
				FairyGUI.DynamicFont obj = new FairyGUI.DynamicFont();
				ToLua.PushObject(L, obj);
				return 1;
			}
			else if (count == 2)
			{
				string arg0 = ToLua.CheckString(L, 1);
				UnityEngine.Font arg1 = (UnityEngine.Font)ToLua.CheckObject(L, 2, typeof(UnityEngine.Font));
				FairyGUI.DynamicFont obj = new FairyGUI.DynamicFont(arg0, arg1);
				ToLua.PushObject(L, obj);
				return 1;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to ctor method: FairyGUI.DynamicFont.New");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Dispose(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			FairyGUI.DynamicFont obj = (FairyGUI.DynamicFont)ToLua.CheckObject<FairyGUI.DynamicFont>(L, 1);
			obj.Dispose();
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetFormat(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 3);
			FairyGUI.DynamicFont obj = (FairyGUI.DynamicFont)ToLua.CheckObject<FairyGUI.DynamicFont>(L, 1);
			FairyGUI.TextFormat arg0 = (FairyGUI.TextFormat)ToLua.CheckObject<FairyGUI.TextFormat>(L, 2);
			float arg1 = (float)LuaDLL.luaL_checknumber(L, 3);
			obj.SetFormat(arg0, arg1);
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int PrepareCharacters(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			FairyGUI.DynamicFont obj = (FairyGUI.DynamicFont)ToLua.CheckObject<FairyGUI.DynamicFont>(L, 1);
			string arg0 = ToLua.CheckString(L, 2);
			obj.PrepareCharacters(arg0);
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetGlyph(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 5);
			FairyGUI.DynamicFont obj = (FairyGUI.DynamicFont)ToLua.CheckObject<FairyGUI.DynamicFont>(L, 1);
			char arg0 = (char)LuaDLL.luaL_checknumber(L, 2);
			float arg1;
			float arg2;
			float arg3;
			bool o = obj.GetGlyph(arg0, out arg1, out arg2, out arg3);
			LuaDLL.lua_pushboolean(L, o);
			LuaDLL.lua_pushnumber(L, arg1);
			LuaDLL.lua_pushnumber(L, arg2);
			LuaDLL.lua_pushnumber(L, arg3);
			return 4;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int DrawGlyph(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 7);
			FairyGUI.DynamicFont obj = (FairyGUI.DynamicFont)ToLua.CheckObject<FairyGUI.DynamicFont>(L, 1);
			float arg0 = (float)LuaDLL.luaL_checknumber(L, 2);
			float arg1 = (float)LuaDLL.luaL_checknumber(L, 3);
			System.Collections.Generic.List<UnityEngine.Vector3> arg2 = (System.Collections.Generic.List<UnityEngine.Vector3>)ToLua.CheckObject(L, 4, typeof(System.Collections.Generic.List<UnityEngine.Vector3>));
			System.Collections.Generic.List<UnityEngine.Vector2> arg3 = (System.Collections.Generic.List<UnityEngine.Vector2>)ToLua.CheckObject(L, 5, typeof(System.Collections.Generic.List<UnityEngine.Vector2>));
			System.Collections.Generic.List<UnityEngine.Vector2> arg4 = (System.Collections.Generic.List<UnityEngine.Vector2>)ToLua.CheckObject(L, 6, typeof(System.Collections.Generic.List<UnityEngine.Vector2>));
			System.Collections.Generic.List<UnityEngine.Color32> arg5 = (System.Collections.Generic.List<UnityEngine.Color32>)ToLua.CheckObject(L, 7, typeof(System.Collections.Generic.List<UnityEngine.Color32>));
			int o = obj.DrawGlyph(arg0, arg1, arg2, arg3, arg4, arg5);
			LuaDLL.lua_pushinteger(L, o);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int DrawLine(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 10);
			FairyGUI.DynamicFont obj = (FairyGUI.DynamicFont)ToLua.CheckObject<FairyGUI.DynamicFont>(L, 1);
			float arg0 = (float)LuaDLL.luaL_checknumber(L, 2);
			float arg1 = (float)LuaDLL.luaL_checknumber(L, 3);
			float arg2 = (float)LuaDLL.luaL_checknumber(L, 4);
			int arg3 = (int)LuaDLL.luaL_checknumber(L, 5);
			int arg4 = (int)LuaDLL.luaL_checknumber(L, 6);
			System.Collections.Generic.List<UnityEngine.Vector3> arg5 = (System.Collections.Generic.List<UnityEngine.Vector3>)ToLua.CheckObject(L, 7, typeof(System.Collections.Generic.List<UnityEngine.Vector3>));
			System.Collections.Generic.List<UnityEngine.Vector2> arg6 = (System.Collections.Generic.List<UnityEngine.Vector2>)ToLua.CheckObject(L, 8, typeof(System.Collections.Generic.List<UnityEngine.Vector2>));
			System.Collections.Generic.List<UnityEngine.Vector2> arg7 = (System.Collections.Generic.List<UnityEngine.Vector2>)ToLua.CheckObject(L, 9, typeof(System.Collections.Generic.List<UnityEngine.Vector2>));
			System.Collections.Generic.List<UnityEngine.Color32> arg8 = (System.Collections.Generic.List<UnityEngine.Color32>)ToLua.CheckObject(L, 10, typeof(System.Collections.Generic.List<UnityEngine.Color32>));
			int o = obj.DrawLine(arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
			LuaDLL.lua_pushinteger(L, o);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int HasCharacter(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			FairyGUI.DynamicFont obj = (FairyGUI.DynamicFont)ToLua.CheckObject<FairyGUI.DynamicFont>(L, 1);
			char arg0 = (char)LuaDLL.luaL_checknumber(L, 2);
			bool o = obj.HasCharacter(arg0);
			LuaDLL.lua_pushboolean(L, o);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetLineHeight(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			FairyGUI.DynamicFont obj = (FairyGUI.DynamicFont)ToLua.CheckObject<FairyGUI.DynamicFont>(L, 1);
			int arg0 = (int)LuaDLL.luaL_checknumber(L, 2);
			int o = obj.GetLineHeight(arg0);
			LuaDLL.lua_pushinteger(L, o);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_nativeFont(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			FairyGUI.DynamicFont obj = (FairyGUI.DynamicFont)o;
			UnityEngine.Font ret = obj.nativeFont;
			ToLua.PushSealed(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index nativeFont on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_nativeFont(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			FairyGUI.DynamicFont obj = (FairyGUI.DynamicFont)o;
			UnityEngine.Font arg0 = (UnityEngine.Font)ToLua.CheckObject(L, 2, typeof(UnityEngine.Font));
			obj.nativeFont = arg0;
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index nativeFont on a nil value");
		}
	}
}

