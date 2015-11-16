Shader "Custom/Cursor"
{
	Properties
	{
		_Color ("Color", Color) = (1,1,1,1)
	}
	SubShader
	{
		Tags { "Queue" = "Overlay" "RenderType" = "Opaque" }
		Pass
		{
			ZTest Always
			Lighting Off
			CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag
            
            float4 vert(float4 v:POSITION) : SV_POSITION {
                return mul (UNITY_MATRIX_MVP, v);
            }
            
            fixed4 _Color;

            fixed4 frag() : SV_Target {
            	fixed4 c = fixed4 (1,1,1,1);
                return c;
            }

            ENDCG
		}
		Pass
		{
			ZTest Always
			Lighting Off
			CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag
            
            float4 vert(float4 v:POSITION) : SV_POSITION {
                return mul (UNITY_MATRIX_MVP, v);
            }
            
            fixed4 _Color;

            fixed4 frag() : SV_Target {
            	fixed4 c = _Color;
                return c;
            }

            ENDCG
		}
	}
}
