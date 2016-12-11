Shader "Unlit/RotationShader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_RotationDegrees ("Rotation Degrees", float) = 90.0
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
			// make fog work
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float _RotationDegrees;

			//vertex shader
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);

				const float PI = 3.141592;
				o.uv.xy -= (0.5);
				float s = sin (_RotationDegrees/180*PI);
				float c = cos (_RotationDegrees/180*PI);

				float2x2 rotationMatrix = float2x2(c, -s,s,c);
				rotationMatrix *= 0.5;
				rotationMatrix += 0.5;
				rotationMatrix = rotationMatrix*2-1;
				o.uv.xy = mul(o.uv.xy, rotationMatrix);
				o.uv.xy += (0.5);

				return o;
			}

			//pixel shader
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				//fixed4 col = tex2D(_MainTex,mul(_Rotation,float4(i.uv,0)));
				fixed4 col = tex2D(_MainTex,i.uv);
				// apply fog
				UNITY_APPLY_FOG(i.fogCoord, col);
				return col;
			}
			ENDCG
		}
	}
}


