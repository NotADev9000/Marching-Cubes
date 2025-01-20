using UnityEngine;

public class TerrainNoiseGenerator : NoiseGenerator
{
    [SerializeField] float noiseScale = 1f;
    [SerializeField] float noiseWeight = 1f;
    [SerializeField, Range(0f, 1f)] float groundLevel = 0.2f;

    protected override void PassNoiseSettingsToShader()
    {
        base.PassNoiseSettingsToShader();
        NoiseShader.SetFloat("_NoiseScale", noiseScale);
        NoiseShader.SetFloat("_GroundLevel", groundLevel / transform.localScale.y);
        NoiseShader.SetFloat("_NoiseWeight", noiseWeight);
    }
}
