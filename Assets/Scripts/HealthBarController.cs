using UnityEngine;

public class HealthBarController : MonoBehaviour
{
    public PlayerDragonController dragon;

    [Header("Health Bar Sprites")]
    public Sprite bar1_1, bar1_2, bar1_3; // Normal speed (3, 2, 1 HP)
    public Sprite bar2_1, bar2_2, bar2_3; // Speed up (3, 2, 1 HP)
    public Sprite bar3_1, bar3_2, bar3_3; // Slow down (3, 2, 1 HP)
    public Sprite barH_1, barH_2;         // Stunned
    public Sprite barG;                   // Game Over

    private SpriteRenderer sr;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        if (dragon == null)
        {
            dragon = FindObjectOfType<PlayerDragonController>();
        }
    }

    void Update()
    {
        UpdateHealthDisplay();
    }
    
    public void UpdateHealthDisplay()
    {
        if (dragon == null || sr == null) return;

        if (dragon.isDead || dragon.health <= 0)
        {
            sr.sprite = barG;
            return;
        }

        if (dragon.isStunned)
        {
            // Stunned logic can still use percentages or be simplified if needed.
            if (dragon.health > 1) sr.sprite = barH_1; // 2 or 3 HP when stunned
            else sr.sprite = barH_2; // 1 HP when stunned
            return;
        }
        
        // Switched from using percentages to checking the integer health value directly.
        // This is more reliable and avoids floating-point errors.
        float speed = PlayerDragonController.scrollSpeedMultiplier;
        int hp = dragon.health;
        
        if (speed > 1f) // Speeding Up
        {
            if (hp == 3) sr.sprite = bar2_1;
            else if (hp == 2) sr.sprite = bar2_2;
            else sr.sprite = bar2_3; // Covers 1 HP
        }
        else if (speed < 1f && speed > 0f) // Slowing Down
        {
            if (hp == 3) sr.sprite = bar3_1;
            else if (hp == 2) sr.sprite = bar3_2;
            else sr.sprite = bar3_3;
        }
        else // Normal Speed (or any other state like speed = 0)
        {
            if (hp == 3) sr.sprite = bar1_1;
            else if (hp == 2) sr.sprite = bar1_2;
            else sr.sprite = bar1_3;
        }
    }
}

