using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorGenerator : MonoBehaviour
{
    public bool isTerrain;
    public Material terrainMat;
    public Material planetMat;
    public Gradient gradient;
    private Texture2D texture;
    private Vector3 SphereCenter;
    const int textureResolution = 50;
    private void InitializeTexture(){
        if (texture == null || texture.width != textureResolution) {
            texture = new Texture2D (textureResolution, 1, TextureFormat.RGBA32, false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        InitializeTexture();
        UpdateTexture();
        if(!isTerrain){
            float radius = GridMetrics.PointsPerChunk/2;
            SphereCenter = new Vector3(radius,radius,radius);
            planetMat.SetVector("sphereCenter",SphereCenter);
            planetMat.SetFloat ("_Radius", radius);
            planetMat.SetTexture ("planet", texture);
        }
        else{
            
            float GridY = GridMetrics.PointsPerChunk;
            terrainMat.SetFloat("GridY",GridY);
            terrainMat.SetTexture ("terrain", texture);
        }
        
    }
    void UpdateTexture () {
        if (gradient != null) {
            Color[] colors = new Color[texture.width];
            for (int i = 0; i < textureResolution; i++) {
                Color gradientCol = gradient.Evaluate (i/ (textureResolution - 1f));
                colors[i] = gradientCol;
            }

            texture.SetPixels (colors);
            texture.Apply ();
        }
    }

}
