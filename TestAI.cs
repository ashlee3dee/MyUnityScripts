using UnityEngine;
using System.Collections;
using Pathfinding;
using UnityEngine.UI;
using ScriptUtilities;
[RequireComponent(typeof(AStarPathNavigator))]
//this class should use the astarpathnavigator script to move and find new paths, all other behavioural logic should
//be contained within this class, including but not limited to:
//1. target tracking
//2. turret movement
//3. weapon firing
//4. health and damage
//5. conditional logic to determine when to seek for a new path
//6. conditional logic to pretest the desired position for objects that may occlude the path of the weapon

//possibly have all enemies be managed by another script, which will contain a list of desired points by other enemies
//this would be useful to discard paths that will end on the desired endpoint of another AI object
//some kind of local collision avoidance would be an ideal feature however, more knowledge is neccesary to write the
//local avoidance algorthm
//AI enemies should also be swapped for a "destroyed" model when hp reaches 0

public class TestAI : MonoBehaviour
{
		public Transform moveTarget;
		public Transform shootTarget;
		public float moveSpeed = 0.0f;
		public float minDistanceFromTarget;
		public float maxDistanceFromTarget;
		public AStarPathNavigator ASPN;
		public void Start ()
		{
				ASPN = gameObject.GetComponent<AStarPathNavigator> ();
				moveTarget = GameObject.FindGameObjectWithTag ("Mothership").transform;
				shootTarget = GameObject.FindGameObjectWithTag ("Player").transform;
				NotificationCenter.DefaultCenter.AddObserver (this, "SafeToPlay");
		}
		void SafeToPlay ()
		{
				SetUpASPN ();
				StartCoroutine ("UpdatePath");
		}
		public void SetUpASPN ()
		{
				ASPN.SetSpeed (moveSpeed);
				ASPN.SetTrueTarget (moveTarget.position);
				ASPN.SetTarget (PickTarget ());
				ASPN.FindPath ();
		}
		public Vector3 PickTarget ()
		{
				float angle = (float)Random.Range (0.0f, 360.0f);
				float radius = (float)Random.Range (minDistanceFromTarget, maxDistanceFromTarget);
				Vector3 proposedTarget = CircleUtils.CalculatePointOnCircle (moveTarget.position, angle, radius);
				RaycastHit hit;
				Ray r = new Ray (moveTarget.position, transform.position - shootTarget.position.normalized);
				
				//Physics.Raycast(proposedTarget,
				return proposedTarget;
		}
		IEnumerator UpdatePath ()
		{
				while (true) {
						if (Vector3.Distance (ASPN.trueTargetPosition, moveTarget.position) > 2) {
								ASPN.SetTrueTarget (moveTarget.position);
								ASPN.SetTarget (PickTarget ());
								ASPN.FindPath ();
						}
						yield return new WaitForSeconds (0.25f);
				}
		}
		public void FixedUpdate ()
		{
				Debug.DrawLine (ASPN.targetPosition, shootTarget.position, Color.red);
				//Debug.DrawRay (transform.position, Vector3.forward * 10);
		}
		void OnDrawGizmos ()
		{
				//Gizmos.DrawWireSphere (ASPN.targetPosition, 1.0f);
		}
}