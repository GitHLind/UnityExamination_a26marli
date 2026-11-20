
using UnityEngine;
using UnityEngine.Events;
using TMPro;
 
public class DialogueNPC : MonoBehaviour
{
    public string npcName = "NPC";
    [TextArea(2, 4)] public string[] lines;
    [TextArea(2, 4)] public string[] alternateLines;
    public int alternateScore = 1;
    public string playerTag = "Player";
 
    public GameObject talkButton;
    public GameObject dialoguePanel;
    public TMP_Text nameText;
    public TMP_Text lineText;
 
    public UnityEvent onDialogueEnd;
    public UnityEvent onDialogueStart;
 
    string[] currentLines;
    int index;
    bool playerNear;
    bool talking;
 
    void Start()
    {
        if (talkButton != null) talkButton.SetActive(false);
        if (dialoguePanel != null) dialoguePanel.SetActive(false);
    }
 
    string[] PickLines()
    {
        bool useAlternate = alternateLines != null && alternateLines.Length > 0 &&
                            ScoreCounter.Instance != null &&
                            ScoreCounter.Instance.Count >= alternateScore;
        return useAlternate ? alternateLines : lines;
    }
 
    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag(playerTag)) return;
 
        playerNear = true;
        if (!talking && talkButton != null) talkButton.SetActive(true);
    }
 
    void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag(playerTag)) return;
 
        playerNear = false;
        if (talkButton != null) talkButton.SetActive(false);
        if (talking) EndDialogue(false);
    }
 
    public void StartDialogue()
    {
        if (!playerNear || talking) return;
 
        currentLines = PickLines();
        if (currentLines.Length == 0) return;
 
        talking = true;
        index = 0;
        if (talkButton != null) talkButton.SetActive(false);
        if (dialoguePanel != null) dialoguePanel.SetActive(true);
        if (nameText != null) nameText.text = npcName;
        if (lineText != null) lineText.text = currentLines[index];
        onDialogueStart.Invoke();
    }
 
    public void NextLine()
    {
        if (!talking) return;
 
        index++;
        if (index >= currentLines.Length)
        {
            EndDialogue(true);
            return;
        }
 
        if (lineText != null) lineText.text = currentLines[index];
    }
 
    void EndDialogue(bool finished)
    {
        talking = false;
        if (dialoguePanel != null) dialoguePanel.SetActive(false);
        if (playerNear && talkButton != null) talkButton.SetActive(true);
        if (finished) onDialogueEnd.Invoke();
    }
}
 