using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

/// <summary>
/// This manager updates the Audio Mixers.
/// </summary>
public class SoundManager : MonoBehaviour
{
    [Header("Audio Mixer Groups")]
    [SerializeField] private AudioMixerGroup masterGroup;
    [SerializeField] private AudioMixerGroup musicGroup;
    [SerializeField] private AudioMixerGroup soundGroup;

    [Header("Exposed Parameters")]
    [SerializeField] private string masterParam = "MasterVolume";
    [SerializeField] private string musicParam = "MusicVolume";
    [SerializeField] private string soundParam = "SoundsVolume";

	void Start ()
    {
        PlayerPrefsManager.onSettingsChanged += UpdateMixerGroups;
	}
	
	void Update ()
    {
		
	}

    void UpdateMixerGroups()
    {
        switch (PlayerPrefsManager.GetSettingsMuteAllSounds())
        {
            case false:
                masterGroup.audioMixer.SetFloat(masterParam, PlayerPrefsManager.GetSettingsMasterValue());
                musicGroup.audioMixer.SetFloat(musicParam, PlayerPrefsManager.GetSettingsMusicValue());
                soundGroup.audioMixer.SetFloat(soundParam, PlayerPrefsManager.GetSettingsSoundValue());
                break;
            case true:
                masterGroup.audioMixer.SetFloat(masterParam, -80f);
                musicGroup.audioMixer.SetFloat(musicParam, -80f);
                soundGroup.audioMixer.SetFloat(soundParam, -80f);
                break;
        }
    }
}
