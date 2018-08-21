using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadialMeshGenerator : MonoBehaviour {

	[SerializeField]
	GameObject radialMesh;

	float recentPop = 0;

	// Update is called once per frame
	void Update () {
		float bandPass = AudioReactive.Instance.PeakLow;
		if(bandPass > 30 && recentPop > 60f/(1f*GlobalState.Instance.Bpm)) {
			Instantiate(radialMesh, transform);
			recentPop = 0;
		}
		recentPop += Time.deltaTime;
	}
}
