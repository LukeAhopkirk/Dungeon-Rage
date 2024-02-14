using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    // This method will be called when the player's health reaches zero
    public GameObject player;
    private static Vector3 lastCheckpointPosition;
    public HUDManager hud;

    public Sprite activatedCheckpointSprite;
    private SpriteRenderer spriteRendererCheckpoint;  
    
    public void Start()
    {
        lastCheckpointPosition = player.transform.position;
    }

    public void UpdateLastCheckpointPosition(Vector3 position)
    {
        lastCheckpointPosition = position;
    }

    public void ActivateCheckpoint(SpriteRenderer spriteRenderer)
    {
        spriteRendererCheckpoint = spriteRenderer;
        spriteRenderer.sprite = activatedCheckpointSprite;
    }
    public void RespawnPlayer()
    {
        if (player != null)
        {
            // Respawn the player at the last checkpoint
            player.transform.position = lastCheckpointPosition;

            // Reset the player's health
            hud.Heal(10000000);
        }
    }
}
