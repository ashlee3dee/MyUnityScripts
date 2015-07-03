using UnityEngine;
using System.Collections;

public class TimedDeath : MonoBehaviour
{

		// Use this for initialization
		public float lifeSpan;
		void Start ()
		{
				Destroy (gameObject, lifeSpan);
		}
}
