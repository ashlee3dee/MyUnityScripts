using UnityEngine;
using System.Collections;

public class DestroyAnythingThatTouches : MonoBehaviour
{
		void OnTriggerEnter (Collider other)
		{
				//Debug.Log ("enemy got to target");
				Destroy (other.gameObject);
		}
		void OnCollisionEnter (Collision c)
		{
				//Debug.Log ("enemy got to target");
				Destroy (c.gameObject);
		}
}
