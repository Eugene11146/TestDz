using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class GlobalSetting : MonoBehaviour
{
    public static List<SpawnButtonList> spawnButtonList;
    public List<SpawnButtonList> spawnButtonListPublic;

    public static float menuLerpTime = 0.3f;
    public float menuLerpTimePublic = 0.3f;

    public static float minSoundDistance = 3f;
    public float minSoundDistancePublic = 3f;

    public static float maxSoundDistance = 20f;
    public float maxSoundDistancePublic = 20f;

    public static float waterLevel= 20f;
    public float waterLevelPublic = 20f;

    public static float envorimentTemperature = 20f;
    public float envorimentTemperaturePublic = 20f;

    public static AudioClip[] splashSounds;
    public AudioClip[] splashSoundsPublic;

    public static ConstraintController defaultConstraintController;
    public ConstraintController defaultConstraintControllerPublic;

    public static List<PhysicsMaterial2D> physicMaterials;
    public List<PhysicsMaterial2D> physicMaterialsPublic;

    public static GameObject defaultConstraintBody;
    public GameObject defaultConstraintBodyPublic;

    public static GameObject grabTransform;
    public GameObject grabTransformPublic;

    public static GameObject splashEffect;
    public GameObject splashEffectPublic;

    public static GameObject fireEffect;
    public GameObject fireEffectPublic;

    public static GameObject soundEmmiter;
    public GameObject soundEmmiterPublic;

    public static GameObject contextTutorial;
    public GameObject contextTutorialPublic;

    public static GameObject rotationTutorial;
    public GameObject rotationTutorialPublic;

    public static GameObject weaponTutorial;
    public GameObject weaponTutorialPublic;

    public static GameObject selectTutorial;
    public GameObject selectTutorialPublic;

    public static Volume volume;
    public Volume volumePublic;

    public static SettingsData settings;
    public SettingsData settingsPublic;

    public static Slider shootingSlider;
    public Slider shootingSliderPublic;

    public static AudioSource UISoundSource;
    public AudioSource UISoundSourcePublic;

    public static AudioMixerGroup audioMixerGroup;
    public AudioMixerGroup audioMixerGroupPublic;

    public static AudioMixerGroup effectsAudioMixerGroup;
    public AudioMixerGroup effectsAudioMixerGroupPublic;
    
    public static SpawnMenuSorting spawnMenu;
    public SpawnMenuSorting spawnMenuPublic;

    public static Material defaultMaterial;
    public Material defaultMaterialPublic;

    public static LayerMask explosionLayerMask;
    public LayerMask explosionLayerMaskPublic;

    public static ContactFilter2D contactFilter;
    public ContactFilter2D contactFilterPublic;

    public static Material materialToChange;
    public Material materialToChangePublic;

    public static Camera mainCamera;
    public static int spawnedObjects = 0;
    public static GameObject resizer;

    private void Awake()
    {
        physicMaterials = physicMaterialsPublic;
        menuLerpTime = menuLerpTimePublic;
        defaultConstraintBody = defaultConstraintBodyPublic;
        minSoundDistance = minSoundDistancePublic;
        maxSoundDistance = maxSoundDistancePublic;
        UISoundSource = UISoundSourcePublic;
        audioMixerGroup = audioMixerGroupPublic;
        effectsAudioMixerGroup = effectsAudioMixerGroupPublic;
        mainCamera = Camera.main;
        splashEffect = splashEffectPublic;
        waterLevel = waterLevelPublic;
        shootingSlider = shootingSliderPublic;
        soundEmmiter = soundEmmiterPublic;
        splashSounds = splashSoundsPublic;
        spawnMenu = spawnMenuPublic;
        explosionLayerMask = explosionLayerMaskPublic;
        defaultMaterial = defaultMaterialPublic;
        envorimentTemperature = envorimentTemperaturePublic;
        fireEffect = fireEffectPublic;
        defaultConstraintController = defaultConstraintControllerPublic;
        contextTutorial = contextTutorialPublic;
        selectTutorial = selectTutorialPublic;
        weaponTutorial = weaponTutorialPublic;
        rotationTutorial = rotationTutorialPublic;
        contactFilter = contactFilterPublic;
        spawnButtonList = spawnButtonListPublic;
        materialToChange = materialToChangePublic;
        grabTransform = grabTransformPublic;
        settings = settingsPublic;
        volume = volumePublic;
    }
}

[System.Serializable]
public struct SpawnButton
{
    public string name;
    public Sprite Icon;
    public GameObject Object;
}
[System.Serializable]
public struct Category
{
    public string name;
    public List<SpawnButton> buttons;
}
[System.Serializable]
public struct SpawnButtonList
{
    public string name;
    public List<Category> category;
}
