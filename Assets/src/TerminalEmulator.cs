using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TerminalEmulator : MonoBehaviour {
  public TMP_Text OutputText;
  public int Columns, Rows;
  public float RowHeight;

  private int CurrentColumn = 0;
  private int CurrentRow = 0;
  private string Buffer = "";

  void BufferWasUpdated() {
    OutputText.text = Buffer + "<color=#0f0>_";
  }

  void NewLine() {
    Buffer += '\n';
    BufferWasUpdated();

    CurrentColumn = 0;
    CurrentRow++;

    if (CurrentRow > Rows) {
      transform.localPosition += Vector3.up * RowHeight;
    }
  }

  public void PrintChar(char c) {
    if (c == '\n') {
      NewLine();
      return;
    } else if (CurrentColumn == Columns) {
      NewLine();
    }

    Buffer += c;
    BufferWasUpdated();
    CurrentColumn++;
  }

  public void Print(string text) {
    for (int i = 0; i < text.Length; i++) {
      PrintChar(text[i]);
    }
  }

  public void PrintLine(string text) {
    Print(text + "\n");
  }

  public void Clear() {
    Buffer = "";
    BufferWasUpdated();
    CurrentColumn = 0;
    CurrentRow = 0;
  }

  void Awake() {
    BufferWasUpdated();
  }
}
