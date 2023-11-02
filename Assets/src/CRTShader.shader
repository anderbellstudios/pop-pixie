Shader "Custom/CRTShader" {
  Properties {
    _MainTex ("Texture", 2D) = "white" {}
    _TexWidth ("Texture Width", Int) = 240
    _TexHeight ("Texture Height", Int) = 180
  }

  SubShader {
    // No culling or depth
    Cull Off ZWrite Off ZTest Always

    Pass {
      CGPROGRAM
      #pragma vertex vert
      #pragma fragment frag

      #include "UnityCG.cginc"

      struct appdata {
        float4 vertex : POSITION;
        float2 uv : TEXCOORD0;
      };

      struct v2f {
        float2 uv : TEXCOORD0;
        float4 vertex : SV_POSITION;
      };

      v2f vert (appdata v) {
        v2f o;
        o.vertex = UnityObjectToClipPos(v.vertex);
        o.uv = v.uv;
        return o;
      }

      sampler2D _MainTex;
      int _TexWidth, _TexHeight;

      fixed4 frag (v2f i) : SV_Target {
        fixed4 col = tex2D(_MainTex, i.uv);

        /**
         * Pixel position in the co-ordinate space of the texture. (0, 0) is
         * bottom-left; (_TexWidth, _TexHeight) is top-right.
         */
        float x = i.uv.x * _TexWidth;
        float y = i.uv.y * _TexHeight;

        // Split each pixel into red, green and blue sub-pixels
        float yOffset = y * 3 % 3;
        if (yOffset <= 1) {
          col.r = 0;
          col.g = 0;
        } else if (yOffset <= 2) {
          col.r = 0;
          col.b = 0;
        } else {
          col.g = 0;
          col.b = 0;
        }

        // Pill effect on each sub-pixel 
        float xOffset = x % 1;
        col.rgb *= 1 - 4 * pow(0.5 - xOffset, 2);

        // Compensate for reduction in brightness
        col.rgb *= 1.75;

        return col;
      }
      ENDCG
    }
  }
}
