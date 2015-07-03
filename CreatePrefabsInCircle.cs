using UnityEngine;
using System.Collections;
using ScriptUtilities;
using System.Collections.Generic;

public class CreatePrefabsInCircle : MonoBehaviour
{
		// Use this for initialization
		public int numberOfObjects;
		public float radius;
		public GameObject prefab;
		public bool drawLines;
		public bool spawnObjects;
		public int circleIndex;
		public List<PrefabGroup> circleList = new List<PrefabGroup> (1);
		private RecursiveScaleChange rsc;
		void Start ()
		{
				if (GetComponent<RecursiveScaleChange> () != null) {
						rsc = GetComponent<RecursiveScaleChange> () as RecursiveScaleChange;
				}
				//SpawnObjects (numberOfObjects, radius, prefab);
				NotificationCenter.DefaultCenter.AddObserver (this, "CityDone");
		}
		public void CityDone ()
		{
				StartCoroutine ("CoSpawnObjects");
		}
		private IEnumerator CoSpawnObjects ()
		{
				if (spawnObjects) {
						foreach (PrefabGroup pg in circleList) {
								foreach (Vector3 p in pg.GetPosList()) {
										GameObject treeCluster = null;
										treeCluster = (GameObject)Instantiate (pg.GetPrefab (), p, Quaternion.identity);
										treeCluster.transform.parent = transform;
										yield return new WaitForSeconds (0.0f);
								}
						}
						rsc.Go ();
				}
				NotificationCenter.DefaultCenter.PostNotification (this, "ScanAStarGrid");
				yield return null;
		}
		
		void OnDrawGizmos ()
		{
				foreach (PrefabGroup pg in circleList) {
						Vector3 lastPos = pg.objectPositions [pg.objectPositions.Count - 1];
						foreach (Vector3 p in pg.objectPositions) {
								Gizmos.color = Color.white;
								Gizmos.DrawLine (lastPos, p);
								Gizmos.color = Color.black;
								Gizmos.DrawWireSphere (p, 1);
								lastPos = p;
						}
				}
				DrawDummyCircle ();
		}
		void DrawDummyCircle ()
		{
				List<Vector3> dummyCircle = CircleUtils.CalculatePointsOnCircle (numberOfObjects, radius, transform.position);
				Vector3 lastPos = dummyCircle [dummyCircle.Count - 1];
				foreach (Vector3 p in dummyCircle) {
						Gizmos.color = Color.magenta;
						Gizmos.DrawLine (lastPos, p);
						Gizmos.color = Color.white;
						Gizmos.DrawWireSphere (p, 1);
						lastPos = p;
				}
		}
		public void AddCircle ()
		{
				Debug.Log ("adding a circle");
				circleList.Add (new PrefabGroup (numberOfObjects, radius, transform.position, prefab));
		}
		[System.Serializable]
		public class PrefabGroup
		{
				public List<Vector3> objectPositions;
				public GameObject objectPrefab;
				public PrefabGroup (int n, float r, Vector3 pos, GameObject go)
				{
						objectPositions = CircleUtils.CalculatePointsOnCircle (n, r, pos);
						objectPrefab = go;
			
				}
				
				public List<Vector3> GetPosList ()
				{
						return objectPositions;
				}
				public GameObject GetPrefab ()
				{
						return objectPrefab;
				}
		}
}

























