using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{

    public int damage = 30;

    private void OnCollisionEnter2D(Collision2D collision) 
    {

        if (collision.gameObject.CompareTag("Walls"))
        {
            Destroy(gameObject);
        }
        else if(collision.gameObject.CompareTag("Enemy")){
            MonsterController Enemy = collision.gameObject.GetComponent<MonsterController>();

            Enemy.TakeDamage(damage);

            Destroy(gameObject);
        }
    }

}
