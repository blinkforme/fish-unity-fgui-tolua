﻿---@class Tween : ABSSequentiable
---@field public timeScale Single
---@field public isBackwards bool
---@field public id Object
---@field public stringId string
---@field public intId Int32
---@field public target Object
---@field public onPlay TweenCallback
---@field public onPause TweenCallback
---@field public onRewind TweenCallback
---@field public onUpdate TweenCallback
---@field public onStepComplete TweenCallback
---@field public onComplete TweenCallback
---@field public onKill TweenCallback
---@field public onWaypointChange TweenCallback`1
---@field public easeOvershootOrAmplitude Single
---@field public easePeriod Single
---@field public debugTargetId string
---@field public isRelative bool
---@field public active bool
---@field public fullPosition Single
---@field public hasLoops bool
---@field public playedOnce bool
---@field public position Single
local Tween={ }
DG.Tweening.Tween = Tween