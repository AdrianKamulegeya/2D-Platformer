using System.Collections;
using UnityEngine;
using Pathfinding;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Seeker))]
public class EnemyAI : MonoBehaviour
{

	public Transform target;				//What to chase?
	public float updateRate = 2f;			//How many times each second we update path
	
	public Path path;						//stores AI path
	public float speed = 300f;				//AI speed per second
	public ForceMode2D fMode;				//controls how forces applied to RigidBody
	public float nextWaypointDistance = 3f;	//max distance from AI to waypoint
											//for it to continue to next way point
	
	[HideInInspector]
	public bool pathHasEnded = false;
	
	//Caching
	private Seeker seeker;
	private Rigidbody2D rb;
	
	private int currentWaypoint;			//waypoint we are currently moving towards
	private float nextTimeToSearch;

	private bool searchingForPlayer = false;
	private float searchRate = 0.5f;

	private void Start()
	{
		seeker = GetComponent<Seeker>();
		rb = GetComponent<Rigidbody2D>();

		if (target == null)
		{  

			return;
		}
		
		//Start a new path to target position, then return result to OnPathComplete method
		seeker.StartPath(transform.position, target.position, OnPathComplete);


		StartCoroutine(UpdatePath());
	}

	private IEnumerator SearchForPlayer()
	{
		GameObject searchResult = GameObject.FindGameObjectWithTag("Player");
		if (searchResult == null)
		{
			yield return new WaitForSeconds(searchRate);
			StartCoroutine(SearchForPlayer());
		}
		else
		{
			target = searchResult.transform;
			searchingForPlayer = !searchingForPlayer;
			StartCoroutine(UpdatePath());
			yield return false;
		}
	}
	
	private IEnumerator UpdatePath()
	{
		if (target == null)
		{
			if (!searchingForPlayer)
			{
				searchingForPlayer = !searchingForPlayer;
				StartCoroutine(SearchForPlayer());
			}
			yield return false;
		}
		else
		{
			seeker.StartPath(transform.position, target.position, OnPathComplete);
		}
		
		
		yield return  new WaitForSeconds(1f/updateRate);
		StartCoroutine(UpdatePath());
	}
	
	public void OnPathComplete(Path p)
	{
		Debug.Log("We got a path. Errors? " + p.error);
		if (!p.error)
		{
			path = p;
			currentWaypoint = 0;
		}
	}

	private void FixedUpdate()
	{
		if (target == null)
		{
			if (!searchingForPlayer)
			{
				searchingForPlayer = !searchingForPlayer;
				StartCoroutine(SearchForPlayer());
			}
			return;
		}
		
		//TODO: Always look at player?

		if (path == null)
		{
			return;
		}

		if (currentWaypoint >= path.vectorPath.Count)
		{
			if (pathHasEnded)
			{
				return;
			}
			Debug.Log("End of path reached");
			pathHasEnded = true;
			return;
		}
		
		pathHasEnded = false;
		
		//Direction to next waypoint
		Vector3 direction = (path.vectorPath[currentWaypoint] - transform.position).normalized;
		direction *= speed * Time.fixedDeltaTime;
		
		//Move the AI
		rb.AddForce(direction, fMode);

		float distance = Vector3.Distance(transform.position, path.vectorPath[currentWaypoint]);

		if (distance < nextWaypointDistance)
		{
			currentWaypoint++;
		}
	}
	
}
