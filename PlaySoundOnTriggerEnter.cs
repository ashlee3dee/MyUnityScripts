using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class PlaySoundOnTriggerEnter : MonoBehaviour
{
		void OnTriggerEnter (Collider other)
		{
				gameObject.GetComponent<AudioSource> ().Play ();
		}
}
