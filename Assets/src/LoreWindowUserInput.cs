using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoreWindowUserInput : MonoBehaviour {
  public LoreWindowController LoreWindow;

  public float AxisThreshold;
  public float PanSpeed;
  public float ZoomSpeed, MaxZoom, MinZoom;
  public float ClickZoomThreshold;
  public List<float> ClickZoomLevels;

  private bool IsMouseDown;
  private Vector2 PreviousMousePosition;
  private float TotalMouseMovement;

	void Update() {
    Vector2 panAxes = new Vector2(
      WrappedInput.GetAxis("Horizontal") + WrappedInput.GetAxis("Scroll X"),
      WrappedInput.GetAxis("Vertical") + WrappedInput.GetAxis("Scroll Y")
    );

    if (panAxes.magnitude > AxisThreshold) {
      LoreWindow.Pan(panAxes * PanSpeed * Time.deltaTime);
    }

    if (WrappedInput.GetButtonDown("Click")) {
      PreviousMousePosition = GetViewportMousePosition();
      IsMouseDown = true;
      TotalMouseMovement = 0;
    }

    if (IsMouseDown) {
      Vector2 mousePosition = GetViewportMousePosition();
      Vector2 delta = mousePosition - PreviousMousePosition;
      TotalMouseMovement += delta.magnitude;
      PreviousMousePosition = mousePosition;

      LoreWindow.ViewportPan(-delta);
    }

    if (WrappedInput.GetButtonUp("Click")) {
      IsMouseDown = false;

      if (TotalMouseMovement < ClickZoomThreshold) {
        ClickZoom();
      }
    }

    float zoomDirection = WrappedInput.GetAxis("Zoom");

    if (Mathf.Abs(zoomDirection) > AxisThreshold) {
      ChangeZoom(zoomDirection);
    }
	}

  void ClickZoom() {
    float zoom = LoreWindow.GetZoom();
    int nearestZoomLevelIndex = 0;
    float nearestZoomLevelDistance = float.PositiveInfinity;

    for (int i = 0; i < ClickZoomLevels.Count; i++) {
      float distance = Mathf.Abs(zoom - ClickZoomLevels[i]);

      if (distance < nearestZoomLevelDistance) {
        nearestZoomLevelIndex = i;
        nearestZoomLevelDistance = distance;
      }
    }

    int nextZoomLevelIndex = (nearestZoomLevelIndex + 1) % ClickZoomLevels.Count;
    float nextZoomLevel = ClickZoomLevels[nextZoomLevelIndex];

    LoreWindow.CenterOnContentPoint(GetContentMousePosition());
    LoreWindow.SetZoom(nextZoomLevel);
  }

  void ChangeZoom(float direction) {
    LoreWindow.SetZoom((scale) => Mathf.Clamp(
      scale * (1 + direction * ZoomSpeed * Time.deltaTime),
      MinZoom,
      MaxZoom
    ));
  }

  Vector2 GetViewportMousePosition()
    => LoreWindow.ScreenPointToViewportPoint(Input.mousePosition);

  Vector2 GetContentMousePosition()
    => LoreWindow.ScreenPointToContentPoint(Input.mousePosition);
}
