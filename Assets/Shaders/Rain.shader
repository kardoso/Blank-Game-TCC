// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/Weather Effects/Rain" {
 
    Properties{
        _MainTex("Noise Texture", 2D) = "white" { }
        _CustomColor("Noise Color", Color) = (1,1,1,1)
        _NoiseThreshold("Intensity", Range(0, 1)) = 0
    }
 
    SubShader{
        //We didn't use a Tag in this shader
        Pass{
            CGPROGRAM
             
            //define the functions
            #pragma vertex vert
            #pragma fragment frag
 
            //vertex structure
            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };
             
            //fragment structure
            struct v2f {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };
     
            //linking definitions
            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _CustomColor;
            float _NoiseThreshold;
             
            v2f vert(appdata INvertex) {
                v2f output;
                output.vertex = UnityObjectToClipPos(INvertex.vertex);     //transform to screen
				_MainTex_ST.z -= 2;
                output.uv = INvertex.uv *_MainTex_ST.xy + _MainTex_ST.zw;   //allow tiling and offset
                return output;
            }
     
            float4 frag(v2f INfragment) : SV_Target{
                float4 noise = tex2D(_MainTex, INfragment.uv);  //get noise value
 
                clip(_NoiseThreshold - noise.rgb);              //discard pixel if too low
                 
                return _CustomColor;                            //use uniform colour
            }
 
            ENDCG
        }
    }
}