Shader "Custom/Texture Blend" {
         Properties {
             _Color ("Color", Color) = (1,1,1,1)
             _Blend ("Texture Blend", Range(0,1)) = 0.0
             _MainTex("Texture", 2D) = "white" {}
             _MainTex2 ("Albedo (RGB)", 2D) = "white" {}
         }
         SubShader {
             Tags { "RenderType" = "Opaque" }
             LOD 200
            
             CGPROGRAM

             #pragma surface surf Standard fullforwardshadows
      
      
             sampler2D _MainTex;
             sampler2D _MainTex2;
      
             struct Input {
                 float2 uv_MainTex;
                 float2 uv_MainTex2;
             };
      
             half _Blend;
             fixed4 _Color;
      
             void surf (Input IN, inout SurfaceOutputStandard o) {
                 // Albedo comes from a texture tinted by color
                 fixed4 c = lerp (tex2D (_MainTex, IN.uv_MainTex), tex2D (_MainTex2, IN.uv_MainTex2), _Blend) ;
                 o.Albedo = c.rgb;
                 o.Alpha = c.a;
             }
             ENDCG
         }
         SubShader {
            Tags{ "Queue" = "Transparent" }
			Blend SrcAlpha OneMinusSrcAlpha // Alpha blend

			Pass{
			    CGPROGRAM
                #pragma vertex vert             
                #pragma fragment frag

		        struct vertInput {
			        float4 pos : POSITION;
		        };

		        struct vertOutput {
			        float4 pos : POSITION;
			        fixed3 worldPos : TEXCOORD1;
		        };

		        vertOutput vert(vertInput input) {
			        vertOutput o;
			        o.pos = UnityObjectToClipPos(input.pos);
			        o.worldPos = mul(unity_ObjectToWorld, input.pos).xyz;
			        return o;
		        }

		        uniform int _Points_Length = 0;
		        uniform float4 _Points[100];		// (x, y, z) = position
		        uniform float4 _Properties[100];	// x = radius, y = intensity

		        sampler2D _MainTex;

		        half4 frag(vertOutput output) : COLOR{
			        // Loops over all the points
			        half h = 0;
		            for (int i = 0; i < _Points_Length; i++)
		            {
			            // Calculates the contribution of each point
			            half di = distance(output.worldPos, _Points[i].xyz);

			            half ri = _Properties[i].x;
			            half hi = 1 - saturate(di / ri);

			            h += hi * _Properties[i].y;
		            }

		            // Converts (0-1) according to the heat texture
		            h = saturate(h);
		            half4 color = tex2D(_MainTex, fixed2(h, 0.5));
		            return color;
		        }
			    ENDCG
            }
         }

         FallBack "Diffuse"
     }