﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer), typeof(MeshFilter))]
public class WaveStick : MonoBehaviour {

    [SerializeField]
    float amplifier = 2000;

    int sampleCount = 1024;
    int sticks = 56;
    float speed = 5;


    AudioSource audioSource;
    Mesh mesh;
    float[] samples;
    Vector3[] vertices;


    void UpdateMesh(float[] samples) {
        UpdateVertices(samples);
        UpdateTriangles();


        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
    }

    void UpdateVertices(float[] samples) {
        for (var i = 0; i+8 < vertices.Length; i += 8) {
            float t = (float)i/vertices.Length;
            
            int index = Mathf.RoundToInt(Easing.EaseInExpo(0, samples.Length, 0.99f-t));
            float sample = samples[index]*GlobalState.I.WaveStickAmplifier;
            Vector3 top = Vector3.up * sample/2;
            Vector3 bot = Vector3.down * sample/2;

            Vector3 a = new Vector3(-0.5f, 0, 0.5f);
            Vector3 b = new Vector3(-0.5f, 0, -0.5f);
            Vector3 c = new Vector3(0.5f, 0, -0.5f);
            Vector3 d = new Vector3(0.5f, 0, 0.5f);

            vertices[i + 0] = Spiral(t, top + a);
            vertices[i + 1] = Spiral(t, top + b);
            vertices[i + 2] = Spiral(t, top + c);
            vertices[i + 3] = Spiral(t, top + d);

            vertices[i + 4] = Spiral(t, bot + a);
            vertices[i + 5] = Spiral(t, bot + b);
            vertices[i + 6] = Spiral(t, bot + c);
            vertices[i + 7] = Spiral(t, bot + d);
        }
    }

    void UpdateTriangles() {
        var tris = new int[36 * sticks];

        for (var i = 0; i < sticks; i++) {

            int t = i*36;
            int v = i*8;

            // caps
            tris[t + 0] = v + 0;
            tris[t + 1] = v + 2;
            tris[t + 2] = v + 1;

            tris[t + 3] = v + 0;
            tris[t + 4] = v + 3;
            tris[t + 5] = v + 2;

            tris[t + 6] = v + 4;
            tris[t + 7] = v + 5;
            tris[t + 8] = v + 6;

            tris[t + 9] = v + 4;
            tris[t + 10] = v + 6;
            tris[t + 11] = v + 7;

            // tube
            tris[t + 12] = v + 0;
            tris[t + 13] = v + 4;
            tris[t + 14] = v + 3;

            tris[t + 15] = v + 3;
            tris[t + 16] = v + 4;
            tris[t + 17] = v + 7;

            tris[t + 18] = v + 3;
            tris[t + 19] = v + 7;
            tris[t + 20] = v + 2;

            tris[t + 21] = v + 2;
            tris[t + 22] = v + 7;
            tris[t + 23] = v + 6;

            tris[t + 24] = v + 2;
            tris[t + 25] = v + 6;
            tris[t + 26] = v + 1;

            tris[t + 27] = v + 1;
            tris[t + 28] = v + 6;
            tris[t + 29] = v + 5;

            tris[t + 30] = v + 1;
            tris[t + 31] = v + 5;
            tris[t + 32] = v + 4;

            tris[t + 33] = v + 1;
            tris[t + 34] = v + 4;
            tris[t + 35] = v + 0;
        }

        mesh.MarkDynamic();
        mesh.vertices = vertices;
        mesh.triangles = (tris);
    }

    // point along a spiral based on how far along it is
    Vector3 Spiral(float t, Vector3 offset, float phase=0) {
        t += Time.time / 20f ;
        t = t % 1;
        float radius = 10;
        float turns = 3*2*Mathf.PI;
        float distance = -40;

        float degRot = Mathf.Rad2Deg * turns * t;
        Vector3 p = Quaternion.Euler(0, 0, degRot) * offset;

        float x = radius * Mathf.Cos(turns * t + phase);
        float y = radius * Mathf.Sin(turns * t + phase);
        float z = distance * t;

        if (GlobalState.I.BendTunnel) {
            const float circle = (Mathf.PI / 2f);
            const float rotate = (Mathf.PI / 2.4f);
            y += Mathf.Sin(circle*t + rotate) * 50f - 50f;
            z = Mathf.Cos(circle*t + rotate) * 50f - 5f; 
            return p + new Vector3(x, y, z);
        }
        else {
            return p + new Vector3(x, y, z);
        }
    }

    // Use this for initialization
    void Start () {
        mesh = new Mesh();
        samples = new float[sampleCount];
        vertices = new Vector3[8 * sticks];


        UpdateMesh(samples);
        GetComponent<MeshFilter>().mesh = mesh;

        audioSource = GetComponent<AudioSource>();
        audioSource.clip = Microphone.Start(null, true, 100, 44100);
        audioSource.loop = true;
        while (!(Microphone.GetPosition(null) > 0)) { }
        Debug.Log("start playing... position is " + Microphone.GetPosition(null));
        audioSource.Play();
    }

	// Update is called once per frame
	void Update () {
        audioSource.GetSpectrumData(samples, 0, FFTWindow.BlackmanHarris);
        UpdateMesh(samples);
    }
}
