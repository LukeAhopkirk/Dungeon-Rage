using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 3f;
    private bool isFacingRight = true;
    public Rigidbody2D rb;
    public Animator animator;

    private Vector2 movement;
    private bool isDashing = false;
    private float dashTime = 0.2f;
    private float dashDistance = 2f;
    private float dashCooldown = 2f; // Cooldown time for the dash (currently set to zero for immediate use)
    private float playerOffset = 0.1f; // Adjust as needed to avoid colliding with walls during a dash
    private float lastDashTime = 0f;

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

            // Check for dash input and cooldown
            if (Input.GetKeyDown(KeyCode.Space) && Time.time - lastDashTime > dashCooldown)
            {
                StartCoroutine(Dash());
                lastDashTime = Time.time;
            }


            // Update animator
            animator.SetFloat("Speed", movement.sqrMagnitude);

            // Flip character if needed
            Flip();
        }
    }

    private void FixedUpdate()
    {
        if (!isDashing)
        {
            // Move the character
            rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
        }
    }

    IEnumerator Dash()
    {
        isDashing = true;

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
