Shader "Unlit/HealthBarShader"
{
    Properties
    {
        _LeftColor ("Left Color", Color) = (1,1,1,1)
        _RightColor ("Right Color", Color) = (1,1,1,1)
        _MaxHealth ("Max Health", float) = 1.0
        _CurrentHealth("Current Health", float) = 1.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

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
            float4 _LeftColor;
            float4 _RightColor;
            float _MaxHealth;
            float _CurrentHealth;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float normalizedHealth = _CurrentHealth/_MaxHealth;
                float healthPool = step(normalizedHealth, i.uv.x);
                fixed3 result =  ((1 - healthPool * 1) * _LeftColor.rgb);
                result += healthPool * _RightColor;
                return fixed4(result, 1);
            }
            ENDCG
        }
    }
}
