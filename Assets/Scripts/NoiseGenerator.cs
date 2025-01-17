using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class NoiseGenerator : MonoBehaviour
{
    [Header("Noise Shader")]
    [SerializeField] private ComputeShader NoiseShader;

    [Space()]
    [Header("Noise Settings")]
    [SerializeField] float noiseScale = 0.08f;
    [SerializeField] float amplitude = 200;
    [SerializeField] float frequency = 0.004f;
    [SerializeField] int octaves = 6;
    [SerializeField, Range(0f, 1f)] float groundPercent = 0.2f;

    [Space()]
    [Header("Chunk to Update")]
    [SerializeField] private MeshGenerator _chunk;

    private ComputeBuffer _weightsBuffer;

    private void Awake()
    {
        CreateBuffers();
    }

    private void OnDestroy()
    {
        ReleaseBuffers();
    }

    private void OnValidate()
    {
        if (EditorApplication.isPlaying && _chunk != null)
        {
            _chunk.SettingsUpdated = true;
        }
    }

    public float[] GetNoise()
    {
        float[] noiseValues =
            new float[GridMetrics.PointsPerChunk * GridMetrics.PointsPerChunk * GridMetrics.PointsPerChunk];

        NoiseShader.SetBuffer(0, "_Weights", _weightsBuffer);

        NoiseShader.SetInt("_ChunkSize", GridMetrics.PointsPerChunk);
        NoiseShader.SetFloat("_NoiseScale", noiseScale);
        NoiseShader.SetFloat("_Amplitude", amplitude);
        NoiseShader.SetFloat("_Frequency", frequency);
        NoiseShader.SetInt("_Octaves", octaves);
        NoiseShader.SetFloat("_GroundPercent", groundPercent);

        int numThreadGroups = Mathf.CeilToInt(GridMetrics.PointsPerChunk / GridMetrics.NumThreads);
        NoiseShader.Dispatch(
            0, numThreadGroups, numThreadGroups, numThreadGroups
        );

        _weightsBuffer.GetData(noiseValues);

        return noiseValues;
    }

    private void CreateBuffers()
    {
        _weightsBuffer = new ComputeBuffer(
            GridMetrics.PointsPerChunk * GridMetrics.PointsPerChunk * GridMetrics.PointsPerChunk, sizeof(float)
        );
    }

    private void ReleaseBuffers()
    {
        _weightsBuffer.Release();
    }
}
