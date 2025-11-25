using System;
using TMPro;
using UnityEngine;

public class DialogueUI : MonoBehaviour
{
   [SerializeField]
   private GameObject dialoguePanel;
   [SerializeField]
   private string[]lines;

   public TextMeshProUGUI dialogueText;

   private int index;

   public bool isOpen =>dialoguePanel.activeSelf;

    void Start()
    {
        dialoguePanel.SetActive(false);
    }

    public void ShowDialogue(string[] dialogueLines)
    {
        lines = dialogueLines;
        index = 0;
        dialoguePanel.SetActive(true);
        ShowLine();
    }

    private void ShowLine()
    {
       dialogueText.text = lines[index];
    }

    public void NextLine()
    {
        index++;
        if (index < lines.Length)
        {
            ShowLine();
        }
        else
        {
            dialoguePanel.SetActive(false);
        }
    }
}
