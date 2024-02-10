using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    // This method will be called when the player's health reaches zero
    public GameObject player;
    public GameObject checkpoint;
    private static Vector3 lastCheckpointPosition;

    public void Start()
    {
        lastCheckpointPosition = player.transform.position;
    }
    public void OnCollision2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            lastCheckpointPosition = collision.gameObject.transform.position;
        }
    }

    public void RespawnPlayer()
    {
        if (player != null)
        {
            // Respawn the player at the last checkpoint
            player.transform.position = lastCheckpointPosition;
            HUDManager.healthAmount = HUDManager.baseHealthAmount;
        }
    }
}
