using UnityEngine;
using System.Collections;

public class LaserScript : MonoBehaviour
{

		// Use this for initialization
		public float speed;
		public GameObject firePrefab;
		void Start ()
		{
		}
	
		// Update is called once per frame
		void FixedUpdate ()
		{
				//transform.TransformDirection ((Vector3.forward * speed) * Time.deltaTime);
				transform.Translate ((Vector3.forward * speed) * Time.deltaTime);
		}
		void OnTriggerEnter (Collider other)
		{
				GameObject fire_fx;
				fire_fx = (GameObject)Instantiate (firePrefab, transform.position, Quaternion.identity);
				fire_fx.transform.SetParent (other.transform);
		}
		
}
