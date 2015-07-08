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
		//public 
		public float moveSpeed = 0.0f;
		public float minDistanceFromTarget;
		public float maxDistanceFromTarget;
		public AStarPathNavigator ASPN;
		public GameObject weapon;
		public LayerMask raycastMask;
		private bool isSafeToPlay;
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
				isSafeToPlay = true;
		}
		public void SetUpASPN ()
		{
				ASPN.SetSpeed (moveSpeed);
				//ASPN.SetTrueTarget (moveTarget.position);
				ASPN.SetTarget (PickNewTarget (moveTarget.position));
				ASPN.FindPath ();
		}
		public void TargetMet ()
		{
				ASPN.StopMove ();
		}
		public void FireWeapon ()
		{
				weapon.transform.LookAt (shootTarget.position);
		}
		public bool TestTarget (Vector3 t)
		{
				//weapon.transform.rotation = Quaternion.identity;
				Vector3 proposedTarget = t;
				Vector3 heading = Vector3Utils.Subtract (shootTarget.position, t);
				float distance = heading.magnitude;
				//Debug.DrawRay (transform.position, heading / distance);
				Ray r = new Ray (proposedTarget, heading / distance);
				RaycastHit hit;
				//Debug.DrawRay (proposedTarget, heading / distance, Color.white, 100.0f);
				if (Physics.Raycast (r, out hit, 100.0f, raycastMask)) {
						if (hit.transform.tag == "Player") {
								Debug.DrawLine (r.origin, hit.point, Color.green);
								return true;
						}
						Debug.DrawLine (r.origin, hit.point, Color.red);
				}
				
				return false;
		}
		public Vector3 CalculateTarget (Vector3 t)
		{
				float angle = (float)Random.Range (0.0f, 360.0f);
				float radius = (float)Random.Range (minDistanceFromTarget, maxDistanceFromTarget);
				Vector3 proposedTarget = CircleUtils.CalculatePointOnCircle (t, angle, radius);
				return proposedTarget;
		}
		public Vector3 PickNewTarget (Vector3 t)
		{
				Vector3 target = CalculateTarget (t);
				while (!TestTarget (target)) {
						target = CalculateTarget (t);
				}
				return target;
		}
		IEnumerator UpdatePath ()
		{
				while (true) {
						//replace the distance based retargeting system with
						//a raycast occlusion based system
						//should cast a ray towards shot target from proposed target
						//if occludes with any tile, recalculate target
						if (!TestTarget (ASPN.targetPosition)) {
								//ASPN.SetTrueTarget (moveTarget.position);
								ASPN.SetTarget (PickNewTarget (moveTarget.position));
								ASPN.FindPath ();
						}
						yield return new WaitForSeconds (0.25f);
				}
		}
		public void FixedUpdate ()
		{
				//Debug.DrawLine (ASPN.targetPosition, shootTarget.position, Color.red);
				//Debug.DrawRay (transform.position, Vector3.forward * 10);
		}
		void OnDrawGizmos ()
		{
				if (isSafeToPlay) {
						Gizmos.color = Color.green;
						Gizmos.DrawWireSphere (ASPN.targetPosition, 1.0f);
				}
		}
}