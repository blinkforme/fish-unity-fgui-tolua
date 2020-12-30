﻿---@class RenderSettings : Object
---@field public ambientSkyboxAmount Single
---@field public fog bool
---@field public fogStartDistance Single
---@field public fogEndDistance Single
---@field public fogMode number
---@field public fogColor Color
---@field public fogDensity Single
---@field public ambientMode number
---@field public ambientSkyColor Color
---@field public ambientEquatorColor Color
---@field public ambientGroundColor Color
---@field public ambientIntensity Single
---@field public ambientLight Color
---@field public subtractiveShadowColor Color
---@field public skybox Material
---@field public sun Light
---@field public ambientProbe SphericalHarmonicsL2
---@field public customReflection Cubemap
---@field public reflectionIntensity Single
---@field public reflectionBounces Int32
---@field public defaultReflectionMode number
---@field public defaultReflectionResolution Int32
---@field public haloStrength Single
---@field public flareStrength Single
---@field public flareFadeSpeed Single
local RenderSettings={ }
UnityEngine.RenderSettings = RenderSettings