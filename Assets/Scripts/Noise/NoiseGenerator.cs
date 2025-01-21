using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public enum NoiseType : int
{
    OpenSimplex2 = 0,
    OpenSimplex2S = 1,
    Cellular = 2,
    Perlin = 3,
    CubicValue = 4,
    Value = 5
}

public enum FractalType : int
{
    None,
    FBM,
    Ridged,
    Pingpong,
    Progressive,
    Independent
}

[RequireComponent(typeof(MeshGenerator))]
public class NoiseGenerator : MonoBehaviour
{
    [Header("Noise Shader")]
    [SerializeField] protected ComputeShader NoiseShader;

    [Space()]
    [Header("Noise Settings")]
    [SerializeField] NoiseType noiseType = NoiseType.Value;
    [SerializeField] FractalType fractalType = FractalType.None;
    [SerializeField] int seed = 1337;
    [SerializeField] float amplitude = 5;
    [SerializeField, Range(0f, 0.1f)] float frequency = 0.02f;
    [SerializeField] int octaves = 8;
    [SerializeField] float weightStrength = 0f;
    [SerializeField] float lacunarity = 2f;
    [SerializeField] float gain = 0.5f;

    public NoiseType NoiseIndex
    {
        get { return noiseType; }
        set
        {
            noiseType = value;
            _meshGenerator.SettingsUpdated = true;
        }
    }

    public FractalType FractalIndex
    {
        get { return fractalType; }
        set
        {
            fractalType = value;
            _meshGenerator.SettingsUpdated = true;
        }
    }

    public int Seed
    {
        get { return seed; }
        set
        {
            seed = value;
            _meshGenerator.SettingsUpdated = true;
        }
    }

    public float Amplitude
    {
        get { return amplitude; }
        set
        {
            amplitude = value;
            _meshGenerator.SettingsUpdated = true;
        }
    }

    public float Frequency
    {
        get { return frequency; }
        set
        {
            frequency = Mathf.Clamp(value, 0f, 0.1f);
            _meshGenerator.SettingsUpdated = true;
        }
    }

    public int Octaves
    {
        get { return octaves; }
        set
        {
            octaves = Mathf.Max(1, value);
            _meshGenerator.SettingsUpdated = true;
        }
    }

    public float WeightStrength
    {
        get { return weightStrength; }
        set
        {
            weightStrength = value;
            _meshGenerator.SettingsUpdated = true;
        }
    }

    public float Lacunarity
    {
        get { return lacunarity; }
        set
        {
            lacunarity = Mathf.Max(1f, value);
            _meshGenerator.SettingsUpdated = true;
        }
    }

    public float Gain
    {
        get { return gain; }
        set
        {
            gain = Mathf.Clamp01(value);
            _meshGenerator.SettingsUpdated = true;
        }
    }

    // Components:
    protected MeshGenerator _meshGenerator;

    // Buffers:
    private ComputeBuffer _weightsBuffer;

    private void Awake()
    {
        _meshGenerator = GetComponent<MeshGenerator>();

        CreateBuffers();
    }

    private void OnDestroy()
    {
        ReleaseBuffers();
    }

    private void OnValidate()
    {
        if (EditorApplication.isPlaying && _meshGenerator != null)
        {
            _meshGenerator.SettingsUpdated = true;
        }
    }

    public float[] GetNoise()
    {
        // Setup noise shader
        NoiseShader.SetBuffer(0, "_Weights", _weightsBuffer);
        PassNoiseSettingsToShader();

        // Dispatch noise shader
        int numThreadGroups = Mathf.CeilToInt(GridMetrics.PointsPerChunk / GridMetrics.NumThreads);
        NoiseShader.Dispatch(
            0, numThreadGroups, numThreadGroups, numThreadGroups
        );

        // Get noise values
        float[] noiseValues = new float[GridMetrics.PointsPerChunk * GridMetrics.PointsPerChunk * GridMetrics.PointsPerChunk];
        _weightsBuffer.GetData(noiseValues);
        return noiseValues;
    }

    protected virtual void PassNoiseSettingsToShader()
    {
        NoiseShader.SetInt("_NoiseType", (int)noiseType);
        NoiseShader.SetInt("_FractalType", (int)fractalType);
        NoiseShader.SetInt("_ChunkSize", GridMetrics.PointsPerChunk);
        NoiseShader.SetInt("_Seed", seed);
        NoiseShader.SetFloat("_Amplitude", amplitude);
        NoiseShader.SetFloat("_Frequency", frequency);
        NoiseShader.SetInt("_Octaves", octaves);
        NoiseShader.SetFloat("_WeightStrength", weightStrength);
        NoiseShader.SetFloat("_Lacunarity", lacunarity);
        NoiseShader.SetFloat("_Gain", gain);
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
