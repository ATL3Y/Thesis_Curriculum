// Shader created with Shader Forge v1.38 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.38;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:3,bdst:7,dpts:2,wrdp:False,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:3138,x:32719,y:32712,varname:node_3138,prsc:2|emission-4462-OUT,alpha-5915-U;n:type:ShaderForge.SFN_Color,id:7241,x:31858,y:32718,ptovrint:False,ptlb:Color,ptin:_Color,varname:node_7241,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:0,c3:0.8068962,c4:1;n:type:ShaderForge.SFN_Tex2d,id:7696,x:31970,y:32941,ptovrint:False,ptlb:node_7696,ptin:_node_7696,varname:node_7696,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:313b48aa0593cc741802546fa5a1919e,ntxv:2,isnm:False|UVIN-7683-OUT;n:type:ShaderForge.SFN_Time,id:8165,x:31687,y:33258,varname:node_8165,prsc:2;n:type:ShaderForge.SFN_Sin,id:6959,x:31879,y:33345,varname:node_6959,prsc:2|IN-8165-T;n:type:ShaderForge.SFN_Multiply,id:4462,x:32214,y:32781,varname:node_4462,prsc:2|A-7241-RGB,B-7696-RGB;n:type:ShaderForge.SFN_Vector2,id:7393,x:32071,y:33250,varname:node_7393,prsc:2,v1:1,v2:1;n:type:ShaderForge.SFN_Multiply,id:1334,x:32311,y:33348,varname:node_1334,prsc:2|A-7393-OUT,B-6959-OUT;n:type:ShaderForge.SFN_Add,id:7683,x:31679,y:32874,varname:node_7683,prsc:2|A-1334-OUT,B-5915-UVOUT;n:type:ShaderForge.SFN_TexCoord,id:5915,x:31679,y:33022,varname:node_5915,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_ValueProperty,id:3606,x:31464,y:33311,ptovrint:False,ptlb:node_3606,ptin:_node_3606,varname:node_3606,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;proporder:7241-7696;pass:END;sub:END;*/

Shader "Shader Forge/NewShader" {
    Properties {
        _Color ("Color", Color) = (1,0,0.8068962,1)
        _node_7696 ("node_7696", 2D) = "black" {}
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform float4 _Color;
            uniform sampler2D _node_7696; uniform float4 _node_7696_ST;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.pos = UnityObjectToClipPos( v.vertex );
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
////// Lighting:
////// Emissive:
                float4 node_8165 = _Time;
                float2 node_7683 = ((float2(1,1)*sin(node_8165.g))+i.uv0);
                float4 _node_7696_var = tex2D(_node_7696,TRANSFORM_TEX(node_7683, _node_7696));
                float3 emissive = (_Color.rgb*_node_7696_var.rgb);
                float3 finalColor = emissive;
                return fixed4(finalColor,i.uv0.r);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
