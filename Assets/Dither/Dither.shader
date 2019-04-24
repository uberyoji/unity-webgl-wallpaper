Shader "Unlit/Dither"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_PalTex("Palette", 2D) = "white" {}
		_DitherTex("Dither Pattern", 2D) = "white" {}
		_DownScale("Down Scale", float) = 1
	}
		SubShader
	{
		Tags { "RenderType" = "Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog

			#include "UnityCG.cginc"

			#define RGB(r,g,b) (float3(r,g,b) / 255.0)

			/*
			#define SUB_PALETTE_SIZE 6

			static float3 subPalette[6] =
			{
				RGB(0x00, 0x00, 0x00),
				RGB(0x57, 0x17, 0x07), //
				RGB(0xDF, 0x4F, 0x07), //
				RGB(0xCF, 0x7F, 0x0F), //
				RGB(0xBF, 0xA7, 0x27), //
				RGB(0xFF,0xFF,0xFF),
			};
			*/

		
		#define SUB_PALETTE_SIZE 8

			static float3 subPalette[8] =
			{
				RGB(0x00, 0x00, 0x00),

				RGB(0x07, 0x07, 0x07),
				RGB(0x57, 0x17, 0x07), //
//				RGB(0x9F, 0x2F, 0x07), // 
				RGB(0xDF, 0x4F, 0x07), //
//				RGB(0xD7, 0x5F, 0x07), //
				RGB(0xCF, 0x7F, 0x0F), //
//				RGB(0xC7, 0x97, 0x1F), //
				RGB(0xBF, 0xA7, 0x27), //
				RGB(0xB7, 0xB7, 0x37), //

				RGB(0xFF,0xFF,0xFF),
			};
		

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
//                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
			sampler2D _PalTex;
			sampler2D _DitherTex;
            float4 _MainTex_ST;
			float4 _DitherTex_TexelSize;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
//                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

			float3 GetDitheredPalette(float x, float2 pixel)
			{
				float idx = clamp(x, 0.0, 1.0)*float(SUB_PALETTE_SIZE - 1);

				float3 c1 = subPalette[int(idx)];
				float3 c2 = subPalette[int(idx) + 1];
				
				float dith = tex2D(_DitherTex, pixel * _DitherTex_TexelSize.xy).r;
				float mixAmt = float(frac(idx) > dith);

				return lerp(c1, c2, mixAmt);
			}

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                float shade = tex2D(_MainTex, i.uv).r;

			fixed4 color;
			color.rgb = GetDitheredPalette(shade, i.vertex.xy );
				color.a = 1;
									                
                return color;
            }
            ENDCG
        }
    }
}
