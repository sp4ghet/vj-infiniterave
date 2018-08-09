using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalState : MonoBehaviour {

	static GlobalState _instance;
	
	public static GlobalState Instance { get {return _instance;} }

	public Renderer _raymarch;

	string raymarchLerpName = "_Hit";

	public float RayMarchLerp {
		get {
			return _raymarch.material.GetFloat(raymarchLerpName);
		}

		set {
			_raymarch.material.SetFloat(raymarchLerpName, value);
		}
	}

	private void OnEnable() {
		if (GlobalState.Instance == null) {
			GlobalState._instance = this;
		}
	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
