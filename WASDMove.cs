using UnityEngine;
using System.Collections;

public class WASDMove : MonoBehaviour
{
		public float moveSpeed;
		private Vector3 moveVec3;
	
		// Use this for initialization
		void Start ()
		{
				moveVec3 = new Vector3 (0, 0, 0);
		}
		// Update is called once per frame
		void Update ()
		{
				GetInput ();
		}
		void FixedUpdate ()
		{
				Vector3 moveVelocityVec3 = moveVec3 * moveSpeed;
				transform.Translate (moveVelocityVec3);
		}
	
		void GetInput ()
		{
				moveVec3.z = Input.GetAxis ("Vertical");
				moveVec3.y = Input.GetAxis ("Up/Down");
				moveVec3.x = Input.GetAxis ("Horizontal");
		}
}
