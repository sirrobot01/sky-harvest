using UnityEngine;

public class TextureScroller : MonoBehaviour
{
    public float baseScrollSpeed = 0.1f;

    private Renderer rend;
    private float scrollOffset = 0f;

    void Start()
    {
        rend = GetComponent<Renderer>();
    }

    void Update()
    {
        if (GameManager.isLevelOver) return;
        
        scrollOffset += Time.deltaTime * baseScrollSpeed * PlayerDragonController.scrollSpeedMultiplier;
        rend.material.mainTextureOffset = new Vector2(scrollOffset, 0);
    }
}

