using UnityEngine;
using System.Collections;

public class Rotate : MonoBehaviour
{

		// Use this for initialization
		public Vector3 rotation;
		void Start ()
		{
	
		}
	
		// Update is called once per frame
		void Update ()
		{
				transform.Rotate (rotation * Time.deltaTime);
		}
}
