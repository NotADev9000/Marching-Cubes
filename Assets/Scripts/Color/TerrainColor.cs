class TerrainColor : ColorGenerator
{
    protected override void SetMaterialProperties()
    {
        _materialToColor.SetFloat("GridY", GridMetrics.PointsPerChunk);
        _materialToColor.SetTexture("terrain", _texture);
    }
}