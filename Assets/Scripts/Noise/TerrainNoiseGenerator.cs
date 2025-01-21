using UnityEngine;

public class TerrainNoiseGenerator : NoiseGenerator
{
    [SerializeField] float noiseScale = 1f;
    [SerializeField] float noiseWeight = 1f;
    [SerializeField, Range(0f, 1f)] float groundLevel = 0.2f;

    public float NoiseScale
    {
        get { return noiseScale; }
        set
        {
            noiseScale = value;
            _meshGenerator.SettingsUpdated = true;
        }
    }
    
    public float NoiseWeight
    {
        get { return noiseWeight; }
        set
        {
            noiseWeight = value;
            _meshGenerator.SettingsUpdated = true;
        }
    }
    
    public float GroundLevel
    {
        get { return groundLevel; }
        set
        {
            groundLevel = Mathf.Clamp(value, 0f, 1f);
            _meshGenerator.SettingsUpdated = true;
        }
    }
    
    protected override void PassNoiseSettingsToShader()
    {
        base.PassNoiseSettingsToShader();
        NoiseShader.SetFloat("_NoiseScale", noiseScale);
        NoiseShader.SetFloat("_GroundLevel", groundLevel / transform.localScale.y);
        NoiseShader.SetFloat("_NoiseWeight", noiseWeight);
    }
}
