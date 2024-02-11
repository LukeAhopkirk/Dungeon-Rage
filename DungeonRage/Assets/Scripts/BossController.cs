using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BossController : MonoBehaviour
{

    public GameObject FloatingTextPrefab;
    // Target of the chase
    // (initialise via the Inspector Panel)
    public GameObject target;

    //Instacne of the HUD manager
    public HUDManager hud;

    private bool flip;

    //Instances of the enemeis spawned
    public GameObject basicEnemy;
    public GameObject tankEnemy;
    public GameObject rangedEnemy;



    ////Instance of steering basics
    SteeringBasics steeringBasics;

    // Reference to animator component
    Animator animator;


    //time between boss attacks
    public float minInterval;
    public float maxInterval;

    //Health of enemy
    public float health = 1000;
    private float maxHealth;

    //prefab to spawn around boss
    public GameObject shieldPrefab;

    //List to hold all the different enemeis
    HashSet<GameObject> enemyTypes = new HashSet<GameObject>();

    //number of enemies spawned each routine
    public float maxEnemies = 5;

    //Time betweeen enemies spawning
    public float minSpawnInterval = 0.5f;
    public float maxSpawnInterval = 1f;

    //method to control the player entering room and boss starting to attack
    private static bool attacking = false;

    //Boolean to control whether the next attack should be spawning, set to true so anu
    private bool spawningNext = true;

    //Reference to the boss shot
    public GameObject projectilePrefab;

    //Float to hold the force of the projectile.
    public float speed;

    private bool isChasing = true;

    //Settor method for to start the boss attacking
    public static void setAttacking()
    {
        attacking = true;
        Debug.Log($"Test setter method attacking is {attacking}");
    }

    // Start is called before the first frame update
    void Start()
    {
        //set the intial health to the max health
        maxHealth = health;

        //Add the intial basic enemy to the hashset
        enemyTypes.Add(basicEnemy);
        //Find game object with player tag and set to target
        target = GameObject.FindGameObjectWithTag("Player");

        // Initialise the reference to the Animator component
        animator = GetComponent<Animator>();

        steeringBasics = GetComponent<SteeringBasics>();

        ////Set the HUD to to the obejct with hud Tag
        hud = GameObject.FindGameObjectWithTag("HUD").GetComponent<HUDManager>();
    }

    // Update is called once per frame
    void Update()
    {
        faceTarget();
        //the attacking varaible is set the first time the player enters the room in room controller
        //Debug.Log(attacking == true);
        if (attacking)
        {
            Debug.Log("Starting routine");
            StartCoroutine(AttackCoroutine());
            attacking = false;
        }

        if (steeringBasics != null && isChasing)
        {
            //Travel towards the target object at certain speed.
            Vector3 accel = steeringBasics.Arrive(target.transform.position);

            steeringBasics.Steer(accel);
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        Debug.Log($"boss taken damage {damage} health left {health}");


        if (health <= 0)
        {
            Debug.Log("dead");
            Destroy(gameObject);
            //animator.SetTrigger("death");
            hud.GetExperience(10);
        }
        if (health < 2f / 3f * maxHealth)
        {
            enemyTypes.Add(tankEnemy);
        }
        if (health < 1f / 3f * maxHealth)
        {
            enemyTypes.Add(rangedEnemy);
        }

        if (FloatingTextPrefab)
        {
            ShowFloatingText(damage.ToString());
        }
    }

    void ShowFloatingText(string text)
    {
        var go = Instantiate(FloatingTextPrefab, transform.position, Quaternion.identity, transform);
        var textMeshPro = go.GetComponent<TextMeshProUGUI>();

        if (textMeshPro == null)
        {
            // If TextMeshPro component is not found, try getting TextMeshPro - Text component
            var textMeshProText = go.GetComponent<TextMeshPro>();
            if (textMeshProText != null)
            {
                // Set text for TextMeshPro - Text component
                textMeshProText.text = text;
                if (flip)
                {
                    Vector3 scale = textMeshProText.transform.localScale;
                    scale.x *= -1f;
                    textMeshProText.transform.localScale = scale;
                }
            }
        }
    }

    private IEnumerator AttackCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minInterval, maxInterval));
            if (spawningNext)
            {
                isChasing = false;
                Rigidbody2D rb = GetComponent<Rigidbody2D>();
                rb.velocity = Vector2.zero;
                GameObject sheild = Instantiate(shieldPrefab, transform.position, Quaternion.identity);
                spawnEnemies();
                yield return new WaitForSeconds(3);
                Destroy(sheild);
                spawningNext = false;
                isChasing = true;
            }
            else
            {
                animator.SetTrigger("charge");
                spawningNext = true;
            }
        }


    }
    //Method to spawn enemies
    private void spawnEnemies()
    {
        Debug.Log("spawning");
        //Create shield game object around player


        //spawn enemies
        foreach (var enemyType in enemyTypes)
        {
            spawnEnemyType(enemyType);
        }
    }


    private void spawnEnemyType(GameObject enemyType)
    {
        float noSpawn = 0;
        while(noSpawn < maxEnemies)
        {
            // Generate a random angle in radians
            float angle = Random.Range(0f, Mathf.PI * 2f); // 0 to 2π

            // Calculate random position within the radius
            float radius = Random.Range(0f, 2f); // Within 0 to 2 units
            float spawnX = transform.position.x + radius * Mathf.Cos(angle);
            float spawnY = transform.position.y + radius * Mathf.Sin(angle);

            Vector2 spawnPosition = new Vector2(spawnX, spawnY);

            Instantiate(enemyType, spawnPosition, Quaternion.identity);
            noSpawn++;
        }
    }

    //Method that shoots outburst(like rage ability 3)
    private void fire()
    {
        for(int rounds = 0; rounds <=3; rounds++)
        {
            StartCoroutine(FireCoroutine());
        }
        animator.SetTrigger("decharge");
    }

    private IEnumerator FireCoroutine()
    {
            GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);

            // Get the Rigidbody2D component of the spell object
            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();

            // Ignore the collision between the projectile and the caster
            Physics2D.IgnoreCollision(projectile.GetComponent<Collider2D>(), GetComponent<Collider2D>());

            // Create a vector for the position of the projectile
            Vector2 pos = new Vector2(projectile.transform.position.x, projectile.transform.position.y);

            // Create a vector to hold the player's position
            Vector2 targetPos = new Vector2(target.transform.position.x, target.transform.position.y);

            // Calculate the direction from the player to the target
            Vector2 directionToPlayer = (targetPos - pos).normalized;

            // Calculate a random offset within a radius around the player
            float randomOffsetX = Random.Range(-0.2f, 0.2f);
            float randomOffsetY = Random.Range(-0.2f, 0.2f);

            // Apply the random offset to the direction
            Vector2 direction = directionToPlayer + new Vector2(randomOffsetX, randomOffsetY);


            // Add force in the direction
            rb.AddForce(direction * speed, ForceMode2D.Impulse);

        yield return new WaitForSeconds(1);

    }

    private void faceTarget()
    {
        Vector3 scale = transform.localScale;


        if (target.transform.position.x > transform.position.x)
        {
            scale.x = Mathf.Abs(scale.x) * -1;
            //* (flip ? -1 : 1);
            flip = true;
        }
        else
        {
            scale.x = Mathf.Abs(scale.x);
            //*(flip ? -1 : 1);
            flip = false;
        }
        transform.localScale = scale;

    }

}
