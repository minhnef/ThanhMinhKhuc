using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SFXManager : MonoBehaviour
{
    [SerializeField]
    private AudioSource mussicSource;
    [SerializeField]
    private AudioSource sfxSource; // Assign AudioSources in the Inspector

    [SerializeField]
    private AudioClip[] sfxClips; // Assign AudioClips in the Inspector
    public static SFXManager instance;
    private const string SFX_VOLUME_KEY = "sfxVolume";

    private const string MUSIC_VOLUME_KEY = "musicVolume";


    void Awake()
    {
        instance = this;

        SceneManager.sceneLoaded += OnSceneLoaded;


        InitVolume();
    }



    void Start()
    {
        BindAllButtons();
    }

    void InitVolume()
    {
        float sfxVolume = PlayerPrefs.GetFloat(SFX_VOLUME_KEY, 1.0f);
        float musicVolume = PlayerPrefs.GetFloat(MUSIC_VOLUME_KEY, 1.0f);
        SetVolumSFXSources(sfxVolume);
        SetVolumMusicSources(musicVolume);
    }

    public void SetVolumSFXSources(float volume)
    {

        if (sfxSource != null)
        {
            sfxSource.volume = volume;
            PlayerPrefs.SetFloat(SFX_VOLUME_KEY, volume);
            PlayerPrefs.Save();
        }
    }

    public void SetVolumMusicSources(float volume)
    {
        if (mussicSource != null)
        {
            mussicSource.volume = volume;
            PlayerPrefs.SetFloat(MUSIC_VOLUME_KEY, volume);
            PlayerPrefs.Save();
        }
    }

    public void PlaySFX(SFXType sfxType, bool isLoop = false)
    {
        // Debug.Log($"Play SFX {sfxType}");
        sfxSource.Stop();
        int index = (int)sfxType;
        if (index < 0 || index >= sfxClips.Length)
        {
            Debug.LogError("Invalid SFXType index: " + index);
            return;
        }

        AudioClip clip = sfxClips[index];
        if (clip != null)
        {
            sfxSource.loop = isLoop;
            if (isLoop)
            {
                sfxSource.clip = clip;
                sfxSource.Play();
            }
            else
            {
                sfxSource.PlayOneShot(clip);
            }
        }
        else
        {
            Debug.LogError("AudioSource for " + sfxType + " is null.");
        }
    }

    public void PlaySFX(AudioClip clip, float pitch = 1f, bool isLoop = false)
    {
        if (sfxSource == null) return;

        sfxSource.Stop();
        sfxSource.loop = isLoop;
        sfxSource.pitch = pitch;
        if (isLoop)
        {
            sfxSource.clip = clip;
            sfxSource.Play();
        }
        else
        {
            sfxSource.PlayOneShot(clip);
        }
    }

    public float GetVolumSFX()
    {
        return PlayerPrefs.GetFloat(SFX_VOLUME_KEY, 1.0f); ;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        BindAllButtons();
    }

    private void BindAllButtons()
    {
        Button[] buttons = FindObjectsByType<Button>(FindObjectsSortMode.None);
        foreach (Button btn in buttons)
        {
            if (btn != null)
            {
                btn.onClick.RemoveListener(PlayButtonClick);
                btn.onClick.AddListener(PlayButtonClick);
            }
        }
        // Debug.Log($"Bound SFX to {buttons.Length} buttons in scene {SceneManager.GetActiveScene().name}");
    }

    private void PlayButtonClick()
    {
        PlaySFX(SFXType.BUTTON_CLICK);
    }

}
public enum SFXType
{
    BUTTON_CLICK,
    SLASH1,
    SLASH2,
    TIGER_SCRAP,
    TIGER_ROAR,
    TELEPORT,
    DASH,

}