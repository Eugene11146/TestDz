using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.Rendering;

public class SettingsData : MonoBehaviour
{
    public AudioMixer mixer;
    public ChangeSettingsWindow changeSettingsWindow;

    public Slider UIVolumeSlider;
    public Slider AmbientVolumeSlider;
    public Slider EffectsVolumeSlider;
    public Slider SlowMotionSlider;
    public Slider UIScaleSlider;
    public Slider ZoomSensitivitySlider;

    public Toggle GoreToggle;
    public Toggle BloomToggle;
    public Toggle VignetteToggle;
    public Toggle MuteToggle;

    public int KillCount = 0;

    public float UIVolume = 100;
    public float AmbientVolume = 100;
    public float EffectsVolume = 100;
    public float slowMotionScale = 0.1f;
    public float UIScale = 0f;
    public float ZoomSensitivity = 1f;
    public bool Gore = true;
    public bool Bloom = true;
    public bool Vignette = true;
    public bool Mute = false;

    public bool contextTutorial = false;
    public bool selectTutorial = false;
    public bool weaponTutorial = false;
    public bool rotationTutorial = false;

    private void Start()
    {
        LoadVolume();
        if (changeSettingsWindow != null)
            changeSettingsWindow.ChangeWindow(0);

        InvokeRepeating("AutoSave", 0f, 60f);
    }

    void AutoSave()
    {
        SaveVolume();
    }

    public void GetVolume()
    {
        if (UIVolumeSlider != null)
        {
            UIVolume = UIVolumeSlider.value;
            AmbientVolume = AmbientVolumeSlider.value;
            EffectsVolume = EffectsVolumeSlider.value;
            slowMotionScale = SlowMotionSlider.value;
            UIScale = UIScaleSlider.value;
            ZoomSensitivity = ZoomSensitivitySlider.value;
            Gore = GoreToggle.isOn;
            Bloom = BloomToggle.isOn;
            Vignette = VignetteToggle.isOn;
            Mute = MuteToggle.isOn;
            if (Mute)
            {
                mixer.SetFloat("MasterVolume", -80);
            }
            else
            {
                mixer.SetFloat("MasterVolume", 0);
            }
            if (GlobalSetting.volume != null)
            {
                foreach (VolumeComponent volumeComponent in GlobalSetting.volume.profile.components)
                {
                    if (volumeComponent.name.Contains("Bloom"))
                    {
                        volumeComponent.active = Bloom;
                    }
                    if (volumeComponent.name.Contains("Vignette"))
                    {
                        volumeComponent.active = Vignette;
                    }
                }
            }
            if (GlobalSetting.mainCamera.TryGetComponent(out CameraMovementForPhone cameraMovementForPhone))
            {
                cameraMovementForPhone.SetScrollSensitivity();
            }
        }
    }

    private void Update()
    {
        if (Mute)
        {
            mixer.SetFloat("MasterVolume", -80);
        }
        else
        {
            mixer.SetFloat("MasterVolume", 0);
        }
    }

    public void SetVolume()
    {
        mixer.SetFloat("UIVolume", UIVolume * 0.8f - 80);
        mixer.SetFloat("AmbientVolume", AmbientVolume * 0.8f - 80);
        mixer.SetFloat("EffectsVolume", EffectsVolume * 0.8f - 80);
        if (Mute)
        {
            mixer.SetFloat("MasterVolume", -80);
        }
        else
        {
            mixer.SetFloat("MasterVolume", 0);
        }

        if (UIVolumeSlider != null)
        {
            UIVolumeSlider.SetValueWithoutNotify(UIVolume);
            UIVolumeSlider.GetComponentInChildren<SliderToInputField>().Apply(UIVolume);

            AmbientVolumeSlider.SetValueWithoutNotify(AmbientVolume);
            AmbientVolumeSlider.GetComponentInChildren<SliderToInputField>().Apply(AmbientVolume);

            EffectsVolumeSlider.SetValueWithoutNotify(EffectsVolume);
            EffectsVolumeSlider.GetComponentInChildren<SliderToInputField>().Apply(EffectsVolume);

            SlowMotionSlider.SetValueWithoutNotify(slowMotionScale);
            SlowMotionSlider.GetComponentInChildren<SliderToInputField>().Apply(slowMotionScale);

            UIScaleSlider.SetValueWithoutNotify(UIScale);
            UIScaleSlider.GetComponentInChildren<SliderToInputField>().Apply(UIScale);

            ZoomSensitivitySlider.SetValueWithoutNotify(ZoomSensitivity);
            ZoomSensitivitySlider.GetComponentInChildren<SliderToInputField>().Apply(ZoomSensitivity);

            GoreToggle.isOn = Gore;
            BloomToggle.isOn = Bloom;
            VignetteToggle.isOn = Vignette;
            MuteToggle.isOn = Mute;
        }
    }

