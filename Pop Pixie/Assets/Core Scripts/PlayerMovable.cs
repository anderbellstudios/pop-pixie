using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovable : MonoBehaviour {

  public float speed;

  private Rigidbody2D rb;

  void Start() {
    rb = GetComponent<Rigidbody2D>();
  }

  void FixedUpdate() {
    float moveHorizontal = Input.GetAxis("Horizontal");
    float moveVertical = Input.GetAxis("Vertical");

    Vector2 movement = speed * new Vector2(moveHorizontal, moveVertical);

    rb.MovePosition(rb.position + movement * Time.fixedDeltaTime);
  }
}
