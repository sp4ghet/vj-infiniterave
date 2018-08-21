using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadialMesh : MonoBehaviour {

	const float TAU = Mathf.PI * 2;

	[SerializeField]
	float speed = 1;
	[SerializeField]
	float lifeTime = 10;
	[SerializeField]
	float popDuration = 2;
	[SerializeField]
	Easing.Ease easing = Easing.Ease.EaseOutBack;

	[SerializeField]
	MeshFilter filter;

	Easing.Function easingFunction;

	Mesh mesh;
	Vector3[] vertices;
	int[] tris;
	int subdivisions = 3;
	int e = 4; //edges, or vertices per subdivision
	int tv = 24;
	float maxRadius = 10;

	float startTime;

	float rotationSpeed;

	IEnumerator Pop(float duration) {
		for (float t = 0; t < duration; t += Time.deltaTime) {
			float radius = easingFunction(0, maxRadius, t / duration);
			float side = easingFunction(0, 0.5f, t / duration);

			for (int i = 0; i < subdivisions; i++) {
				Quaternion rot = Quaternion.Euler(0, 0, (i/(float)subdivisions) * 360 );
				vertices[i*e] = rot * new Vector3(0, radius - side, 0);
				vertices[i*e + 1] = rot * new Vector3(0, radius - side, side*2);
				vertices[i*e + 2] = rot * new Vector3(0, radius + side, 0);
				vertices[i*e + 3] = rot * new Vector3(0, radius + side, side*2);

				tris[i*tv + 0] = (i * e + 0) % (subdivisions * 4);
				tris[i*tv + 1] = (i * e + 4) % (subdivisions * 4);
				tris[i*tv + 2] = (i * e + 2) % (subdivisions * 4);

				tris[i*tv + 3] = (i * e + 2) % (subdivisions * 4);
				tris[i*tv + 4] = (i * e + 4) % (subdivisions * 4);
				tris[i*tv + 5] = (i * e + 6) % (subdivisions * 4);

				tris[i*tv + 6] = (i * e + 2) % (subdivisions * 4);
				tris[i*tv + 7] = (i * e + 6) % (subdivisions * 4);
				tris[i*tv + 8] = (i * e + 3) % (subdivisions * 4);

				tris[i*tv + 9] = (i * e + 3) % (subdivisions * 4);
				tris[i*tv + 10] = (i * e + 6) % (subdivisions * 4);
				tris[i*tv + 11] = (i * e + 7) % (subdivisions * 4);

				tris[i*tv + 12] = (i * e + 3) % (subdivisions * 4);
				tris[i*tv + 13] = (i * e + 7) % (subdivisions * 4);
				tris[i*tv + 14] = (i * e + 1) % (subdivisions * 4);

				tris[i*tv + 15] = (i * e + 1) % (subdivisions * 4);
				tris[i*tv + 16] = (i * e + 7) % (subdivisions * 4);
				tris[i*tv + 17] = (i * e + 5) % (subdivisions * 4);

				tris[i*tv + 18] = (i * e + 1) % (subdivisions * 4);
				tris[i*tv + 19] = (i * e + 5) % (subdivisions * 4);
				tris[i*tv + 20] = (i * e + 0) % (subdivisions * 4);

				tris[i*tv + 21] = (i * e + 0) % (subdivisions * 4);
				tris[i*tv + 22] = (i * e + 5) % (subdivisions * 4);
				tris[i*tv + 23] = (i * e + 4) % (subdivisions * 4);
			}

			mesh.vertices = vertices;
			mesh.triangles = tris;
			mesh.RecalculateNormals();
			mesh.RecalculateBounds();
			yield return new WaitForEndOfFrame();
		}
	}


	// Use this for initialization
	void Start () {
		filter = GetComponent<MeshFilter>();
		mesh = new Mesh();
		filter.mesh = mesh;
		vertices = new Vector3[12];
		tris = new int[72];

		transform.localRotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
		rotationSpeed = Random.Range(-360, 360);

		startTime = Time.time;

		easingFunction = Easing.GetEasingFunction(easing);
		StartCoroutine(Pop(popDuration));
	}

	// Update is called once per frame
	void Update () {
		if(Time.time - startTime >= lifeTime) {
			Destroy(gameObject);
		}

		transform.position -= Vector3.forward*speed*Time.deltaTime;
		transform.localRotation *= Quaternion.Euler(0, 0, rotationSpeed*Time.deltaTime);
	}
}
