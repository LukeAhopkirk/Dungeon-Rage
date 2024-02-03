using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellCast : MonoBehaviour
{

    public Transform SpellCastPos;
    public GameObject SpellTypePrefab;
    //public Vector2 mousePos;

    public float force = 10f;


    // Update is called once per frame
    void Update()
    {
        if (!HUDManager.isPaused)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                CastSpell();
            }
        }
    }

    void CastSpell()
    {
        // Find the mouse position
        //mousePos = PlayerMovement.cam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Instantiate the spell object
        GameObject Spell = Instantiate(SpellTypePrefab, SpellCastPos.position, Quaternion.identity);

        // Ignore collisions between the player and the spell
        Physics2D.IgnoreCollision(Spell.GetComponent<Collider2D>(), GetComponent<Collider2D>());


        Vector2 pos = new Vector2(SpellCastPos.position.x, SpellCastPos.position.y);

        // Calculate the direction from the SpellCastPos to the mouse position
        Vector2 direction = (mousePos - pos).normalized;

        // Get the Rigidbody2D component of the spell object
        Rigidbody2D rb = Spell.GetComponent<Rigidbody2D>();



        // Add force in the direction of the mouse
        rb.AddForce(direction * force, ForceMode2D.Impulse);


        //GameObject Spell = Instantiate(SpellTypePrefab, SpellCastPos.position, SpellCastPos.rotation);
        //Rigidbody2D rb = Spell.GetComponent<Rigidbody2D>();
        //rb.AddForce(SpellCastPos.up * force, ForceMode2D.Impulse);
    }
}
