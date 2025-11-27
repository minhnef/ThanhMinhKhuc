using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [Header("NPC infor")]
    public Sprite avatar;
    public string npcName;
    [TextArea(2,5)]
    public string[] dialogues;
}
