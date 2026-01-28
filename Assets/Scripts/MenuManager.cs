using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private  Button playBtn, settingBtn;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playBtn.onClick.AddListener(OnPlayBtnClicked);
        settingBtn.onClick.AddListener(OnSettingBtnClicked);
    }

    void OnPlayBtnClicked()
    {
        SceneManager.LoadScene("Map1");
    }

    void OnSettingBtnClicked()
    {
        var setting = Resources.Load<GameObject>("SettingCanvas");
        Instantiate(setting);
    }
}
