using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AudioReactive : MonoBehaviour {

	static AudioReactive instance;

	public static AudioReactive Instance { get { return instance; } }

	Lasp.FilterType highPass = Lasp.FilterType.HighPass;
	Lasp.FilterType bandPass = Lasp.FilterType.BandPass;
	Lasp.FilterType lowPass  = Lasp.FilterType.LowPass;
	Lasp.FilterType byPass   = Lasp.FilterType.Bypass;

	const float kSilence = -40; // -40 dBFS = silence

	float[] _highWave, _bandWave, _lowWave, _byWave;
	const int waveSize = 512;

	float peakHigh, peakBand, peakLow, peakBy;
	float rmsHigh, rmsBand, rmsLow, rmsBy;

	#region properties

	public static int WaveSize => waveSize;
	public static float KSilence => kSilence;

	public float PeakHigh {
		get {
			return peakHigh;
		}
	}
	public float PeakBand {
		get {
			return peakBand;
		}
	}
	public float PeakLow {
		get {
			return peakLow;
		}
	}
	public float PeakBy {
		get {
			return peakBy;
		}
	}

	public float RmsHigh {
		get {
			return rmsHigh;
		}
	}
	public float RmsBand {
		get {
			return rmsBand;
		}
	}
	public float RmsLow {
		get {
			return rmsLow;
		}
	}
	public float RmsBy {
		get {
			return rmsBy;
		}
	}

	#endregion

	#region MonoBehaviour

	private void OnEnable() {
		if (AudioReactive.Instance == null) {
			AudioReactive.instance = this;
		}
	}

	// Use this for initialization
	void Start () {
		_highWave = new float[WaveSize];
		_bandWave = new float[WaveSize];
		_lowWave = new float[WaveSize];
		_byWave = new float[WaveSize];
	}
	
	// Update is called once per frame
	void Update () {
		peakHigh = Lasp.AudioInput.GetPeakLevelDecibel(highPass) - kSilence;
		peakBand = Lasp.AudioInput.GetPeakLevelDecibel(bandPass) - kSilence;
		peakLow = Lasp.AudioInput.GetPeakLevelDecibel(lowPass) - kSilence;
		peakBy = Lasp.AudioInput.GetPeakLevelDecibel(byPass) - kSilence;

		rmsHigh = Lasp.AudioInput.CalculateRMSDecibel(highPass) - kSilence;
		rmsBand = Lasp.AudioInput.CalculateRMSDecibel(bandPass) - kSilence;
		rmsLow = Lasp.AudioInput.CalculateRMSDecibel(lowPass) - kSilence;
		rmsBy = Lasp.AudioInput.CalculateRMSDecibel(byPass) - kSilence;

		Lasp.AudioInput.RetrieveWaveform(highPass, _highWave);
		Lasp.AudioInput.RetrieveWaveform(bandPass, _bandWave);
		Lasp.AudioInput.RetrieveWaveform(lowPass, _lowWave);
		Lasp.AudioInput.RetrieveWaveform(byPass, _byWave);

		_highWave = _highWave.Select(x => x - kSilence).ToArray();
		_bandWave = _bandWave.Select(x => x - kSilence).ToArray();
		_lowWave = _lowWave.Select(x => x - kSilence).ToArray();
		_byWave = _byWave.Select(x => x - kSilence).ToArray();
	}
	#endregion
}
