Shader"Custom/BubblingWithNoiseTextureAndDepthPeeling" {
    Properties {
        _Color ("Main Color", Color) = (1,1,1,0.5)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _NoiseTex ("Noise Texture", 2D) = "white" {}
        _BubblingSpeed ("Bubbling Speed", Range(0, 10)) = 1
        _BubblingMagnitude ("Bubbling Magnitude", Range(0, 1)) = 0.1
    }
    SubShader {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }

        Pass {
Name"BACKFACES"
            Tags
{"LightMode" = "Always"
}

Cull Front

ZWrite On

ColorMask 0

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

v2f vert(appdata v)
{
    v2f o;
    o.vertex = UnityObjectToClipPos(v.vertex);
    o.uv = v.uv;
    return o;
}

fixed4 frag(v2f i) : SV_Target
{
    return fixed4(1, 1, 1, 1);
}
            ENDCG
        }

        Pass {
Name"FRONTFACES"
            Tags
{"LightMode" = "ForwardBase"
}

Blend SrcAlpha
OneMinusSrcAlpha
            Cull
Back
            ZWrite
Off
            ZTest
LEqual

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

sampler2D _MainTex;
sampler2D _NoiseTex;
fixed4 _Color;
float _BubblingSpeed;
float _BubblingMagnitude;

v2f vert(appdata v)
{
    v2f o;
    o.vertex = UnityObjectToClipPos(v.vertex);
    o.uv = v.uv;
    return o;
}

fixed4 frag(v2f i) : SV_Target
{
    float2 offset = _BubblingMagnitude * tex2D(_NoiseTex, i.uv + _Time.y * _BubblingSpeed).r;
    float2 uv = i.uv + offset;

    fixed4 texColor = tex2D(_MainTex, uv);
    fixed4 color = _Color * texColor;
    return color;
}
            ENDCG
        }
    }
FallBack"Diffuse"
}
