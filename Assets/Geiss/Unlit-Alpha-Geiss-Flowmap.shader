// Unity built-in shader source. Copyright (c) 2016 Unity Technologies. MIT license (see license.txt)

// Unlit alpha-blended shader.
// - no lighting
// - no lightmap support

Shader "Unlit/GeissFlow" {
Properties {
    _MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
	_FlowTex("Flow (RGB) Trans (A)", 2D) = "white" {}
	_FlowIntensity("Flow Intensity", Range(0,1)) = 0.1
	_Color("Main Color", Color) = (1,1,1,1)
		
}

SubShader {
    Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
    LOD 100

    ZWrite Off
    Blend SrcAlpha OneMinusSrcAlpha

    Pass {
        CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata_t {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f {
                float4 vertex : SV_POSITION;
                float2 texcoord : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                UNITY_VERTEX_OUTPUT_STEREO
            };

            sampler2D _MainTex;
			sampler2D _FlowTex;
            float4 _MainTex_ST;
			float4 _Color;
			float _FlowIntensity;

            v2f vert (appdata_t v)
            {
                v2f o;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
				fixed2 off = tex2D(_FlowTex, i.texcoord).rg;
				off *= 2;
				off -= 1;
				off *= _FlowIntensity;
                fixed4 col = tex2D(_MainTex, i.texcoord+off);
                // UNITY_APPLY_FOG(i.fogCoord, col);
				col = col * _Color;
                return col;
            }
        ENDCG
    }
}

}
