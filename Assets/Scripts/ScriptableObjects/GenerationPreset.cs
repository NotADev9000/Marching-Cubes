using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Generation Preset", menuName = "MarchingCubes")]
public class GenerationPreset : ScriptableObject
{
    // Mesh
    public MeshType Mesh;
    public float IsoLevel;
    
    // Noise
    public NoiseType Noise;
    public FractalType Fractal;
    public int Seed;
    public float Amplitude;
    public float Frequency;
    public int Octaves;
    public float WeightStrength;
    public float Lacunarity;
    public float Gain;
    
    // Terrain-specific
    public float NoiseScale;
    public float NoiseWeight;
    public float GroundLevel;
}
