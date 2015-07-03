using UnityEngine;
using System.Collections;
//Note this line, if it is left out, the script won't know that the class 'Path' exists and it will throw compiler errors
//This line should always be present at the top of scripts which use pathfinding
using Pathfinding;
[RequireComponent(typeof(Seeker))]
public class AStarPathNavigator : MonoBehaviour
{
		//The point to move to
		//point that is calculated by AI
		public Vector3 targetPosition;
		//point that AI is calculating with
		public Vector3 trueTargetPosition;
		//the seeker
		private Seeker seeker;
		//the AI
		private TestAI ai;
	
		//The calculated path
		public Path path;
	
		//The AI's speed per second
		public float speed;
	
		//The max distance from the AI to a waypoint for it to continue to the next waypoint
		public float nextWaypointDistance = 3;
	
		//The waypoint we are currently moving towards
		private int currentWaypoint = 0;
		public void Start ()
		{
				seeker = GetComponent<Seeker> ();
				ai = GetComponent<TestAI> ();
		}
		public void SetTrueTarget (Vector3 t)
		{
				trueTargetPosition = t;
		}
		public void SetTarget (Vector3 t)
		{
				targetPosition = t;
		}
		public void SetSpeed (float s)
		{
				speed = s;
		}
		public void FindPath ()
		{
				StopCoroutine ("CoMove");
				seeker.StartPath (transform.position, targetPosition, OnPathComplete);
		}
		public void StartMove ()
		{
				StartCoroutine ("CoMove");
		}
		public void StopMove ()
		{
				StopCoroutine ("CoMove");
		}
		IEnumerator CoMove ()
		{
				while (true) {
						if (currentWaypoint < path.vectorPath.Count) {
								//Vector3 lookPos = ScriptUtilities.Vector3Utils.Subtract (path.vectorPath [currentWaypoint], transform.position);
								//lookPos = lookPos.normalized * speed;
								transform.LookAt (path.vectorPath [currentWaypoint]);
								transform.Translate ((Vector3.forward * speed) * Time.fixedDeltaTime);
								if (Vector3.Distance (transform.position, path.vectorPath [currentWaypoint]) < nextWaypointDistance) {
										currentWaypoint++;
								}
						}
						yield return new WaitForSeconds (0.05f);
				}
		}
		public void OnPathComplete (Path p)
		{
				Debug.Log ("Yay, we got a path back. Did it have an error? " + p.error);
				if (!p.error) {
						path = p;
						//Reset the waypoint counter
						currentWaypoint = 0;
						StartMove ();
				}
		}
	
		public void FixedUpdate ()
		{
				if (path == null) {
						//We have no path to move after yet
						return;
				}
		
				if (currentWaypoint >= path.vectorPath.Count) {
						//StopMove ();
						return;
				}
		}
} 