using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SettingsMenu : MonoBehaviour
{
    Resolution[] resolutions;
    public TMP_Dropdown resolutionDropdown;
    public TMP_Dropdown languajeDropdown;
    public TMP_Dropdown resolutionDropdownIngame;
    public TMP_Dropdown languajeDropdownIngame;
    public AudioMixer audioMixer;

    private void Start()
    {
        GetResolutions();
        GetLanguaje();
    }

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("volume", Mathf.Log10(volume) * 20);
    }

    private void GetResolutions()
    {
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        resolutionDropdownIngame.ClearOptions();
        List<string> options = new List<string>();
        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);
            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
        resolutionDropdownIngame.AddOptions(options);
        resolutionDropdownIngame.value = currentResolutionIndex;
        resolutionDropdownIngame.RefreshShownValue();
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }
    public void GetLanguaje()
    {
        switch (PlayerPrefs.GetString("Language"))
        {
            case "en":
                languajeDropdown.value = 0;
                languajeDropdownIngame.value = 0;
                LanguageSetter.instance.ChangeFont(LanguageSetter.instance.normalFont);
                break;
            case "es":
                languajeDropdown.value = 1;
                languajeDropdownIngame.value = 1;
                LanguageSetter.instance.ChangeFont(LanguageSetter.instance.normalFont);
                break;
            case "fr":
                languajeDropdown.value = 2;
                languajeDropdownIngame.value = 2;
                LanguageSetter.instance.ChangeFont(LanguageSetter.instance.normalFont);
                break;
            case "pt":
                languajeDropdown.value = 3;
                languajeDropdownIngame.value = 3;
                LanguageSetter.instance.ChangeFont(LanguageSetter.instance.normalFont);
                break;
            case "jp":
                languajeDropdown.value = 4;
                languajeDropdownIngame.value = 4;
                LanguageSetter.instance.ChangeFont(LanguageSetter.instance.japanFont);
                break;
        }
    }
    public void SetLanguage(int languageIndex)
    {
        switch (languageIndex)
        {
            case 0:
                LanguageManager.instance.SetLanguage("en");
                LanguageSetter.instance.ChangeFont(LanguageSetter.instance.normalFont);
                break;
            case 1:
                LanguageManager.instance.SetLanguage("es");
                LanguageSetter.instance.ChangeFont(LanguageSetter.instance.normalFont);
                break;
            case 2:
                LanguageManager.instance.SetLanguage("fr");
                LanguageSetter.instance.ChangeFont(LanguageSetter.instance.normalFont);
                break;
            case 3:
                LanguageManager.instance.SetLanguage("pt");
                LanguageSetter.instance.ChangeFont(LanguageSetter.instance.normalFont);
                break;
            case 4:
                LanguageManager.instance.SetLanguage("jp");
                LanguageSetter.instance.ChangeFont(LanguageSetter.instance.japanFont);
                break;
        }
        LanguageSetter.instance.SetLanguage();
    }
    public void SetScreenMode(int id)
    {
        switch (id)
        {
            case 0:
                Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
                break;
            case 1:
                Screen.fullScreenMode = FullScreenMode.Windowed;
                break;
            case 2:
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                break;
        }
    }
        public void SetQuality(int qualityIndex)
        {
            QualitySettings.SetQualityLevel(qualityIndex);
        }

        public void Quit()
        {
            Application.Quit();
        }
    }
