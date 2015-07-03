using UnityEngine;
using System.Collections;

public class ObjectSpawner : MonoBehaviour
{
		//create coroutine that handles enemy wave spawning
		//do this by starting with an integer below 10
		//spawn that many enemies with a x seconds pause between each (public variable)
		//where t is the iterator increased upon by 1 each time the loop
		//satisfies all conditional gameplay checks
		//USE EXPONENTIAL GROWTH FORMULA
		//spawns a new wave of enemies t time after last wave is destroyed
		//do this by passing the variable above that determines the number of enemies to be spawned
		//to a helper function that does gameobject child counts and compares the result
		
		// Use this for initialization
		public GameObject prefab;
		public float delay;
		public int maxObjects;
		private bool isSafeToPlay;
		private bool isSpawning = false;
		int currentObjects;
		void Start ()
		{
				NotificationCenter.DefaultCenter.AddObserver (this, "SafeToPlay");
				/*MyGUIWindow newWindow = gameObject.AddComponent<MyGUIWindow> ();
				newWindow.SetDimensions (new Vector2 (220, 10), 100);
				newWindow.SetWindowName ("spawner");
				newWindow.AddButton ("start", this, "SpawnObjects");
				newWindow.AddButton ("stop", this, "StopSpawning");*/
		}
		private void SpawnObjects ()
		{
				if (isSafeToPlay) {
						isSpawning = true;
						StartCoroutine ("CoSpawnObjects");
				}
		}
		private void StopSpawning ()
		{
				if (isSafeToPlay) {
						isSpawning = false;
						StopCoroutine ("CoSpawnObjects");
				}
		}
		void ToggleSpawning ()
		{
				if (isSpawning == false) {
						SpawnObjects ();
				} else {
						StopSpawning ();
				}
		}
		private IEnumerator CoSpawnObjects ()
		{
				while (currentObjects < maxObjects) {
						GameObject newObject = (GameObject)Instantiate (prefab, transform.position, Quaternion.identity);
						newObject.transform.SetParent (transform);
						yield return new WaitForSeconds (delay);
				}
				yield return null;
		}
		private void SafeToPlay ()
		{
				isSafeToPlay = true;
		}
}
