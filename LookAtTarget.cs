using UnityEngine;
using System.Collections;

public class LookAtTarget : MonoBehaviour
{

		// Use this for initialization
		public Transform target;
		void Start ()
		{
	
		}
	
		// Update is called once per frame
		void Update ()
		{
				transform.LookAt (target);
		}
}
