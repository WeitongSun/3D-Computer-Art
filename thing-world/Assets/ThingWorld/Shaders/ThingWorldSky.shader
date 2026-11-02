// Thing World — stylized sky.
// A three-band gradient for day and for night, a sun disc, stars, and an optional
// painted panorama on top. It follows the Day Night Cycle automatically (_TW_Day, _TW_SunDir).
// Light enough for Meta Quest; works with single-pass instanced stereo.
Shader "ThingWorld/Sky"
{
    Properties
    {
        [Header(Day)]
        _DayTop ("Day: top", Color) = (0.30, 0.55, 0.92, 1)
        _DayHorizon ("Day: horizon", Color) = (0.86, 0.90, 0.95, 1)
        _DayBottom ("Day: below horizon", Color) = (0.55, 0.58, 0.62, 1)

        [Header(Night)]
        _NightTop ("Night: top", Color) = (0.02, 0.03, 0.09, 1)
        _NightHorizon ("Night: horizon", Color) = (0.10, 0.12, 0.25, 1)
        _NightBottom ("Night: below horizon", Color) = (0.03, 0.03, 0.06, 1)

        [Header(Gradient)]
        _HorizonHeight ("Horizon height", Range(-0.3, 0.3)) = 0
        _HorizonSoftness ("Horizon softness", Range(0.02, 1.5)) = 0.35

        [Header(Sun)]
        _SunSize ("Sun size", Range(0, 0.2)) = 0.04
        _SunGlow ("Sun glow", Range(0, 1)) = 0.35
        _SunTint ("Sun tint", Color) = (1, 0.95, 0.85, 1)

        [Header(Stars)]
        _StarDensity ("Star density", Range(0, 1)) = 0.5
        _StarBrightness ("Star brightness", Range(0, 3)) = 1.2

        [Header(Optional painted panorama)]
        [NoScaleOffset] _Panorama ("Panorama (equirectangular, alpha = how much shows)", 2D) = "black" {}
        _PanoramaStrength ("Panorama strength", Range(0, 1)) = 0
        _PanoramaNightBrightness ("Panorama brightness at night", Range(0, 1)) = 0.25
        _PanoramaRotation ("Panorama rotation", Range(0, 360)) = 0
    }

    SubShader
    {
        Tags { "Queue" = "Background" "RenderType" = "Background" "PreviewType" = "Skybox" }
        Cull Off
        ZWrite Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 3.0
            #include "UnityCG.cginc"

            fixed4 _DayTop, _DayHorizon, _DayBottom;
            fixed4 _NightTop, _NightHorizon, _NightBottom;
            half _HorizonHeight, _HorizonSoftness;
            half _SunSize, _SunGlow;
            fixed4 _SunTint;
            half _StarDensity, _StarBrightness;
            sampler2D _Panorama;
            half _PanoramaStrength, _PanoramaNightBrightness, _PanoramaRotation;

            // Set every frame by the Day Night Cycle.
            float _TW_Day;
            float4 _TW_SunDir;
            float4 _TW_SunColor;

            struct appdata
            {
                float4 vertex : POSITION;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float3 dir : TEXCOORD0;
                UNITY_VERTEX_OUTPUT_STEREO
            };

            v2f vert (appdata v)
            {
                v2f o;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
                o.pos = UnityObjectToClipPos(v.vertex);
                o.dir = v.vertex.xyz;
                return o;
            }

            float hash31(float3 p)
            {
                p = frac(p * 0.1031);
                p += dot(p, p.yzx + 33.33);
                return frac((p.x + p.y) * p.z);
            }

            float3 bandColor(float y, float3 top, float3 horizon, float3 bottom)
            {
                float above = saturate((y - _HorizonHeight) / _HorizonSoftness);
                float below = saturate((_HorizonHeight - y) / (_HorizonSoftness * 0.5));
                float3 c = lerp(horizon, top, above);
                return lerp(c, bottom, below);
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float3 dir = normalize(i.dir);

                // Before the Day Night Cycle runs, _TW_Day is 0 and _TW_SunDir is zero:
                // treat a missing sun as plain daylight with the sun overhead.
                float hasSun = step(0.001, dot(_TW_SunDir.xyz, _TW_SunDir.xyz));
                float day = lerp(1.0, saturate(_TW_Day), hasSun);
                float3 sunDir = hasSun > 0.5 ? normalize(_TW_SunDir.xyz) : float3(0, 1, 0);
                float3 sunColor = hasSun > 0.5 ? _TW_SunColor.rgb : float3(1, 1, 1);

                float3 dayCol = bandColor(dir.y, _DayTop.rgb, _DayHorizon.rgb, _DayBottom.rgb);
                float3 nightCol = bandColor(dir.y, _NightTop.rgb, _NightHorizon.rgb, _NightBottom.rgb);
                float3 col = lerp(nightCol, dayCol, day);

                // Sun disc and glow (also a soft moon at night, from the same light).
                float sd = dot(dir, sunDir);
                float disc = smoothstep(1.0 - _SunSize * _SunSize - 0.0005, 1.0 - _SunSize * _SunSize, sd);
                float glow = pow(saturate(sd), 8.0) * _SunGlow;
                col += (disc + glow) * _SunTint.rgb * sunColor * step(-0.05, sunDir.y);

                // Stars: only at night, only above the horizon.
                float3 cell = floor(dir * 180.0);
                float h = hash31(cell);
                float star = step(1.0 - _StarDensity * 0.02, h) * saturate(dir.y * 4.0);
                col += star * _StarBrightness * (1.0 - day);

                // Optional painted panorama.
                float rot = radians(_PanoramaRotation);
                float u = frac(atan2(dir.x, dir.z) / (2.0 * UNITY_PI) + 0.5 + rot / (2.0 * UNITY_PI));
                float v = asin(clamp(dir.y, -1.0, 1.0)) / UNITY_PI + 0.5;
                fixed4 pano = tex2Dlod(_Panorama, float4(u, v, 0, 0));
                float panoLight = lerp(_PanoramaNightBrightness, 1.0, day);
                col = lerp(col, pano.rgb * panoLight, pano.a * _PanoramaStrength);

                return fixed4(col, 1);
            }
            ENDCG
        }
    }
    Fallback Off
}
