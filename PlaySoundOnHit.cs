using UnityEngine;
using System.Collections;

[RequireComponent (typeof(AudioSource))]
public class PlaySoundOnHit : MonoBehaviour
{

		// Use this for initialization
		public float thresholdVelocity;
		public AudioSource soundEffect;
		void Start ()
		{
		}
	
		// Update is called once per frame
		void Update ()
		{
	
		}
	
		void OnCollisionEnter (Collision collision)
		{
				if (collision.relativeVelocity.magnitude >= thresholdVelocity) {
						soundEffect.Play ();
				}
		}
}
