using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonAssetIntegrator : MonoBehaviour
{
    [Header("Button Asset Prefabs")]
    [Tooltip("Drag your Unity Store button prefabs here")]
    public GameObject primaryButtonPrefab;
    public GameObject secondaryButtonPrefab;
    public GameObject successButtonPrefab;
    public GameObject dangerButtonPrefab;
    
    [Header("Integration Settings")]
    public bool replaceExistingButtons = true;
    public bool preserveButtonText = true;
    public bool preserveButtonCallbacks = true;
    
    [ContextMenu("Replace All Buttons with Asset Prefabs")]
    public void ReplaceAllButtons()
    {
        EnhancedButton[] enhancedButtons = FindObjectsByType<EnhancedButton>(FindObjectsSortMode.None);
        
        foreach (EnhancedButton enhancedButton in enhancedButtons)
        {
            ReplaceButtonWithPrefab(enhancedButton);
        }
        
        Debug.Log($"Replaced {enhancedButtons.Length} buttons with asset prefabs.");
    }
    
    void ReplaceButtonWithPrefab(EnhancedButton enhancedButton)
    {
        // Get the appropriate prefab for this button style
        GameObject prefabToUse = GetPrefabForStyle(enhancedButton.buttonStyle);
        if (prefabToUse == null)
        {
            Debug.LogWarning($"No prefab assigned for button style: {enhancedButton.buttonStyle}");
            return;
        }
        
        // Store original button data
        Button originalButton = enhancedButton.GetComponent<Button>();
        RectTransform originalRect = enhancedButton.GetComponent<RectTransform>();
        string originalText = GetButtonText(enhancedButton);
        var originalCallbacks = preserveButtonCallbacks ? originalButton.onClick : null;
        
        // Store transform data
        Vector3 position = originalRect.localPosition;
        Vector2 sizeDelta = originalRect.sizeDelta;
        Transform parent = enhancedButton.transform.parent;
        int siblingIndex = enhancedButton.transform.GetSiblingIndex();
        
        // Instantiate new button from prefab
        GameObject newButton = Instantiate(prefabToUse, parent);
        newButton.transform.SetSiblingIndex(siblingIndex);
        
        // Configure the new button
        RectTransform newRect = newButton.GetComponent<RectTransform>();
        newRect.localPosition = position;
        newRect.sizeDelta = sizeDelta;
        
        // Add EnhancedButton component to the new prefab instance
        EnhancedButton newEnhanced = newButton.GetComponent<EnhancedButton>();
        if (newEnhanced == null)
        {
            newEnhanced = newButton.AddComponent<EnhancedButton>();
        }
        
        // Copy settings from original
        newEnhanced.buttonStyle = enhancedButton.buttonStyle;
        newEnhanced.buttonSize = enhancedButton.buttonSize;
        newEnhanced.useCustomSprites = false; // Using prefab graphics instead
        
        // Set text if preserving
        if (preserveButtonText && !string.IsNullOrEmpty(originalText))
        {
            newEnhanced.SetButtonText(originalText);
        }
        
        // Restore callbacks if preserving
        if (preserveButtonCallbacks && originalCallbacks != null)
        {
            Button newButtonComponent = newButton.GetComponent<Button>();
            if (newButtonComponent != null)
            {
                newButtonComponent.onClick = originalCallbacks;
            }
        }
        
        // Destroy original button
        DestroyImmediate(enhancedButton.gameObject);
        
        Debug.Log($"Replaced {enhancedButton.name} with {prefabToUse.name} prefab");
    }
    
    GameObject GetPrefabForStyle(EnhancedButton.ButtonStyle style)
    {
        switch (style)
        {
            case EnhancedButton.ButtonStyle.Primary: return primaryButtonPrefab;
            case EnhancedButton.ButtonStyle.Secondary: return secondaryButtonPrefab;
            case EnhancedButton.ButtonStyle.Success: return successButtonPrefab;
            case EnhancedButton.ButtonStyle.Danger: return dangerButtonPrefab;
            default: return primaryButtonPrefab;
        }
    }
    
    string GetButtonText(EnhancedButton button)
    {
        TextMeshProUGUI textComponent = button.GetComponentInChildren<TextMeshProUGUI>();
        if (textComponent != null)
        {
            return textComponent.text;
        }
        
        // Fallback to regular Text component
        Text regularText = button.GetComponentInChildren<Text>();
        if (regularText != null)
        {
            return regularText.text;
        }
        
        return "";
    }
    
    [ContextMenu("Test Single Button Replacement")]
    public void TestSingleReplacement()
    {
        // Find first enhanced button and replace it
        EnhancedButton testButton = FindFirstObjectByType<EnhancedButton>();
        if (testButton != null)
        {
            ReplaceButtonWithPrefab(testButton);
        }
        else
        {
            Debug.Log("No EnhancedButton found to test with.");
        }
    }
}