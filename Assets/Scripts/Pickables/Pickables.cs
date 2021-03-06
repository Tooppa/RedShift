using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class Pickables : MonoBehaviour
{
    public PickableObjects data;
    private SpriteRenderer _spriteRenderer;
    public bool IsNote { private set; get; }
    public bool HasFuel { private set; get; }
    public bool RocketBoots { private set; get; }
    public bool Gun { private set; get; }
    public bool Flashlight { private set; get; }
    public bool PowerfulGun { private set; get; }
    
    public bool ForceGlove { private set; get; }
    [HideInInspector]public int fuel;

    public void Awake()
    {
        // Loading from a save triggers reconstruct of items. During that, data might be null
        if (data == null)
        {
            this.enabled = false;
            return;
        }

        this.enabled = true; // Awake can be called manually. Ensure that the script is active.
        
        if(TryGetComponent(out _spriteRenderer))
            _spriteRenderer.sprite = data.sprite;
        
        IsNote = data.note.Length > 0;
        HasFuel = data.fuel > 0;
        RocketBoots = data.rocketBoots;
        Gun = data.gun;
        Flashlight = data.flashlight;
        fuel = data.fuel;
        PowerfulGun = data.breakObjectsWithGun;
        ForceGlove = data.forceGlove;
        var light2D = transform.GetComponentInChildren<Light2D>();
        if (light2D)
            light2D.enabled = Flashlight;

    }

    public Sprite GetSprite()
    {
        return data.sprite;
    }

    public string GetNote()
    {
        return data.note;
    }

    public string GetStats()
    {
        return data.statsForUpgrades;
    }

    public Sprite GetPicture()
    {
        return data.notePicture ? data.notePicture : null;
    }
}
