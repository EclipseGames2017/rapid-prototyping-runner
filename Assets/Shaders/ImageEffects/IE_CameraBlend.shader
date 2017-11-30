Shader "ImageEffects/IE_CameraBlend"
{
	Properties
	{
		// Blit fills _MainTex with the src paramater
		_MainTex ("Texture", 2D) = "white" {}
		// the second camera's image
		_SecondTex("AltTexture", 2D) = "black" {}
		// texture to use when blending between the two textures
		_Transition("Gradient", 2D) = "white" {}
		// where along the transition to sample
		_Cutoff ("Cutoff", Range(0.0, 1.0)) = .05
		// screen height to width ratio, so the transition is sampled square and does not get squished
		_Ratio ("YScale", Float) = 0.625
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
			
			// declaring the global variables for the frag shader
			sampler2D _MainTex;
			sampler2D _SecondTex;
			sampler2D _Transition;
			float _Cutoff;
			float _Ratio;

			fixed4 frag (v2f i) : SV_Target
			{
				// set the colour by sampling the _MainTex
				fixed4 col = tex2D(_MainTex, i.uv);
				
				// if the colour of the transition is greater or equal to the cutoff value
				if (tex2D(_Transition, i.uv * float2(1.0, _Ratio)).r >= _Cutoff)
				{
					// replace the pixel with the pixel from the second camera
					col = tex2D(_SecondTex, i.uv);
				}
				
				return col;
			}
			ENDCG
		}
	}
}
