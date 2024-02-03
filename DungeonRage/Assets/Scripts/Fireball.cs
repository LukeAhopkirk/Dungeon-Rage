using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    public int damage = 30;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Walls"))
        {
            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("Enemy"))
        {
            MonsterController enemy = collision.gameObject.GetComponent<MonsterController>();

            if (enemy != null)
            {
                enemy.TakeDamage(damage);
                RageSystem rageSystem = FindObjectOfType<RageSystem>();
                if (rageSystem != null)
                {
                    rageSystem.DealDamage(damage);
                }
            }

            Destroy(gameObject);
        }
    }
}
