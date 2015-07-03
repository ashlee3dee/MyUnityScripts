using UnityEngine;
using System.Collections;

public class BulletScript : MonoBehaviour
{

		// Use this for initialization
		public float speed;
		void Start ()
		{
				transform.parent = null;
		}
	
		// Update is called once per frame
		void FixedUpdate ()
		{
				if (transform.position.y >= 10)
						Destroy (gameObject);
				transform.Translate ((Vector3.forward * speed) * Time.fixedDeltaTime);
		}
		void OnTriggerEnter (Collider other)
		{
				GetComponent<AudioSource> ().Play ();
				Destroy (gameObject, 0.125f);
		}
}
