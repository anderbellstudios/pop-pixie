using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AssistMenuEvents : AMenu {
  public StepperInput DamageReductionStepper;

  public override void LocalStart() {
    DamageReductionStepper.Options = Enumerable.Range(0, 11).Select(n =>
      String.Format("{0}%", n * 10)
    ).ToList();

    UpdateSteppers();

    DamageReductionStepper.OnChange.AddListener(DamageReductionChanged);
  }

  public void DamageReductionChanged(int index, string label) {
    AssistModeData.DamageReduction = ((decimal)index) * 0.1M;
  }

  public void ResetToDefaults() {
    AssistModeData.ResetToDefaults();
    UpdateSteppers();
  }

  void UpdateSteppers() {
    DamageReductionStepper.Value = (int)(AssistModeData.DamageReduction * 10);
    DamageReductionStepper.UpdateLabel();
  }
}
