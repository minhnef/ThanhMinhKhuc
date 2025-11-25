using UnityEngine;

public class PlayerDialogue : MonoBehaviour
{
    [SerializeField]
    private DialogueUI dialogueUI;
    private DialogueTrigger npcDialogue;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.E)&&npcDialogue!=null)
        {
            if (dialogueUI.isOpen)
            {
                dialogueUI.NextLine();
            }
            else
            {
                dialogueUI.ShowDialogue(npcDialogue.dialogues);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("NPC"))
        {
            npcDialogue = collision.GetComponent<DialogueTrigger>();
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("NPC"))
        {
            npcDialogue=null;
        }
    }


}
