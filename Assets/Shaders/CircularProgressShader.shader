Shader "Custom/CircularProgressShader" {
	Properties {
		_Color ("Progress Bar Color", Color) = (0,0,1,1)
	    _Percent ("Percent", float) = 1
	    _MainTex ("Texture", 2D) = "white" { }
	}
	SubShader {
		Tags {"Queue"="AlphaTest" "IgnoreProjector"="True" "RenderType"="TransparentCutout"}
		LOD 100

		Lighting Off
	    Pass {
	    	Blend SrcAlpha OneMinusSrcAlpha

CGPROGRAM
#pragma vertex vert
#pragma fragment frag

#include "UnityCG.cginc"

float4 _Color;
float _Percent;
sampler2D _MainTex;

struct v2f {
    float4  pos : SV_POSITION;
    float2  uv : TEXCOORD0;
};

float4 _MainTex_ST;

v2f vert (appdata_base v)
{
    v2f o;
    o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
    o.uv = TRANSFORM_TEX (v.texcoord, _MainTex);
    return o;
}

half4 frag (v2f i) : COLOR
{
    half4 texcol = tex2D (_MainTex, i.uv);
    
    if (texcol.a > 0.5) {
    	if (_Percent > 0.5) {
    		texcol.b -= 0.0001f;
    	} else {
    		texcol.b += 0.0001f;
    	}
    	if (texcol.b > _Percent) {
    		discard;
    	} else {
    		texcol.a = _Color.a;
    	}
		texcol.rgb = _Color.rgb;
    } else {
    	discard;
    }
    return texcol;
}
ENDCG

	    }
	}
	Fallback "Diffuse"
} 