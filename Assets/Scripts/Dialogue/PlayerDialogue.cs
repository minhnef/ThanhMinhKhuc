using UnityEngine;

public class PlayerDialogue : MonoBehaviour
{
    [SerializeField]
    private DialogueUI dialogueUI;
    private DialogueTrigger currentNPC;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && currentNPC != null)
        {
            if (dialogueUI.isOpen)
            {

                dialogueUI.NextLine();
            }
            else
            {
                dialogueUI.StartDialogue(currentNPC);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("NPC"))
        {
            currentNPC = collision.GetComponent<DialogueTrigger>();
            // dialogueUI.npcImage.sprite = currentNPC.avatar;
            // dialogueUI.npcNameText
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("NPC"))
        {
            currentNPC = null;
        }
    }


}
