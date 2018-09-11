using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalState : MonoBehaviour {

    static GlobalState _instance;

    [SerializeField]
    float _bpm;

    [SerializeField, ColorUsage(false, true)]
    Color baseColor;

    [SerializeField]
    bool isolineActive = true;

    [SerializeField]
    bool enableSkybox;

    [SerializeField]
    bool smoothFollow;

    [SerializeField]
    float raymarchLerpValue = 0;

    [SerializeField]
    float warp;

    [SerializeField]
    uint fishCount;

    [SerializeField]
    bool vedaEnabled;

    [SerializeField]
    bool twistEnabled;

    [SerializeField]
    int subdivisions;

    [SerializeField]
    RadialMesh.RadialState mode;

    [SerializeField]
    bool subdivisionsRandom;

    [SerializeField]
    bool bendTunnel;

    [SerializeField]
    bool waveStickEnabled;

    [SerializeField]
    float waveStickAmplifier;

    public static GlobalState I { get {return _instance;} }

    #region PublicProperties

    public float RayMarchLerp {
        get {
            return raymarchLerpValue;
        }

        set {
            raymarchLerpValue = value;
        }
    }
    public float Bpm {
        get {
            return _bpm;
        }

        set {
            _bpm = value;
        }
    }
    public Color BaseColor {
        get {
            return baseColor;
        }

        set {
            baseColor = value;
        }
    }
    public bool IsolineActive {
        get {
            return isolineActive;
        }

        set {
            isolineActive = value;
        }
    }
    public bool EnableSkybox {
        get {
            return enableSkybox;
        }

        set {
            enableSkybox = value;
        }
    }
    public bool SmoothFollow {
        get {
            return smoothFollow;
        }

        set {
            smoothFollow = value;
        }
    }
    public float Warp {
        get {
            return warp;
        }

        set {
            warp = value;
        }
    }
    public uint FishCount {
        get {
            return fishCount;
        }

        set {
            fishCount = value;
        }
    }

    public RadialMesh.RadialState RadialMeshMode {
        get {
            return mode;
        }

        set {
            mode = value;
        }
    }
    public bool TwistEnabled {
        get {
            return twistEnabled;
        }

        set {
            twistEnabled = value;
        }
    }
    public int Subdivisions {
        get {
            return subdivisions;
        }

        set {
            if(value < 3) {
                subdivisions = 3;
            }
            subdivisions = value;
        }
    }
    public bool SubdivisionsRandom {
        get {
            return subdivisionsRandom;
        }

        set {
            subdivisionsRandom = value;
        }
    }

    public bool VedaEnabled {
        get {
            return vedaEnabled;
        }

        set {
            vedaEnabled = value;
        }
    }

    public bool BendTunnel {
        get {
            return bendTunnel;
        }

        set {
            bendTunnel = value;
        }
    }

    public bool WaveStickEnabled {
        get {
            return waveStickEnabled;
        }

        set {
            waveStickEnabled = value;
        }
    }

    public float WaveStickAmplifier {
        get {
            return waveStickAmplifier;
        }

        set {
            waveStickAmplifier = value;
        }
    }

    #endregion

    private void OnEnable() {
        if (GlobalState.I == null) {
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
