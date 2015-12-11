Shader "Particle Distort Texture/Scale" 
{
Properties {
	_DistortX ("Distortion in X", Range (0,2)) = 1
	_DistortY ("Distortion in Y", Range (0,2)) = 0
	_MaskTex ("_MaskTex A", 2D) = "white" {}
	_Distort ("_Distort A", 2D) = "white" {}
	_ScaleX  ("Distort Scale X", float) = 1
	_ScaleY  ("Distort Scale Y", float) = 1
	_MainTex ("_MainTex RGBA", 2D) = "white" {}
	_AlphaM ("Alpha Multiplier", Range (0,10)) = 1
}

Category {
	Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
	Blend SrcAlpha One
	Cull Off Lighting Off ZWrite Off

	Lighting Off
	
	SubShader {
		Pass {
		
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			sampler2D _MainTex;
			sampler2D _Distort;
			sampler2D _MaskTex;
			
			struct appdata_t {
				fixed4 vertex : POSITION;
				fixed2 texcoord : TEXCOORD0;
				fixed4 color : COLOR;
			};

			struct v2f {
				fixed4 vertex : SV_POSITION;
				fixed2 texcoord : TEXCOORD0;
				fixed2 texcoord1 : TEXCOORD1;
				fixed4 color : COLOR;
			};
			
			fixed4 _MainTex_ST;
			fixed4 _Distort_ST;
			fixed4 _MaskTex_ST;
			
			fixed _DistortX;
			fixed _DistortY;
			fixed _AlphaM;
			
			fixed _ScaleX;
			fixed _ScaleY;

			v2f vert (appdata_t v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.texcoord1 = v.texcoord;
				o.texcoord =  TRANSFORM_TEX(v.texcoord, _Distort);
				o.color = v.color;

				return o;
			}
			
			fixed4 frag (v2f i) : Color
			{
				fixed tex = tex2D(_MaskTex, i.texcoord1).a;
				
				fixed2 trantex = fixed2(i.texcoord.x-0.5,i.texcoord.y-0.5);
				
				fixed dirx = 1;
				fixed diry = 1;
				
				if(trantex.x>0.5)
				{
					dirx=1;
				}
				else
				{
					dirx=-1;
				}

				if(trantex.y>0.5)
				{
					diry=1;
				}
				else
				{
					diry=-1;
				}

				fixed distort = tex2D(_Distort, fixed2(
														trantex.x*i.color.a*_ScaleX
														,
														trantex.y*i.color.a*_ScaleY
														)).a;
				
				fixed2 distortUV = fixed2((
									i.texcoord1.x+distort*dirx
									)*_DistortX,(
									i.texcoord1.y+distort*diry
									)*_DistortY);
									
				fixed4 tex2 = tex2D(_MainTex,distortUV);
				
				return fixed4(tex2.rgb*i.color.rgb,tex2.a*tex*_AlphaM*i.color.a);
			}
			ENDCG 
		}
	}	
}
}
