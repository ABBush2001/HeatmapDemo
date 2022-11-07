// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/Texture Blend" {
         Properties {
             _Color ("Color", Color) = (1,1,1,1)
             _Blend ("Texture Blend", Range(0,1)) = 0.0
             _MainTex("Texture", 2D) = "white" {}
             _MainTex2 ("Texture", 2D) = "white" {}
         }
             
         SubShader {
            Tags{ "Queue" = "Transparent" }
			Blend SrcAlpha OneMinusSrcAlpha // Alpha blend

            //first pass for background texture
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

				sampler2D _MainTex2;
				float4 _MainTex2_ST;
            
				v2f vert (appdata v)
				{
					v2f o;
					o.vertex = UnityObjectToClipPos(v.vertex);
					o.uv = TRANSFORM_TEX(v.uv, _MainTex2);
					UNITY_TRANSFER_FOG(o,o.vertex);
					return o;
				}
            
				fixed4 frag (v2f i) : SV_Target
				{
					// sample the texture
					fixed4 col = tex2D(_MainTex2, i.uv);
					// apply fog
					UNITY_APPLY_FOG(i.fogCoord, col);
					return col;
				}
				ENDCG
			}

            //second pass for heatmap
			Pass
			{
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

                    //here is where i blend the headscale color with the other texture (_MainTex2)
		            //half4 color = lerp(tex2D(_MainTex, fixed2(h, 0.5)), tex2D(_MainTex2, output.worldPos.xy), _Blend);
                    //below is what was here before instead of the above line
                    half4 color = tex2D(_MainTex, fixed2(h, 0.5));
                   
		            return color;
		        }

			    ENDCG
            }
         }

         /*SubShader {
            Pass {
                Name "ColorReplacement"

                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #pragma fragmentoption ARB_precision_hint_fastest
                #include "UnityCG.cginc"

                struct v2f
                {
                    float4  pos : SV_POSITION;
                    float2  uv : TEXCOORD0;
                };
 
                v2f vert (appdata_tan v)
                {
                    v2f o;
                    o.pos = UnityObjectToClipPos(v.vertex);
                    o.uv = v.texcoord.xy;
                    return o;
                }
               
                sampler2D _MainTex;
                sampler2D _MainTex2;
 
                float4 frag(v2f i) : COLOR
                {
                    // SURFACE COLOUR
                    float greyscale = tex2D(_MainTex, i.uv).r;
               
                    // RESULT
                    float4 result;
                    result.rgb = tex2D(_MainTex2, float2(greyscale, 0.5)).rgb;
                    result.a = tex2D(_MainTex, i.uv).a;
                    return result;
                }
                ENDCG

            }
         }*/

         FallBack "Diffuse"
     }