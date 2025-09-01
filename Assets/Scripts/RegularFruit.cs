using UnityEngine;

public class Fruit : MonoBehaviour
{
    public enum FruitType
    {
        // Regular fruits
        Apple,
        Grape,
        Watermelon,
        Dragonfruit,
        
        // Golden variants (double points of regular)
        GoldApple,
        GoldGrape,
        GoldWatermelon,
        GoldDragonfruit,
        
        // Special fruits
        HeartFruit,  // Heals 1 HP or gives 50 points if full health
        RottenApple  // Damages player like an obstacle
    }

    [Header("Fruit Settings")]
    public FruitType fruitType;
    
    [Header("Visual Effects")]
    public GameObject collectEffect; // Optional particle effect
    public float glowIntensity = 1f; // For golden fruits

    private int GetScoreValue()
    {
        switch (fruitType)
        {
            // Regular fruits
            case FruitType.Apple: return 10;
            case FruitType.Grape: return 10;
            case FruitType.Watermelon: return 20;
            case FruitType.Dragonfruit: return 20;
            
            // Golden fruits (double points of regular)
            case FruitType.GoldApple: return 20;
            case FruitType.GoldGrape: return 20;
            case FruitType.GoldWatermelon: return 40;
            case FruitType.GoldDragonfruit: return 40;
            
            // Special fruits
            case FruitType.HeartFruit: return 50; // If health is full
            case FruitType.RottenApple: return 0; // No points for rotten
            
            default: return 0;
        }
    }

    void Start()
    {
        // Add glow effect to golden fruits
        if (IsGoldenFruit())
        {
            AddGlowEffect();
        }
    }

    void Update()
    {
        if (GameManager.isLevelOver) return;
        
        float speed = 4f * PlayerDragonController.scrollSpeedMultiplier * Time.deltaTime;
        transform.position += Vector3.left * speed;
        
        // Add floating animation for heart fruit
        if (fruitType == FruitType.HeartFruit)
        {
            float floatY = Mathf.Sin(Time.time * 3f) * 0.1f;
            transform.position += Vector3.up * floatY * Time.deltaTime;
        }
    }

    private bool collected = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (collected) return;

        if (other.CompareTag("Player"))
        {
            collected = true;
            PlayerDragonController player = other.GetComponent<PlayerDragonController>();
            
            HandleFruitCollection(player);
            
            Destroy(gameObject);
        }
        else
        {
            Debug.Log($"Collision with non-player: {other.name} (Tag: {other.tag})");
        }
    }
    
    void HandleFruitCollection(PlayerDragonController player)
    {
        switch (fruitType)
        {
            case FruitType.HeartFruit:
                HandleHeartFruit(player);
                break;
                
            case FruitType.RottenApple:
                HandleRottenFruit(player);
                break;
                
            default:
                HandleRegularFruit();
                break;
        }
    }
    
    void HandleRegularFruit()
    {
        int value = GetScoreValue();
        
        if (ScoreManager.instance != null)
        {
            ScoreManager.instance.AddScore(value, transform.position); // Pass position for score pop-up
        }
        else
        {
            Debug.LogError("ScoreManager.instance is null!");
        }
        
        // Play appropriate sound
        if (AudioManager.instance != null)
        {
            if (IsGoldenFruit())
            {
                AudioManager.instance.PlayPowerUp(); // Special sound for golden
            }
            else
            {
                AudioManager.instance.PlayFruitSound(GetBaseFruitType());
            }
        }
        
        // Screen flash for golden fruits
        if (IsGoldenFruit() && ScreenFlashManager.instance != null)
        {
            ScreenFlashManager.instance.FlashGold();
        }
        
        // Spawn collection effect
        if (collectEffect != null)
        {
            Instantiate(collectEffect, transform.position, Quaternion.identity);
        }
    }
    
    void HandleHeartFruit(PlayerDragonController player)
    {
        if (player.health < player.maxHealth) // Can heal
        {
            int oldHealth = player.health;
            player.health++;
            Debug.Log($"Health restored! {oldHealth} → {player.health} (Max: {player.maxHealth})");
            
            // Screen flash for healing
            if (ScreenFlashManager.instance != null)
            {
                ScreenFlashManager.instance.FlashHeal();
            }
            
            // Update health bar if it exists
            HealthBarController healthBar = FindFirstObjectByType<HealthBarController>();
            if (healthBar != null)
            {
                // Assuming you have an UpdateHealthDisplay method
                healthBar.UpdateHealthDisplay();
            }
        }
        else
        {
            Debug.Log($"Health already at maximum ({player.maxHealth}). Giving points instead.");
            // Give points instead if health is full
            if (ScoreManager.instance != null)
            {
                ScoreManager.instance.AddScore(GetScoreValue(), transform.position); // Pass position for pop-up
            }
            Debug.Log("Health full - received points instead!");
        }
        
        if (AudioManager.instance != null)
        {
            AudioManager.instance.PlayPowerUp();
        }
    }
    
    void HandleRottenFruit(PlayerDragonController player)
    {
        // Damage player like an obstacle
        if (!player.isInvincible && !player.isDead)
        {
            // Use reflection to call private TakeDamage method or make it public
            player.GetComponent<PlayerDragonController>().SendMessage("TakeDamage", SendMessageOptions.DontRequireReceiver);
            Debug.Log("Rotten fruit damaged player!");
        }
    }
    
    bool IsGoldenFruit()
    {
        return fruitType >= FruitType.GoldApple && fruitType <= FruitType.GoldDragonfruit;
    }
    
    FruitType GetBaseFruitType()
    {
        // Convert golden fruit to base type for audio
        switch (fruitType)
        {
            case FruitType.GoldApple: return FruitType.Apple;
            case FruitType.GoldGrape: return FruitType.Grape;
            case FruitType.GoldWatermelon: return FruitType.Watermelon;
            case FruitType.GoldDragonfruit: return FruitType.Dragonfruit;
            default: return fruitType;
        }
    }
    
    void AddGlowEffect()
    {
        // Simple glow effect using color modulation
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        if (renderer != null)
        {
            // Add a subtle glow
            renderer.color = new Color(1f, 1f, 0.8f, 1f); // Slight golden tint
            
            // Add a more complex glow shader here if needed
        }
    }
}