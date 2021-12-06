Shader "Custom/HologramWallsShader" {
  Properties {
    _MainTex ("Sprite Texture", 2D) = "white" {}
    _Color ("Tint", Color) = (1,1,1,1)
    _UnitsWidth ("Units Width", Float) = 1
    _UnitsOn ("Units On", Float) = 2
    _UnitsOff ("Units Off", Float) = 2
    _MinHeight ("Min Height", Range(0, 1)) = 0.0
    _Speed ("Speed", Float) = 30.0
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

      // https://github.com/still-scene/Resources/blob/master/hash-functions.hlsl
      float hash(float p) {
        p = (p * .1031) % 1;
        p *= p + 33.33;
        p *= p + p;
        return p % 1;
      }

      float _UnitsWidth, _UnitsOn, _UnitsOff, _MinHeight, _Speed;

      fixed4 frag(v2f IN) : SV_Target {
        fixed4 c = SampleSpriteTexture(IN.texcoord) * IN.color;

        float scaled_x = IN.texcoord.x * _UnitsWidth;
        float bar_interval = _UnitsOn + _UnitsOff;

        // Vertical bars
        c.a *= scaled_x % bar_interval < _UnitsOn;

        // Oscillating gradients
        int bar_index = floor(scaled_x / bar_interval);
        float relative_height = ((1 + sin(_Speed * _Time + 180 * hash(bar_index))) / 2);
        float height = _MinHeight + (1 - _MinHeight) * relative_height;
        c.a *= height == 0 ? 0 : max(0, (1 - IN.texcoord.y * (1 / height)));

        c.rgb *= c.a;

        return c;
      }

      ENDCG
    }
  }
}
