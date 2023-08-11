Shader "Unlit/SquarePulse"
{
    Properties
    {
        _PulseWidth("Pulse Width", float) = 0.1
        _PulseFadeSize("Pulse Fade Size", float) = 15
        _Color("Color", Color) = (0.8, 0.5, 0.25, 1)
    }
    SubShader
    {
        Blend SrcAlpha OneMinusSrcAlpha
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
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

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float sdQuadraticCircle( in float2 p )
            {
                p = abs(p); if( p.y>p.x ) p=p.yx;

                float a = p.x-p.y;
                float b = p.x+p.y;
                float c = (2.0*b-1.0)/3.0;
                float h = a*a + c*c*c;
                float t;
                if( h>=0.0 )
                {   
                    h = sqrt(h);
                    t = sign(h-a)*pow(abs(h-a),1.0/3.0) - pow(h+a,1.0/3.0);
                }
                else
                {   
                    float z = sqrt(-c);
                    float v = acos(a/(c*z))/3.0;
                    t = -z*(cos(v)+sin(v)*1.732050808);
                }
                t *= 0.5;
                float2 w = float2(-t,t) + 0.75 - t*t - p;
                return length(w) * sign( a*a*0.5+b-1.5 );
            }

            float sdBox( in float2 p, in float2 b )
            {
                float2 d = abs(p)-b;
                return length(max(d,0.0)) + min(max(d.x,d.y),0.0);
            }

            float opOnion( in float2 p, in float b, in float r )
            {
              return abs(sdBox(p, b)) - r;
            }

            float _PulseWidth;
            float _PulseFadeSize;
            float _Timer;
            float4 _Color;
            
            fixed4 frag (v2f i) : SV_Target
            {
                float pulseSpeed = 0.75;
                float pulseSize = 50 - frac(_Timer * pulseSpeed) * 50;
                
                float2 newUV = (i.uv - 0.5) * pulseSize;
                float qSquareSDF = sdBox(newUV, 0.1 * pulseSize);
                qSquareSDF = step(qSquareSDF, 2.5);
                float transparency = 1 - smoothstep(15, 5, pulseSize);
                transparency = min(transparency, qSquareSDF);
                return fixed4(_Color.rgb * qSquareSDF.xxx, transparency);
            }
            ENDCG
        }
    }
}
