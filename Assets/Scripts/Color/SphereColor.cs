using UnityEngine;

public class SphereColor : ColorGenerator
{
    protected override void SetMaterialProperties()
    {
        float radius = GridMetrics.PointsPerChunk / 2;
        Vector3 sphereCenter = new(radius, radius, radius);
        _materialToColor.SetVector("sphereCenter", sphereCenter);
        _materialToColor.SetFloat("_Radius", radius);
        _materialToColor.SetTexture("planet", _texture);
    }
}