using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TerminalEmulatorPrintLineByLine : MonoBehaviour {
  public TerminalEmulator TerminalEmulator;
  public float DelayBetweenLines = 0.1f;
  [TextArea] public string Content;
  public UnityEvent OnFinish;

  private List<string> Lines;
  private int CurrentLine = 0;

  public void Hop() {
    Lines = new List<string>(Content.Split('\n'));
    CurrentLine = 0;
    PrintNextLine();
  }

  void PrintNextLine() {
    if (CurrentLine >= Lines.Count) {
      OnFinish.Invoke();
      return;
    }

    AsyncTimer.BaseTime.SetTimeout(() => {
      TerminalEmulator.PrintLine(Lines[CurrentLine++]);
      PrintNextLine();
    }, DelayBetweenLines);
  }
}
