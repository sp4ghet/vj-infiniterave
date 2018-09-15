using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using Klak.Motion;
using Kino.PostProcessing;
using System;

public class SceneController : MonoBehaviour {

    [SerializeField] GameObject rayMarchingObject;
    [SerializeField] GameObject worldSpaceMarch;
    [SerializeField] GPUBoids fish;
    [SerializeField] SmoothFollow smoothFollow;
    [SerializeField] PostProcessVolume ppVolume;
    [SerializeField] Camera sceneCamera;
    [SerializeField] RadialMeshGenerator radialGenerator;
    [SerializeField] GameObject veda;
    [SerializeField] WaveStick waveStick;
    [SerializeField] GameObject particles;

    PostProcessProfile profile;
    Isoline isoline = null;
    Warp warp = null;
    Recolor recolor;

    float raymarchElapsed;

    uint maxFish = 8192 - 256;
    bool fishActive = true;
    GameObject fishObj;

    float lastBpm = 0;
    int emissionId;

    public void EaseRaymarchObject(float power) {
        StartCoroutine(easeVelocity(GlobalState.I.RayMarchLerp, power, 0.1f));
    }

    IEnumerator easeVelocity(float from, float to, float duration) {
        raymarchElapsed = 0;
        do {
            raymarchElapsed += Time.deltaTime;
            GlobalState.I.RayMarchLerp = Mathf.Lerp(from, to, raymarchElapsed / duration);
            yield return new WaitForEndOfFrame();
        } while (raymarchElapsed <= duration);
    }

    public void ToggleRadialMesh() {
        GlobalState.I.RadialMeshMode = GlobalState.I.RadialMeshMode.Next();
    }

    public void FishCount(float rate) {
        fishActive = rate > float.Epsilon;
        fishObj.SetActive(fishActive);
        if (fishActive) {
            fish.MaxObjectNum = (uint)(maxFish * rate) + 256;
            GlobalState.I.FishCount = (uint)(maxFish * rate) + 256;
        }
    }

    public void ToggleRandomSubdivisions() {
        GlobalState.I.SubdivisionsRandom = !GlobalState.I.SubdivisionsRandom;
    }

    public void ToggleTwist() {
        GlobalState.I.TwistEnabled = !GlobalState.I.TwistEnabled;
    }

    public void ToggleBend() {
        GlobalState.I.BendTunnel = !GlobalState.I.BendTunnel;
    }

    public void JumpCamera() {
        smoothFollow.JumpRandomly();
    }

    public void ToggleCameraMove() {
        GlobalState.I.SmoothFollow = !GlobalState.I.SmoothFollow;
        smoothFollow.enabled = GlobalState.I.SmoothFollow;
        if (!smoothFollow.enabled) {
            smoothFollow.transform.position = Vector3.zero;
            smoothFollow.transform.rotation = Quaternion.identity;
        }
    }

    internal void ToggleParticles() {
        GlobalState.I.ParticlesEnabled = !GlobalState.I.ParticlesEnabled;
        particles.SetActive(GlobalState.I.ParticlesEnabled);
    }

    public void NewColors() {
        GlobalState.I.Colors = GradientGenerator.RandomGradient();
        recolor.fillGradient.value = GlobalState.I.Colors;
        GlobalState.I.BaseColor = (Vector4)UnityEngine.Random.ColorHSV(0,1, 1, 1, 1, 1, 1, 1) * 2.5f;
        waveStick.GetComponent<Renderer>().material.SetColor(emissionId, GlobalState.I.BaseColor);

    internal void ToggleObjectMarch() {
        GlobalState.I.ObjectSpaceEnabled = !GlobalState.I.ObjectSpaceEnabled;
        rayMarchingObject.SetActive(GlobalState.I.ObjectSpaceEnabled);
    }

    internal void ToggleWorldMarch() {
        GlobalState.I.WorldSpaceEnabled = !GlobalState.I.WorldSpaceEnabled;
        worldSpaceMarch.SetActive(GlobalState.I.WorldSpaceEnabled);

    }

    public void SetSubdivisions(float knobValue) {
        uint maxSubs = 50;
        GlobalState.I.Subdivisions = (int)(maxSubs * knobValue) + 3;
    }

    public void ToggleIsoline() {
        GlobalState.I.IsolineActive = !GlobalState.I.IsolineActive;
        isoline.active = GlobalState.I.IsolineActive;
        isoline.lineColor.value = GlobalState.I.BaseColor;
    }

    public void SetWarp(float warpValue) {
        warpValue = Mathf.Clamp01(warpValue);
        GlobalState.I.Warp = warpValue;
    }

    public void ToggleSkyBox() {
        GlobalState.I.EnableSkybox = !GlobalState.I.EnableSkybox;
        if (GlobalState.I.EnableSkybox) {
            sceneCamera.clearFlags = CameraClearFlags.Skybox;
        }
        else {
            sceneCamera.clearFlags = CameraClearFlags.SolidColor;
            sceneCamera.backgroundColor = Color.black;
        }
    }

    public void SetMeshThreshold(float knobValue) {
        float maxThresh = 50;
        GlobalState.I.MeshThreshold = Mathf.Lerp(25, maxThresh, knobValue);
    }

    public void SetRecolorOpacity(float knobValue) {
        GlobalState.I.RecolorOpacity = knobValue;
        recolor.fillOpacity.value = GlobalState.I.RecolorOpacity;
    }

    public void SetBpm() {
        float sinceLastBpm = Time.time - lastBpm;
        if (sinceLastBpm > 2f) {
            lastBpm = Time.time;
            return;
        }

        GlobalState.I.Bpm = 60f / sinceLastBpm;

        lastBpm = Time.time;
    }

    public void ToggleVeda() {
        GlobalState.I.VedaEnabled = !GlobalState.I.VedaEnabled;
        veda.SetActive(GlobalState.I.VedaEnabled);
    }

    public void ToggleWaveStick() {
        GlobalState.I.WaveStickEnabled = !GlobalState.I.WaveStickEnabled;
        waveStick.gameObject.SetActive(GlobalState.I.WaveStickEnabled);
    }

    public void SetWaveStickAmplifier(float rate) {
        rate = Mathf.Clamp01(rate);
        const float maxAmp = 2000;
        GlobalState.I.WaveStickAmplifier = maxAmp * rate;
    }

    // Use this for initialization
    void Start() {
        fishObj = fish.gameObject;

        profile = ppVolume.profile;
        profile.TryGetSettings(out isoline);
        isoline.active = GlobalState.I.IsolineActive;
        profile.TryGetSettings(out warp);
        profile.TryGetSettings(out recolor);

        ToggleSkyBox();ToggleSkyBox();
        veda.SetActive(GlobalState.I.VedaEnabled);
        waveStick.gameObject.SetActive(GlobalState.I.WaveStickEnabled);
        
        emissionId = Shader.PropertyToID("_EmissionColor");
        particles.SetActive(GlobalState.I.ParticlesEnabled);
        rayMarchingObject.SetActive(GlobalState.I.ObjectSpaceEnabled);
        worldSpaceMarch.SetActive(GlobalState.I.WorldSpaceEnabled);
    }
}
