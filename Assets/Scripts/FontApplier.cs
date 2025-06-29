using UnityEngine;
using TMPro;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class FontApplier : MonoBehaviour
{
    [Header("Force Apply Fonts")]
    [Tooltip("Apply fonts from UITheme to all text elements")]
    public bool applyOnStart = true;
    
    void Start()
    {
        if (applyOnStart)
        {
            ApplyFontsToAllText();
        }
    }
    
    [ContextMenu("Apply Fonts to All Text")]
    public void ApplyFontsToAllText()
    {
        UIThemeManager theme = UIThemeManager.Instance;
        if (theme == null)
        {
            Debug.LogError("No UITheme found! Make sure DefaultUITheme exists in Resources folder.");
            return;
        }
        
        // Find all TextMeshPro components
        TextMeshProUGUI[] allTexts = FindObjectsByType<TextMeshProUGUI>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        int fontsApplied = 0;
        
        foreach (TextMeshProUGUI text in allTexts)
        {
            // Skip if text is part of a button (handled by button system)
            if (text.GetComponentInParent<UnityEngine.UI.Button>() != null)
                continue;
                
            // Check if it has EnhancedTextDisplay for style-based font assignment
            EnhancedTextDisplay enhanced = text.GetComponent<EnhancedTextDisplay>();
            if (enhanced != null)
            {
                ApplyFontByStyle(text, enhanced.textStyle, theme);
                fontsApplied++;
            }
            else
            {
                // Fallback: Apply body font to all unspecified text
                if (theme.bodyFont != null)
                {
                    text.font = theme.bodyFont;
                    fontsApplied++;
                }
            }
        }
        
        Debug.Log($"Applied fonts to {fontsApplied} text elements using theme: {theme.name}");
    }
    
    void ApplyFontByStyle(TextMeshProUGUI text, EnhancedTextDisplay.TextStyle style, UIThemeManager theme)
    {
        switch (style)
        {
            case EnhancedTextDisplay.TextStyle.Heading1:
            case EnhancedTextDisplay.TextStyle.Heading2:
            case EnhancedTextDisplay.TextStyle.Heading3:
                if (theme.headingFont != null)
                {
                    text.font = theme.headingFont;
                    Debug.Log($"Applied heading font ({theme.headingFont.name}) to: {text.gameObject.name}");
                }
                break;
                
            case EnhancedTextDisplay.TextStyle.Score:
            case EnhancedTextDisplay.TextStyle.Timer:
                if (theme.scoreFont != null)
                {
                    text.font = theme.scoreFont;
                    Debug.Log($"Applied score font ({theme.scoreFont.name}) to: {text.gameObject.name}");
                }
                break;
                
            case EnhancedTextDisplay.TextStyle.Button:
                if (theme.buttonFont != null)
                {
                    text.font = theme.buttonFont;
                    Debug.Log($"Applied button font ({theme.buttonFont.name}) to: {text.gameObject.name}");
                }
                break;
                
            default:
                if (theme.bodyFont != null)
                {
                    text.font = theme.bodyFont;
                    Debug.Log($"Applied body font ({theme.bodyFont.name}) to: {text.gameObject.name}");
                }
                break;
        }
    }
    
    [ContextMenu("List All Available Fonts")]
    public void ListAvailableFonts()
    {
        TMP_FontAsset[] allFonts = Resources.FindObjectsOfTypeAll<TMP_FontAsset>();
        
        Debug.Log("=== AVAILABLE SDF FONTS ===");
        foreach (TMP_FontAsset font in allFonts)
        {
#if UNITY_EDITOR
            Debug.Log($"• {font.name} (Path: {AssetDatabase.GetAssetPath(font)})");
#else
            Debug.Log($"• {font.name}");
#endif
        }
        
        UIThemeManager theme = UIThemeManager.Instance;
        if (theme != null)
        {
            Debug.Log("=== THEME FONT ASSIGNMENTS ===");
            Debug.Log($"Heading: {(theme.headingFont ? theme.headingFont.name : "NOT SET")}");
            Debug.Log($"Body: {(theme.bodyFont ? theme.bodyFont.name : "NOT SET")}");
            Debug.Log($"Button: {(theme.buttonFont ? theme.buttonFont.name : "NOT SET")}");
            Debug.Log($"Score: {(theme.scoreFont ? theme.scoreFont.name : "NOT SET")}");
        }
    }
}