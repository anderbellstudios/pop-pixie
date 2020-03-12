using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Flash : MonoBehaviour {

  public float Duration; 
  public Renderer Target;
  public bool DefaultEnabled = true;

  public void BeginFlashing() {
    if ( Duration > 0 ) StartCoroutine( Coroutine() );
  }

  private IEnumerator Coroutine() {
    int flashes = (int)( Duration / 0.2f );

    for(var n = 0; n < flashes; n++) {
      Target.enabled = true;
      yield return new WaitForSeconds(0.1f);
      Target.enabled = false;
      yield return new WaitForSeconds(0.1f);
    }

    Target.enabled = DefaultEnabled;
  }

}
