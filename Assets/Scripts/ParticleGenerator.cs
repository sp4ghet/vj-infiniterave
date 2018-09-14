using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleGenerator : MonoBehaviour {

    [SerializeField]
    GameObject particles;

    float recentPop = 0;

    // Update is called once per frame
    void Update() {
        float peak = AudioReactive.Instance.PeakBand;
        int subdivisions = GlobalState.I.Subdivisions;

        if (peak > GlobalState.I.MeshThreshold
            && recentPop > 60f / (2f * GlobalState.I.Bpm)
        ) {
            var particle = Instantiate(particles, transform);
            var circle = Random.insideUnitCircle;
            particle.transform.position = (Vector3)circle * 10 + Random.insideUnitSphere;

            recentPop = 0;
        }
        recentPop += Time.deltaTime;
    }
}
