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
        // Reduce or set to zero
        if (health - amount > 0)
        {
            health -= amount * reduction;
        }
        else
        {
            health = 0;
            isAlive = false;
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
        // ..
    }

    // Set buffed on/off
    public void SetBuff(bool enabled)
    {
        // En/Disable amp
        dmgAmp = enabled ? 2.0f : 1.0f;
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