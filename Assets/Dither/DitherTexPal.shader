Shader "Unlit/DitherTexPal"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_PalTex("Palette", 2D) = "white" {}
		_PalCoord("Palette Coord", Range(0,1)) = 0.5
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
			float4 _MainTex_ST;

			sampler2D _PalTex;
			float _PalCoord;
			float4 _PalTex_TexelSize;

			sampler2D _DitherTex;            
			float4 _DitherTex_TexelSize;
			float _DownScale;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

			float3 GetDitheredPalette(float x, float2 pixel)
			{
				float ncx = x + _PalTex_TexelSize.x;
				float3 c1 = tex2D(_PalTex, float2(x, x)); // _PalCoord) );
				float3 c2 = tex2D(_PalTex, float2( ncx, ncx)); // _PalCoord));
				
				float dith = tex2D(_DitherTex, pixel * _DownScale * _DitherTex_TexelSize.xy).r;
				float mixAmt = float( clamp(x, 0.0, 1.0) > dith );

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
