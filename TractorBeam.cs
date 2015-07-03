using UnityEngine;
using System.Collections;

public class TractorBeam : MonoBehaviour
{
		public float tractorForce;
		public float tractorLength;
		public Transform target;
		void OnTriggerenter (Collider other)
		{
		}
		void OnTriggerStay (Collider other)
		{
				Vector3 force = ScriptUtilities.Vector3Utils.Subtract (target.position, other.transform.position);
				float distance = Vector3.Distance (target.position, other.transform.position);
				float fallOffMultiplier = distance / tractorLength;
				Debug.DrawLine (target.position, other.transform.position, Color.blue);
				other.attachedRigidbody.AddForce (force.normalized * (tractorForce / fallOffMultiplier));
		}
		void OnTriggerExit (Collider other)
		{
				Debug.DrawLine (target.position, other.transform.position, Color.red);
		}
}
