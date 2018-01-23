// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/FillCircuitTex" 
{
	Properties{
		_UnfilledColor( "Unfilled Color", Color ) = ( 0.0, 0.0, 0.0, 1.0 )
		_MainTex( "Diffuse Texture", 2D ) = "white"{}
		_FillMap( "Fill Map", 2D ) = "white"{}
		_SpecColor( "Specular Color", Color ) = ( 1.0, 1.0, 1.0, 1.0 )
		_Shininess( "Shininess", Float ) = 10.0
		_RimColor( "Rim Color", Color ) = ( 1.0, 1.0, 1.0, 1.0 )
		_RimPower( "Rim Power", Range( 1.0, 10.0 ) ) = 3.0
		_FillAmount( "Fill Amount", Range( 0.0, 1.0 ) ) = 0.0
		_Flicker("Flicker", Range( 0.0, 1.0 ) ) = 0.0
	}
	SubShader
		{
		Pass
		{
			Tags { "LightMode" = "ForwardBase" "Queue" = "Transparent" }

			// ZWrite Off // don't write to depth buffer 

			Blend SrcAlpha OneMinusSrcAlpha // use alpha blending

			CGPROGRAM
			//pragmas
			#pragma vertex vert
			#pragma fragment frag
			//#pragma exclude_renderers Flash

			//user defined variables
			uniform float4 _UnfilledColor; 
			uniform float4 _SpecColor;
			uniform float4 _RimColor;
			uniform float _Shininess;
			uniform float _RimPower;
			uniform sampler2D _MainTex;
			uniform float4 _MainTex_ST;
			uniform sampler2D _FillMap;
			uniform float _FillAmount;
			uniform float _Flicker;

			// Unity difined variables
			uniform float4 _LightColor0;

			// Base input struct
			struct vertexInput
			{
				float4 vertex: POSITION;
				float3 normal: NORMAL;
				float4 texcoord: TEXCOORD0;
			};

			struct vertexOutput
			{
				float4 pos: SV_POSITION;
				float4 tex: TEXCOORD0;
				float4 posworld: TEXCOORD1;
				float3 normalDir: TEXCOORD2;
			};

			// Vertex function
			vertexOutput vert(vertexInput v)
			{
				vertexOutput o;

				o.pos = UnityObjectToClipPos(v.vertex);
				o.posworld = mul(unity_ObjectToWorld, v.vertex);
				o.normalDir = normalize( mul(unity_WorldToObject, float4(v.normal, 0.0) ).xyz );
				o.tex = v.texcoord;
				return o;
			}

			// Fragment function
			float4 frag(vertexOutput o):COLOR
			{
				float3 normalDirection = o.normalDir;
				float3 viewDirection = normalize( _WorldSpaceCameraPos.xyz - o.posworld.xyz );
				float3 lightDirection;
				float atten;
				if( _WorldSpaceLightPos0.w == 0.0 )
				{ // Directional lights
					atten = 1.0;
					lightDirection = normalize( _WorldSpaceLightPos0.xyz );
				}
				else
				{
					float3 fragmentToLight = _WorldSpaceLightPos0.xyz - o.posworld.xyz;
					float dist = length( fragmentToLight );
					atten = 1.0/dist;
					lightDirection = normalize( fragmentToLight );
				}

				// Lighting
				float3 diffuseReflection = atten * _LightColor0.xyz * saturate( dot ( normalDirection, lightDirection ) );
				float3 specularReflection = diffuseReflection * _SpecColor.xyz * pow( saturate( dot( reflect( -lightDirection, normalDirection ), viewDirection ) ), _Shininess );

				// Rim lighting
				float3 rim = 1-saturate( dot( viewDirection, normalDirection ) );
				float3 rimLighting = saturate( dot( lightDirection, normalDirection )*_RimColor.xyz* _LightColor0.xyz*pow( rim, _RimPower ) );

				float3 lightFinal = UNITY_LIGHTMODEL_AMBIENT.xyz + diffuseReflection + specularReflection + rimLighting;

				// Texture Map
				half2 uv = o.tex.xy*_MainTex_ST.xy + _MainTex_ST.zw;
				float4 tex = tex2D( _MainTex, uv );
				float4 fill = tex2D( _FillMap, uv );
				float4 finalColor = ( 1.0, 1.0, 1.0, 1.0 );

				// If the main texture's alpha value is low, don't draw it. 
				if ( tex.w < 0.5 ) 
				{
					finalColor = float4( 0.0, 0.0, 0.0, 0.0 );
				}
				else 
				{
					// If the fill alpha value is low, fill in this area.
					if ( fill.w < 0.5 ) 
					{
						if ( uv.y < _FillAmount )
						{
							finalColor = float4( tex.xyz, 1.0f); 
						}
						else
						{
							finalColor = _UnfilledColor * (1.0 - _Flicker) + tex * _Flicker;
						}
					}
					// If the fill alpha value is high, fill in using the main texture
					else
					{
						finalColor = float4( tex.xyz * lightFinal, 1.0 );
					}
				}

				return finalColor;
			}
			ENDCG
		}

	}
//	FallBack "Diffuse"
}
