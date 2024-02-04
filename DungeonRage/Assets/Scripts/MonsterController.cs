using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityMovementAI;

public class MonsterController : MonoBehaviour
{
	// Target of the chase
	// (initialise via the Inspector Panel)
	public GameObject target;

	//Instacne of the HUD manager
	public HUDManager hud;

	//Boolean variable to control whether enemy is chasing or not
	//intialised to false so monster doesnt start chasing straight away
	bool isChasing = false;


	bool isEnraged = false;

	bool idle = false;

	// Radius of the circle
	public float radius = 2f;
	// Number of points on circle's circumference
	int numPoints = 64;

	private bool isTouching = false;


	//Instance of steering basics
	SteeringBasics steeringBasics;

	// Reference to animator component
	Animator anim;

	//Bool to declare if monster is flipped or not
	private bool flip;

	public int health = 100;


	// Use this for initialization
	void Start()
	{
		//Find game object with player tag and set to target
		target = GameObject.FindGameObjectWithTag("Player");


        // Initialise the reference to the Animator component
        anim = GetComponent<Animator>();

		steeringBasics = GetComponent<SteeringBasics>();

		////Set the HUD to to the obejct with hud Tag
		hud = GameObject.FindGameObjectWithTag("HUD").GetComponent<HUDManager>();
	}

	// Update is called once per frame
	void Update()
	{
		//Make enemy allways face target
		faceTarget();

		//Debug.Log("idle: " + idle);
		if (idle == false)
		{
			isEnraged = targetCheck();

		}
		//Debug.Log("Enraged: " + isEnraged);
		if (isEnraged)
		{
			anim.SetTrigger("enrage");

		}


		//if (steeringBasics != null && isChasing && isAttacking == false)
		if (steeringBasics != null && isChasing)
		{
			anim.SetTrigger("run");
			//Travel towards the target object at certain speed.

			Vector3 accel = steeringBasics.Arrive(target.transform.position);

			steeringBasics.Steer(accel);

		}

		float distanceToPlayer = Vector2.Distance(transform.position, target.transform.position);
		//Debug.Log("Distance: " + distanceToPlayer);
		//Debug.Log("IS chasing " + isChasing + " is attack + " + isAttacking);

		if (distanceToPlayer <= 0.75f)
		{
			anim.SetTrigger("attack");
		}

	}

	private bool targetCheck()
	{
		// Compute the angle between two triangles in the cricle
		float delta = 2f * Mathf.PI / (float)(numPoints - 1);
		// Stat with angle of 0
		float alpha = 0f;

		// Specify the layer mast for ray casting - ray casting will
		// interact with layer 8 (player) and 9 (walls)
		int layerMask = 1 << 8|1 << 9;

		//Cast rays in circle around monster
		for (int i = 1; i <= numPoints; i++)
		{
			//Radius and alpha give a position of a point around
			//the circle in spherical coordinates 

			// Compute position x from spherical coordinates
			float x = radius * Mathf.Cos(alpha);
			// Compute position y from spherical coordinates
			float y = radius * Mathf.Sin(alpha);

			// Create a ray
			Vector2 ray = new Vector2(x, y);
			ray.x *= transform.lossyScale.x;
			ray.y *= transform.lossyScale.y;

			RaycastHit2D hit = Physics2D.Raycast(transform.position, ray, ray.magnitude, layerMask);
			if (hit.collider != null && hit.collider.tag == "Player")
			{
				return true;
			}
			alpha += delta;
		}

		return false;
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.CompareTag("Player"))
        {
			isTouching = true;
        }
    }

	private void OnTriggerExit2D(Collider2D collision)
	{

		if (collision.gameObject.CompareTag("Player"))
		{
			isTouching = false;
		}
	}



    public void enragedFinished()
	{
		isEnraged = false;
		isChasing = true;

	}

	public void Attack()
	{
		//Vector2 pos = transform.localPosition;
		isChasing = false;

        if (isTouching)
        {
			hud.DealDamage(5);
        }

        //transform.localPosition = pos;

    }

	public void AttackFinished()
	{
		isChasing = true;
		anim.SetTrigger("run");

	}

	private void faceTarget()
	{
		Vector3 scale = transform.localScale;
		

		if (target.transform.position.x < transform.position.x)
		{
			scale.x = Mathf.Abs(scale.x) * -1;
			//* (flip ? -1 : 1);
		}
		else
		{
			scale.x = Mathf.Abs(scale.x);
			//*(flip ? -1 : 1);
		}
		transform.localScale = scale;


	}

	public void TakeDamage(int damage)
    {
		health -= damage;

		if(health <= 0)
        {
			Destroy(gameObject);
        }

    }

	public void Knockback(Vector2 direction, float force)
	{
		// Stop chasing
		isChasing = false;

		// Apply knockback force
		Rigidbody2D rb = GetComponent<Rigidbody2D>();
		rb.velocity = Vector2.zero;
		rb.AddForce(direction * force, ForceMode2D.Impulse);

		// Start a coroutine to simulate a knockback duration
		StartCoroutine(KnockbackDuration());
	}

	private IEnumerator KnockbackDuration()
	{
		// Wait for a short duration to simulate knockback effect
		yield return new WaitForSeconds(0.5f);

		// Resume chasing after knockback duration
		isChasing = true;
		anim.SetTrigger("run");
	}
}
