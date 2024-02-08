using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class SpellCast : MonoBehaviour
{

    public Transform SpellCastPos;
    public GameObject SpellTypePrefab;
    public Animator animator;

    public bool spellReady = true;
    private float Timer = 0f;
    public float cooldownLength = .2f;

    public Image imageCooldown;

    public float cooldownAnimationTime;
    private Coroutine cooldownCoroutine;
    private float lastFireballTime = 0f;


    public float force = 10f;

    public GameObject originalFireballPrefab; 
    public GameObject currentFireballPrefab;

    private RageSystem rageSystem; 


    private void Start()
    {
        imageCooldown.fillAmount = 0f;
        cooldownAnimationTime = cooldownLength;
        currentFireballPrefab = originalFireballPrefab;
        rageSystem = FindObjectOfType<RageSystem>();

    }

    // Update is called once per frame
    void Update()
    {
        if (!HUDManager.isPaused)
        {
            if (!spellReady)
            {
                Timer += Time.deltaTime;
                if (Timer > cooldownLength)
                {
                   spellReady = true;
                   Timer = 0;
                }
            }
            if (spellReady && Input.GetButtonDown("Fire1"))
            {
                if (PlayerMovement.isMoving)
                {
                    animator.SetTrigger("shot1");
                }
                else
                {
                    animator.SetTrigger("shot2");
                }

                imageCooldown.fillAmount = 1.0f;
                spellReady = false;
                lastFireballTime = Time.time;

                if (Time.time - lastFireballTime < cooldownLength)
                {
                    Timer = Time.time - lastFireballTime;
                    imageCooldown.fillAmount = 1 - Timer / cooldownLength;

                    if (Timer > 0f && cooldownCoroutine == null)
                    {
                        cooldownCoroutine = StartCoroutine(CooldownFillAnimation());
                    }
                }
                else
                {
                    imageCooldown.fillAmount = 0.0f;

                    if (cooldownCoroutine != null)
                    {
                        StopCoroutine(cooldownCoroutine);
                        cooldownCoroutine = null;
                    }
                }
            }
        }
    }


    IEnumerator CooldownFillAnimation()
    {
        float startTime = Time.time;
        float elapsedTime = 0f;
        float startFill = imageCooldown.fillAmount;

        while (elapsedTime < cooldownAnimationTime)
        {
            imageCooldown.fillAmount = Mathf.Lerp(startFill, 0f, elapsedTime / cooldownAnimationTime);
            elapsedTime = Time.time - startTime;
            yield return null;
        }

        imageCooldown.fillAmount = 0f;
        cooldownCoroutine = null;
    }





    void CastSpell()
    {

        //animator.SetTrigger("shot1");

        // Find the mouse position
        //mousePos = PlayerMovement.cam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        GameObject Spell = Instantiate(rageSystem.GetCurrentFireballPrefab(), SpellCastPos.position, Quaternion.identity);


        // Instantiate the spell object

        // Ignore collisions between the player and the spell
        Physics2D.IgnoreCollision(Spell.GetComponent<Collider2D>(), GetComponent<Collider2D>());


        Vector2 pos = new Vector2(SpellCastPos.position.x, SpellCastPos.position.y);

        // Calculate the direction from the SpellCastPos to the mouse position
        Vector2 direction = (mousePos - pos).normalized;

        // Get the Rigidbody2D component of the spell object
        Rigidbody2D rb = Spell.GetComponent<Rigidbody2D>();



        // Add force in the direction of the mouse
        rb.AddForce(direction * force, ForceMode2D.Impulse);


        if (direction.x < 0)
        {
            // Get the current scale of the spell
            Vector3 scale = Spell.transform.localScale;

            // Flip the scale along the x-axis to reverse the rotation
            scale.x *= -1;

            // Apply the new scale to the spell
            Spell.transform.localScale = scale;
        }

        //animator.SetTrigger("run");



        //GameObject Spell = Instantiate(SpellTypePrefab, SpellCastPos.position, SpellCastPos.rotation);
        //Rigidbody2D rb = Spell.GetComponent<Rigidbody2D>();
        //rb.AddForce(SpellCastPos.up * force, ForceMode2D.Impulse);
    }
}
