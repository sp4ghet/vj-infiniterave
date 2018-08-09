using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MidiJack;

public class SceneController : MonoBehaviour {

	[SerializeField] GameObject rayMarchingObject;
	float _raymarchElapsed;

	void NoteOn(MidiChannel channel, int note, float velocity) {
		StartCoroutine(easeVelocity(GlobalState.Instance.RayMarchLerp, velocity, 0.1f));
	}

	void NoteOff(MidiChannel channel, int note) {
		StartCoroutine(easeVelocity(GlobalState.Instance.RayMarchLerp, 0f, 0.1f));
	}

	IEnumerator easeVelocity(float from, float to, float duration) {
		_raymarchElapsed = 0;
		do {
			_raymarchElapsed += Time.deltaTime;
			GlobalState.Instance.RayMarchLerp = Mathf.Lerp(from, to, _raymarchElapsed / duration);
			yield return new WaitForEndOfFrame();
		} while (_raymarchElapsed <= duration);
	}

	void OnEnable() {
		MidiMaster.noteOnDelegate += NoteOn;
		MidiMaster.noteOffDelegate += NoteOff;
	}

	void OnDisable() {
		MidiMaster.noteOnDelegate -= NoteOn;
		MidiMaster.noteOffDelegate -= NoteOff;
	}

	// Use this for initialization
	void Start () {
		GlobalState.Instance._raymarch = rayMarchingObject.GetComponent<Renderer>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
