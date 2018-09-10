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
        ,toggleCameraMove = 50
        ,toggleRadialMesh = 36
        ,toggleTwist = 37
        ,toggleRandomSubdivisions = 38
        ,toggleVeda = 39
	}

	enum MidiKnobs : int {
		fishKnob = 72
        ,warpKnob = 17
        ,subdivisionsKnob = 79
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
        case MidiNotes.toggleCameraMove:
            sceneController.ToggleCameraMove();
            break;
        case MidiNotes.toggleRadialMesh:
            sceneController.ToggleRadialMesh();
            break;
        case MidiNotes.toggleTwist:
            sceneController.ToggleTwist();
            break;
        case MidiNotes.toggleRandomSubdivisions:
            sceneController.ToggleRandomSubdivisions();
            break;
        case MidiNotes.toggleVeda:
            sceneController.ToggleVeda();
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
        case MidiKnobs.warpKnob:
            sceneController.SetWarp(knobValue);
            break;
        case MidiKnobs.subdivisionsKnob:
            sceneController.SetSubdivisions(knobValue);
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
