using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ColorGenerator : MonoBehaviour
{
    [SerializeField] protected Material _materialToColor;
    [SerializeField] private Gradient _gradient;

    protected Texture2D _texture;
    private const int _textureResolution = 50;

    private void Update()
    {
        InitializeTexture();
        UpdateTexture();
        SetMaterialProperties();
    }

    private void InitializeTexture()
    {
        if (_texture == null || _texture.width != _textureResolution)
        {
            _texture = new Texture2D(_textureResolution, 1, TextureFormat.RGBA32, false);
        }
    }

    private void UpdateTexture()
    {
        if (_gradient != null)
        {
            Color32[] colors = new Color32[_texture.width];
            for (int i = 0; i < _textureResolution; i++)
            {
                Color gradientCol = _gradient.Evaluate(i / (_textureResolution - 1f));
                colors[i] = gradientCol;
            }

            _texture.SetPixels32(colors);
            _texture.Apply();
        }
    }

    protected abstract void SetMaterialProperties();
}
