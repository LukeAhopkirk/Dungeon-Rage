using System.Collections;
using System.Runtime.CompilerServices;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public float baseMoveSpeed = 3f;
    public float moveSpeed;
    private bool isFacingRight = true;
    public Rigidbody2D rb;
    public Animator animator;

    public float baseDamageMultiplier = 1.0f;
    public float damageMultiplier;

    private Vector2 movement;
    private bool isDashing = false;
    private float dashTime = 0.2f;
    private float dashDistance = 2f;
    public float dashCooldown = 3f; // Cooldown time for the dash 
    private float playerOffset = 0.1f; 
    private float lastDashTime = 0f;

    private float cooldownTimer = 0.0f;
    public Image imageCooldown;

    public float cooldownAnimationTime;
    private Coroutine cooldownCoroutine;

    public static bool isMoving;

    public SkillPointManager skillPointManager;
    private void Start()
    {

        imageCooldown.fillAmount = 0f;

        moveSpeed = baseMoveSpeed;

        cooldownAnimationTime = dashCooldown;

        skillPointManager = GameObject.FindObjectOfType<SkillPointManager>();

        foreach(var stat in skillPointManager.stats)
        {
            if (stat.statName == "Agility")
            {
                stat.OnStatChanged += UpdateMoveSpeed;
            }
            if(stat.statName == "Intelligence")
            {
                stat.OnStatChanged += UpdateDamageMultiplier;
            }
        }
    }

    void UpdateDamageMultiplier(float intelligence)
    {
        damageMultiplier = baseDamageMultiplier + intelligence * 5f;
    }
    void UpdateMoveSpeed(float newAgility)
    {
        moveSpeed = baseMoveSpeed + newAgility * 0.05f;
    }
    void Update()
    {
        if (!isDashing)
        {
            // Input for movement
            float horizontalInput = Input.GetAxisRaw("Horizontal");
            float verticalInput = Input.GetAxisRaw("Vertical");

            // Normalizing vector for diagonal movement
            Vector2 inputVector = new Vector2(horizontalInput, verticalInput);
            movement = inputVector.magnitude == 0 ? Vector2.zero : inputVector.normalized;
            
            if (Input.GetKeyDown(KeyCode.Space) && Time.time - lastDashTime > dashCooldown)
            {
                StartCoroutine(Dash());
                lastDashTime = Time.time;
                imageCooldown.fillAmount = 1.0f;
            }

            if(Time.time - lastDashTime < dashCooldown)
            {
                cooldownTimer = Time.time - lastDashTime;
                imageCooldown.fillAmount = 1 - cooldownTimer / dashCooldown;

                if(cooldownTimer > 0f && cooldownCoroutine == null)
                {
                    cooldownCoroutine = StartCoroutine(CooldownFillAnimation());
                }
            }
            else
            {
                imageCooldown.fillAmount = 0.0f;

                if(cooldownCoroutine != null)
                {
                    StopCoroutine(cooldownCoroutine);
                    cooldownCoroutine = null;
                }
            }

            if (horizontalInput == 0 && verticalInput == 0)
            {
                isMoving = false;
            }
            else
            {
                isMoving = true;
            }


            // Update animator
            animator.SetFloat("Speed", movement.sqrMagnitude);

            // Flip character if needed
            Flip();

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

    private void FixedUpdate()
    {
        if (!isDashing)
        {
            // Move the character
            rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
        }
    }

    public IEnumerator Dash()
    {
        //Gathers all the game objects tagged enemy to deal with the dashing collisions.
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        isDashing = true;

        //Goes through each enemy and ignores the collision during the dash.
        foreach (GameObject enemy in enemies)
        {
            Physics2D.IgnoreCollision(rb.GetComponent<Collider2D>(), enemy.GetComponent<Collider2D>(), true);
        }

        // Normalize the movement vector to ensure consistent dash distance in all directions
        Vector2 dashDirection = movement.magnitude == 0 ? Vector2.zero : movement.normalized;

        // Calculate the target position for the dash
        Vector2 dashTarget = (Vector2)transform.position + dashDirection * dashDistance;

        // Specify the layer mask to only detect collisions with objects on the "Walls" layer
        LayerMask wallLayer = LayerMask.GetMask("Walls");

        // Perform a raycast to check for collisions in the dash direction
        RaycastHit2D hit = Physics2D.Raycast(rb.position, dashDirection, dashDistance, wallLayer);

        if (hit.collider != null && hit.collider.CompareTag("Walls"))
        {
            // If there's a wall, reduce dash distance to avoid collision
            Vector2 closestPoint = hit.collider.ClosestPoint(rb.position);
            dashTarget = closestPoint + (closestPoint - (Vector2)transform.position).normalized * playerOffset;
        }

        float startTime = Time.time;
        float elapsedTime = 0f;

        // Move the player towards the dash target over time
        while (elapsedTime < dashTime)
        {
            rb.MovePosition(Vector2.Lerp(rb.position, dashTarget, elapsedTime / dashTime));
            elapsedTime = Time.time - startTime;
            yield return null;
        }

        // Reset the dash state
        isDashing = false;


        //resets collisions back with enemys
        foreach (GameObject enemy in enemies)
        {
            Physics2D.IgnoreCollision(rb.GetComponent<Collider2D>(), enemy.GetComponent<Collider2D>(), false);
        }


    }

    private void Flip()
    {
        // Flip character sprite based on movement direction
        if (isFacingRight && movement.x < 0f || !isFacingRight && movement.x > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }
}
