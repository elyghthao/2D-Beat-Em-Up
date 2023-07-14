Shader "Unlit/PostFXMask"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Distort("Distort", float) = 1
    }
    CGINCLUDE
        #include "UnityCG.cginc"

        struct appdata
        {
            float4 vertex : POSITION;
            float2 uv : TEXCOORD0;
            float3 normal : NORMAL;
        };

        struct v2f
        {
            float2 uv : TEXCOORD0;
            float4 vertex : SV_POSITION;
            float4 screenPosition : TEXCOORD1;
            float3 normal : TEXCOORD2;
            float3 viewDir : TEXCOORD3;
        };

        sampler2D _MainTex;
        sampler2D _GlobalRenderTexture;
        float _Distort;

        v2f vert (appdata v)
        {
            v2f o;
            o.vertex = UnityObjectToClipPos(v.vertex);
            o.uv = v.uv;
            o.screenPosition = ComputeScreenPos(o.vertex);
            o.normal = UnityObjectToWorldNormal(v.normal);
            o.viewDir = normalize(WorldSpaceViewDir(v.vertex));
            return o;
        }
    ENDCG
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 uvDistort = tex2D(_MainTex, i.uv);
                float2 textureCoordinate = i.screenPosition.xy / i.screenPosition.w;
                textureCoordinate = textureCoordinate * 2 - 1;
                //textureCoordinate += uvDistort.rg * _Distort;
                textureCoordinate = textureCoordinate * 0.5 + 0.5;
                fixed4 col = tex2D(_GlobalRenderTexture, textureCoordinate);
                return col;
            }
            ENDCG
        }
    }
}
