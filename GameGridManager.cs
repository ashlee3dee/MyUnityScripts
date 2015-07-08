using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
public class GameGridManager : MonoBehaviour
{
		GameTile[,] tiles;// = new GameTile[,];
		// Use this for initialization
		public GameObject destroyedTile;
		private bool gridSpawned = false;
		private GameTile selectedTile;
		private int enemyDeathCount = 0;
		//private MyGUIWindow window = new MyGUIWindow ();
		//public GameObject killCountTextGameObject;
		public LayerMask raycastMask;
		public Text killCountText;
		public AstarPath AStarGrid;
		//Vector2 selectedTilePos;
		void Start ()
		{
				//selectedTilePos = new Vector2 (0.0f, 0.0f);
				//tiles = new GameTile[,]{};
				/*MyGUIWindow window = gameObject.AddComponent<MyGUIWindow> ();
				window.SetDimensions (new Vector2 (Screen.width / 2, 10), 200);
				window.SetWindowName ("game grid control");
				window.AddButton ("reparent tiles", this, "ReparentTiles");
				//window.AddButton ("update the tile data", this, "TestTile");
				*/
				//window = gameObject.AddComponent<MyGUIWindow> ();
				//window.SetDimensions (new Vector2 (10, 40), 100.0f);
				//window.SetWindowName ("killcount");
				//window.AddLabel (enemyDeathCount + "");
				NotificationCenter.DefaultCenter.AddObserver (this, "GetAllTilesInGrid");
				NotificationCenter.DefaultCenter.AddObserver (this, "DestroyTiles");
				//NotificationCenter.DefaultCenter.AddObserver (this, "ScanAStarGrid");
				NotificationCenter.DefaultCenter.AddObserver (this, "EnemyDestroyed");
				NotificationCenter.DefaultCenter.AddObserver (this, "ScanAStarGrid");
		}
		// Update is called once per frame
		void FixedUpdate ()
		{
				if (gridSpawned) {
						//HandleInput ();
				}
		}
		void EnemyDestroyed ()
		{
				enemyDeathCount++;
				
				killCountText.text = "Kill Count: " + enemyDeathCount;
		}
		void ScanAStarGrid ()
		{
				AStarGrid.enabled = true;
				AStarGrid.Scan ();
				NotificationCenter.DefaultCenter.PostNotification (this, "SafeToPlay");
		}
		void DestroyTiles ()
		{
				StartCoroutine ("CoDestroyTiles");
		}
		void ReparentTiles ()
		{
				Transform tT = GameObject.FindGameObjectWithTag ("TestTag").transform;
				int cC = transform.childCount;
				for (int i = 0; i < cC; i++) {
						transform.GetChild (i).SetParent (tT);
				}
		}
		IEnumerator CoDestroyTiles ()
		{
				NotificationCenter.DefaultCenter.PostNotification (this, "Destroy");
				yield return new WaitForSeconds (0.01f);
				for (int ix =0; ix<tiles.GetLength(0); ix++) {
						for (int iy =0; iy <tiles.GetLength(1); iy++) {
								//GameTile tile = ;
								if (!tiles [ix, iy]) {
										GPoint gP = new GPoint (new Vector2 (ix + 1, iy + 1), GPoint.PType.TILE_DESTROYED);
										GameObject newTile = (GameObject)Instantiate (destroyedTile, gP.GetWorldPosition (), Quaternion.identity);
										newTile.name = "/x:" + gP.GetPosition ().x + "$/y:" + gP.GetPosition ().y + "$/t:" + gP.GetPType () + "$";
										newTile.transform.parent = transform;
										tiles [ix, iy] = newTile.GetComponent<GameTile> ();
										tiles [ix, iy].SetData ();
								}
						}
				}
				AStarGrid.Scan ();
				StopCoroutine ("CoDestroyTiles");
		}
		void SelectNewTile (Vector2 newPos)
		{
				if (selectedTile != null)
						selectedTile.DeSelect ();
				if (newPos != -Vector2.one) {
						selectedTile = tiles [(int)newPos.x, (int)newPos.y];
						selectedTile.Select ();
				} else {
						selectedTile = null;
				}
		}
		void HandleInput ()
		{
				
				//Vector3 p = Camera.main.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, 5));
				Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
				RaycastHit hit;
				if (Physics.Raycast (ray, out hit, 100.0f, raycastMask)) {
						Debug.DrawLine (ray.origin, hit.point);
						//tiles [(int)selectedTilePos.x, (int)selectedTilePos.y].DeSelect ();
						if (hit.collider.CompareTag ("Tile")) {
								Vector2 pos = hit.transform.GetComponent<GameTile> ().position;
								//if (pos != selectedTile.position)
								SelectNewTile (pos);
								if (Input.GetKeyDown (KeyCode.Space)) {
										selectedTile.ToggleActive ();
								}
						}
				} else {
						SelectNewTile (-Vector2.one);
						//tiles [(int)selectedTilePos.x, (int)selectedTilePos.y].DeSelect ();
				}
		}
		void GetAllTilesInGrid ()
		{
				Debug.Log ("setting data and creating references to all tiles");
				NotificationCenter.DefaultCenter.PostNotification (this, "SetData");
				int maxX = 0, maxY = 0;
				for (int i = 0; i < gameObject.transform.childCount; i++) {
						GameTile tGT = gameObject.transform.GetChild (i).GetComponent<GameTile> ();
						if (tGT.position.x > maxX/*tile x greater than last tile x*/) {
								maxX = (int)tGT.position.x;
						}
						if (tGT.position.y > maxY/*tile y greater than last tile y*/) {
								maxY = (int)tGT.position.y;
						}
				}
				tiles = new GameTile[maxX + 1, maxY + 1];
				for (int i = 0; i < gameObject.transform.childCount; i++) {
						GameTile tGT = gameObject.transform.GetChild (i).GetComponent<GameTile> ();
						tiles [(int)tGT.position.x, (int)tGT.position.y] = tGT;
				}
				gridSpawned = true;
				//NotificationCenter.DefaultCenter.PostNotification (this, "ScanAStarGrid");
				NotificationCenter.DefaultCenter.PostNotification (this, "CityDone");
		}
		void OnGui ()
		{
				GUI.Label (new Rect (Screen.width / 2, Screen.height / 2, 50, 50), "selected: " + selectedTile.position + selectedTile.type);
		}
}
