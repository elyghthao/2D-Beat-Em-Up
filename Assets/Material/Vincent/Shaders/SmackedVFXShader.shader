Shader "Custom/SmackedVFXShader"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Emission("Emission", Color) = (1,1,1,1)
        _FadeSpeed("Fade Speed", float) = 0.5
        _Radius("Radius", float) = 0.5
    }
    SubShader
    {
        Tags { "Queue" = "Transparent" "RenderType"="Transparent" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard alpha:blend

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
        };
        
        fixed4 _Color;
        half3 _Emission;
        float _FadeSpeed;
        float _Radius;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        float sdCircle(float2 p, float r)
        {
            return length(p) - r;
        }

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            float circleSDF = sdCircle(IN.uv_MainTex - 0.5, _Radius);
            float constrainedCircle = smoothstep(-0.15, 0.15, circleSDF);
            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            c.a *= constrainedCircle;
            c.rgb += _Color.rgb;

            // Debugging
            
            o.Albedo = c.rgb;
            //o.Smoothness = 0;
            //o.Metallic = 0;
            // Metallic and smoothness come from slider variables
            o.Emission = _Emission * c.a;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
