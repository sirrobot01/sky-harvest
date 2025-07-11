using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI targetScoreText;
    [SerializeField]
    public GameObject scorePopUpPrefab;
    
    [Header("Pop-Up Settings")]
    public Transform popUpCanvasTransform; // Use a regular Transform for World Space

    public int currentScore = 0;
    
    [Header("Enhanced UI")]
    // Set to false by default to ensure the standard score text updates.
    public bool useEnhancedUI = false; 
    
    private EnhancedTextDisplay scoreDisplay;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }
    
    void Start()
    {
        // Set the initial score display to zero when the game starts.
        UpdateScoreDisplay(); 
        SetupEnhancedUI();

        UpdateTargetScoreDisplay();
    }
    
    void SetupEnhancedUI()
    {
        if (useEnhancedUI && scoreText != null)
        {
            scoreDisplay = scoreText.gameObject.GetComponent<EnhancedTextDisplay>();
            if (scoreDisplay == null)
            {
                scoreDisplay = scoreText.gameObject.AddComponent<EnhancedTextDisplay>();
                scoreDisplay.textStyle = EnhancedTextDisplay.TextStyle.Score;
                scoreDisplay.textColor = EnhancedTextDisplay.TextColor.Score;
                scoreDisplay.enableShadow = true;
                scoreDisplay.enableOutline = true;
            }
        }
    }

    public void AddScore(int amount, Vector3 worldPosition)
    {
        currentScore += amount;
        UpdateScoreDisplay();

        if (scorePopUpPrefab != null && popUpCanvasTransform != null)
        {
            // Set Z-depth for pop-up to ensure it appears in front of the background.
            float popUpZ = -1f;
            Vector3 spawnPosition = new Vector3(worldPosition.x, worldPosition.y, popUpZ);

            // Instantiate the pop-up prefab at the fruit's location and parent it to the canvas.
            GameObject popUp = Instantiate(scorePopUpPrefab, spawnPosition, Quaternion.identity, popUpCanvasTransform);
            
            // Get the pop-up's script and set the score text.
            ScorePopUp popUpScript = popUp.GetComponent<ScorePopUp>();
            if (popUpScript != null)
            {
                popUpScript.SetText("+" + amount.ToString());
            }
            else
            {
                Debug.LogError("ScorePopUp script is missing on the instantiated prefab!");
            }
        }
    }
    
    void UpdateTargetScoreDisplay()
    {
        if (targetScoreText != null && GameManager.instance != null)
        {
            targetScoreText.text = "Target: " + GameManager.instance.scoreRequirement.ToString("N0");
        }
    }
    void UpdateScoreDisplay()
    {
        if (useEnhancedUI && scoreDisplay != null)
        {
            scoreDisplay.UpdateScore(currentScore);
        }
        else if (scoreText != null)
        {
            scoreText.text = "Score: " + currentScore.ToString("N0");
        }
    }

    public int GetScore()
    {
        return currentScore;
    }
}

