using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UISystemIntegrator : MonoBehaviour
{
    [Header("Integration Settings")]
    [Tooltip("Automatically upgrade all UI elements in the scene")]
    public bool autoUpgradeOnStart = true;
    
    [Tooltip("Apply theme after upgrading components")]
    public bool applyThemeAfterUpgrade = true;
    
    [Header("Upgrade Options")]
    public bool upgradeButtons = true;
    public bool upgradeTextElements = true;
    public bool addThemeAppliers = true;
    
    [Header("Default Button Settings")]
    public EnhancedButton.ButtonStyle defaultButtonStyle = EnhancedButton.ButtonStyle.Primary;
    public EnhancedButton.ButtonSize defaultButtonSize = EnhancedButton.ButtonSize.Medium;
    
    [Header("Default Text Settings")]
    public EnhancedTextDisplay.TextStyle defaultTextStyle = EnhancedTextDisplay.TextStyle.Body;
    public EnhancedTextDisplay.TextColor defaultTextColor = EnhancedTextDisplay.TextColor.Primary;
    
    void Start()
    {
        if (autoUpgradeOnStart)
        {
            UpgradeAllUIElements();
        }
    }
    
    [ContextMenu("Upgrade All UI Elements")]
    public void UpgradeAllUIElements()
    {
        Debug.Log("Starting UI system integration...");
        
        int buttonsUpgraded = 0;
        int textElementsUpgraded = 0;
        int themeAppliersAdded = 0;
        
        if (upgradeButtons)
        {
            buttonsUpgraded = UpgradeAllButtons();
        }
        
        if (upgradeTextElements)
        {
            textElementsUpgraded = UpgradeAllTextElements();
        }
        
        if (addThemeAppliers)
        {
            themeAppliersAdded = AddThemeAppliers();
        }
        
        if (applyThemeAfterUpgrade)
        {
            ApplyThemeToAll();
        }
        
        Debug.Log($"UI Integration Complete! Upgraded {buttonsUpgraded} buttons, {textElementsUpgraded} text elements, added {themeAppliersAdded} theme appliers.");
    }
    
    int UpgradeAllButtons()
    {
        Button[] allButtons = FindObjectsByType<Button>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        int upgraded = 0;
        
        foreach (Button button in allButtons)
        {
            // Skip if already has EnhancedButton
            if (button.GetComponent<EnhancedButton>() != null) continue;
            
            EnhancedButton enhanced = button.gameObject.AddComponent<EnhancedButton>();
            
            // Configure based on button name or context
            ConfigureEnhancedButton(enhanced, button);
            
            upgraded++;
            Debug.Log($"Upgraded button: {button.gameObject.name}");
        }
        
        return upgraded;
    }
    
    void ConfigureEnhancedButton(EnhancedButton enhanced, Button originalButton)
    {
        // Set defaults
        enhanced.buttonStyle = defaultButtonStyle;
        enhanced.buttonSize = defaultButtonSize;
        
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
        
        // Set button text from existing text component
        TextMeshProUGUI textComponent = originalButton.GetComponentInChildren<TextMeshProUGUI>();
        if (textComponent != null && !string.IsNullOrEmpty(textComponent.text))
        {
            enhanced.SetButtonText(textComponent.text.ToUpper());
        }
    }
    
    int UpgradeAllTextElements()
    {
        TextMeshProUGUI[] allTexts = FindObjectsByType<TextMeshProUGUI>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        int upgraded = 0;
        
        foreach (TextMeshProUGUI text in allTexts)
        {
            // Skip if already has EnhancedTextDisplay
            if (text.GetComponent<EnhancedTextDisplay>() != null) continue;
            
            // Skip button text (handled by EnhancedButton)
            if (text.GetComponentInParent<Button>() != null) continue;
            
            EnhancedTextDisplay enhanced = text.gameObject.AddComponent<EnhancedTextDisplay>();
            
            // Configure based on text content or context
            ConfigureEnhancedText(enhanced, text);
            
            upgraded++;
            Debug.Log($"Upgraded text element: {text.gameObject.name}");
        }
        
        return upgraded;
    }
    
    void ConfigureEnhancedText(EnhancedTextDisplay enhanced, TextMeshProUGUI originalText)
    {
        // Set defaults
        enhanced.textStyle = defaultTextStyle;
        enhanced.textColor = defaultTextColor;
        
        // Configure based on text name/content/size
        string textName = originalText.gameObject.name.ToLower();
        string textContent = originalText.text.ToLower();
        float fontSize = originalText.fontSize;
        
        // Determine text style based on context
        if (textName.Contains("title") || fontSize > 40f)
        {
            enhanced.textStyle = EnhancedTextDisplay.TextStyle.Heading1;
            enhanced.textColor = EnhancedTextDisplay.TextColor.Primary;
            enhanced.enablePulse = true;
        }
        else if (textName.Contains("subtitle") || textName.Contains("header") || fontSize > 30f)
        {
            enhanced.textStyle = EnhancedTextDisplay.TextStyle.Heading2;
            enhanced.textColor = EnhancedTextDisplay.TextColor.Primary;
        }
        else if (textName.Contains("score") || textContent.Contains("score"))
        {
            enhanced.textStyle = EnhancedTextDisplay.TextStyle.Score;
            enhanced.textColor = EnhancedTextDisplay.TextColor.Score;
            enhanced.enableShadow = true;
            enhanced.enableOutline = true;
        }
        else if (textName.Contains("timer") || textName.Contains("time") || textContent.Contains("time"))
        {
            enhanced.textStyle = EnhancedTextDisplay.TextStyle.Timer;
            enhanced.textColor = EnhancedTextDisplay.TextColor.Timer;
            enhanced.enableShadow = true;
        }
        else if (textName.Contains("rank") || textContent.Contains("rank") || textContent.Contains("★"))
        {
            enhanced.textStyle = EnhancedTextDisplay.TextStyle.Heading2;
            enhanced.textColor = EnhancedTextDisplay.TextColor.Success;
            enhanced.enablePulse = true;
        }
        else if (fontSize < 16f)
        {
            enhanced.textStyle = EnhancedTextDisplay.TextStyle.Caption;
            enhanced.textColor = EnhancedTextDisplay.TextColor.Secondary;
        }
        else
        {
            enhanced.textStyle = EnhancedTextDisplay.TextStyle.Body;
            enhanced.textColor = EnhancedTextDisplay.TextColor.Primary;
        }
    }
    
    int AddThemeAppliers()
    {
        // Add theme appliers to key UI containers
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
            Debug.Log($"Added theme applier to canvas: {canvas.gameObject.name}");
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
        
        Debug.Log($"Applied theme to {appliers.Length} UI containers.");
    }
    
    [ContextMenu("Create Default UI Theme")]
    public void CreateDefaultUITheme()
    {
        // This would be done manually in Unity Editor
        Debug.Log("To create a UI Theme asset:");
        Debug.Log("1. Right-click in Project window");
        Debug.Log("2. Go to Create > Sky Harvest > UI Theme");
        Debug.Log("3. Name it 'DefaultUITheme'");
        Debug.Log("4. Place it in Resources folder");
        Debug.Log("5. Configure colors and fonts as desired");
    }
    
    [ContextMenu("Reset All UI Elements")]
    public void ResetAllUIElements()
    {
        // Remove all enhanced components (for testing)
        EnhancedButton[] buttons = FindObjectsByType<EnhancedButton>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        for (int i = 0; i < buttons.Length; i++)
        {
            DestroyImmediate(buttons[i]);
        }
        
        EnhancedTextDisplay[] texts = FindObjectsByType<EnhancedTextDisplay>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        for (int i = 0; i < texts.Length; i++)
        {
            DestroyImmediate(texts[i]);
        }
        
        UIThemeApplier[] appliers = FindObjectsByType<UIThemeApplier>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        for (int i = 0; i < appliers.Length; i++)
        {
            DestroyImmediate(appliers[i]);
        }
        
        Debug.Log("Reset all UI elements to original state.");
    }
}