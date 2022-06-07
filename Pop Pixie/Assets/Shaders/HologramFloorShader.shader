Shader "Custom/HologramFloorShader" {
  Properties {
    _MainTex ("Sprite Texture", 2D) = "white" {}
    _Color ("Tint", Color) = (1,1,1,1)
    _PixelsOn ("Pixels On", Int) = 2
    _PixelsOff ("Pixels Off", Int) = 2
  }

  SubShader {
    Tags { 
      "Queue"="Transparent" 
      "IgnoreProjector"="True" 
      "RenderType"="Transparent" 
      "PreviewType"="Plane"
      "CanUseSpriteAtlas"="True"
    }

    Cull Off
    Lighting Off
    ZWrite Off
    Blend One OneMinusSrcAlpha

    Pass {
      CGPROGRAM

      #pragma vertex vert
      #pragma fragment frag

      #include "UnityCG.cginc"
      
      struct appdata_t {
        float4 vertex   : POSITION;
        float4 color    : COLOR;
        float2 texcoord : TEXCOORD0;
      };

      struct v2f {
        float4 vertex    : SV_POSITION;
        fixed4 color     : COLOR;
        float2 texcoord  : TEXCOORD0;
      };
      
      fixed4 _Color;

      v2f vert(appdata_t IN) {
        v2f OUT;

        OUT.vertex = UnityObjectToClipPos(IN.vertex);
        OUT.texcoord = IN.texcoord;
        OUT.color = IN.color * _Color;

        return OUT;
      }

      sampler2D _MainTex;
      sampler2D _AlphaTex;

      float _AlphaSplitEnabled;

      fixed4 SampleSpriteTexture (float2 uv) {
        fixed4 color = tex2D(_MainTex, uv);

#if UNITY_TEXTURE_ALPHASPLIT_ALLOWED
        if (_AlphaSplitEnabled)
          color.a = tex2D(_AlphaTex, uv).r;
#endif //UNITY_TEXTURE_ALPHASPLIT_ALLOWED

        return color;
      }

      int _PixelsOn, _PixelsOff;

      float positiveMod(float x, int n) {
        return (x % n + n) % n;
      }

      fixed4 frag(v2f IN) : SV_Target {
        fixed4 c = SampleSpriteTexture(IN.texcoord) * IN.color;

        float2 referencePoint = (mul(unity_CameraProjection, _WorldSpaceCameraPos) / 2);
        float offsetX = referencePoint.x * _ScreenParams.x;
        float offsetY = -1 * referencePoint.y * _ScreenParams.y;

        c.a *= 
          (positiveMod(offsetX + IN.vertex.x, _PixelsOn + _PixelsOff) < _PixelsOn) |
          (positiveMod(offsetY + IN.vertex.y, _PixelsOn + _PixelsOff) < _PixelsOn);

        c.rgb *= c.a;
        return c;
      }

      ENDCG
    }
  }
}
