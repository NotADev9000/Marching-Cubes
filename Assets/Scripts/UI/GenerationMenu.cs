using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public enum PresetType
{
    Base,
    Example1,
    Example2
}

public enum MeshType
{
    Sphere,
    Terrain
}

public class GenerationMenu : MonoBehaviour
{
    public GameObject SphereGenerator;
    public GameObject TerrainGenerator;

    public GameObject _currentGenerator;
    
    public GameObject CurrentGenerator
    {
        get { return _currentGenerator; }
        set
        {
            _currentGenerator = value;
            _meshGenerator = _currentGenerator.GetComponent<MeshGenerator>();
            _noiseGenerator = _currentGenerator.GetComponent<NoiseGenerator>();
        }
    }
    
    private MeshGenerator _meshGenerator;
    private NoiseGenerator _noiseGenerator;
    
    private VisualElement _rootElement;
    
    private VisualElement _panelElement;
    
    // Mesh Options
    private EnumField _meshTypeField;
    private Slider _isoSlider;
    
    // Noise Options
    private TextField _seedField;
    private Slider _noiseScaleSlider;
    private Slider _amplitudeSlider;
    private Slider _frequencySlider;
    private Slider _octavesSlider;
    private Slider _groundPercentSlider;
    private Slider _noiseWeightSlider;
    private Slider _weightStrengthSlider;
    private Slider _lacunaritySlider;
    private Slider _gainSlider;

    private bool _isPanelHidden;
    
    // Templates

    // Start is called before the first frame update
    IEnumerator Start()
    {
        _rootElement = GetComponent<UIDocument>().rootVisualElement;
        
        _panelElement = _rootElement.Q<VisualElement>("Panel");
        
        // Mesh options
        _isoSlider = _rootElement.Q<Slider>("IsoLevelSlider");
        _meshTypeField = _rootElement.Q<EnumField>("MeshTypeEnumField");
        _meshTypeField.Init(MeshType.Sphere);
        
        // Noise options
        _seedField = _rootElement.Q<TextField>("SeedField");
        _noiseScaleSlider = _rootElement.Q<Slider>("NoiseScaleSlider");
        _amplitudeSlider = _rootElement.Q<Slider>("AmplitudeSlider");
        _frequencySlider = _rootElement.Q<Slider>("FrequencySlider");
        _octavesSlider = _rootElement.Q<Slider>("OctavesSlider");
        _groundPercentSlider = _rootElement.Q<Slider>("GroundPercentSlider");
        _noiseWeightSlider = _rootElement.Q<Slider>("NoiseWeightSlider");
        _weightStrengthSlider = _rootElement.Q<Slider>("WeightStrengthSlider");
        _lacunaritySlider = _rootElement.Q<Slider>("LacunaritySlider");
        _gainSlider = _rootElement.Q<Slider>("GainSlider");
        
        // Mesh change events
        _meshTypeField.RegisterValueChangedCallback(evt =>
        {
            SphereGenerator.SetActive(true);
            TerrainGenerator.SetActive(true);
            switch (evt.newValue)
            {
                case MeshType.Sphere:
                    CurrentGenerator = SphereGenerator;
                    TerrainGenerator.SetActive(false);
                    
                    _noiseScaleSlider.AddToClassList("hidden");
                    _groundPercentSlider.AddToClassList("hidden");
                    _noiseWeightSlider.AddToClassList("hidden");
                    break;
                case MeshType.Terrain:
                    CurrentGenerator = TerrainGenerator;
                    SphereGenerator.SetActive(false);
                    
                    _noiseScaleSlider.RemoveFromClassList("hidden");
                    _groundPercentSlider.RemoveFromClassList("hidden");
                    _noiseWeightSlider.RemoveFromClassList("hidden");
                    break;
            }
        });
            
        _isoSlider.RegisterValueChangedCallback((evt) =>
        {
            _meshGenerator.IsoLevel = evt.newValue;
        });
        
        // Noise change events
        _seedField.RegisterValueChangedCallback((evt) =>
        {
            _noiseGenerator.Seed = int.Parse(evt.newValue);
        });

        _noiseScaleSlider.RegisterValueChangedCallback((evt) =>
        {
            TerrainNoiseGenerator terrainNoiseGenerator = _noiseGenerator as TerrainNoiseGenerator;
            terrainNoiseGenerator.NoiseScale = evt.newValue;
        });

        _amplitudeSlider.RegisterValueChangedCallback((evt) =>
        {
            _noiseGenerator.Amplitude = evt.newValue;
        });

        _frequencySlider.RegisterValueChangedCallback((evt) =>
        {
            _noiseGenerator.Frequency = evt.newValue;
        });

        _octavesSlider.RegisterValueChangedCallback((evt) =>
        {
            _noiseGenerator.Octaves = (int)evt.newValue;
        });

        _groundPercentSlider.RegisterValueChangedCallback((evt) =>
        {
            TerrainNoiseGenerator terrainNoiseGenerator = _noiseGenerator as TerrainNoiseGenerator;
            terrainNoiseGenerator.GroundLevel = evt.newValue;
        });

        _noiseWeightSlider.RegisterValueChangedCallback((evt) =>
        {
            TerrainNoiseGenerator terrainNoiseGenerator = _noiseGenerator as TerrainNoiseGenerator;
            terrainNoiseGenerator.NoiseWeight = evt.newValue;
        });

        _weightStrengthSlider.RegisterValueChangedCallback((evt) =>
        {
            _noiseGenerator.WeightStrength = evt.newValue;
        });

        _lacunaritySlider.RegisterValueChangedCallback((evt) =>
        {
            _noiseGenerator.Lacunarity = evt.newValue;
        });

        _gainSlider.RegisterValueChangedCallback((evt) =>
        {
            _noiseGenerator.Gain = evt.newValue;
        });

        yield return null;
        
        // Initialise base values here
        _meshTypeField.value = MeshType.Terrain;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (_isPanelHidden)
            {
                _panelElement.RemoveFromClassList("hidden");
            }
            else
            {
                _panelElement.AddToClassList("hidden");
            }

            _isPanelHidden = !_isPanelHidden;
        }
    }
}
