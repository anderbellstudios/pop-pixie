using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PredictiveAim : MonoBehaviour, IDirectionManager {
  public float ProjectileSpeed;

  public Vector3 Direction => ComputeDirection();

  Vector3 ComputeDirection() {
    Vector3 playerPosition = PlayerGameObject.Current.transform.position;
    Vector3 playerVelocity = PlayerGameObject.EstimatedVelocity;
    Vector3 toPlayer = playerPosition - transform.position;

    // Quadratic formula variables
    float a = playerVelocity.sqrMagnitude - ProjectileSpeed * ProjectileSpeed;
    float b = -2 * Vector3.Dot(playerVelocity, toPlayer);
    float c = toPlayer.sqrMagnitude;

    List<float> possibleTimes = a == 0f
      ? (b == 0f ? new List<float> {} : new List<float> { -c / b })
      : new List<float> {
        (-b + Mathf.Sqrt(b * b - 4 * a * c)) / (2 * a),
        (-b - Mathf.Sqrt(b * b - 4 * a * c)) / (2 * a)
      };

    List<float> nonNegativeTimes = possibleTimes.Where(t => t >= 0).ToList();

    if (nonNegativeTimes.Count == 0)
      return toPlayer.normalized;

    float bestTime = nonNegativeTimes.Min();

    return (playerPosition + playerVelocity * bestTime - transform.position).normalized;
  }
}
