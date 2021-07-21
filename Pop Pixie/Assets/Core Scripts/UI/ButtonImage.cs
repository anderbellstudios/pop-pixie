using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonImage : MonoBehaviour {

  public Image Image;
  public string ButtonName;

  private string Prefix = null;
  private LowPriorityBehaviour LowPriorityBehaviour;
 
  void Awake() {
    LowPriorityBehaviour = new LowPriorityBehaviour();
  }

  void Update() {
    LowPriorityBehaviour.EveryNFrames(10, () => {
      string _prefix = WrappedInput.ControllerPrefix() ?? "Kb+M";

      if ( _prefix != Prefix ) {
        Prefix = _prefix;
        UpdateImage();
      }
    });
  }

  void UpdateImage() {
    Image.sprite = Resources.Load<Sprite>( ImagePath() );
  }

  string ImagePath() {
    return System.IO.Path.Combine( "Button Icons", Prefix, ButtonName );
  }

}
