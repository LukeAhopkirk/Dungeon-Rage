/*
	Created by: Lech Szymanski
				lech.szymanski@otago.ac.nz
				COSC360: Computer Game Design
*/

using UnityEngine;
using System.Collections;

/* This is an example script using A* pathfinding to chase a
 * target game object*/

public class Chase : MonoBehaviour
{

	// Target of the chase
	// (initialise via the Inspector Panel)
	public GameObject target = null;

	//Boolean variable to control whether enemy is chasing or not
	//intialised to false so monster doesnt start chasing straight away
	bool isChasing = false;

	// Radius of the circle
	float radius = 2f;
	// Number of points on circle's circumference
	int numPoints = 64;


	// Chaser's speed
	// (initialise via the Inspector Panel)
	public float speed;

	// Chasing game object must have a AStarPathfinder component - 
	// this is a reference to that component, which will get initialised
	// in the Start() method
	private AStarPathfinder pathfinder = null;

	// Use this for initialization
	void Start()
	{
		//Get the reference to object's AStarPathfinder component
		pathfinder = transform.GetComponent<AStarPathfinder>();
	}

	// Update is called once per frame
	void Update()
	{

		// Compute the angle between two triangles in the cricle
		float delta = 2f * Mathf.PI / (float)(numPoints - 1);
		// Stat with angle of 0
		float alpha = 0f;

		// Specify the layer mast for ray casting - ray casting will
		// interact with layer 8 (player) and 7 (walls)
		int layerMask = 1 << 8 | 1 << 7;

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
				//Debug.Log("Ray hit: " + hit.collider.name);
				isChasing = true;
				
			}
			alpha += delta;
		}

		if (pathfinder != null && isChasing)
		{
			//Travel towards the target object at certain speed.
			pathfinder.GoTowards(target, speed);
		}

		float distanceToPlayer = Vector2.Distance(transform.position, target.transform.position);

		if(distanceToPlayer <= 0.5f)
        {
			//Debug.Log("Distance: " + distanceToPlayer);
		}
	}



    //void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;

    //    // Compute the angle between two triangles in the cricle
    //    float delta = 2f * Mathf.PI / (float)(numPoints - 1);
    //    // Stat with angle of 0
    //    float alpha = 0f;

    //    //Other vertices will be positioned evenly around the circle
    //    for (int i = 1; i <= numPoints; i++)
    //    {
    //        //Radius and alpha give a position of a point around
    //        //the circle in spherical coordinates 

    //        // Compute position x from spherical coordinates
    //        float x = radius * Mathf.Cos(alpha);
    //        // Compute position y from spherical coordinates
    //        float y = radius * Mathf.Sin(alpha);

    //        // Create a ray
    //        Vector3 ray = new Vector3(x, y, transform.position.z);

    //        Gizmos.DrawRay(transform.position, ray);

    //        // Increase the angle to get the next positon around the circle
    //        alpha += delta;
    //    }
    //}
}
