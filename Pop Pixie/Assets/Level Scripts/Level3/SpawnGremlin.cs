using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnGremlin : MonoBehaviour {

  public GameObject Prefab;

  public void Spawn() {
    var gremlin = Instantiate( Prefab );
    gremlin.transform.position = transform.position;
    gremlin.GetComponent<EnemyAI>().Engaged = true;
  }

}
