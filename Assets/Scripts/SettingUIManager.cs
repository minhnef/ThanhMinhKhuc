using System;
using UnityEngine;
using UnityEngine.UI;

public class SettingUIManager : MonoBehaviour
{
    [Header("Popup UI Elements")]
    [SerializeField]
    private Slider musicSlider;
    [SerializeField]
    private Slider sfxSlider;

    private const string MUSIC_VOLUME_KEY = "musicVolume";
    private const string SFX_VOLUME_KEY = "sfxVolume";
    

    void Start()
    {

        
        float musicVol = PlayerPrefs.GetFloat(MUSIC_VOLUME_KEY, 1f);
        float sfxVol = PlayerPrefs.GetFloat(SFX_VOLUME_KEY, 1f);

        musicSlider.value = musicVol;
        sfxSlider.value = sfxVol;

        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);

        SetMusicVolume(musicVol);
        SetSFXVolume(sfxVol);

    }

    private void SetMusicVolume(float vol)
    {
        PlayerPrefs.SetFloat(MUSIC_VOLUME_KEY, vol);
        PlayerPrefs.Save();
        SFXManager.instance?.SetVolumMusicSources(vol);
    }

    private void SetSFXVolume(float vol)
    {
        PlayerPrefs.SetFloat(SFX_VOLUME_KEY, vol);
        PlayerPrefs.Save();
        SFXManager.instance?.SetVolumSFXSources(vol);

    }

    public void OnBtnAboutClicked()
    {
        Debug.Log("About button clicked.");
        // Implementation depends on your UI setup
        var aboutPrefab = Resources.Load<GameObject>("Prefabs/AboutPanel");
        Instantiate(aboutPrefab);
    }

    public void OnBtnQuitClicked()
    {
        Debug.Log("Quit button clicked.");
        Application.Quit();
    }

    public void OnBtnCloseClicked()
    {
        Destroy(gameObject);
        Time.timeScale = 1f;
    }

}
