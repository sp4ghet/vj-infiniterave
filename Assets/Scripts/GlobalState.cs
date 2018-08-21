using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalState : MonoBehaviour {

    static GlobalState _instance;

    [SerializeField]
    float _bpm;

    [SerializeField]
    Color baseColor;

    public static GlobalState Instance { get {return _instance;} }

    float _raymarchLerpValue = 0;

    public float RayMarchLerp {
        get {
            return _raymarchLerpValue;
        }

        set {
            _raymarchLerpValue = value;
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
