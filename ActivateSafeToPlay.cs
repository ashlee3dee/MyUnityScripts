using UnityEngine;
using System.Collections;

public class ActivateSafeToPlay : MonoBehaviour
{

		// Use this for initialization
		public GameObject targetGameObject;
		public bool Do;
		void Start ()
		{
				if (Do) {
						NotificationCenter.DefaultCenter.AddObserver (this, "SafeToPlay");
				}
		}
		void SafeToPlay ()
		{
				targetGameObject.SetActive (true);
		}
}
