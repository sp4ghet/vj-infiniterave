using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using Klak.Motion;
using Kino.PostProcessing;

public class SceneController : MonoBehaviour {

	[SerializeField] GameObject rayMarchingObject;
	[SerializeField] GPUBoids fish;
	[SerializeField] SmoothFollow mainCamera;
	[SerializeField] PostProcessVolume ppVolume;
	[SerializeField] Camera sceneCamera;
	[SerializeField] bool enableSkybox = true;
	[SerializeField] GameObject water;

	PostProcessProfile profile;
	Isoline isoline = null;
	bool isolineActive = true;

	float raymarchElapsed;

	uint maxFish = 8192 - 256;
	bool fishActive = true;
	GameObject fishObj;

	float lastBpm = 0;



	public void EaseRaymarchObject(float power) {
		StartCoroutine(easeVelocity(GlobalState.Instance.RayMarchLerp, power, 0.1f));
	}

	IEnumerator easeVelocity(float from, float to, float duration) {
		raymarchElapsed = 0;
		do {
			raymarchElapsed += Time.deltaTime;
			GlobalState.Instance.RayMarchLerp = Mathf.Lerp(from, to, raymarchElapsed / duration);
			yield return new WaitForEndOfFrame();
		} while (raymarchElapsed <= duration);
	}

	public void FishCount(float rate) {
		fishActive = rate > float.Epsilon;
		fishObj.SetActive(fishActive);
		if (fishActive) {
			fish.MaxObjectNum = (uint)(maxFish * rate) + 256;
		}
	}

	public void JumpCamera() {
		mainCamera.JumpRandomly();
	}

	public void ToggleIsoline() {
		
		isolineActive = !isolineActive;
		isoline.active = isolineActive;
	}

	public void ToggleSkyBox() {
		enableSkybox = !enableSkybox;
		if (enableSkybox) {
			sceneCamera.clearFlags = CameraClearFlags.Skybox;
			water.SetActive(true);
		}
		else {
			sceneCamera.clearFlags = CameraClearFlags.SolidColor;
			sceneCamera.backgroundColor = Color.black;
			water.SetActive(false);
		}
	}

	public void SetBpm() {
		float sinceLastBpm = Time.time - lastBpm;
		if(sinceLastBpm > 2f) {
			lastBpm = Time.time;
			return;
		}

		GlobalState.Instance.Bpm = 60f / sinceLastBpm;

		lastBpm = Time.time;
	}

	// Use this for initialization
	void Start () {
		fishObj = fish.gameObject;

		profile = ppVolume.profile;
		profile.TryGetSettings(out isoline);
		isolineActive = isoline.active;
	}
	
	// Update is called once per frame
	void Update () {
	}
}
