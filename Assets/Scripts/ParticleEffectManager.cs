using UnityEngine;

public class ParticleEffectManager : MonoBehaviour
{
    [Header("Fruit Collection Effects")]
    public ParticleSystem fruitCollectEffect;
    public ParticleSystem goldenFruitEffect;
    public ParticleSystem heartFruitEffect;
    
    [Header("Damage Effects")]
    public ParticleSystem damageEffect;
    public ParticleSystem explosionEffect;
    
    [Header("Dragon Trail")]
    public ParticleSystem dragonTrail;
    
    public static ParticleEffectManager instance;
    
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public void PlayFruitCollectEffect(Vector3 position, Fruit.FruitType fruitType)
    {
        ParticleSystem effect = GetFruitEffect(fruitType);
        if (effect != null)
        {
            effect.transform.position = position;
            effect.Play();
        }
    }
    
    public void PlayDamageEffect(Vector3 position)
    {
        if (damageEffect != null)
        {
            damageEffect.transform.position = position;
            damageEffect.Play();
        }
    }
    
    public void PlayExplosionEffect(Vector3 position)
    {
        if (explosionEffect != null)
        {
            explosionEffect.transform.position = position;
            explosionEffect.Play();
        }
    }
    
    public void StartDragonTrail(Transform dragonTransform)
    {
        if (dragonTrail != null)
        {
            dragonTrail.transform.SetParent(dragonTransform);
            dragonTrail.transform.localPosition = Vector3.zero;
            dragonTrail.Play();
        }
    }
    
    public void StopDragonTrail()
    {
        if (dragonTrail != null)
        {
            dragonTrail.Stop();
        }
    }
    
    ParticleSystem GetFruitEffect(Fruit.FruitType fruitType)
    {
        switch (fruitType)
        {
            case Fruit.FruitType.GoldApple:
            case Fruit.FruitType.GoldGrape:
            case Fruit.FruitType.GoldWatermelon:
            case Fruit.FruitType.GoldDragonfruit:
                return goldenFruitEffect;
                
            case Fruit.FruitType.HeartFruit:
                return heartFruitEffect;
                
            default:
                return fruitCollectEffect;
        }
    }
    
    // Create basic particle effects programmatically if none assigned
    void Start()
    {
        if (fruitCollectEffect == null)
            fruitCollectEffect = CreateBasicFruitEffect();
        if (goldenFruitEffect == null)
            goldenFruitEffect = CreateGoldenFruitEffect();
        if (heartFruitEffect == null)
            heartFruitEffect = CreateHeartFruitEffect();
        if (damageEffect == null)
            damageEffect = CreateDamageEffect();
        if (explosionEffect == null)
            explosionEffect = CreateExplosionEffect();
        if (dragonTrail == null)
            dragonTrail = CreateDragonTrailEffect();
    }
    
    ParticleSystem CreateBasicFruitEffect()
    {
        GameObject effectObj = new GameObject("FruitCollectEffect");
        effectObj.transform.SetParent(transform);
        ParticleSystem ps = effectObj.AddComponent<ParticleSystem>();
        
        var main = ps.main;
        main.startLifetime = 0.5f;
        main.startSpeed = 3f;
        main.startSize = 0.1f;
        main.startColor = Color.green;
        main.maxParticles = 15;
        
        var emission = ps.emission;
        emission.rateOverTime = 0;
        emission.SetBursts(new ParticleSystem.Burst[] {
            new ParticleSystem.Burst(0.0f, 15)
        });
        
        var shape = ps.shape;
        shape.enabled = true;
        shape.shapeType = ParticleSystemShapeType.Circle;
        shape.radius = 0.3f;
        
        // Use default particle material (no custom material needed)
        // var renderer = ps.GetComponent<ParticleSystemRenderer>();
        // renderer.material = GetDefaultSpriteMaterial();
        
        return ps;
    }
    
    ParticleSystem CreateGoldenFruitEffect()
    {
        GameObject effectObj = new GameObject("GoldenFruitEffect");
        effectObj.transform.SetParent(transform);
        ParticleSystem ps = effectObj.AddComponent<ParticleSystem>();
        
        var main = ps.main;
        main.startLifetime = 1f;
        main.startSpeed = 4f;
        main.startSize = 0.15f;
        main.startColor = Color.yellow;
        main.maxParticles = 25;
        
        var emission = ps.emission;
        emission.rateOverTime = 0;
        emission.SetBursts(new ParticleSystem.Burst[] {
            new ParticleSystem.Burst(0.0f, 25)
        });
        
        var shape = ps.shape;
        shape.enabled = true;
        shape.shapeType = ParticleSystemShapeType.Circle;
        shape.radius = 0.4f;
        
        // Use default particle material (no custom material needed)
        // var renderer = ps.GetComponent<ParticleSystemRenderer>();
        // renderer.material = GetDefaultSpriteMaterial();
        
        return ps;
    }
    
