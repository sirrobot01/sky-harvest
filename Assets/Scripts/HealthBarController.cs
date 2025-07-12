using UnityEngine;

public class HealthBarController : MonoBehaviour
{
    public PlayerDragonController dragon;

    [Header("Health Bar Sprites")]
    public Sprite bar1_1, bar1_2, bar1_3;
    public Sprite bar2_1, bar2_2, bar2_3;
    public Sprite bar3_1, bar3_2, bar3_3;
    public Sprite barH_1, barH_2;
    public Sprite barG;

    private SpriteRenderer sr;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
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
            float stunnedHealthPercent = (float)dragon.health / dragon.maxHealth;
            if (stunnedHealthPercent > 0.33f) sr.sprite = barH_1;  // Medium/high health when stunned
            else sr.sprite = barH_2;  // Low health when stunned
            return;
        }

        float speed = PlayerDragonController.scrollSpeedMultiplier;
        int hp = dragon.health;

        // Determine sprite based on speed & health (scale to maxHealth)
        float healthPercent = (float)hp / dragon.maxHealth;
        
        if (speed > 1f)
        {
            if (healthPercent > 0.66f) sr.sprite = bar2_1;  // High health
            else if (healthPercent > 0.33f) sr.sprite = bar2_2;  // Medium health
            else sr.sprite = bar2_3;  // Low health
        }
        else if (speed < 1f && speed > 0f)
        {
            if (healthPercent > 0.66f) sr.sprite = bar3_1;
            else if (healthPercent > 0.33f) sr.sprite = bar3_2;
            else sr.sprite = bar3_3;
        }
        else
        {
            if (healthPercent > 0.66f) sr.sprite = bar1_1;
            else if (healthPercent > 0.33f) sr.sprite = bar1_2;
            else sr.sprite = bar1_3;
        }
    }
}

