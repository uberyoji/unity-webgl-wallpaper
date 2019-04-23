Shader "Unlit/Dither"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_PalTex("Palette", 2D) = "white" {}
		_DitherTex("Texture", 2D) = "white" {}
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

			#define SUB_PALETTE_SIZE 4

			static float3 subPalette[4] =
			{
				RGB(0,  0,  0),
				RGB(152, 75, 67),
				RGB(121,193,200),
				RGB(255,255,255),
			};

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
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
                UNITY_TRANSFER_FOG(o,o.vertex);
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
