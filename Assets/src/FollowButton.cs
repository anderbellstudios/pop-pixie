using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FollowButton : AInspectable {

  public SpriteRenderer SpriteRenderer;
  public Sprite NormalSprite;
  public Sprite HoverSprite;

  public override void OnInspect() {
    Application.OpenURL("https://twitter.com/AnderbellStds");
  }

  public override void OnPlayerOver() {
    SpriteRenderer.sprite = HoverSprite;
  }

  public override void OnPlayerOut() {
    SpriteRenderer.sprite = NormalSprite;
  }

}
