using System.Collections;
using UnityEngine;
using Pathfinding;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Seeker))]
public class EnemyAI : MonoBehaviour
{
	// What are we chasing?
	public Transform target;

	// The update rate of the path
	public float updateRate = 2f;

	// 
	private Seeker seeker;
	private Rigidbody2D rb;

	//Path
	public Path path;

	// AI speed
	public float speed = 300f;
	public ForceMode2D fMode;

	[HideInInspector]
	public bool pathIsEnded = false;

	// Max distance for the AI from one point till it goes to the next point
	public float nextDirPointDistance = 3;

	// Point we're moving towards
	private int currentDirPoint = 0;

	private bool searchingForPlayer = false;

	void Start()
	{
		seeker = GetComponent<Seeker>();
		rb = GetComponent<Rigidbody2D>();

		if (target == null)
		{
            if (!searchingForPlayer)
            {
				searchingForPlayer = true;
				StartCoroutine(searchForPlayer());
            }
			return;
		}

		// Path to target position is sent to OnPathComplete
		seeker.StartPath(transform.position, target.position, OnPathComplete);

		StartCoroutine(UpdatePath());
	}
	IEnumerator searchForPlayer()
    {
		GameObject searchResult = GameObject.FindGameObjectWithTag("Player");
		if (searchResult == null)
		{
			yield return new WaitForSeconds(0.5f);
			StartCoroutine(searchForPlayer());
		}
		else
        {
			target = searchResult.transform;
			searchingForPlayer = false;
			StartCoroutine (UpdatePath());
			yield return false;
        }
    }
	IEnumerator UpdatePath()
	{
		if (target == null)
		{
			if (!searchingForPlayer)
			{
				searchingForPlayer = true;
				StartCoroutine(searchForPlayer());
			}
			yield return false;
		}

		else
		{

			// Path to target position is sent to OnPathComplete
			seeker.StartPath(transform.position, target.position, OnPathComplete);

			yield return new WaitForSeconds(1f / updateRate);
			StartCoroutine(UpdatePath());
		}
	}

	public void OnPathComplete(Path p)
	{
		if (!p.error)
		{
			path = p;
			currentDirPoint = 0;
		}
	}

	//Better for physics to use fixedupdate rather than update
	void FixedUpdate()
	{
		if (target == null)
		{
			if (!searchingForPlayer)
			{
				searchingForPlayer = true;
				StartCoroutine(searchForPlayer());
			}
			return;
		}

		if (path == null)
			return;

		if (currentDirPoint >= path.vectorPath.Count)
		{
			if (pathIsEnded)
				return;

			Debug.Log("End of path reached.");
			pathIsEnded = true;
			return;

            //Players new position
            StartCoroutine(searchForPlayer());
			pathIsEnded = true;
			return;
		
		}
		pathIsEnded = false;

		//Direction to the next direction point
		Vector3 dir = (path.vectorPath[currentDirPoint] - transform.position).normalized;
		dir *= speed * Time.fixedDeltaTime;

		//AI Movement
		rb.AddForce(dir, fMode);

		float dist = Vector3.Distance(transform.position, path.vectorPath[currentDirPoint]);
		if (dist < nextDirPointDistance)
		{
			currentDirPoint++;
			return;
		}
	}

}
