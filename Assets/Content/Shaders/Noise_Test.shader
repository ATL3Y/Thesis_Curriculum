// Shader created with Shader Forge v1.38 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.38;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:1,dpts:2,wrdp:True,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:3138,x:33633,y:32889,varname:node_3138,prsc:2|emission-5326-OUT;n:type:ShaderForge.SFN_Color,id:7241,x:32603,y:32686,ptovrint:False,ptlb:Color,ptin:_Color,varname:node_7241,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.07843138,c2:0.3921569,c3:0.7843137,c4:1;n:type:ShaderForge.SFN_Step,id:2317,x:32559,y:33150,varname:node_2317,prsc:2|A-3284-OUT,B-5393-RGB;n:type:ShaderForge.SFN_Tex2d,id:5393,x:32126,y:32981,ptovrint:False,ptlb:node_5393,ptin:_node_5393,varname:node_5393,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:94dea9a2c3913d449adf9f3cf191e517,ntxv:2,isnm:False;n:type:ShaderForge.SFN_ValueProperty,id:102,x:32155,y:33179,ptovrint:False,ptlb:node_102,ptin:_node_102,varname:node_102,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0.5;n:type:ShaderForge.SFN_Slider,id:3284,x:32141,y:33322,ptovrint:False,ptlb:node_3284,ptin:_node_3284,varname:node_3284,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0,max:1;n:type:ShaderForge.SFN_Tex2d,id:1873,x:32173,y:32734,ptovrint:False,ptlb:node_1873aaaa,ptin:_node_1873aaaa,varname:node_1873,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:929ba4738c21ddb43879ad412cd6ca10,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Add,id:3441,x:32740,y:32902,varname:node_3441,prsc:2|A-1873-RGB,B-681-OUT;n:type:ShaderForge.SFN_Multiply,id:681,x:32559,y:33025,varname:node_681,prsc:2|A-1873-RGB,B-5969-RGB,C-2317-OUT;n:type:ShaderForge.SFN_Color,id:5969,x:32292,y:32898,ptovrint:False,ptlb:node_5969,ptin:_node_5969,varname:node_5969,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:0,c3:0,c4:1;n:type:ShaderForge.SFN_Noise,id:5326,x:33181,y:33093,varname:node_5326,prsc:2|XY-261-OUT;n:type:ShaderForge.SFN_Time,id:3932,x:32495,y:33296,varname:node_3932,prsc:2;n:type:ShaderForge.SFN_Vector2,id:8194,x:32719,y:33396,varname:node_8194,prsc:2,v1:0.5,v2:0.5;n:type:ShaderForge.SFN_Append,id:3011,x:32755,y:33260,varname:node_3011,prsc:2|A-3932-T,B-3932-TSL;n:type:ShaderForge.SFN_Sin,id:5491,x:32931,y:33150,varname:node_5491,prsc:2|IN-3011-OUT;n:type:ShaderForge.SFN_TexCoord,id:356,x:32931,y:33311,varname:node_356,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_Add,id:261,x:33127,y:33321,varname:node_261,prsc:2|A-356-UVOUT,B-5491-OUT;proporder:7241-5393-102-3284-1873-5969;pass:END;sub:END;*/

Shader "Shader Forge/NewestShader" {
    Properties {
        _Color ("Color", Color) = (0.07843138,0.3921569,0.7843137,1)
        _node_5393 ("node_5393", 2D) = "black" {}
        _node_102 ("node_102", Float ) = 0.5
        _node_3284 ("node_3284", Range(0, 1)) = 0
        _node_1873aaaa ("node_1873aaaa", 2D) = "white" {}
        _node_5969 ("node_5969", Color) = (1,0,0,1)
    }
    SubShader {
        Tags {
            "RenderType"="Opaque"
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
            #pragma multi_compile_fwdbase_fullshadows
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
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
                float4 node_3932 = _Time;
                float2 node_261 = (i.uv0+sin(float2(node_3932.g,node_3932.r)));
                float2 node_5326_skew = node_261 + 0.2127+node_261.x*0.3713*node_261.y;
                float2 node_5326_rnd = 4.789*sin(489.123*(node_5326_skew));
                float node_5326 = frac(node_5326_rnd.x*node_5326_rnd.y*(1+node_5326_skew.x));
                float3 emissive = float3(node_5326,node_5326,node_5326);
                float3 finalColor = emissive;
                return fixed4(finalColor,1);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
