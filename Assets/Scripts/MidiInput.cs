using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MidiJack;

public class MidiInput : MonoBehaviour {

	[SerializeField] SceneController sceneController;

	enum MidiNotes : int {
		rayMarchNote = 44
		,randomJump = 45
		,isolineToggle = 46
		,skyboxToggle = 47
		,bpmTap = 51
	}

	enum MidiKnobs : int {
		fishKnob = 72
	}

	void NoteOn(MidiChannel channel, int note, float velocity) {
		MidiNotes _note = (MidiNotes)note;
		switch (_note) {
		case MidiNotes.rayMarchNote:
			sceneController.EaseRaymarchObject(velocity);
			break;
		case MidiNotes.randomJump:
			sceneController.JumpCamera();
			break;
		case MidiNotes.isolineToggle:
			sceneController.ToggleIsoline();
			break;
		case MidiNotes.skyboxToggle:
			sceneController.ToggleSkyBox();
			break;
		case MidiNotes.bpmTap:
			sceneController.SetBpm();
			break;
		}
	}

	void NoteOff(MidiChannel channel, int note) {
		MidiNotes _note = (MidiNotes)note;
		switch (_note) {
		case MidiNotes.rayMarchNote:
			sceneController.EaseRaymarchObject(0);
			break;
		}
	}

	void Knob(MidiChannel channel, int knobNumber, float knobValue) {
		MidiKnobs knob = (MidiKnobs)knobNumber;
		switch (knob) {
		case MidiKnobs.fishKnob:
			sceneController.FishCount(knobValue);
			break;
		}
	}

	void OnEnable() {
		MidiMaster.noteOnDelegate += NoteOn;
		MidiMaster.noteOffDelegate += NoteOff;
		MidiMaster.knobDelegate += Knob;
	}

	void OnDisable() {
		MidiMaster.noteOnDelegate -= NoteOn;
		MidiMaster.noteOffDelegate -= NoteOff;
		MidiMaster.knobDelegate -= Knob;
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
