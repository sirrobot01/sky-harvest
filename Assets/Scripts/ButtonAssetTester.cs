using UnityEngine;

public class ButtonAssetTester : MonoBehaviour
{
    [Header("Testing")]
    [Tooltip("Click to test loading your button assets")]
    public bool testAssetLoading = false;
    
    void Start()
    {
        if (testAssetLoading)
        {
            TestAllButtonAssets();
        }
    }
    
    [ContextMenu("Test Asset Loading")]
    public void TestAllButtonAssets()
    {
        Debug.Log("Testing button asset loading from Resources/ButtonAssets/...");
        
        // Test each button asset
        TestAsset("PrimaryButton");
        TestAsset("SecondaryButton"); 
        TestAsset("SucessButton"); // Note: matches your filename spelling
        TestAsset("DangerButton");
    }
    
    void TestAsset(string assetName)
    {
        Sprite sprite = Resources.Load<Sprite>("ButtonAssets/" + assetName);
        if (sprite != null)
        {
            Debug.Log($"✓ Successfully loaded: {assetName} (Size: {sprite.texture.width}x{sprite.texture.height})");
        }
        else
        {
            Debug.LogError($"✗ Failed to load: {assetName}");
            Debug.LogError($"Expected path: Resources/ButtonAssets/{assetName}.png");
        }
    }
    
    [ContextMenu("Apply Assets to All Enhanced Buttons")]
    public void ApplyAssetsToAllButtons()
    {
        EnhancedButton[] buttons = FindObjectsByType<EnhancedButton>(FindObjectsSortMode.None);
        
        foreach (EnhancedButton button in buttons)
        {
            // Enable custom sprites
            button.useCustomSprites = true;
            
            // Trigger sprite refresh
            button.SetButtonStyle(button.buttonStyle);
        }
        
        Debug.Log($"Applied custom assets to {buttons.Length} Enhanced Buttons");
    }
}