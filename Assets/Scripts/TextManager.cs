using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TextManager : MonoBehaviour
{
    [Header("UI Images")]
    public GameObject gameOverImage;
    public GameObject levelCompleteImage;
    public GameObject slowDownImage;
    public GameObject speedUpImage;

    [Header("Buttons")]
    public GameObject retryButton;
    public GameObject titleButton;

    public GameObject retryButton_Complete;
    public GameObject titleButton_Complete;

    [Header("Timing")]
    public float displayDuration = 2f;

    public void ShowGameOver()
    {
        gameOverImage.SetActive(true);
        retryButton.SetActive(true);
        titleButton.SetActive(true);
    }

    public void ShowLevelComplete()
    {
        levelCompleteImage.SetActive(true);
        retryButton_Complete.SetActive(true);
        titleButton_Complete.SetActive(true);
    }

    public void ShowSlowDown() => StartCoroutine(ShowTemporary(slowDownImage));
    public void ShowSpeedUp() => StartCoroutine(ShowTemporary(speedUpImage));

    private IEnumerator ShowTemporary(GameObject imageObject)
    {
        imageObject.SetActive(true);
        yield return new WaitForSeconds(displayDuration);
        imageObject.SetActive(false);
    }

    // BUTTON EVENTS
    public void RetryLevel()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    public void GoToTitleScreen()
    {
        SceneManager.LoadScene("TitleScreen");
    }
}




