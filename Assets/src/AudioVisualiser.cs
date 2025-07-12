using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class AudioVisualiser : MonoBehaviour {
  public Transform BarContainer;
  public PlaySound PlaySound;
  public float Sensitivity = 1;
  public AnimationCurve EQ = AnimationCurve.Constant(0, 63, 1);
  public float DecayRate = 30;

  private FMOD.DSP FFT;
  private FMOD.ChannelGroup ChannelGroup;
  private RectTransform[] BarTransforms = new RectTransform[64];
  private float[] Spectrum = new float[64];
  private float[] SmoothedSamples = new float[64];

  void Awake() {
    int barCount = BarContainer.childCount;

    if (Debug.isDebugBuild) {
      if (barCount != 64) {
        throw new Exception("AudioVisualiser: BarContainer must have 64 children");
      }
    }

    for (int i = 0; i < barCount; i++) {
      BarTransforms[i] = BarContainer.GetChild(i).GetComponent<RectTransform>();
    }

    PlaySound.OnPlay.AddListener(AttachDSP);
  }

  void AttachDSP() {
    FMOD.RESULT createDSPResult = FMODUnity.RuntimeManager.CoreSystem.createDSPByType(
      FMOD.DSP_TYPE.FFT,
      out FFT
    );
    if (createDSPResult != FMOD.RESULT.OK) {
      Debug.LogError("Failed to create FFT DSP");
      return;
    }

    FFT.setParameterInt((int)FMOD.DSP_FFT.WINDOWTYPE, (int)FMOD.DSP_FFT_WINDOW.RECT);
    FFT.setParameterInt((int)FMOD.DSP_FFT.WINDOWSIZE, 64);
    FMODUnity.RuntimeManager.StudioSystem.flushCommands();

    FMOD.RESULT getChannelGroupResult = PlaySound.EventInstance.getChannelGroup(out ChannelGroup);
    if (getChannelGroupResult != FMOD.RESULT.OK) {
      Debug.LogError("Failed to get channel group from bus");
      return;
    }

    FMOD.RESULT addDSPResult = ChannelGroup.addDSP(FMOD.CHANNELCONTROL_DSP_INDEX.HEAD, FFT);
    if (addDSPResult != FMOD.RESULT.OK) {
      Debug.LogError("Failed to add DSP to channel group");
    }
  }

  void OnDestroy() {
    if (ChannelGroup.hasHandle() && FFT.hasHandle()) {
      ChannelGroup.removeDSP(FFT);
    }
  }

  void Update() {
    if (FFT.hasHandle()) {
      IntPtr spectrumDataPointer;

      FMOD.RESULT getSpectrumDataResult = FFT.getParameterData(
        (int)FMOD.DSP_FFT.SPECTRUMDATA,
        out spectrumDataPointer,
        out _
      );
      if (getSpectrumDataResult != FMOD.RESULT.OK) {
        Debug.LogError("Failed to get spectrum data from FFT");
      }

      FMOD.DSP_PARAMETER_FFT spectrumData = (FMOD.DSP_PARAMETER_FFT)
        Marshal.PtrToStructure(spectrumDataPointer, typeof(FMOD.DSP_PARAMETER_FFT));
      spectrumData.getSpectrum(0, ref Spectrum);
    }

    for (int i = 0; i < 64; i++) {
      float instantaneous = Mathf.Clamp(
        Mathf.Log(Spectrum[i] + 1) * EQ.Evaluate(i) * Sensitivity,
        0.01f,
        1f
      );

      float smoothed = SmoothedSamples[i] = Math.Max(
        instantaneous,
        Mathf.Lerp(SmoothedSamples[i], instantaneous, Time.deltaTime * DecayRate)
      );

      BarTransforms[i].localScale = new Vector3(1, smoothed, 1);
    }
  }
}
