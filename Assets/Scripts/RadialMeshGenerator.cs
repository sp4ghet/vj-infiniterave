using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadialMeshGenerator : MonoBehaviour {

	[SerializeField]
	GameObject radialMesh;

	float recentPop = 0;

    // Update is called once per frame
    void Update () {
		float peak = AudioReactive.Instance.PeakLow;
        int subdivisions = GlobalState.I.Subdivisions;

		if(peak > GlobalState.I.MeshThreshold 
            && recentPop > 60f/(1f*GlobalState.I.Bpm) 
            && GlobalState.I.RadialMeshMode != RadialMesh.RadialState.off
        ) {
            var radialMeshInstance = Instantiate(radialMesh, transform);
            var mesh = radialMeshInstance.GetComponent<RadialMesh>();
            if (GlobalState.I.SubdivisionsRandom) {
                mesh.Subdivisions = Random.Range(3, subdivisions);
            }
            else {
                mesh.Subdivisions = subdivisions;
            }

            recentPop = 0;
		}
		recentPop += Time.deltaTime;
	}
}
