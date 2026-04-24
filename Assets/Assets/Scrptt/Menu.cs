using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject mainMenuPanel;
    public GameObject creditPanel;

    public void StartGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

    // 👉 เปิด Credit
    public void ShowCredits()
    {
        mainMenuPanel.SetActive(false);
        creditPanel.SetActive(true);
    }

    // 👉 กลับไป Menu
    public void BackToMenu()
    {
        creditPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }

    // 👉 กลับไปหน้าเมนูจาก Scene อื่น
    public void LoadMenuScene()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void CloseCredits()
{
    creditPanel.SetActive(false);
    mainMenuPanel.SetActive(true);
}
}