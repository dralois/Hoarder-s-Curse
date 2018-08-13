using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    // Singleton instance
    private static PlayerManager _instance = null;    
    // Vars
    private float health = 1.0f;
    private float reduction = 1.0f;
    private float dmgAmp = 1.0f;
    // Keeps track ingame
    private StatsUI statsUI;
    
    // Keeps track if player can still do stuff
    public bool isAlive { get; private set; }
    public bool isBuffed { get; private set; }

    private StatsUI PlayerStatsUI
    {
        get
        {
            if (statsUI == null)
            {
                statsUI = FindObjectOfType<StatsUI>();
            }
            // Return UI
            return statsUI;
        }
    }

    // Apply a certain amount of damage
    public void ApplyDamage(float amount)
    {
        // Reduce or set to kill player
        if (health - amount > 0)
        {
            health -= amount * reduction;
        }
        else
        {
            Kill();
        }
        // Set Health
        PlayerStatsUI.SetHealthRemaining(health);
    }

    // Sets resistance to a percentage
    public void setResistance(float resist)
    {
        reduction = 1 - resist;
    }

    // Kills the player instantly
    public void Kill()
    {
        // Reduce health to zero
        health = 0;
        PlayerStatsUI.SetHealthRemaining(0);
        isAlive = false;
        // Load end scene
        LevelManager.Instance.LoadLevel(0);
    }

    public void Revive()
    {
        // Revive
        health = 1;
        isAlive = true;
        // Reset inventory
        InventoryManager.Instance.Empty();
    }

    // Set buffed on/off
    public void SetBuff(bool enabled)
    {
        // En/Disable amp
        dmgAmp = enabled ? 2.0f : 1.0f;
        isBuffed = enabled;
        // En/Disable amp UI
        PlayerStatsUI.SetBuffEnabled(enabled);
    }

    // Get current damage amplification
    public float DmgAmp()
    {
        return dmgAmp;
    }

    /// <summary>
    /// Sets the health back do full hp
    /// </summary>
    public void HealingPotion()
    {
        health = 1f;
        PlayerStatsUI.SetHealthRemaining(health);
    }

    #region Singleton

    public static PlayerManager Instance
    {
        get
        {
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else if (_instance != this)
            Destroy(gameObject);

        // Make sure it persits
        DontDestroyOnLoad(gameObject);
        // Player should be alive at the start
        isAlive = true;
    }

    #endregion
}