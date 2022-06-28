using System;
using System.Text.RegularExpressions;

public class DialoguePreprocessor {
  const char DelayChar = '\u200B';

  float CharactersPerSecond;

  public DialoguePreprocessor(float charactersPerSecond) {
    CharactersPerSecond = charactersPerSecond;
  }

  public string Preprocess(string input) {
    int slowAmount = 0;

    string output = "";

    foreach (Match match in Regex.Matches(input, @"<(?<command>delay|slow)=(?<amount>[0-9]+)>|<.+?>|.+?", RegexOptions.Singleline)) {
      Group commandGroup = match.Groups["command"];

      if (commandGroup.Success) {
        int amount = int.Parse(match.Groups["amount"].Value);

        switch (commandGroup.Value) {
          case "delay":
            output += new string(DelayChar, (int) (CharactersPerSecond * (amount / 1000f)));
            break;

          case "slow":
            slowAmount = amount;
            break;
        }
      } else if (match.Value[0] == '<') {
        output += match.Value;
      } else {
        output += match.Value + new string(DelayChar, slowAmount);
      }
    }

    return output;
  }
}
