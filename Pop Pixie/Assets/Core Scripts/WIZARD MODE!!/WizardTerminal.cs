using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class WizardTerminal : MonoBehaviour {

  public TMP_InputField InputField;
  public TextMeshProUGUI OutputArea;

  void Update() {
    if ( Input.GetKeyDown("return") ) {
      ProcessInput();
    }
  }

  void ProcessInput() {
    string cmd = InputField.text;
    InputField.text = "";
    ProcessCommand(cmd);
  }

  void ProcessCommand( string cmd ) {
    if ( cmd == "" || cmd == " " )
      return;

    OutputArea.text += "> " + cmd + "\n";

    string firstWord = Regex.Match(cmd, @"^([\w\-]+)").Value;

    IWizardTerminalCommand commandManager = null;

    switch ( firstWord ) {
      case "help":
        commandManager = new WizHelp();
        break;

      case "skip":
        commandManager = new WizSkip();
        break;

      default:
        commandManager = new WizUnknownCommand();
        break;
    }

    string result = commandManager.Main( cmd.Split(' ') );
    OutputArea.text += result;
  }

  public void EnsureInputSelected() {
    InputField.Select();
    InputField.OnSelect(null);
    InputField.ActivateInputField();
  }
}

interface IWizardTerminalCommand {
  string Main( string[] args );
}

class WizUnknownCommand : IWizardTerminalCommand {
  public string Main( string[] args ) {
    return "Unknown command\n";
  }
}

class WizHelp : IWizardTerminalCommand {
  public string Main( string[] args ) {
    return "- help\n- skip <scene name>\n";
  }
}

class WizSkip : IWizardTerminalCommand {
  public string Main( string[] args ) {
    if ( args.Length < 2 )
      return "Usage: skip <scene name>\n";

    string sceneName = string.Join( " ", args.Skip(1) );
    SceneManager.LoadScene(sceneName);

    return "";
  }
}