    public void ResetTutorial()
    {
        contextTutorial = false;
        selectTutorial = false;
        weaponTutorial = false;
        rotationTutorial = false;
        SaveVolume();
    }

    public void SaveVolume()
    {
        GetVolume();

        SaveObject saveObject = new SaveObject
        {
            UIVolume = UIVolume,
            AmbientVolume = AmbientVolume,
            EffectsVolume = EffectsVolume,
            slowMotionScale = slowMotionScale,
            UIScale = UIScale,
            ZoomSensitivity = ZoomSensitivity,
            Gore = Gore,
            Bloom = Bloom,
            Vignette = Vignette,
            Mute = Mute,
            ContextTutorial = contextTutorial,
            SelectTutorial = selectTutorial,
            WeaponTutorial = weaponTutorial,
            RotationTutorial = rotationTutorial,
            KillCount = KillCount,
        };
        string json = JsonUtility.ToJson(saveObject);
        File.WriteAllText(Application.persistentDataPath + "/setting.txt", json);
    }

    public void LoadVolume()
    {
        if (File.Exists(Application.persistentDataPath + "/setting.txt"))
        {
            string saveString = File.ReadAllText(Application.persistentDataPath + "/setting.txt");
            SaveObject loadedObject = JsonUtility.FromJson<SaveObject>(saveString);
            UIVolume = loadedObject.UIVolume;
            AmbientVolume = loadedObject.AmbientVolume;
            EffectsVolume = loadedObject.EffectsVolume;
            slowMotionScale = loadedObject.slowMotionScale;
            UIScale = loadedObject.UIScale;
            ZoomSensitivity = loadedObject.ZoomSensitivity;
            Gore = loadedObject.Gore; 
            Bloom = loadedObject.Bloom;
            Vignette = loadedObject.Vignette;
            Mute = loadedObject.Mute;
            contextTutorial = loadedObject.ContextTutorial;
            selectTutorial = loadedObject.SelectTutorial;
            weaponTutorial = loadedObject.WeaponTutorial;
            rotationTutorial = loadedObject.RotationTutorial;
            KillCount = loadedObject.KillCount;
        }
        else
        {
            UIVolume = 100;
            AmbientVolume = 100;
            EffectsVolume = 100;
            slowMotionScale = 0.1f;
            UIScale = 0f;
            ZoomSensitivity = 1f;
            Gore = true;
            Bloom = true;
            Vignette = true;
            Mute = false;
            contextTutorial = false;
            selectTutorial = false;
            weaponTutorial = false;
            rotationTutorial = false;
            KillCount = 0;
}
        SetVolume();
    }

    class SaveObject
    {
        public float UIVolume;
        public float AmbientVolume;
        public float EffectsVolume;
        public float slowMotionScale;
        public float UIScale;
        public float ZoomSensitivity;
        public bool Gore;
        public bool Bloom;
        public bool Vignette;
        public bool Mute;
        public bool ContextTutorial;
        public bool SelectTutorial;
        public bool WeaponTutorial;
        public bool RotationTutorial;
        public int KillCount;
    }
}
