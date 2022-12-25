"Material_Name" = StaticMaterialDef {
        name: string = "Material_Name"
        type: u32 = 0
        defaultTechnique: string = "normal"
        samplerValues: list2[embed] = {
            StaticMaterialShaderSamplerDef {
                samplerName: string = "DiffuseTexture"
                textureName: string = "Diffuse_Name"
                addressW: u32 = 1
            }
            StaticMaterialShaderSamplerDef {
                samplerName: string = "Noise_Texture"
                textureName: string = "ASSETS/Shared/Materials/SRX_FireTest_B.dds"
            }
            StaticMaterialShaderSamplerDef {
                samplerName: string = "EmissionTex"
                textureName: string = "ASSETS/Shared/Materials/Base_Caustic_Noise.Season2022_SRT_Preseason.dds"
            }
            StaticMaterialShaderSamplerDef {
                samplerName: string = "EmissionMaskTex"
                textureName: string = "Emissive_Name"
            }
        }
        paramValues: list2[embed] = {
            StaticMaterialShaderParamDef {
                name: string = "Height_Start_End"
                value: vec4 = { -375, 750, 0, 0 }
            }
            StaticMaterialShaderParamDef {
                name: string = "Transition_Speed_Factor"
                value: vec4 = { 4, 0, 0, 0 }
            }
            StaticMaterialShaderParamDef {
                name: string = "LowQualityColor"
                value: vec4 = { 0.100000001, 0.5, 0.100000001, 0.5 }
            }
            StaticMaterialShaderParamDef {
                name: string = "WaveTintColor"
                value: vec4 = { 0.100000001, 0.5, 0.100000001, 0.5 }
            }
            StaticMaterialShaderParamDef {
                name: string = "RotationSpeed"
                value: vec4 = { 0.150000006, 0, 0, 0 }
            }
            StaticMaterialShaderParamDef {
                name: string = "EmiRotationSpeed"
                value: vec4 = { 0.0175000001, 0, 0, 0 }
            }
            StaticMaterialShaderParamDef {
                name: string = "EmissionColor"
                value: vec4 = { 0.119999997, 0.224999994, 0.0799999982, 0 }
            }
            StaticMaterialShaderParamDef {
                name: string = "EmissionTexUV"
                value: vec4 = { 2, 2, 0, 0 }
            }
        }
        shaderMacros: map[string,string] = {
            "NO_BAKED_LIGHTING" = "1"
            "DISABLE_DEPTH_FOG" = "1"
            "PREMULTIPLIED_ALPHA" = "1"
        }
        techniques: list[embed] = {
            StaticMaterialTechniqueDef {
                name: string = "normal"
                passes: list[embed] = {
                    StaticMaterialPassDef {
                        shader: link = "Shaders/Environment/SRX_Blend_Hextech_Island"
                        cullEnable: bool = false
                        blendEnable: bool = true
                        dstColorBlendFactor: u32 = 7
                        dstAlphaBlendFactor: u32 = 7
                    }
                }
            }
        }
        childTechniques: list[embed] = {
            StaticMaterialChildTechniqueDef {
                name: string = "env_transition"
                parentName: string = "normal"
                shaderMacros: map[string,string] = {
                    "ENV_TRANSITION" = "1"
                }
            }
        }
    }