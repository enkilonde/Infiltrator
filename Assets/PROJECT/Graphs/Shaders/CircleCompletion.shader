// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// Shader created with Shader Forge v1.27 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.27;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,lico:1,lgpr:1,limd:1,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:1,dpts:2,wrdp:True,dith:0,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:False,qofs:0,qpre:2,rntp:3,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False;n:type:ShaderForge.SFN_Final,id:4013,x:33334,y:33264,varname:node_4013,prsc:2|diff-1371-OUT,clip-1841-OUT;n:type:ShaderForge.SFN_Tex2d,id:2847,x:32879,y:33014,ptovrint:False,ptlb:node_2847,ptin:_node_2847,varname:node_2847,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:b66bceaf0cc0ace4e9bdc92f14bba709,ntxv:0,isnm:False;n:type:ShaderForge.SFN_TexCoord,id:4280,x:31601,y:33110,varname:node_4280,prsc:2,uv:0;n:type:ShaderForge.SFN_RemapRange,id:9144,x:31822,y:33110,varname:node_9144,prsc:2,frmn:0,frmx:1,tomn:-1,tomx:1|IN-4280-UVOUT;n:type:ShaderForge.SFN_ComponentMask,id:8012,x:31990,y:33110,varname:node_8012,prsc:2,cc1:0,cc2:1,cc3:-1,cc4:-1|IN-9144-OUT;n:type:ShaderForge.SFN_ArcTan2,id:2606,x:32181,y:33110,varname:node_2606,prsc:2,attp:1|A-8012-G,B-8012-R;n:type:ShaderForge.SFN_Subtract,id:1476,x:32384,y:33110,varname:node_1476,prsc:2|A-2606-OUT,B-699-OUT;n:type:ShaderForge.SFN_Slider,id:8891,x:31769,y:33370,ptovrint:False,ptlb:RemplissageCircle,ptin:_RemplissageCircle,varname:node_8891,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0,max:1;n:type:ShaderForge.SFN_RemapRange,id:699,x:32181,y:33279,varname:node_699,prsc:2,frmn:0,frmx:1,tomn:-1,tomx:1|IN-8891-OUT;n:type:ShaderForge.SFN_Ceil,id:1606,x:32550,y:33110,varname:node_1606,prsc:2|IN-1476-OUT;n:type:ShaderForge.SFN_Distance,id:4979,x:32350,y:33486,varname:node_4979,prsc:2|A-5268-UVOUT,B-9717-OUT;n:type:ShaderForge.SFN_Vector3,id:9717,x:32173,y:33588,varname:node_9717,prsc:2,v1:0.5,v2:0.5,v3:0;n:type:ShaderForge.SFN_TexCoord,id:5268,x:32173,y:33450,varname:node_5268,prsc:2,uv:0;n:type:ShaderForge.SFN_Step,id:3773,x:32515,y:33486,varname:node_3773,prsc:2|A-4979-OUT,B-1348-OUT;n:type:ShaderForge.SFN_Slider,id:1348,x:32113,y:33717,ptovrint:False,ptlb:TaillCircle,ptin:_TaillCircle,varname:node_1348,prsc:2,glob:False,taghide:True,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.4753235,max:1;n:type:ShaderForge.SFN_Multiply,id:2068,x:32790,y:33286,varname:node_2068,prsc:2|A-1606-OUT,B-3773-OUT;n:type:ShaderForge.SFN_Multiply,id:1371,x:33122,y:33264,varname:node_1371,prsc:2|A-2847-RGB,B-1841-OUT;n:type:ShaderForge.SFN_Clamp01,id:1841,x:32956,y:33286,varname:node_1841,prsc:2|IN-2068-OUT;proporder:2847-8891-1348;pass:END;sub:END;*/

