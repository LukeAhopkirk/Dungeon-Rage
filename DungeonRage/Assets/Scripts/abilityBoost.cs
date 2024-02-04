using System.Collections;
using UnityEngine;

public class abilityBoost : MonoBehaviour
{
    // Reference to the PlayerMovement script
    private PlayerMovement playerMovement;

    // Original player stats
    private float originalDamageMultiplier;
    private float originalMoveSpeed;
    private float originalDashCooldown;

    // Boosted stats during Ability 2
    public float boostedDamageMultiplier = 2.0f;
    public float boostedMoveSpeed = 1.2f;
    public float reducedDashCooldown = 0.5f;

    private void Start()
    {
        // Find the PlayerMovement script in the scene
        playerMovement = FindObjectOfType<PlayerMovement>();

        // Store original player stats
        originalDamageMultiplier = playerMovement.damageMultiplier;
        originalMoveSpeed = playerMovement.baseMoveSpeed;
        originalDashCooldown = playerMovement.dashCooldown;
    }

    public void ActivateAbilityBoost()
    {
        // Apply boosted stats
        playerMovement.damageMultiplier = boostedDamageMultiplier;
        playerMovement.baseMoveSpeed *= boostedMoveSpeed;
        playerMovement.dashCooldown *= reducedDashCooldown;

        // Debug the changes
        Debug.Log($"AbilityBoost: Activated. Damage Multiplier: {playerMovement.damageMultiplier}, Move Speed: {playerMovement.baseMoveSpeed}, Dash Cooldown: {playerMovement.dashCooldown}");
    }

    public void DeactivateAbilityBoost()
    {
        // Restore original stats
        playerMovement.damageMultiplier = originalDamageMultiplier;
        playerMovement.baseMoveSpeed = originalMoveSpeed;
        playerMovement.dashCooldown = originalDashCooldown;

        // Debug the changes
        Debug.Log($"AbilityBoost: Deactivated. Damage Multiplier: {playerMovement.damageMultiplier}, Move Speed: {playerMovement.baseMoveSpeed}, Dash Cooldown: {playerMovement.dashCooldown}");
    }
}
