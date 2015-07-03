using UnityEngine;
using System.Collections;

public class FollowMouse : MonoBehaviour
{

		// Use this for initialization
		private bool isSafeToPlay = false;
		public LayerMask raycastMask;
		void Start ()
		{
				NotificationCenter.DefaultCenter.AddObserver (this, "SafeToPlay");
		}
		void SafeToPlay ()
		{
				isSafeToPlay = true;
		}
		// Update is called once per frame
		void FixedUpdate ()
		{
				if (isSafeToPlay) {
						Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
						RaycastHit hit;
						if (Physics.Raycast (ray, out hit, 1000.0f, raycastMask)) {
								Vector3 onFloor = new Vector3 (hit.point.x, 0.0f, hit.point.z);
								//Instantiate (prefab, onFloor, Quaternion.identity);
								//MoveShip (onFloor);
								transform.position = onFloor;
								Debug.DrawLine (ray.origin, onFloor, Color.green);
						}
				}
		}
}
