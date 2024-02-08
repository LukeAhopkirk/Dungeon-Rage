using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankController : MonoBehaviour
{

    // Target of the chase
    // (initialise via the Inspector Panel)
    public GameObject target;

    //Instacne of the HUD manager
    public HUDManager hud;

    bool idle = false;

    // Radius of the circle
    public float radius = 2f;

    // Number of points on circle's circumference
    int numPoints = 64;

    //Instance of steering basics
    SteeringBasics steeringBasics;

    // Reference to animator component
    Animator animator;

    //Boolean variable to control whether enemy is chasing or not
    //intialised to false so monster doesnt start chasing straight away
    bool isChasing = false;


    // Start is called before the first frame update
    void Start()
    {
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

        if (idle == false)
        {
            isChasing = targetCheck(); 

        }

        if (steeringBasics != null && isChasing)
        {
            animator.SetTrigger("walk");
            //Travel towards the target object at certain speed.

            Vector3 accel = steeringBasics.Arrive(target.transform.position);

            steeringBasics.Steer(accel);
        }

        float distanceToPlayer = Vector2.Distance(transform.position, target.transform.position);
        if (distanceToPlayer <= 1)
        {
            animator.SetTrigger("attack");
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
        int layerMask = 1 << 8 | 1 << 9;

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

    private void faceTarget()
    {
        Vector3 scale = transform.localScale;


        if (target.transform.position.x > transform.position.x)
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

}
