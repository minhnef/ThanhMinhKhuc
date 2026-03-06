using TMPro;
using UnityEngine;

public class NotitManage : MonoBehaviour
{
    public static NotitManage Instance { get; private set; }
    public TextMeshProUGUI textMeshProUGUI;
    private Animator animator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Instance = this;
        // animator = GetComponent<Animator>();
        gameObject.SetActive(false);
    }

    public void ShowNotification(string message)
    {
        gameObject.SetActive(true);
        textMeshProUGUI.text = message;
    }
}
