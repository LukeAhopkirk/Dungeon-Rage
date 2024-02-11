using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    public Image healthBar;
    public Image experienceBar;
    public Image rageBar;
    public Button[] spellButtons;
    public GameObject pauseMenu;

    public static float baseHealthAmount = 100f;
    public static float healthAmount;
    public float experienceAmount = 0f;
    public float rageAmount = 0f;

    public static bool isPaused;
    private bool isAbilityDraining = false; // Flag to track ability draining state

    private bool isInvincible = false;

    public SkillPointManager skillPointManager;

    public CheckpointManager checkPoint;
    public Animator animator;

    private bool isDying = false;


    // Start is called before the first frame update
    void Start()
    {
        SetPauseMenuActive(false);       

        healthBar.fillAmount = baseHealthAmount / 100f;
        experienceBar.fillAmount = experienceAmount / 100f;
        rageBar.fillAmount = rageAmount / 100f;

        healthAmount = baseHealthAmount;
        skillPointManager = GameObject.FindObjectOfType<SkillPointManager>();
        foreach(var stat in skillPointManager.stats)
        {
            if (stat.statName == "Endurance")
            {
                stat.OnStatChanged += UpdateHealthAmount;
            }
        }

 

        for (int i = 0; i < spellButtons.Length; i++)
        {
            int index = i;
            spellButtons[i].onClick.AddListener(() => OnSpellButtonClicked(index));
        }

        // Subscribe to the DealDamageEvent
        RageSystem.DealDamageEvent += HandleDealDamageEvent;
    }
    void UpdateHealthAmount(float newEndurance)
    {
        Debug.Log($"New Endurance: {newEndurance}");

        // Calculate healthAmount based on baseHealthAmount and newEndurance
        healthAmount = baseHealthAmount + newEndurance + 10f;

        // Update the healthBar fill amount based on the calculated healthAmount
        healthBar.fillAmount = healthAmount / 100f;

        Debug.Log($"Updated healthAmount: {healthAmount}");
    }

    public void DealDamage(float damage)
    {
        if(isInvincible)
        {
            return;
        }
        healthAmount -= damage;
        healthBar.fillAmount = healthAmount / 100f;

        if(healthAmount <= 0 && !isDying)
        {
            isDying = true;
            // Player is dead
            TriggerDeathAnimation();
            Debug.Log("Player is dead");
        }
        else
        {
            return;
        }
    }

    private void TriggerDeathAnimation()
    {
        animator.SetTrigger("Death");
        StartCoroutine(RespawnAfterDeathAnimation());
    }

    private IEnumerator RespawnAfterDeathAnimation()
    {
        float deathAnimationDuration = 1.03f;
        animator.SetTrigger("Respawned");
        yield return new WaitForSeconds(deathAnimationDuration);

        checkPoint.RespawnPlayer();
        isDying = false;
    }

    public void SetInvincibilityState(bool isInvincible)
    {
        this.isInvincible = isInvincible;
    }

    public void Heal(float healingAmount)
    {
        healthAmount += healingAmount;
        healthAmount = Mathf.Clamp(healthAmount, 0f, 100f);

        healthBar.fillAmount = healthAmount / 100f;
    }

    public void GetExperience(float experience)
    {
        if(experienceAmount >= 100f)
        {
            experienceAmount = 0f;
            skillPointManager.LevelUp();
        }
        experienceAmount += experience;
        experienceAmount = Mathf.Clamp(experienceAmount, 0f, 100f);

        experienceBar.fillAmount = experienceAmount / 100f;
    }

    public void GetRage(float rage)
    {
        // Update rageAmount based on the gained rage, only if not in ability draining state
        if (!isAbilityDraining)
        {
            rageAmount += rage;
            rageAmount = Mathf.Clamp(rageAmount, 0f, 100f);
        }

        UpdateRageBar();
    }

    public void TogglePause()
    {
        isPaused = !Time.timeScale.Equals(0f);
        SetPauseMenuActive(isPaused);
        Time.timeScale = isPaused ? 0f : 1f;
    }

    private void SetPauseMenuActive(bool isActive)
    {
        pauseMenu.SetActive(isActive);
    }

    public void OnSpellButtonClicked(int buttonIndex)
    {
        switch (buttonIndex)
        {
            case 0:
                // Cast spell 1
                break;
            case 1:
                // Cast spell 2
                break;
            case 2:
                // Cast spell 3
                break;
            default:
                break;
        }
    }

    private void UpdateRageBar()
    {
        rageBar.fillAmount = rageAmount / 100f;
        //Debug.Log($"Updated Rage Bar. Rage Amount: {rageAmount}");
    }

    private void HandleDealDamageEvent(float damage)
    {
        if (damage < 0)
        {
            // Handle ability drain (negative damage)
            Debug.Log("Ability drain");
            rageAmount += damage;
            rageAmount = Mathf.Clamp(rageAmount, 0f, 100f);
        }
        else
        {
            // Handle normal damage
            rageAmount += damage * RageSystem.damageRageRatio;
            rageAmount = Mathf.Clamp(rageAmount, 0f, 100f);
            Debug.Log(rageAmount);
        }

        UpdateRageBar();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    // Add a public method to set the ability draining state from other scripts
    public void SetAbilityDrainingState(bool isDraining)
    {
        isAbilityDraining = isDraining;
    }
}
