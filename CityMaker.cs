using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
namespace CityMaker
{
		public class CityMaker : MonoBehaviour
		{
				public Grid gameGrid;
				private List<Grid> grids = new List<Grid> ();
				public List<GameObject> tilePrefabs;
				public List<GameObject> buildingPrefabs;
				public List<GameObject> housePrefabs;
				private List<GPoint.PType> buildingTypes;
				public Vector2 size, position, blockSize;
				public Vector2 windowPos;
				public float windowWidth;
				public int walkSteps = 0;
		
				public GPoint.PType fillType;
				public GameObject tilePool;
				void Start ()
				{
						tilePool = (GameObject)GameObject.FindGameObjectWithTag ("GGM");
						/*MyGUIWindow window = gameObject.AddComponent<MyGUIWindow> ();
						window.SetWindowName ("game utility");
						window.SetDimensions (windowPos, windowWidth);
						window.AddButton ("make game grid", this, "MakeGameGrid");
						window.AddButton ("add grid", this, "AddGrid");
						window.AddButton ("destroy buffer grids", this, "DestroyBufferGrids");
						window.AddButton ("clear buffer grids", this, "ClearBufferGrids");
						window.AddButton ("spawntiles", this, "SpawnTiles");
						window.AddButton ("stop spawn", this, "StopSpawn");
						window.AddButton ("destroy tiles", this, "DestroyTiles");
						window.AddButton ("merge buffers to game grid", this, "MergeBuffersToGameGrid");
						window.AddButton ("clear game grid", this, "ClearGameGrid");
						window.AddButton ("add a random walk to the game grid", this, "AddRandomWalkToGameGrid");
						window.AddButton ("add a random walk to last grid", this, "AddRandomWalkToLastGrid");
						window.AddButton ("create a demo city", this, "CreateDemo");
						window.AddButton ("reset scene", this, "ResetScene");*/
						
				}
				public void ResetScene ()
				{
						Application.LoadLevel (Application.loadedLevel);
						//yield return null;
				}
				// Update is called once per frame
				public void MakeGameGrid ()
				{
						gameGrid = new Grid (size, new Vector2 (0.0f, 0.0f), GPoint.PType.EMPTY);
				}
				public void MakeGameGrid (Vector2 s, Vector2 pos, GPoint.PType fT)
				{
						Debug.Log ("made a " + s.x + " by " + s.y + " grid of " + fT);
						gameGrid = new Grid (s, pos, fT);
				}
				public void CreateDebug ()
				{
						MakeGameGrid ();
						AddGridToBuffer (size, Vector2.zero, GPoint.PType.TILE_TEST);
						MergeBuffersToGameGrid ();
						SpawnTiles ();
				}
				public void CreateDemo ()
				{
						//make a game grid 
						//divide into 4
						//sand in corners
						//houses in sand
						//buildings above all
						//merge
						//spawn
						//AddGrid ();
						MakeGameGrid ();
						AddGridToBuffer (size, Vector2.zero, GPoint.PType.TILE_SAND);
						AddRandomWalkToLastGrid (500, GPoint.PType.TILE_HOUSE);
						AddGridToBuffer (new Vector2 (size.x / 2, size.y / 2), Vector2.zero, GPoint.PType.TILE_GRASS);
						AddRandomWalkToLastGrid (250, GPoint.PType.TILE_BUILDING);
						AddGridToBuffer (new Vector2 (size.x / 2, size.y / 2), new Vector2 (size.x / 2, size.y / 2), GPoint.PType.TILE_GRASS);
						AddRandomWalkToLastGrid (150, GPoint.PType.TILE_BUILDING);
						MergeBuffersToGameGrid ();
						gameGrid.UpdatePoints (GUtils.RoadCreator.Generate (gameGrid, blockSize));
						gameGrid.UpdatePoints (GUtils.TreePlacer.Generate (gameGrid, 100));
						GUtils.RotateTiles (gameGrid);
						SpawnTiles ();
				}
				void Update ()
				{
						//HandleInput ();
				}
				public void ShowGridData ()
				{
						gameGrid.ShowGridDataInConsole ();
				}
				/*void HandleInput ()
				{
				}*/
				public void OnDrawGizmos ()
				{
						if (gameGrid != null) {
								gameGrid.Draw ();
						}
				
						foreach (Grid g in grids) {
								g.Draw ();
						}
				}
				public void AddGridToBuffer ()
				{
						grids.Add (new Grid (size, position, fillType));
				}
				public void AddGridToBuffer (Vector2 s, Vector2 pos, GPoint.PType fT)
				{
						grids.Add (new Grid (s, pos, fT));
				}
				public void DestroyBufferGrids ()
				{
						grids.Clear ();
				}
				public void SpawnTiles ()
				{
						StartCoroutine (CoSpawnTiles ());
				}
				public void DestroyTiles ()
				{
						Debug.Log ("destroying all tiles");
						for (int i =0; i<tilePool.transform.childCount; i++) {
								Destroy (tilePool.transform.GetChild (i).gameObject);
						}
				}
				public void AddRandomWalkToGameGrid ()
				{
						gameGrid.UpdatePoints (GUtils.RandomWalk.Generate (gameGrid, fillType, walkSteps));
				}
				public void AddRandomWalkToLastGrid ()
				{
						grids [grids.Count - 1].UpdatePoints (GUtils.RandomWalk.Generate (grids [grids.Count - 1], fillType, walkSteps));
				}
				public void AddRandomWalkToLastGrid (int steps, GPoint.PType fT)
				{
						grids [grids.Count - 1].UpdatePoints (GUtils.RandomWalk.Generate (grids [grids.Count - 1], fT, steps));
				}
				public void ClearGameGrid ()
				{
						gameGrid.Reset ();
				}
				public void ClearBufferGrids ()
				{
						foreach (Grid g in grids) {
								g.Clear ();
						}
				}
				public void MergeBuffersToGameGrid ()
				{
						Debug.Log ("merging buffer grids to game grid");
						Grid g = GUtils.MergeBufferDown (grids, gameGrid);
						gameGrid.UpdatePoints (g);
				}
				IEnumerator CoSpawnTiles ()
				{
						Debug.Log ("spawning tiles...");
						int count = 0;
						foreach (GPoint gp in gameGrid.GetPoints()) {
								InstantiateTile (gp);
								count++;
								if (count == 4) {
										count = 0;
										yield return new WaitForSeconds (0.0f);
								}
								
						}
						Debug.Log ("done spawning tiles!");
						NotificationCenter.DefaultCenter.PostNotification (this, "GetAllTilesInGrid");
						yield return null;
				}
				public void StopSpawn ()
				{
						StopAllCoroutines ();
				}
				public float CalculateScale (Vector2 tilePos)
				{
						//float scale = 0.0f;
						Vector2 gameGridMiddle = new Vector2 (gameGrid.GetSize ().x / 2, gameGrid.GetSize ().y / 2);
						float distance = Mathf.Sqrt (Mathf.Abs ((tilePos.x - gameGridMiddle.x) + (tilePos.y - gameGridMiddle.y)));
						return distance;
				}
				public void InstantiateTile (GPoint gp)
				{
						int index = 0;
						switch (gp.GetPType ()) {
						case GPoint.PType.DEBUG:
								index = 0;
								break;
						case GPoint.PType.TILE_TEST:
								index = 1;
								break;
						case GPoint.PType.TILE_GRASS:
								index = 2;
								break;
						case GPoint.PType.TILE_SAND:
								index = 3;
								break;
						case GPoint.PType.TILE_ROADI:
								index = 4;
								break;
						case GPoint.PType.TILE_ROADH:
								index = 5;
								break;
						case GPoint.PType.TILE_ROADV:
								index = 6;
								break;
						case GPoint.PType.TILE_TREE:
								index = 7;
								break;
						case GPoint.PType.TILE_BUILDING:
								index = 8;
								break;
						case GPoint.PType.TILE_HOUSE:
								index = 9;
								break;
						default:
								index = -1;
								break;
						}
						if (index >= 0) {
								float scale = 1;
								GameObject newTile = null;
								//Destroy (newTile);
								if (index < 8) {
										newTile = (GameObject)Instantiate (tilePrefabs [index], gp.GetWorldPosition (), Quaternion.identity);
										if (index == 7) {
												scale = Random.Range (0.75f, 1.25f);
										}
								} else if (index == 8) {
										int addup = Random.Range (0, buildingPrefabs.Count ());
										scale = Random.Range (0.75f, 1.25f);
										newTile = (GameObject)Instantiate (buildingPrefabs [addup], gp.GetWorldPosition (), Quaternion.identity);
								} else if (index == 9) {
										int addup = Random.Range (0, housePrefabs.Count ());
										newTile = (GameObject)Instantiate (housePrefabs [addup], gp.GetWorldPosition (), Quaternion.identity);
								}
								newTile.name = "/x:" + gp.GetPosition ().x + "$/y:" + gp.GetPosition ().y + "$/t:" + gp.GetPType () + "$";
								newTile.transform.parent = tilePool.transform;
								newTile.transform.localScale = new Vector3 (1, scale, 1);
								if (gp.GetPType () == GPoint.PType.TILE_BUILDING || gp.GetPType () == GPoint.PType.TILE_HOUSE) {
										switch (gp.GetDirection ()) {
										case GPoint.Direction.NORTH:
												//Debug.Log ("tile turned north");
												newTile.transform.LookAt (newTile.transform.position + Vector3.back);
												//newTile.transform.Translate (Vector3.up * 1);
												break;
										case GPoint.Direction.SOUTH:
												//Debug.Log ("tile turned south");
												newTile.transform.LookAt (newTile.transform.position + Vector3.forward);
												//newTile.transform.Translate (Vector3.up * 2);
					
												break;
										case GPoint.Direction.EAST:
												//Debug.Log ("tile turned east");
												newTile.transform.LookAt (newTile.transform.position + Vector3.left);
												//newTile.transform.Translate (Vector3.up * 3);
												break;
										case GPoint.Direction.WEST:
												//Debug.Log ("tile turned west");
												newTile.transform.LookAt (newTile.transform.position + Vector3.right);
												//newTile.transform.Translate (Vector3.up * 4);
												break;
										default:
												break;
										}
										;
								}
						}
				}

		}
		static class GUtils
		{
				public static Grid MergeBufferDown (List<Grid> buffer, Grid g)
				{
						//Debug.Log ("merging the buffer to the game grid...");
						//Grid outputGrid = new Grid (g);
						foreach (Grid bg in buffer) {
								//Debug.Log ("merging a grid");
								g.UpdatePoints (MergeGrids (g, bg));
						}
						return g;
				}
				static Grid MergeGrids (Grid a, Grid b)
				{
						a.UpdatePoints (b);
						return a;
				}
				public enum Direction
				{
						origin,
						n,
						e,
						s,
						w,
						ne,
						se,
						sw,
						nw}
				;
				public static GPoint.PType ChooseBuildingType (Grid g, Vector2 pos)
				{
						return GPoint.PType.DEBUG;
				}
				public static void RotateTiles (Grid g)
				{
						Debug.Log ("solving tile rotation...");
						foreach (GPoint gp in g.GetPoints()) {
								switch (gp.GetPType ()) {
								case GPoint.PType.TILE_BUILDING:
										g.UpdatePointDirection (gp.GetPosition (), (CalculateDirection (g, gp)));
										break;
								case GPoint.PType.TILE_HOUSE:
										g.UpdatePointDirection (gp.GetPosition (), (CalculateDirection (g, gp)));
										break;
								default:
										//nothing
										break;
								}
						}
				}
				public static GPoint.Direction CalculateDirection (Grid g, GPoint gp)
				{
						ScanPoint scp = new ScanPoint (gp.GetPosition ());
						List<string> dirs = new List<string> ();
						string direction = "";
						switch (g.GetPoint (scp.VirtualMove (Direction.n)).GetPType ()) {
						case GPoint.PType.TILE_ROADH:
								dirs.Add ("N");
								break;
						case GPoint.PType.TILE_ROADV:
								dirs.Add ("N");
								break;
						default:
				//nothing
								break;
						}
						
						switch (g.GetPoint (scp.VirtualMove (Direction.s)).GetPType ()) {
						case GPoint.PType.TILE_ROADH:
								dirs.Add ("S");
								break;
						case GPoint.PType.TILE_ROADV:
								dirs.Add ("S");
								break;
						default:
				//nothing
								break;
						}
						
						switch (g.GetPoint (scp.VirtualMove (Direction.e)).GetPType ()) {
						case GPoint.PType.TILE_ROADH:
								dirs.Add ("E");
								break;
						case GPoint.PType.TILE_ROADV:
								dirs.Add ("E");
								break;
						default:
				//nothing
								break;
								
						}
						
						switch (g.GetPoint (scp.VirtualMove (Direction.w)).GetPType ()) {
						case GPoint.PType.TILE_ROADH:
								dirs.Add ("W");
								break;
						case GPoint.PType.TILE_ROADV:
								dirs.Add ("W");
								break;
						default:
				//nothing
								break;
						}
/*						foreach (string s in dirs) {
								Debug.Log (s);
						}*/
						int val = (int)Random.Range (0, dirs.Count ());
						//Debug.Log ("integer " + val);
						//Debug.Log ("directions " + dirs.Count ());
						direction = dirs.ElementAt (val);
						//Debug.Log ("direction chosen" + direction);
						switch (direction) {
						case "N":
								//Debug.Log ("turn north");
								return GPoint.Direction.NORTH;
						//break;
						case "S":
								//Debug.Log ("turn south");
								return GPoint.Direction.SOUTH;
						//break;
						case "E":
								//Debug.Log ("turn east");
								return GPoint.Direction.EAST;
						//break;
						case "W":
								//Debug.Log ("turn west");
								return GPoint.Direction.WEST;
						//break;
						default:
								return GPoint.Direction.NORTH;
						//break;
						}
						;
						//return GPoint.Direction.NORTH;
				}
				public static class RoadCreator
				{
						public static List<GPoint> Generate (Grid finalGrid, Vector2 blockSize)
						{
								List<GPoint> roadOrigins = FindRoadOrigins (finalGrid, blockSize);
								List<GPoint> roadsVH = AddRoadsVerticalHorizontal (roadOrigins, blockSize);
								List<GPoint> roadOriginsPlusVH = roadOrigins.Union<GPoint> (roadsVH).ToList<GPoint> ();
								List<GPoint> fixedRoads = FixRoads (finalGrid, roadOriginsPlusVH, blockSize);
								List<GPoint> fixedRoadsPlusVHO = roadOriginsPlusVH.Union<GPoint> (fixedRoads).ToList<GPoint> ();
								return fixedRoadsPlusVHO;
						}
						public static List<GPoint> FindRoadOrigins (Grid g, Vector2 blockSize)
						{
								List<GPoint> roadOrigins = new List<GPoint> ();
								//iterate right by blocksize
								for (float ix=1.0f; ix<g.GetXYMax().x; ix+=blockSize.x) {
										//iterate up by blocksize
										for (float iy=1.0f; iy<g.GetXYMax().y; iy+=blockSize.y) {
												//start at the next block right and up
												//so the previous iteration set is the 1
												//and where this iteration set starts is the 2
												//this is because we start at ix2 = 1.0f and iy2 = 1.0f
												//10000100001
												//00000000000
												//02000020000
												//10000100001
												//iterate right
												for (float ix2=1.0f; ix2<blockSize.x; ix2+=1.0f) {
														//iterate up
														for (float iy2=1.0f; iy2<blockSize.y; iy2+=1.0f) {
																//check if the tile at the given point is anything other
																//than a background tile
																if (!g.GetPoint (new Vector2 (ix + ix2, iy + iy2)).IsBackgroundTile ()) {
																		//if it is, then add a road intersection at start point
																		roadOrigins.Add (new GPoint (new Vector2 (ix, iy), GPoint.PType.TILE_ROADI));
																}
														}
												}
												
										}
								}
								return roadOrigins;
						}
						public static List<GPoint> AddRoadsVerticalHorizontal (List<GPoint> roadOrigins, Vector2 blockSize)
						{
								List<GPoint> roadsVH = new List<GPoint> ();
								//in this function we will iterate through each origin
								//we will create horizontal roads by iterating right until the iterator = blocksize.x
								//and we will create vertical roads by iterating upwards until the iterator = blocksize.y
								foreach (GPoint gp in roadOrigins) {
										//create the horizontal roads
										for (float ix=1.0f; ix<blockSize.x; ix+=1.0f) {
												roadsVH.Add (new GPoint (new Vector2 (ix + gp.GetPosition ().x, gp.GetPosition ().y), GPoint.PType.TILE_ROADH));
										}
										//create the veritcal roads
										for (float iy=1.0f; iy<blockSize.y; iy+=1.0f) {
												roadsVH.Add (new GPoint (new Vector2 (gp.GetPosition ().x, iy + gp.GetPosition ().y), GPoint.PType.TILE_ROADV));
										}
								}
								return roadsVH;
						}
						public static List<GPoint> FixRoads (Grid g, List<GPoint> rVHplusO, Vector2 blockSize)
						{
								//we will need to create a temporary grid that contains all of the new points
								//then iterate through it by the blocksize as done earlier
								//for each intersection check if there is a road to the left or down
								//if there is one left, then create a vertical road as done earlier
								//if there is one down, then create a horizontal road as done earlier
								//also create an intersection at the point
								List<GPoint> fixedRoads = new List<GPoint> ();
								Grid tG = new Grid (g);
								tG.Fill (GPoint.PType.TILE_GRASS);
								tG.UpdatePoints (rVHplusO);
								for (float ix=1.0f; ix<tG.GetXYMax().x; ix+=blockSize.x) {
										//iterate up by blocksize
										for (float iy=1.0f; iy<tG.GetXYMax().y; iy+=blockSize.y) {
												//iterate right by blocksize
												//set the scan point to the current position on the grid
												ScanPoint sp1 = new ScanPoint (new Vector2 (ix, iy));
												//move the scanpoint to the left and check if is anything but background
												if (!tG.GetPoint (sp1.VirtualMove (Direction.w)).IsBackgroundTile ()) {
														//create a vertical road blocksize.y units long
														//probably going to do this with a for loop
														fixedRoads.Add (new GPoint (sp1.Position (), GPoint.PType.TILE_ROADI));
														fixedRoads.Add (new GPoint (new Vector2 (sp1.Position ().x, sp1.Position ().y + blockSize.y), GPoint.PType.TILE_ROADI));
														for (float iy2 = 1.0f; iy2<blockSize.y; iy2+=1.0f) {
																fixedRoads.Add (new GPoint (new Vector2 (sp1.Position ().x, sp1.Position ().y + iy2), GPoint.PType.TILE_ROADV));
														}
												}
												if (!tG.GetPoint (sp1.VirtualMove (Direction.s)).IsBackgroundTile ()) {
														//create a horizontal road blocksize.x units long
														//probably going to do this with a for loop
														fixedRoads.Add (new GPoint (sp1.Position (), GPoint.PType.TILE_ROADI));
														fixedRoads.Add (new GPoint (new Vector2 (sp1.Position ().x + blockSize.x, sp1.Position ().y), GPoint.PType.TILE_ROADI));
														for (float ix2 = 1.0f; ix2<blockSize.x; ix2+=1.0f) {
																fixedRoads.Add (new GPoint (new Vector2 (sp1.Position ().x + ix2, sp1.Position ().y), GPoint.PType.TILE_ROADH));
														}
												}
										}
								}
								return fixedRoads;
						}
				}
				public static class TreePlacer
				{
						public static List<GPoint> Generate (Grid g, int numOfTrees)
						{
								List<GPoint> trees = new List<GPoint> ();
								int i = 0;
								while (i < numOfTrees) {
										int rX = (int)Random.Range (g.GetXYMin ().x, g.GetXYMax ().x);
										int rY = (int)Random.Range (g.GetXYMin ().y, g.GetXYMax ().y);
										if (g.GetPoint (rX, rY).GetPType () == GPoint.PType.TILE_GRASS) {
												trees.Add (new GPoint (new Vector2 ((float)rX, (float)rY), GPoint.PType.TILE_TREE));
												i++;
										}
								}
								return trees;
						}
				}
				public static class RandomWalk
				{
						public static List<GPoint> Generate (Grid g, GPoint.PType walkType, int steps)
						{
								//make the last direction random, to randomise the opposite of the first direction
								//start at 1
								//pick a new direction
								//if the direction is okay
								//move the test vector
								//if the new position is in bounds
								//add the point
								//iterate the loop
								//set the last direction to the new direction
								List<GPoint> points = new List<GPoint> ();
								//set the scanpoint in the middle of the grid
								ScanPoint scan = new ScanPoint (g.GetSize () / 2);
								//set the last direction as a new direction
								Direction lastDir = PickNewDirection ();
								//set the stepcount at 1
								int stepcount = 1;
								//if the stepcount is less than or equal to the max number of steps
								while (stepcount<=steps) {
										//pick a new direction and set it as the new direction
										Direction newDir = PickNewDirection ();
										//if the new direction is not the opposite of the last direction
										if (IsValidDirection (newDir, lastDir)) {
												//move the testposition in the new direction
												Vector2 testPos = scan.VirtualMove (newDir);
												//check if the new position is within the constraints
												if (g.PositionInBounds (testPos)) {
														//if it is, move the scanpoint in the direction
														scan.Move (newDir);
														//iterate the stepcount
														stepcount++;
														//add a new gpoint to the list of the designated type
														points.Add (new GPoint (scan.Position (), walkType));
														//Debug.Log ("point added to grid at " + scan.Position ());
												}
												//set the last dir to the new dir to ensure accurate direction checking
												//when the loop repeats
												lastDir = newDir;
										}
										//otherwise, do absolutely nothing
								}
								Debug.Log ("generated a " + points.Count + " point line of " + walkType);
								Grid tg = new Grid (g);
								tg.Clear ();
								tg.UpdatePoints (points);
								List<GPoint> processed = new List<GPoint> ();
								processed = PostProcess (tg, walkType, walkType);
								return processed;
						}
						private static bool ProcessPoint (Grid g, ScanPoint sc, GPoint.PType st, List<Direction[]> scanMethod)
						{
				
								foreach (Direction[] dpat in scanMethod) {
										bool match = true;
										/*if (g.GetPoint (sc.VirtualMove (dpat [0])).GetPType () == st &&
												g.GetPoint (sc.VirtualMove (dpat [1])).GetPType () == st &&
												g.GetPoint (sc.VirtualMove (dpat [2])).GetPType () == st) {
												return true;
										}*/
										for (int i =0; i <dpat.Length; i++) {
												if (g.GetPoint (sc.VirtualMove (dpat [i])).GetPType () != st) {
														match = false;
												}
										}
										if (match) {
												return match;
										}
								}
								return false;
						}
						private static List<GPoint> PostProcess (Grid g, GPoint.PType scanType, GPoint.PType fillType)
						{
								
								List<GPoint> processedPoints = new List<GPoint> ();
								Debug.Log ("scanning " + g.GetPoints (scanType).Count + "unique points");
								List<Direction[]> ds = new List<Direction[]> ();
								ds.Add (new Direction[]{Direction.n, Direction.ne, Direction.e});
								ds.Add (new Direction[]{Direction.e, Direction.se, Direction.s});
								ds.Add (new Direction[]{Direction.s, Direction.sw, Direction.w});
								ds.Add (new Direction[]{Direction.w, Direction.nw, Direction.n});
								foreach (GPoint gp in g.GetPoints(scanType)) {
										//List<bool> values = new List<bool> ();
										ScanPoint scan = new ScanPoint (gp.GetPosition ());
										if (ProcessPoint (g, scan, scanType, ds)) {
												processedPoints.Add (new GPoint (scan.Position (), fillType));
										}
								}
								Debug.Log (processedPoints.Count + " processed points added");
								return processedPoints;
						}
						private static Direction PickNewDirection ()
						{
								int index = Random.Range (0, 4);
								//Debug.Log ("picking new direction " + index);
								return GetDirectionFromIndex (index);
						}
						private static Direction GetDirectionFromIndex (int index)
						{
								switch (index) {
								case 0:
										return Direction.n;
								case 1:
										return Direction.e;
								case 2:
										return Direction.s;
								case 3:
										return Direction.w;
								default:
										return Direction.origin;
								}
						}
						private static bool IsValidDirection (Direction newDir, Direction lastDir)
						{
								if (newDir != OppositeDirection (lastDir)) {
										return true;
								}
								//Debug.Log ("new direction is opposite last, staying still");
								return false;
						}
						private static Direction OppositeDirection (Direction dir)
						{
								switch (dir) {
								case Direction.n:
										return Direction.s;
								case Direction.e:
										return Direction.w;
								case Direction.s:
										return Direction.n;
								case Direction.w:
										return Direction.e;
								case Direction.ne:
										return Direction.sw;
								case Direction.se:
										return Direction.nw;
								case Direction.sw:
										return Direction.ne;
								case Direction.nw:
										return Direction.se;
								default:
										return Direction.origin;
								}
						}

				}
				class ScanPoint
				{
						private int x = 0;
						private int y = 0;
						public ScanPoint (Vector2 startpos)
						{
								x = (int)startpos.x;
								y = (int)startpos.y;
						}
						public Vector2 Position ()
						{
								return new Vector2 (x, y);
						}
						public void SetPosition (Vector2 newPos)
						{
								x = (int)newPos.x;
								y = (int)newPos.y;
						}
						public Vector2 VirtualMove (GUtils.Direction dir)
						{
								Vector2 newpos = Position ();
								switch (dir) {
								case GUtils.Direction.n:
										newpos.y += 1;
										break;
								case GUtils.Direction.e:
										newpos.x += 1;
										break;
								case GUtils.Direction.s:
										newpos.y -= 1;
										break;
								case GUtils.Direction.w:
										newpos.x -= 1;
										break;
								case GUtils.Direction.ne:
										newpos.y += 1;
										newpos.x += 1;
										break;
								case GUtils.Direction.se:
										newpos.y -= 1;
										newpos.x += 1;
										break;
								case GUtils.Direction.sw:
										newpos.y -= 1;
										newpos.x -= 1;
										break;
								case GUtils.Direction.nw:
										newpos.y += 1;
										newpos.x -= 1;
										break;
								default:
					//nothing
										break;
								}
								;
								return newpos;
						}
						public void Move (GUtils.Direction dir)
						{
								switch (dir) {
								case GUtils.Direction.n:
										y += 1;
										break;
								case GUtils.Direction.e:
										x += 1;
										break;
								case GUtils.Direction.s:
										y -= 1;
										break;
								case GUtils.Direction.w:
										x -= 1;
										break;
								case GUtils.Direction.ne:
										y += 1;
										x += 1;
										break;
								case GUtils.Direction.se:
										y -= 1;
										x += 1;
										break;
								case GUtils.Direction.sw:
										y -= 1;
										x -= 1;
										break;
								case GUtils.Direction.nw:
										y += 1;
										x -= 1;
										break;
								default:
					//nothing
										break;
								}
								;
						}
				}
		}
}