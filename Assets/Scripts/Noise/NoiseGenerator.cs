using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(MeshGenerator))]
public class NoiseGenerator : MonoBehaviour
{
    [Header("Noise Shader")]
    [SerializeField] protected ComputeShader NoiseShader;

    [Space()]
    [Header("Noise Settings")]
    [SerializeField] int seed = 1337;
    [SerializeField] float amplitude = 5;
    [SerializeField, Range(0f, 0.1f)] float frequency = 0.02f;
    [SerializeField] int octaves = 8;
    [SerializeField] float weightStrength = 0f;
    [SerializeField] float lacunarity = 2f;
    [SerializeField] float gain = 0.5f;

    // Components:
    private MeshGenerator _meshGenerator;

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
