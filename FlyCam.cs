using UnityEngine;
using System.Collections;

/*	flycam 1.1a
	by nicholas alaniz
	17 0ctober, 2014
	//features//
	vertical and horizontal movement
		acheived with rigidbody/force combo
	vertical and horizontal rotation
		acheived with direct transform rotation*/
//component requirements
[RequireComponent (typeof(Rigidbody))]
[RequireComponent(typeof(Camera))]
public class FlyCam : MonoBehaviour
{
		// Use this for initialization
		//public vars
		public Camera cam;
		public float moveSpeed;
		public float rotateSpeed;
		public bool drawDebugLines;
		//private vars
		private Vector3 moveVec3;
		//private Vector3 rotVec3;
		public GameObject target;
		private Vector3 offset;
		void Awake ()
		{
				//cam.depthTextureMode = DepthTextureMode.DepthNormals;
				moveVec3 = new Vector3 (0, 0, 0);
				//rotVec3 = new Vector3 (0, 0, 0);
				if (moveSpeed == 0.0f) {
						string speedWarning = "set the speed multiplier(s) in the inspector.";
						Debug.LogWarning (speedWarning);
				}
		}
		void Start ()
		{
				//offset = new Vector3 (0.0f, 5.0f, -5.0f);
				//target = GameObject.FindGameObjectWithTag ("Mothership").transform.GetChild (0).gameObject;
				//transform.position = target.transform.position + offset;
		}
		
		// Update is called once per frame
		void Update ()
		{
				//handle input every frame
				GetInput ();
				//debug
				
				if (drawDebugLines) {
						DebugDraw ();
				}

		}
		void FixedUpdate ()
		{
				//doing all physics caluclations in fixedupdate for accuracy
				//calculate and apply rotation vector
				//Vector3 rotateVelocityVec3 = rotVec3 * rotateSpeed;
				Vector3 moveVelocityVec3 = moveVec3 * moveSpeed;
				//transform rotations need to be done idependently
				//transform.position = target.transform.position + offset;
				//calculate and apply movement vector
				//transform.RotateAround (target.transform.position, target.transform.right, rotVec3.x); // x rot
				//transform.RotateAround (target.transform.position, target.transform.up, rotVec3.y); // y rot
				//Camera.main.transform.RotateAround (transform.position, transform.right, rotateVelocityVec3.x * Time.deltaTime);
				//transform.RotateAround (transform.position, Vector3.up, rotateVelocityVec3.y * Time.deltaTime);
				Quaternion yRotQuaternion = new Quaternion (0.0f, transform.rotation.y, 0.0f, transform.rotation.w);
				GetComponent<Rigidbody> ().AddForce (transform.rotation * new Vector3 (moveVelocityVec3.x, moveVelocityVec3.y, moveVelocityVec3.z));
				
		}
		
		void GetInput ()
		{
				//get inputs using default axis interface
				moveVec3.z = Input.GetAxis ("Vertical");
				moveVec3.y = Input.GetAxis ("Elevation");
				moveVec3.x = Input.GetAxis ("Horizontal");
				
				//rotVec3.y = Input.GetAxis ("Rotation");
				//rotVec3.x = Input.GetAxis ("Elevation");
		}
		void DebugDraw ()
		{
				//both ray endpoints are rotated first
				//movement vector debug
				//Debug.DrawRay (transform.position, (transform.rotation * moveVec3) * 2.5f, Color.black);
				//rotation vector debug
				//Debug.DrawRay (transform.position, (transform.rotation * rotVec3) * 2.5f, Color.red);
		}
}
