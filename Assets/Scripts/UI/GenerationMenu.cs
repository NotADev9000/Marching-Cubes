using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GenerationMenu : MonoBehaviour
{
    public MeshGenerator SceneMeshGenerator;

    public NoiseGenerator SceneNoiseGenerator;
    
    private VisualElement _rootElement;
    
    // Mesh Options
    private Slider _isoSlider;
    
    // Noise Options
    private Slider _noiseScaleSlider;
    private Slider _amplitudeSlider;
    private Slider _frequencySlider;
    private Slider _octavesSlider;
    private Slider _groundPercentSlider;
    
    // Templates

    // Start is called before the first frame update
    void Start()
    {
        _rootElement = GetComponent<UIDocument>().rootVisualElement;
        
        _isoSlider = _rootElement.Q<Slider>("IsoLevelSlider");
        
        _noiseScaleSlider = _rootElement.Q<Slider>("NoiseScaleSlider");
        _amplitudeSlider = _rootElement.Q<Slider>("AmplitudeSlider");
        _frequencySlider = _rootElement.Q<Slider>("FrequencySlider");
        _octavesSlider = _rootElement.Q<Slider>("OctavesSlider");
        _groundPercentSlider = _rootElement.Q<Slider>("GroundPercentSlider");
        
        _noiseScaleSlider.RegisterValueChangedCallback(OnChangeNoiseScale);
    }

    void OnChangeNoiseScale(ChangeEvent<float> evt)
    {
        
    }
}
