Shader "Custom/OutlineVertexExtrusion"
{
    // View: Highlight Outline (2pts)
    //
    // A classic "inverted hull" outline: the object is drawn twice.
    // Pass 1 draws a slightly larger copy of the mesh, pushed outward along each
    // vertex's normal, with its front faces culled (so only the back-facing,
    // outward-pushed shell remains visible) — this shell shows as an outline
    // silhouette behind the real object. Pass 2 draws the object normally on top.
    //
    // Setup:
    //  1. Assets -> Create -> Material, name it e.g. "OutlineMaterial".
    //  2. In the Inspector, change its Shader dropdown to Custom -> OutlineVertexExtrusion.
    //  3. Adjust Outline Color / Outline Width / Base Color to taste.
    //  4. Drag this material onto any object in your scene (e.g. one of your walls,
    //     or the planet sphere) to satisfy the Highlight Outline rubric point.

    Properties
    {
        _OutlineColor ("Outline Color", Color) = (0, 0, 0, 1)
        _OutlineWidth ("Outline Width", Range(0.0, 0.1)) = 0.02
        _BaseColor ("Base Color", Color) = (1, 1, 1, 1)
    }

    SubShader
    {
        Tags { "RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline" }

        // --- Pass 1: the outline shell ---
        Pass
        {
            Name "Outline"
            Cull Front
            ZWrite On

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            CBUFFER_START(UnityPerMaterial)
                float4 _OutlineColor;
                float _OutlineWidth;
            CBUFFER_END

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS   : NORMAL;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
            };

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                // Push each vertex outward along its normal by _OutlineWidth,
                // making this copy of the mesh slightly larger than the original.
                float3 posOS = IN.positionOS.xyz + IN.normalOS * _OutlineWidth;
                OUT.positionHCS = TransformObjectToHClip(posOS);
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                return _OutlineColor;
            }
            ENDHLSL
        }

        // --- Pass 2: the actual object surface, drawn normally on top ---
        Pass
        {
            Name "Base"
            Cull Back

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            CBUFFER_START(UnityPerMaterial)
                float4 _BaseColor;
            CBUFFER_END

            struct Attributes
            {
                float4 positionOS : POSITION;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
            };

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                return _BaseColor;
            }
            ENDHLSL
        }
    }
}
