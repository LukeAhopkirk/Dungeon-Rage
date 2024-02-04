using TMPro;
using System.Collections;
using UnityEngine;

public class RageSystem : MonoBehaviour
{
    public TextMeshProUGUI rageText;
    public float knockBackRageThreshold = 70f;
    public float ability1Cost = 25f;
    public float ability1Cooldown = 1.5f;
    private float ability1CooldownTimer = 0f;

    public delegate void OnDealDamage(float damage);
    public static event OnDealDamage DealDamageEvent;

    private float currentRage = 0f;
    private bool isAbility2Active = false;
    private bool isAbility3Active = false;

    private float ability2DrainRate = 10f;
    private float ability3DrainRate = 15f;

    // Flag to prevent gaining rage during ability drain
    private bool isAbilityDraining = false;

    // Reference to HUDManager
    private HUDManager hudManager;

    private KnockbackAbility knockbackAbility;
    private abilityBoost abilityBoost;


    void Start()
    {
        // Find the HUDManager script in the scene
        hudManager = FindObjectOfType<HUDManager>();

        // Find the ability scripts in the scene
        knockbackAbility = FindObjectOfType<KnockbackAbility>();
        abilityBoost = FindObjectOfType<abilityBoost>();
    }


    void Update()
    {
        // Update ability cooldown timer
        ability1CooldownTimer -= Time.deltaTime;
        ability1CooldownTimer = Mathf.Clamp(ability1CooldownTimer, 0f, ability1Cooldown);

        if (Input.GetKeyDown(KeyCode.Alpha1) && currentRage >= ability1Cost && ability1CooldownTimer <= 0f)
        {
            UseAbility1();
        }

        if (Input.GetKeyDown(KeyCode.Alpha2) && currentRage >= 65f && !isAbility2Active)
        {
            UseAbility2();
        }

        if (Input.GetKeyDown(KeyCode.Alpha3) && currentRage >= 100f && !isAbility3Active)
        {
            UseAbility3();
        }

        // Debug.Log($"Current Rage: {currentRage}");
        rageText.text = $"{Mathf.Round(currentRage)}";
    }

    public void DealDamage(float damage)
    {
        float damageRageRatio = 0.5f;

        // Only gain rage if not currently draining abilities
        if (!isAbilityDraining && damage > 0)
        {
            float gainedRage = damage * damageRageRatio;
            currentRage = Mathf.Clamp(currentRage + gainedRage, 0f, 100f);
            //Debug.Log($"Gained {gainedRage} rage for dealing {damage} damage.");
            DealDamageEvent?.Invoke(damage); 
        }
    }

    void UseAbility1()
    {
        Debug.Log("Using Ability 1: Knocking back enemy sprite");

        // Call the knockback ability from the KnockbackAbility script
        knockbackAbility.UseKnockbackAbility();

        DrainRagePoints(ability1Cost);
        ability1CooldownTimer = ability1Cooldown;

        // Invoke the DealDamageEvent with a negative value to represent rage depletion
        DealDamageEvent?.Invoke(-ability1Cost);
    }

    void UseAbility2()
    {
        Debug.Log("Using Ability 2: Increased damage and attack speed");
        isAbility2Active = true;

        // Activate Ability Boost
        abilityBoost.ActivateAbilityBoost();

        StartCoroutine(DrainRageOverTime(ability2DrainRate, DeactivateAbility2, true));
    }

    void DeactivateAbility2()
    {
        isAbility2Active = false;
        Debug.Log("Ability 2 deactivated");

        // Deactivate Ability Boost
        abilityBoost.DeactivateAbilityBoost();
    }

    void UseAbility3()
    {
        Debug.Log("Using Ability 3: Invincibility and more powerful attack");
        isAbility3Active = true;
        StartCoroutine(DrainRageOverTime(ability3DrainRate, DeactivateAbility3, true));
    }

    void DeactivateAbility3()
    {
        isAbility3Active = false;
        Debug.Log("Ability 3 deactivated");
    }

    IEnumerator DrainRageOverTime(float drainRate, System.Action onDeactivate, bool isAbilityDrain = false)
    {
        // Set ability draining state in HUDManager
        hudManager.SetAbilityDrainingState(true);

        isAbilityDraining = true;

        while (currentRage > 0f && (isAbility2Active || isAbility3Active))
        {
            float drainAmount = drainRate * Time.deltaTime;
            if (isAbilityDrain)
            {
                // Invoke DealDamageEvent with negative value during ability drain
                DealDamageEvent?.Invoke(-drainAmount);
            }
            DrainRagePoints(drainAmount);
            yield return null;
        }

        onDeactivate?.Invoke();
        isAbilityDraining = false;

        // Reset ability draining state in HUDManager
        hudManager.SetAbilityDrainingState(false);
    }

    void DrainRagePoints(float amount)
    {
        currentRage = Mathf.Clamp(currentRage - amount, 0f, 100f);
        //Debug.Log($"Drained {amount} rage. Current Rage: {currentRage}");
    }
}
