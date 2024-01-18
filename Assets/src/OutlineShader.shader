Shader "Custom/OutlineShader" {
  Properties {
    _MainTex ("Sprite Texture", 2D) = "white" {}
    _Color ("Tint", Color) = (1,1,1,1)
    _Rect ("Rect", Vector) = (0,0,1,1)
    [Toggle] _Enabled ("Enabled", Float) = 1
    _OutlineColor ("Outline Color", Color) = (1,1,0,1)
    _OutlineSize ("Outline Size", Float) = 0.01
    _OpacityProgress ("Opacity Progress", Float) = 1
    _OpacityMin ("Min Opacity", Float) = 0
    _OpacityMax ("Max Opacity", Float) = 1
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

      fixed4 _Rect;
      float _Enabled;
      fixed4 _OutlineColor;
      float _OpacityProgress;
      float _OutlineSize;
      float _OpacityMin;
      float _OpacityMax;

      float _AlphaSplitEnabled;

      fixed4 SampleSpriteTexture (float2 uv) {
        if (
          _Enabled && (
            uv.x < _Rect.x ||
            uv.x > _Rect.z ||
            uv.y < _Rect.y ||
            uv.y > _Rect.w
          )
        ) return fixed4(0, 0, 0, 0);

        fixed4 color = tex2D(_MainTex, uv);

#if UNITY_TEXTURE_ALPHASPLIT_ALLOWED
        if (_AlphaSplitEnabled)
          color.a = tex2D(_AlphaTex, uv).r;
#endif //UNITY_TEXTURE_ALPHASPLIT_ALLOWED

        return color;
      }

      fixed4 frag(v2f IN) : SV_Target {
        fixed4 c = SampleSpriteTexture(IN.texcoord) * IN.color;
        if (_Enabled && c.a != 0) {
          if (
            SampleSpriteTexture(IN.texcoord + float2(0, _OutlineSize)).a == 0 ||
            SampleSpriteTexture(IN.texcoord + float2(0, -_OutlineSize)).a == 0 ||
            SampleSpriteTexture(IN.texcoord + float2(_OutlineSize, 0)).a == 0 ||
            SampleSpriteTexture(IN.texcoord + float2(-_OutlineSize, 0)).a == 0
          ) return lerp(c, _OutlineColor, lerp(_OpacityMin, _OpacityMax, _OpacityProgress));
        }
        return c;
      }

      ENDCG
    }
  }
}
