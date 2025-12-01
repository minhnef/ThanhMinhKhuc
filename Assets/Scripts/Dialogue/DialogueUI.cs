using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField]
    private GameObject dialoguePanel;
    public Image npcImage;
    public TextMeshProUGUI npcNameText;
    public TextMeshProUGUI dialogueText;

    [Header("Typing setting")]
    public float typingSpeed = 0.3f;

    [SerializeField]
    private string[] lines;
    private int index;
    private bool isTyping;
    private Coroutine typingCoroutine;

    public bool isOpen => dialoguePanel.activeSelf;

    void Start()
    {
        dialoguePanel.SetActive(false);
    }

    public void StartDialogue(DialogueTrigger npcDialogue)
    {
        npcImage.sprite = npcDialogue.avatar;
        npcNameText.text = npcDialogue.npcName;
        lines = npcDialogue.dialogues;
        index = 0;
        dialoguePanel.SetActive(true);
        StartTyping(lines[index]);
    }

    private void StartTyping(string line)
    {
        Debug.Log(line);
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }
        typingCoroutine = StartCoroutine(TypeLine(line));
    }

    private IEnumerator TypeLine(string line)
    {
        isTyping = true;
        dialogueText.text = "";

        foreach (char c in line)
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }
        isTyping = false;
    }

    public void NextLine()
    {
        if (isTyping)
        {
            StopCoroutine(typingCoroutine);

            dialogueText.text = lines[index];
            isTyping = false;
            return;
        }
        index++;
        if (index < lines.Length)
        {
            StartTyping(lines[index]);
        }
        else
        {
            CloseDialogue();
        }
    }

    private void CloseDialogue()
    {
        dialoguePanel.SetActive(false);
    }
}