Shader "Shader Forge/CircleCompletion" {
    Properties {
        _node_2847 ("node_2847", 2D) = "white" {}
        _RemplissageCircle ("RemplissageCircle", Range(0, 1)) = 0
        [HideInInspector]_TaillCircle ("TaillCircle", Range(0, 1)) = 0.4753235
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "Queue"="AlphaTest"
            "RenderType"="TransparentCutout"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma multi_compile_fog
            #pragma exclude_renderers gles3 metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            uniform float4 _LightColor0;
            uniform sampler2D _node_2847; uniform float4 _node_2847_ST;
            uniform float _RemplissageCircle;
            uniform float _TaillCircle;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                LIGHTING_COORDS(3,4)
                UNITY_FOG_COORDS(5)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3 normalDirection = i.normalDir;
                float2 node_8012 = (i.uv0*2.0+-1.0).rg;
                float node_1841 = saturate((ceil(((atan2(node_8012.g,node_8012.r)/3.14159265359)-(_RemplissageCircle*2.0+-1.0)))*step(distance(i.uv0,float3(0.5,0.5,0)),_TaillCircle)));
                clip(node_1841 - 0.5);
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                float3 lightColor = _LightColor0.rgb;
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.xyz;
/////// Diffuse:
                float NdotL = max(0.0,dot( normalDirection, lightDirection ));
                float3 directDiffuse = max( 0.0, NdotL) * attenColor;
                float3 indirectDiffuse = float3(0,0,0);
                indirectDiffuse += UNITY_LIGHTMODEL_AMBIENT.rgb; // Ambient Light
                float4 _node_2847_var = tex2D(_node_2847,TRANSFORM_TEX(i.uv0, _node_2847));
                float3 diffuseColor = (_node_2847_var.rgb*node_1841);
                float3 diffuse = (directDiffuse + indirectDiffuse) * diffuseColor;
/// Final Color:
                float3 finalColor = diffuse;
                fixed4 finalRGBA = fixed4(finalColor,1);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
        Pass {
            Name "FORWARD_DELTA"
            Tags {
                "LightMode"="ForwardAdd"
            }
            Blend One One
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDADD
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #pragma multi_compile_fwdadd_fullshadows
            #pragma multi_compile_fog
            #pragma exclude_renderers gles3 metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            uniform float4 _LightColor0;
            uniform sampler2D _node_2847; uniform float4 _node_2847_ST;
            uniform float _RemplissageCircle;
            uniform float _TaillCircle;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                LIGHTING_COORDS(3,4)
                UNITY_FOG_COORDS(5)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3 normalDirection = i.normalDir;
                float2 node_8012 = (i.uv0*2.0+-1.0).rg;
                float node_1841 = saturate((ceil(((atan2(node_8012.g,node_8012.r)/3.14159265359)-(_RemplissageCircle*2.0+-1.0)))*step(distance(i.uv0,float3(0.5,0.5,0)),_TaillCircle)));
                clip(node_1841 - 0.5);
                float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
                float3 lightColor = _LightColor0.rgb;
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.xyz;
/////// Diffuse:
                float NdotL = max(0.0,dot( normalDirection, lightDirection ));
                float3 directDiffuse = max( 0.0, NdotL) * attenColor;
                float4 _node_2847_var = tex2D(_node_2847,TRANSFORM_TEX(i.uv0, _node_2847));
                float3 diffuseColor = (_node_2847_var.rgb*node_1841);
                float3 diffuse = directDiffuse * diffuseColor;
/// Final Color:
                float3 finalColor = diffuse;
                fixed4 finalRGBA = fixed4(finalColor * 1,0);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
        Pass {
            Name "ShadowCaster"
            Tags {
                "LightMode"="ShadowCaster"
            }
            Offset 1, 1
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_SHADOWCASTER
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcaster
            #pragma multi_compile_fog
            #pragma exclude_renderers gles3 metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            uniform float _RemplissageCircle;
            uniform float _TaillCircle;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                V2F_SHADOW_CASTER;
                float2 uv0 : TEXCOORD1;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex );
                TRANSFER_SHADOW_CASTER(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                float2 node_8012 = (i.uv0*2.0+-1.0).rg;
                float node_1841 = saturate((ceil(((atan2(node_8012.g,node_8012.r)/3.14159265359)-(_RemplissageCircle*2.0+-1.0)))*step(distance(i.uv0,float3(0.5,0.5,0)),_TaillCircle)));
                clip(node_1841 - 0.5);
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
