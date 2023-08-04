Shader "Unlit/ShockwavePostEffect"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    CGINCLUDE
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
        float4 _MainTex_ST;

        v2f vert (appdata v)
        {
            v2f o;
            o.vertex = UnityObjectToClipPos(v.vertex);
            o.uv = TRANSFORM_TEX(v.uv, _MainTex);
            return o;
        }
    ENDCG
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass // 0
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            fixed4 _ScreenTint;

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                return col * _ScreenTint;
            }
            ENDCG
        }
        Pass // 1
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            float4 _MainTex_TexelSize;

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                float scale = 1;
                float3 blurSample =
                    tex2D(_MainTex, i.uv + _MainTex_TexelSize.xy * float2(-scale, -scale)) +    // Down, left
                    tex2D(_MainTex, i.uv + _MainTex_TexelSize.xy * float2(-scale, scale)) +    // Down, right
                    tex2D(_MainTex, i.uv + _MainTex_TexelSize.xy * float2(scale, -scale)) +    // Up, left
                    tex2D(_MainTex, i.uv + _MainTex_TexelSize.xy * float2(scale, scale));     // Up, right
                return fixed4(blurSample * 0.25, 1);
            }
            ENDCG
        }
        Pass // 2
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            float4 _MainTex_TexelSize;

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                float scale = 0.5;
                float3 blurSample =
                    tex2D(_MainTex, i.uv + _MainTex_TexelSize.xy * float2(-scale, -scale)) +    // Down, left
                    tex2D(_MainTex, i.uv + _MainTex_TexelSize.xy * float2(-scale, scale)) +    // Down, right
                    tex2D(_MainTex, i.uv + _MainTex_TexelSize.xy * float2(scale, -scale)) +    // Up, left
                    tex2D(_MainTex, i.uv + _MainTex_TexelSize.xy * float2(scale, scale));     // Up, right
                return fixed4(blurSample * 0.25, 1);
            }
            ENDCG
        }
        Pass // 3
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            float _UpperFeather;
            float _BottomFeather;
            float _RippleIntensity;
            float _RippleSpeed;
            float4 _Position;
            
            fixed4 frag (v2f i) : SV_Target
            {
                // position dot for Debugging
                // float2 distanceToPos = distance(i.uv, _Position.xy);
                // float dot = step(distanceToPos, 0.005);
                // float3 dotColor = float3(1 , 0, 0);
                
                // Ripple effect
                //float2 preUV = i.uv - _Position.xy + 0.5;
                float2 newUV = i.uv * 2 - 1;
                newUV = newUV * 2;
                float timer = frac(_Time.y * _RippleSpeed);
                float len = length(newUV);
                float upperRing = smoothstep(len + _UpperFeather, len - _BottomFeather, timer);
                float inverseRing = 1 - upperRing;
                float finalRing = upperRing * inverseRing;
                float2 finalUV = i.uv + newUV * finalRing * _RippleIntensity * (1 - timer);
                fixed4 col = tex2D(_MainTex, finalUV);
                return fixed4(col.xyz, 1);
            }
            ENDCG
        }
    }
}
