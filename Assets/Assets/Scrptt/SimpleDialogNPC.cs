using UnityEngine;
using TMPro;

public class SimpleDialogNPC : MonoBehaviour
{
    public GameObject dialogUI;
    public TextMeshProUGUI dialogText;

    public string[] lines;
    int currentLine = 0;

    bool isTalking = false;
    bool canTalk = false;

    public GameObject talkHint;

    void Update()
    {
        if (canTalk && Input.GetKeyDown(KeyCode.F))
        {
            if (!isTalking)
                StartDialog();
            else
                NextLine();
        }
    }

    void StartDialog()
    {
        isTalking = true;
        dialogUI.SetActive(true);
        talkHint.SetActive(false);

        currentLine = 0;
        dialogText.text = lines[currentLine];
    }

    void NextLine()
    {
        currentLine++;

        if (currentLine < lines.Length)
        {
            dialogText.text = lines[currentLine];
        }
        else
        {
            EndDialog();
        }
    }

    void EndDialog()
    {
        dialogUI.SetActive(false);
        isTalking = false;

        // 👇 ไม่มี spawn boss
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            canTalk = true;
            talkHint.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            canTalk = false;
            talkHint.SetActive(false);

            if (isTalking)
                EndDialog(); // ปิดเฉยๆ
        }
    }
}