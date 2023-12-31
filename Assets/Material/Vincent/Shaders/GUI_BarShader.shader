Shader "Unlit/GUI_BarShader"
{
    Properties
    {
        _LeftColor ("Left Color", Color) = (1,1,1,1)
        _RightColor ("Right Color", Color) = (1,1,1,1)
        _MaxValue ("Max Value", float) = 1.0
        _CurrentValue("Current Value", float) = 1.0
        _BoarderRound("Boarder Round", float) = 0.02
        _BarRound("Bar Round", float) = 0.02
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
            float _MaxValue;
            float _CurrentValue;
            float _BoarderRound;
            float _BarRound;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float sdSegment(float2 p, float2 a, float2 b)
            {
                float2 pa = p-a, ba = b-a;
                float h = clamp(dot(pa, ba)/dot(ba, ba), 0.0, 1.0 );
                return length(pa - ba*h);
            }

            float sdRoundedSegment(float2 p, float2 a, float2 b, float r)
            {
                return sdSegment(p, a, b) - r;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 boarderLeftPoint = float2(0.05, 0.5);
                float2 boarderRightPoint = float2(0.95, 0.5);
                float2 p = float2(i.uv);
                float3 boarderSdf = float3(
                    sdRoundedSegment(p, boarderLeftPoint, boarderRightPoint, _BoarderRound).xxx
                    );
                boarderSdf = ceil(boarderSdf);
                clip(0.5 - boarderSdf);
                float2 barLeftPoint = float2(0.05, 0.5);
                float2 barRightPoint = float2(0.95, 0.5);
                float3 barSdf = float3(
                    sdRoundedSegment(p, barLeftPoint, barRightPoint, _BarRound).xxx
                    );
                barSdf = ceil(barSdf);
                float3 result = 1 - (boarderSdf + barSdf);
                float normalizedValue = (_CurrentValue + 3.2)/(_MaxValue + 6.4);
                float valuePool = step(normalizedValue, i.uv.x);
                float3 colResult = ((1 - valuePool * 1) * _LeftColor.rgb);
                colResult += valuePool * _RightColor;
                result = min(result, colResult);
                return fixed4(result, 1);
            }
            ENDCG
        }
    }
}
