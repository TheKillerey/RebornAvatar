"Material_Name" = StaticMaterialDef {
        name: string = "Material_Name"
        type: u32 = 0
        defaultTechnique: string = "normal"
        samplerValues: list2[embed] = {
            StaticMaterialShaderSamplerDef {
                samplerName: string = "DiffuseTexture"
                textureName: string = "Texture_Name"
                addressW: u32 = 1
            }
        }
        switches: list2[embed] = {
            StaticMaterialSwitchDef {
                name: string = "MASK_FX_IN_MAP_CENTER"
                on: bool = false
            }
        }
        techniques: list[embed] = {
            StaticMaterialTechniqueDef {
                name: string = "normal"
                passes: list[embed] = {
                    StaticMaterialPassDef {
                        shader: link = "Shaders/Environment/SRX_Blend_Master"
                        cullEnable: bool = false
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