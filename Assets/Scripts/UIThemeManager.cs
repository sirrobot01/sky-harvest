using UnityEngine;
using TMPro;

[CreateAssetMenu(fileName = "UI Theme", menuName = "Sky Harvest/UI Theme")]
public class UIThemeManager : ScriptableObject
{
    [Header("Color Palette")]
    public Color primaryColor = new Color(0.2f, 0.6f, 1f); // Blue
    public Color secondaryColor = new Color(0.4f, 0.4f, 0.4f); // Gray
    public Color accentColor = new Color(1f, 0.6f, 0f); // Orange
    public Color backgroundColor = new Color(0.1f, 0.1f, 0.15f); // Dark blue
    
    [Header("Text Colors")]
    public Color primaryTextColor = Color.white;
    public Color secondaryTextColor = new Color(0.7f, 0.7f, 0.7f);
    public Color successTextColor = new Color(0.2f, 0.8f, 0.2f);
    public Color warningTextColor = new Color(1f, 0.8f, 0f);
    public Color dangerTextColor = new Color(0.8f, 0.2f, 0.2f);
    
    [Header("Fonts")]
    [Tooltip("Bold font for titles (Orbitron-Bold recommended)")]
    public TMP_FontAsset headingFont;
    [Tooltip("Regular font for body text (Rubik-Regular recommended)")]
    public TMP_FontAsset bodyFont;
    [Tooltip("Bold font for buttons (Rubik-Bold recommended)")]
    public TMP_FontAsset buttonFont;
    [Tooltip("Special font for scores/timers (Orbitron-Regular recommended)")]
    public TMP_FontAsset scoreFont;
    
    [Header("Font Sizes")]
    public float h1Size = 48f;
    public float h2Size = 36f;
    public float h3Size = 28f;
    public float bodySize = 18f;
    public float captionSize = 14f;
    public float buttonSize = 20f;
    
    private static UIThemeManager _instance;
    public static UIThemeManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = Resources.Load<UIThemeManager>("DefaultUITheme");
                if (_instance == null)
                {
                    Debug.LogWarning("No UI Theme found in Resources folder. Creating default theme.");
                    _instance = CreateInstance<UIThemeManager>();
                }
            }
            return _instance;
        }
    }
    
    public void ApplyToButton(EnhancedButton button)
    {
        if (button == null) return;
        
        switch (button.buttonStyle)
        {
            case EnhancedButton.ButtonStyle.Primary:
                button.primaryColor = primaryColor;
                break;
            case EnhancedButton.ButtonStyle.Secondary:
                button.secondaryColor = secondaryColor;
                break;
            case EnhancedButton.ButtonStyle.Success:
                button.successColor = successTextColor;
                break;
            case EnhancedButton.ButtonStyle.Danger:
                button.dangerColor = dangerTextColor;
                break;
        }
    }
    
    public void ApplyToText(EnhancedTextDisplay textDisplay)
    {
        if (textDisplay == null) return;
        
        TextMeshProUGUI textComponent = textDisplay.GetComponent<TextMeshProUGUI>();
        if (textComponent == null) return;
        
        // Apply appropriate font
        switch (textDisplay.textStyle)
        {
            case EnhancedTextDisplay.TextStyle.Heading1:
            case EnhancedTextDisplay.TextStyle.Heading2:
            case EnhancedTextDisplay.TextStyle.Heading3:
                if (headingFont != null) textComponent.font = headingFont;
                break;
            case EnhancedTextDisplay.TextStyle.Button:
                if (buttonFont != null) textComponent.font = buttonFont;
                break;
            case EnhancedTextDisplay.TextStyle.Score:
            case EnhancedTextDisplay.TextStyle.Timer:
                if (scoreFont != null) textComponent.font = scoreFont;
                break;
            default:
                if (bodyFont != null) textComponent.font = bodyFont;
                break;
        }
    }
}

// Component to automatically apply theme to UI elements
public class UIThemeApplier : MonoBehaviour
{
    [Header("Auto Apply")]
    public bool applyOnStart = true;
    public bool applyToChildren = true;
    
    void Start()
    {
        if (applyOnStart)
        {
            ApplyTheme();
        }
    }
    
    public void ApplyTheme()
    {
        if (applyToChildren)
        {
            ApplyThemeToChildren();
        }
        else
        {
            ApplyThemeToThis();
        }
    }
    
    void ApplyThemeToThis()
    {
        // Apply theme to buttons
        EnhancedButton button = GetComponent<EnhancedButton>();
        if (button != null)
        {
            UIThemeManager.Instance.ApplyToButton(button);
        }
        
        // Apply theme to text
        EnhancedTextDisplay textDisplay = GetComponent<EnhancedTextDisplay>();
        if (textDisplay != null)
        {
            UIThemeManager.Instance.ApplyToText(textDisplay);
        }
    }
    
    void ApplyThemeToChildren()
    {
        // Apply to all child buttons
        EnhancedButton[] childButtons = GetComponentsInChildren<EnhancedButton>();
        foreach (EnhancedButton button in childButtons)
        {
            UIThemeManager.Instance.ApplyToButton(button);
        }
        
        // Apply to all child text displays
        EnhancedTextDisplay[] childTexts = GetComponentsInChildren<EnhancedTextDisplay>();
        foreach (EnhancedTextDisplay textDisplay in childTexts)
        {
            UIThemeManager.Instance.ApplyToText(textDisplay);
        }
    }
    
    [ContextMenu("Apply Theme Now")]
    void ApplyThemeFromContextMenu()
    {
        ApplyTheme();
    }
}