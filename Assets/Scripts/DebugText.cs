using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DebugText : MonoBehaviour {

    [SerializeField]
    TextMeshProUGUI bpmText;
    [SerializeField]
    TextMeshProUGUI colorText;
    [SerializeField]
    TextMeshProUGUI cameraText;
    [SerializeField]
    TextMeshProUGUI isolineText;
    [SerializeField]
    TextMeshProUGUI boidsText;
    [SerializeField]
    TextMeshProUGUI raymarchText;
    [SerializeField]
    TextMeshProUGUI warpText;
    [SerializeField]
    TextMeshProUGUI vedaText;
    [SerializeField]
    TextMeshProUGUI twistText;
    [SerializeField]
    TextMeshProUGUI radialMeshText;
    [SerializeField]
    TextMeshProUGUI subdivisionsText;
    [SerializeField]
    TextMeshProUGUI subRandomText;


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        bpmText.text = $"bpm: {GlobalState.I.Bpm}";
        colorText.color = GlobalState.I.BaseColor + Color.black;
        colorText.text = $"Base color: {GlobalState.I.BaseColor}";
        cameraText.text = $"Camera: {(GlobalState.I.SmoothFollow ? "move" : "static")}";
        isolineText.text = $"Isoline: {(GlobalState.I.IsolineActive ? "enabled" : "disabled")}";
        boidsText.text = $"Boids: {GlobalState.I.FishCount}";
        raymarchText.text = $"Raymarch: {GlobalState.I.RayMarchLerp}";
        warpText.text = $"Warp: {GlobalState.I.Warp}";
        vedaText.text = $"Veda: {GlobalState.I.VedaEnabled}";
        twistText.text = $"Twist: {GlobalState.I.TwistEnabled}";
        radialMeshText.text = $"Radial Mesh: {GlobalState.I.RadialMeshMode}";
        subdivisionsText.text = $"Subdivisions: {GlobalState.I.Subdivisions}";
        subRandomText.text = $"Sub Random: {GlobalState.I.SubdivisionsRandom}";
	}
}
