using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TerminalEmulatorRunCommand : MonoBehaviour {
  public TerminalEmulator TerminalEmulator;
  public float DelayBetweenChars;
  public float DelayBeforeStart, DelayBeforeEnter;
  public bool PrintShellBefore = false;
  public bool PrintShellAfter = true;
  public string Command;
  [TextArea] public string CommandResult;
  public UnityEvent OnFinish;

  private int CurrentOffset = 0;

  public void Hop() {
    if (PrintShellBefore)
      PrintShell();
    CurrentOffset = 0;
    AsyncTimer.BaseTime.SetTimeout(PrintNextChar, DelayBeforeStart);
  }

  private void PrintNextChar() {
    if (CurrentOffset > Command.Length - 1) {
      FinishedTypingCommand();
      return;
    }

    AsyncTimer.BaseTime.SetTimeout(() => {
      TerminalEmulator.PrintChar(Command[CurrentOffset++]);
      PrintNextChar();
    }, DelayBetweenChars);
  }

  private void FinishedTypingCommand() {
    AsyncTimer.BaseTime.SetTimeout(() => {
      TerminalEmulator.PrintLine("");
      TerminalEmulator.Print(CommandResult);
      if (CommandResult.Length > 0)
        TerminalEmulator.PrintLine("");
      Finished();
    }, DelayBeforeEnter);
  }

  private void Finished() {
    if (PrintShellAfter)
      PrintShell();
    OnFinish.Invoke();
  }

  private void PrintShell() {
    TerminalEmulator.Print("amanda@localhost:~$ ");
  }
}
