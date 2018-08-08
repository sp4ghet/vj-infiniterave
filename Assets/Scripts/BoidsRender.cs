using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidsRender : MonoBehaviour {

	[SerializeField]
	Mesh m_mesh;

	[SerializeField]
	Material m_instancingMaterial;

	[SerializeField]
	GPUBoids m_boids;

	[SerializeField]
	Vector3 m_objectScale = new Vector3(1, 1, 1);

	uint[] args = new uint[5] { 0, 0, 0, 0, 0 };
	ComputeBuffer argsBuffer;
	

	public Vector3 ObjectScale { get { return m_objectScale; } }

	// Use this for initialization
	void Start () {
		argsBuffer = new ComputeBuffer(1, args.Length * sizeof(uint), 
			ComputeBufferType.IndirectArguments);
	}
	
	// Update is called once per frame
	void Update () {
		RenderInstanceMesh();
	}

	private void OnDisable() {
		if ( argsBuffer != null) {
			argsBuffer.Release();
		}
		argsBuffer = null;
	}

	private void RenderInstanceMesh() {

		if(m_instancingMaterial == null || m_boids == null ||
			!SystemInfo.supportsInstancing) {
			return;
		}

		uint numIndices = m_mesh.GetIndexCount(0);

		args[0] = numIndices;
		args[1] = m_boids.MaxObjectNum;

		argsBuffer.SetData(args);

		m_instancingMaterial.SetBuffer("_BoidDataBuffer", m_boids.BoidDataBuffer);
		m_instancingMaterial.SetVector("_ObjectScale", ObjectScale);

		Bounds bounds = new Bounds(
			m_boids.SimulationCenter,
			m_boids.SimulationSize
			);

		Graphics.DrawMeshInstancedIndirect(
			m_mesh,
			0,
			m_instancingMaterial,
			bounds,
			argsBuffer
			);
	}
}
