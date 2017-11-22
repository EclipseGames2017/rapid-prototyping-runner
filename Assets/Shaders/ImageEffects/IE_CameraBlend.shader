﻿Shader "ImageEffects/IE_CameraBlend"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_SecondTex("AltTexture", 2D) = "black" {}
		_Transition("Gradient", 2D) = "white" {}
		_Cutoff ("Cutoff", Range(0.0, 1.0)) = .05
	}
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			sampler2D _MainTex;
			sampler2D _SecondTex;
			sampler2D _Transition;
			float _Cutoff;

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);
				
				if (tex2D(_Transition, i.uv).r >= _Cutoff)
				{
					col = tex2D(_SecondTex, i.uv);
				}
				
				return col;
			}
			ENDCG
		}
	}
}
