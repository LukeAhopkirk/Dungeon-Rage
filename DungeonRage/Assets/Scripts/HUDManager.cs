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

    // Start is called before the first frame update
    void Start()
    {
        healthAmount = baseHealthAmount;
        SetPauseMenuActive(false);

        healthBar.fillAmount = healthAmount / 100f;
        experienceBar.fillAmount = experienceAmount / 100f;
        rageBar.fillAmount = rageAmount / 100f;

        for (int i = 0; i < spellButtons.Length; i++)
        {
            int index = i;
            spellButtons[i].onClick.AddListener(() => OnSpellButtonClicked(index));
        }

        // Subscribe to the DealDamageEvent
        RageSystem.DealDamageEvent += HandleDealDamageEvent;
    }

    public void DealDamage(float damage)
    {
        healthAmount -= damage;
        healthBar.fillAmount = healthAmount / 100f;
    }

    public void Heal(float healingAmount)
    {
        healthAmount += healingAmount;
        healthAmount = Mathf.Clamp(healthAmount, 0f, 100f);

        healthBar.fillAmount = healthAmount / 100f;
    }

    public void GetExperience(float experience)
    {
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
        Debug.Log($"Updated Rage Bar. Rage Amount: {rageAmount}");
    }

    private void HandleDealDamageEvent(float damage)
    {
        if (damage < 0)
        {
            // Handle ability drain (negative damage)
            rageAmount += damage;
            rageAmount = Mathf.Clamp(rageAmount, 0f, 100f);
        }
        else
        {
            // Handle normal damage
            rageAmount += damage * 0.5f;
            rageAmount = Mathf.Clamp(rageAmount, 0f, 100f);
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
