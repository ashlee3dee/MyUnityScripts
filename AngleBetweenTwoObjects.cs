using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AngleBetweenTwoObjects : MonoBehaviour
{

		// Use this for initialization
		public Transform target;
		public float angle;
		public Text text;
		private Vector3 startpos;

		void Start ()
		{
				startpos = transform.position;
		}
	
		// Update is called once per frame

		void Update ()
		{
				Vector3 targetDir = target.position - transform.position;
				Vector3 forward = transform.forward;
				angle = Vector3.Angle (targetDir, forward);
				text.text = "angle: " + angle;
				Debug.DrawLine (startpos, transform.position, Color.cyan);
				Debug.DrawLine (transform.position, target.position, Color.cyan);
		}
}
