using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class GPUBoids : MonoBehaviour {

	[Serializable]
	struct BoidData {
		public Vector3 Position;
		public Vector3 Velocity;
	}

	public ComputeBuffer BoidDataBuffer {
		get {
			return boidDataBuffer;
		}
	}

	public Vector3 SimulationCenter {
		get {
			return simulationCenter;
		}
	}

	public Vector3 SimulationSize {
		get {
			return simulationSize;
		}
	}

	public uint MaxObjectNum {
		get {
			return maxObjectNum;
		}
		set {
			maxObjectNum = value;
		}
	}

	[SerializeField]
	ComputeShader boidsCS;

	[SerializeField]
	Vector3 simulationCenter = Vector3.zero;

	[SerializeField]
	Vector3 simulationSize = new Vector3(32, 32, 32);

	[SerializeField, Range(256, 32768)]
	uint maxObjectNum = 16384;

	[SerializeField, Range(3, 20)]
	private float maxVelocity = 10;

	[SerializeField, Range(3, 20)]
	private float wallRepulsionStrength = 5;

	const int SIMULATION_BLOCK_SIZE = 256;

	ComputeBuffer boidDataBuffer;
	ComputeBuffer boidForceBuffer;

	[SerializeField, Range(0, 10)]
	private float alignmentRadius;
	[SerializeField, Range(0, 10)]
	private float separationRadius;
	[SerializeField, Range(0, 10)]
	private float cohesionRadius;

	[SerializeField, Range(1, 10)]
	private float separationStrength;
	[SerializeField, Range(1, 10)]
	private float alignmentStrength;
	[SerializeField, Range(1, 10)]
	private float cohesionStrength;

	#region MonoBehaviour
	// Use this for initialization
	void Start() {
		InitBuffers();
	}
	private void OnDrawGizmos() {
		Gizmos.color = Color.cyan;
		Gizmos.DrawWireCube(SimulationCenter, SimulationSize);
	}

	// Update is called once per frame
	void Update() {
		RunSimulation();
	}

	private void OnDestroy() {
		ReleaseBuffers();
	}
	#endregion

	private void InitBuffers() {
		boidDataBuffer = new ComputeBuffer((int)maxObjectNum, Marshal.SizeOf(typeof(BoidData)));
		boidForceBuffer = new ComputeBuffer((int)maxObjectNum, Marshal.SizeOf(typeof(Vector3)));

		var boidForces = new Vector3[maxObjectNum];
		var boidDatas = new BoidData[maxObjectNum];
		for (int i = 0; i < maxObjectNum; i++) {
			boidForces[i] = Vector3.zero;
			boidDatas[i].Position = UnityEngine.Random.insideUnitSphere * 1f;
			boidDatas[i].Velocity = UnityEngine.Random.insideUnitSphere * 0.1f;
		}
		boidForceBuffer.SetData(boidForces);
		boidDataBuffer.SetData(boidDatas);
		boidForces = null;
		boidDatas = null;
	}

	private void ReleaseBuffers() {
		if (boidDataBuffer != null) {
			boidDataBuffer.Release();
			boidDataBuffer = null;
		}
		if (boidForceBuffer != null) {
			boidForceBuffer.Release();
			boidForceBuffer = null;
		}
	}

	private void RunSimulation() {

		var cs = boidsCS;

		int forceKernel = cs.FindKernel("ForceCS");
		int integrateKernel = cs.FindKernel("IntegrateCS");

		int threadGroupSize = Mathf.CeilToInt(maxObjectNum / SIMULATION_BLOCK_SIZE);

		cs.SetInt("_MaxBoidObjectNum", (int)maxObjectNum);
		cs.SetFloat("_MaxSpeed", maxVelocity);
		cs.SetFloat("_MaxSteerForce", 0.5f);

		cs.SetVector("_WallCenter", SimulationCenter);
		cs.SetVector("_WallSize", SimulationSize);
		cs.SetFloat("_AvoidWallWeight", wallRepulsionStrength);

		cs.SetFloat("_SeparationNeighborhoodRadius", separationRadius);
		cs.SetFloat("_AlignmentRadius", alignmentRadius);
		cs.SetFloat("_CohesionNeighborhoodRadius", cohesionRadius);
		cs.SetFloat("_SeparationWeight", separationStrength);
		cs.SetFloat("_AlignmentWeight", alignmentStrength);
		cs.SetFloat("_CohesionWeight", cohesionStrength);

		cs.SetBuffer(forceKernel, "_BoidDataBufferRead", boidDataBuffer);
		cs.SetBuffer(forceKernel, "_BoidForceBufferWrite", boidForceBuffer);
		cs.Dispatch(forceKernel, threadGroupSize, 1, 1);

		cs.SetFloat("_DeltaTime", Time.deltaTime);
		cs.SetBuffer(integrateKernel, "_BoidForceBufferRead", boidForceBuffer);
		cs.SetBuffer(integrateKernel, "_BoidDataBufferWrite", boidDataBuffer);
		cs.Dispatch(integrateKernel, threadGroupSize, 1, 1);
	}
}