    ParticleSystem CreateHeartFruitEffect()
    {
        GameObject effectObj = new GameObject("HeartFruitEffect");
        effectObj.transform.SetParent(transform);
        ParticleSystem ps = effectObj.AddComponent<ParticleSystem>();
        
        var main = ps.main;
        main.startLifetime = 0.8f;
        main.startSpeed = 2f;
        main.startSize = 0.2f;
        main.startColor = Color.red;
        main.maxParticles = 10;
        
        var emission = ps.emission;
        emission.rateOverTime = 0;
        emission.SetBursts(new ParticleSystem.Burst[] {
            new ParticleSystem.Burst(0.0f, 10)
        });
        
        var shape = ps.shape;
        shape.enabled = true;
        shape.shapeType = ParticleSystemShapeType.Circle;
        shape.radius = 0.2f;
        
        // Use default particle material (no custom material needed)
        // var renderer = ps.GetComponent<ParticleSystemRenderer>();
        // renderer.material = GetDefaultSpriteMaterial();
        
        return ps;
    }
    
    ParticleSystem CreateDamageEffect()
    {
        GameObject effectObj = new GameObject("DamageEffect");
        effectObj.transform.SetParent(transform);
        ParticleSystem ps = effectObj.AddComponent<ParticleSystem>();
        
        var main = ps.main;
        main.startLifetime = 0.3f;
        main.startSpeed = 6f;
        main.startSize = 0.12f;
        main.startColor = Color.red;
        main.maxParticles = 20;
        
        var emission = ps.emission;
        emission.rateOverTime = 0;
        emission.SetBursts(new ParticleSystem.Burst[] {
            new ParticleSystem.Burst(0.0f, 20)
        });
        
        var shape = ps.shape;
        shape.enabled = true;
        shape.shapeType = ParticleSystemShapeType.Circle;
        shape.radius = 0.3f;
        
        // Use default particle material (no custom material needed)
        // var renderer = ps.GetComponent<ParticleSystemRenderer>();
        // renderer.material = GetDefaultSpriteMaterial();
        
        return ps;
    }
    
    ParticleSystem CreateExplosionEffect()
    {
        GameObject effectObj = new GameObject("ExplosionEffect");
        effectObj.transform.SetParent(transform);
        ParticleSystem ps = effectObj.AddComponent<ParticleSystem>();
        
        var main = ps.main;
        main.startLifetime = 0.5f;
        main.startSpeed = 8f;
        main.startSize = 0.2f;
        main.startColor = new Color(1f, 0.5f, 0f); // Orange
        main.maxParticles = 30;
        
        var emission = ps.emission;
        emission.rateOverTime = 0;
        emission.SetBursts(new ParticleSystem.Burst[] {
            new ParticleSystem.Burst(0.0f, 30)
        });
        
        var shape = ps.shape;
        shape.enabled = true;
        shape.shapeType = ParticleSystemShapeType.Circle;
        shape.radius = 0.3f;
        
        // Use default particle material (no custom material needed)
        // var renderer = ps.GetComponent<ParticleSystemRenderer>();
        // renderer.material = GetDefaultSpriteMaterial();
        
        return ps;
    }
    
    ParticleSystem CreateDragonTrailEffect()
    {
        GameObject effectObj = new GameObject("DragonTrail");
        effectObj.transform.SetParent(transform);
        ParticleSystem ps = effectObj.AddComponent<ParticleSystem>();
        
        var main = ps.main;
        main.startLifetime = 1f;
        main.startSpeed = 0.5f;
        main.startSize = 0.08f;
        main.startColor = new Color(0.5f, 0.8f, 1f, 0.7f); // Light blue
        main.maxParticles = 50;
        
        var emission = ps.emission;
        emission.rateOverTime = 20;
        
        var shape = ps.shape;
        shape.enabled = true;
        shape.shapeType = ParticleSystemShapeType.Circle;
        shape.radius = 0.1f;
        
        // Use default particle material (no custom material needed)
        // var renderer = ps.GetComponent<ParticleSystemRenderer>();
        // renderer.material = GetDefaultSpriteMaterial();
        
        return ps;
    }
    
    // Material method no longer needed
    /*
    Material GetDefaultSpriteMaterial()
    {
        // Use the built-in default sprite material
        return Resources.GetBuiltinResource<Material>("Default-Material.mat");
    }
    */
}