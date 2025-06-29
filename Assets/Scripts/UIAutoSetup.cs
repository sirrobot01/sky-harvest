using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class UIAutoSetup
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    static void AutoSetupUI()
    {
        // This runs automatically after every scene loads
        Debug.Log($"Auto-setting up UI for scene: {SceneManager.GetActiveScene().name}");
        
        // Find or create UI setup manager
        GameObject setupManager = GameObject.Find("UIAutoSetupManager");
        if (setupManager == null)
        {
            setupManager = new GameObject("UIAutoSetupManager");
            setupManager.AddComponent<UIAutoSetupManager>();
        }
        
        // Apply UI enhancements
        UIAutoSetupManager manager = setupManager.GetComponent<UIAutoSetupManager>();
        if (manager != null)
        {
            manager.SetupUIForScene();
        }
    }
}

public class UIAutoSetupManager : MonoBehaviour
{
    [Header("Auto Setup Settings")]
    public bool debugMode = true;
    
    private void Start()
    {
        // Setup UI when this component starts
        SetupUIForScene();
    }
    
    public void SetupUIForScene()
    {
        if (debugMode)
            Debug.Log("Starting automatic UI setup...");
            
        int buttonsUpgraded = 0;
        int textElementsUpgraded = 0;
        int themeAppliersAdded = 0;
        
        // Step 1: Upgrade all buttons to EnhancedButton
        buttonsUpgraded = UpgradeAllButtons();
        
        // Step 2: Upgrade all text elements to EnhancedTextDisplay  
        textElementsUpgraded = UpgradeAllTextElements();
        
        // Step 3: Add theme appliers to canvases
        themeAppliersAdded = AddThemeAppliers();
        
        // Step 4: Apply theme
        ApplyThemeToAll();
        
        if (debugMode)
        {
            Debug.Log($"UI Auto-Setup Complete! Upgraded {buttonsUpgraded} buttons, {textElementsUpgraded} texts, added {themeAppliersAdded} theme appliers");
        }
        
        // Clean up this temporary setup object
        Destroy(gameObject, 1f);
    }
    
    int UpgradeAllButtons()
    {
        UnityEngine.UI.Button[] allButtons = FindObjectsByType<UnityEngine.UI.Button>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        int upgraded = 0;
        
        foreach (UnityEngine.UI.Button button in allButtons)
        {
            // Skip if already has EnhancedButton
            if (button.GetComponent<EnhancedButton>() != null) continue;
            
            EnhancedButton enhanced = button.gameObject.AddComponent<EnhancedButton>();
            ConfigureEnhancedButton(enhanced, button);
            upgraded++;
        }
        
        return upgraded;
    }
    
    void ConfigureEnhancedButton(EnhancedButton enhanced, UnityEngine.UI.Button originalButton)
    {
        // Set defaults
        enhanced.buttonStyle = EnhancedButton.ButtonStyle.Primary;
        enhanced.buttonSize = EnhancedButton.ButtonSize.Medium;
        enhanced.useCustomSprites = true; // Use your PNG assets
        
        // Configure based on button name/context
        string buttonName = originalButton.gameObject.name.ToLower();
        
        if (buttonName.Contains("start") || buttonName.Contains("play"))
        {
            enhanced.buttonStyle = EnhancedButton.ButtonStyle.Success;
            enhanced.buttonSize = EnhancedButton.ButtonSize.Large;
        }
        else if (buttonName.Contains("quit") || buttonName.Contains("exit"))
        {
            enhanced.buttonStyle = EnhancedButton.ButtonStyle.Danger;
            enhanced.buttonSize = EnhancedButton.ButtonSize.Large;
        }
        else if (buttonName.Contains("retry") || buttonName.Contains("again"))
        {
            enhanced.buttonStyle = EnhancedButton.ButtonStyle.Primary;
            enhanced.buttonSize = EnhancedButton.ButtonSize.Medium;
        }
        else if (buttonName.Contains("menu") || buttonName.Contains("back"))
        {
            enhanced.buttonStyle = EnhancedButton.ButtonStyle.Secondary;
            enhanced.buttonSize = EnhancedButton.ButtonSize.Medium;
        }
    }
    
    int UpgradeAllTextElements()
    {
        TMPro.TextMeshProUGUI[] allTexts = FindObjectsByType<TMPro.TextMeshProUGUI>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        int upgraded = 0;
        
        foreach (TMPro.TextMeshProUGUI text in allTexts)
        {
            // Skip if already has EnhancedTextDisplay
            if (text.GetComponent<EnhancedTextDisplay>() != null) continue;
            
            // Skip button text (handled by EnhancedButton)
            if (text.GetComponentInParent<UnityEngine.UI.Button>() != null) continue;
            
            EnhancedTextDisplay enhanced = text.gameObject.AddComponent<EnhancedTextDisplay>();
            ConfigureEnhancedText(enhanced, text);
            upgraded++;
        }
        
        return upgraded;
    }
    
    void ConfigureEnhancedText(EnhancedTextDisplay enhanced, TMPro.TextMeshProUGUI originalText)
    {
        // Set defaults
        enhanced.textStyle = EnhancedTextDisplay.TextStyle.Body;
        enhanced.textColor = EnhancedTextDisplay.TextColor.Primary;
        
        // Configure based on text name/content/size
        string textName = originalText.gameObject.name.ToLower();
        string textContent = originalText.text.ToLower();
        float fontSize = originalText.fontSize;
        
        // Determine text style based on context
        if (textName.Contains("title") || fontSize > 40f)
        {
            enhanced.textStyle = EnhancedTextDisplay.TextStyle.Heading1;
            enhanced.enablePulse = true;
        }
        else if (textName.Contains("score") || textContent.Contains("score"))
        {
            enhanced.textStyle = EnhancedTextDisplay.TextStyle.Score;
            enhanced.enableShadow = true;
        }
        else if (textName.Contains("timer") || textName.Contains("time"))
        {
            enhanced.textStyle = EnhancedTextDisplay.TextStyle.Timer;
            enhanced.enableShadow = true;
        }
        else if (textName.Contains("rank") || textContent.Contains("★"))
        {
            enhanced.textStyle = EnhancedTextDisplay.TextStyle.Heading2;
            enhanced.enablePulse = true;
        }
    }
    
    int AddThemeAppliers()
    {
        Canvas[] allCanvases = FindObjectsByType<Canvas>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        int added = 0;
        
        foreach (Canvas canvas in allCanvases)
        {
            // Skip if already has UIThemeApplier
            if (canvas.GetComponent<UIThemeApplier>() != null) continue;
            
            UIThemeApplier applier = canvas.gameObject.AddComponent<UIThemeApplier>();
            applier.applyOnStart = true;
            applier.applyToChildren = true;
            added++;
        }
        
        return added;
    }
    
    void ApplyThemeToAll()
    {
        UIThemeApplier[] appliers = FindObjectsByType<UIThemeApplier>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        
        foreach (UIThemeApplier applier in appliers)
        {
            applier.ApplyTheme();
        }
    }
}