class TerrainColor : ColorGenerator
{
    protected override void SetMaterialProperties()
    {
        float gridY = GridMetrics.PointsPerChunk;
        _materialToColor.SetFloat("GridY", gridY);
        _materialToColor.SetTexture("terrain", _texture);
    }
}