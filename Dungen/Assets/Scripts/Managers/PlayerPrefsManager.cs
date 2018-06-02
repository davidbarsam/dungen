using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlayerPrefsManager : MonoBehaviour
{
    
    [Header("Buttons")]
    public Toggle batterySaverToggle;
    public Toggle muteAllSoundsToggle;
    public Slider masterSoundSlider;
    public Slider musicSoundSlider;
    public Slider soundsSlider;
    public Button exitAndSaveButton;

    public delegate void OnSettingsChanged();
    public static OnSettingsChanged onSettingsChanged;

    private void Start()
    {
        RefreshSettings();
        onSettingsChanged += ChangeQuality;
    }

    private void ChangeQuality()
    {
        switch (GetSettingsBatterySaver())
        {
            case false:
                QualitySettings.vSyncCount = 1;
                break;
            case true:
                QualitySettings.vSyncCount = 2;
                break;
        }
    }

    #region Public Interface Functions

    public void RefreshSettings()
    {
        batterySaverToggle.isOn = GetSettingsBatterySaver();
        muteAllSoundsToggle.isOn = GetSettingsMuteAllSounds();
        masterSoundSlider.value = GetSettingsMasterValue();
        musicSoundSlider.value = GetSettingsMusicValue();
        soundsSlider.value = GetSettingsSoundValue();
        onSettingsChanged();
    }

    public void SaveBatterySaver()
    {
        SetSettingsBatterySaver(batterySaverToggle.isOn);
        onSettingsChanged();
    }

    public void SaveMuteAllSounds()
    {
        SetSettingsMuteAllSounds(muteAllSoundsToggle.isOn);
        onSettingsChanged();
    }

    public void SaveMasterSlider()
    {
        SetSettingsMasterValue(masterSoundSlider.value);
        onSettingsChanged();
    }

    public void SaveMusicSlider()
    {
        SetSettingsMusicValue(musicSoundSlider.value);
        onSettingsChanged();
    }

    public void SaveSoundSlider()
    {
        SetSettingsSoundValue(soundsSlider.value);
        onSettingsChanged();
    }

    public void ResetAllPlayerPrefs()
    {
        // Individually deleting keys is safer than using PlayerPrefs.DeleteAll().
        PlayerPrefs.DeleteKey(PERSONAL_HIGH_SCORE);
        PlayerPrefs.DeleteKey(SETTINGS_BATTERY_SAVER);
        PlayerPrefs.DeleteKey(SETTINGS_MUTE_ALL);
        PlayerPrefs.DeleteKey(SETTINGS_MASTER_VALUE);
        PlayerPrefs.DeleteKey(SETTINGS_MUSIC_VALUE);
        PlayerPrefs.DeleteKey(SETTINGS_SOUND_VALUE);

        RefreshSettings();
    }

    public void NewPlayerDialogDone()
    {
        SetNewPlayer(false);
    }

    #endregion

    #region Personal
    // Personal Statistics
    const string PERSONAL_HIGH_SCORE = "personal_high_score";

    public static void SetPersonalHighScore(float input)
    {
        PlayerPrefs.SetFloat(PERSONAL_HIGH_SCORE, input);
    }
    public static float GetPersonalHighScore()
    {
        if (PlayerPrefs.HasKey(PERSONAL_HIGH_SCORE))
        {
            return PlayerPrefs.GetFloat(PERSONAL_HIGH_SCORE);
        }
        return 0;
    }
    #endregion

    #region Settings
    // Settings
    const string SETTINGS_BATTERY_SAVER = "settings_battery_saver";
    const string SETTINGS_MUTE_ALL = "settings_mute_all";
    const string SETTINGS_MASTER_VALUE = "settings_master_value";
    const string SETTINGS_MUSIC_VALUE = "settings_music_value";
    const string SETTINGS_SOUND_VALUE = "settings_sound_value";

    public static void SetSettingsBatterySaver(bool input)
    {
        switch (input)
        {
            case false:
                PlayerPrefs.SetInt(SETTINGS_BATTERY_SAVER, 0);
                break;
            case true:
                PlayerPrefs.SetInt(SETTINGS_BATTERY_SAVER, 1);
                break;
        }
    }
    public static bool GetSettingsBatterySaver()
    {
        if (PlayerPrefs.HasKey(SETTINGS_BATTERY_SAVER))
        {
            switch (PlayerPrefs.GetInt(SETTINGS_BATTERY_SAVER))
            {
                case 0:
                    return false;
                case 1:
                    return true;
            }
        }

        return false;
    }

    public static void SetSettingsMuteAllSounds(bool input)
    {
        switch (input)
        {
            case false:
                PlayerPrefs.SetInt(SETTINGS_MUTE_ALL, 0);
                break;
            case true:
                PlayerPrefs.SetInt(SETTINGS_MUTE_ALL, 1);
                break;
        }
    }
    public static bool GetSettingsMuteAllSounds()
    {
        if (PlayerPrefs.HasKey(SETTINGS_MUTE_ALL))
        {
            switch (PlayerPrefs.GetInt(SETTINGS_MUTE_ALL))
            {
                case 0:
                    return false;
                case 1:
                    return true;
            }
        }
        return false;
    }

    public static void SetSettingsMasterValue(float input)
    {
        PlayerPrefs.SetFloat(SETTINGS_MASTER_VALUE, input);
    }
    public static float GetSettingsMasterValue()
    {
        if (PlayerPrefs.HasKey(SETTINGS_MASTER_VALUE))
        {
            return PlayerPrefs.GetFloat(SETTINGS_MASTER_VALUE);
        }

        return 0f;
    }

    public static void SetSettingsMusicValue(float input)
    {
        PlayerPrefs.SetFloat(SETTINGS_MUSIC_VALUE, input);
    }
    public static float GetSettingsMusicValue()
    {
        if (PlayerPrefs.HasKey(SETTINGS_MUSIC_VALUE))
        {
            return PlayerPrefs.GetFloat(SETTINGS_MUSIC_VALUE);
        }

        return 0f;
    }

    public static void SetSettingsSoundValue(float input)
    {
        PlayerPrefs.SetFloat(SETTINGS_SOUND_VALUE, input);
    }
    public static float GetSettingsSoundValue()
    {
        if (PlayerPrefs.HasKey(SETTINGS_SOUND_VALUE))
        {
            return PlayerPrefs.GetFloat(SETTINGS_SOUND_VALUE);
        }

        return 0f;
    }
    #endregion

    #region Tutorial

    private const string NEW_PLAYER = "new_player";

    public static void SetNewPlayer(bool input)
    {
        switch (input)
        {
                case false:
                    PlayerPrefs.SetInt(NEW_PLAYER, 0);
                    break;
                case true:
                    PlayerPrefs.SetInt(NEW_PLAYER, 1);
                    break;
        }
    }
    // Returns true by default, as there won't be a key for new players
    public static bool GetNewPlayer()
    {
        if (PlayerPrefs.HasKey(NEW_PLAYER))
        {
            switch (PlayerPrefs.GetInt(NEW_PLAYER))
            {
                case 0:
                    return false;
                case 1:
                    return true;
            }
        }

        return true;

    }

    #endregion
}
