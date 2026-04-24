using UnityEngine;
using TMPro;

public class DialogManager : MonoBehaviour
{
    public GameObject dialogUI;
    public TextMeshProUGUI dialogText;

    public string[] lines;
    int currentLine = 0;

    bool isTalking = false;

    public GameObject bossPrefab;
    public Transform spawnPoint;

    bool canTalk = false;

    public GameObject talkHint;
    public GameObject wallPrefab;
    public Transform wallSpawnPoint;

    void Update()
{
    if (canTalk && Input.GetKeyDown(KeyCode.F))
    {
        if (!isTalking)
        {
            StartDialog();
        }
        else
        {
            NextLine();
        }
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

    void EndDialog(bool spawnBoss = true)
{
    dialogUI.SetActive(false);
    isTalking = false;

    if (spawnBoss)
        SpawnBoss();
}

    void SpawnBoss()
{
    Instantiate(bossPrefab, spawnPoint.position, Quaternion.identity);

    // 👇 สร้างกำแพงปิดทาง
    Instantiate(wallPrefab, wallSpawnPoint.position, wallSpawnPoint.rotation);

    gameObject.SetActive(false);
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
        {
            EndDialog(false); // ✅ แก้ตรงนี้
        }
    }
}
}