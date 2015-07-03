using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*it would be wise to create grids whos dimensions (-2) are factors of the blocksize*/
public class Grid
{
		private Vector2 size, position = new Vector2 (0.0f, 0.0f);
		private Vector2 xyMin = new Vector2 (1.0f, 1.0f);
		private Vector2 xyMax;
		private GPoint[,] gPoints;
		private GPoint.PType gridType;
		public Grid (Grid g)
		{//Vector2 setSize, Vector2 setPosition, GPoint.PType fillType
				xyMax = g.xyMax;
				gridType = g.gridType;
				//bump up the size to create room for the buffer;
				size = g.size;
				//translate
				position = g.position;
				//Debug.Log ("creating a grid FROM a grid " + g.GetSize ().x + "by" + g.GetSize ().y + " grid, filled with " + g.gridType);
				//Debug.Log ("actually a " + size.x + "by" + size.y + " grid");
				//create [,] of empty points
				Initialize ();
				//add a buffer
				MarkBuffer ();
				//fill the center
				//Fill (gridType);
				UpdatePoints (g.GetPoints ());
		}
		public Grid (Vector2 setSize, Vector2 setPosition, GPoint.PType fillType)
		{
				xyMax = setSize + new Vector2 (1.0f, 1.0f);
				gridType = fillType;
				//bump up the size to create room for the buffer;
				SetSize (setSize + new Vector2 (2.0f, 2.0f));
				//translate
				SetPosition (setPosition);
				Debug.Log ("creating a " + setSize.x + "by" + setSize.y + " grid, filled with " + fillType);
				//Debug.Log ("actually a " + size.x + "by" + size.y + " grid");
				//create [,] of empty points
				Initialize ();
				//add a buffer
				MarkBuffer ();
				//fill the center
				Fill (gridType);
		}
		public GPoint.PType GetGridType ()
		{
				return gridType;
		}
		private void Initialize ()
		{
				gPoints = new GPoint[(int)size.x, (int)size.y];
				//Debug.Log ("initializing an empty grid");
				for (int ix = 0; ix < (int)size.x; ix+=1) {
						for (int iy = 0; iy < (int)size.y; iy+=1) {
								Vector2 pos = new Vector2 ((float)ix, (float)iy);
								gPoints [ix, iy] = new GPoint (pos, GPoint.PType.EMPTY);
								//Debug.Log ("tile added at" + ix + "," + iy + "\nwith data" + GetPoint (pos).GetPosition ().x + GetPoint (pos).GetPosition ().x + GPoint.PType.EMPTY);
						}
				}
		}
		private void MarkBuffer ()
		{
				//Debug.Log ("creating the 1 tile buffer");
				//make the x lines
				for (int ix = 0; ix < (int)size.x; ix++) {
						//where to add a point
						Vector2 pos = new Vector2 ((float)ix, 0.0f);
						//bottom row
						gPoints [(int)pos.x, (int)pos.y].UpdateType (GPoint.PType.BUFFER);
						//move up
						pos.y = size.y - 1.0f;
						//move up
						gPoints [(int)pos.x, (int)pos.y].UpdateType (GPoint.PType.BUFFER);
				}
				//make the y lines
				for (int iy = 0; iy < (int)size.y; iy++) {
						//where to add a point
						Vector2 pos = new Vector2 (0.0f, (float)iy);
						//left collumn
						gPoints [(int)pos.x, (int)pos.y].UpdateType (GPoint.PType.BUFFER);
						//move right
						pos.x = size.x - 1.0f;
						//right collumn
						gPoints [(int)pos.x, (int)pos.y].UpdateType (GPoint.PType.BUFFER);
				}
		}
		public void Fill (GPoint.PType fillType)
		{
				//Debug.Log ("filling the grid with" + fillType);
				//this method DOES NOT start at the 0,0 origin but rather at xymin and ends at xymax;
				for (float ix = xyMin.x; ix < xyMax.x; ix+=1.0f) {
						for (float iy = xyMin.y; iy <= xyMax.y; iy+=1.0f) {
								Vector2 pos = new Vector2 (ix, iy);
								gPoints [(int)pos.x, (int)pos.y].UpdateType (fillType);
						}
				}
		}
		public void Reset ()
		{
				Fill (gridType);
		}
		public void Clear ()
		{
				Fill (GPoint.PType.EMPTY);
		}
		public List<GPoint> GetPoints ()
		{
				List<GPoint> points = new List<GPoint> ();
				for (float ix = xyMin.x; ix < xyMax.x; ix+=1.0f) {
						for (float iy = xyMin.y; iy < xyMax.y; iy+=1.0f) {
								Vector2 pos = new Vector2 (ix, iy);
								GPoint gpAtIndex = GetPoint (pos);
								if (gpAtIndex.GetPType () != GPoint.PType.EMPTY)
										//Debug.Log ("adding point val to list: x" + gpAtIndex.GetPosition ().x + "y" + gpAtIndex.GetPosition ().x);
										points.Add (gpAtIndex);
								/*gPoints [(int)pos.x, (int)pos.y].UpdateType (fillType);*/
						}
				}
				return points;
		}
		public List<GPoint> GetPoints (GPoint.PType pointType)
		{
				List<GPoint> points = new List<GPoint> ();
				for (float ix = xyMin.x; ix < xyMax.x; ix+=1.0f) {
						for (float iy = xyMin.y; iy < xyMax.y; iy+=1.0f) {
								Vector2 pos = new Vector2 (ix, iy);
								GPoint gpAtIndex = GetPoint (pos);
								if (gpAtIndex.GetPType () != GPoint.PType.EMPTY && gpAtIndex.GetPType () == pointType) {
										points.Add (gpAtIndex);
								}
						}
				}
				return points;
		}
		public GPoint GetPoint (Vector2 position)
		{
				if (PositionInBounds (position)) {
						return gPoints [(int)position.x, (int)position.y];
				}
				return new GPoint (position, GPoint.PType.EMPTY);
		}
		public GPoint GetPoint (float x, float y)
		{
				if (PositionInBounds (new Vector2 (x, y)))
						return gPoints [(int)x, (int)y];
				return new GPoint (position, GPoint.PType.EMPTY);
		}
		public GPoint GetPoint (int x, int y)
		{
				if (PositionInBounds (new Vector2 ((float)x, (float)y)))
						return gPoints [x, y];
				return new GPoint (position, GPoint.PType.EMPTY);
		}
		public void UpdatePoint (Vector2 pos, GPoint.PType newType)
		{
				if (PositionInBounds (pos))
						gPoints [(int)pos.x, (int)pos.y].UpdateType (newType);
		}
		public void UpdatePoints (List<GPoint> pointsToUpdate)
		{
				foreach (GPoint gp in pointsToUpdate) {
						UpdatePoint (gp.GetPosition (), gp.GetPType ());
				}
		}
		public void UpdatePointDirection (Vector2 pos, GPoint.Direction dir)
		{
				gPoints [(int)pos.x, (int)pos.y].SetDirection (dir);
		}
		public void UpdatePoints (Grid g)
		{
				foreach (GPoint gp in g.GetPoints()) {
						UpdatePoint ((gp.GetPosition () + g.GetPosition ()), gp.GetPType ());
				}
		}
		public Vector2 GetXYMin ()
		{
				return xyMin;
		}
		public Vector2 GetXYMax ()
		{
				return xyMax;
		}
		public bool PositionInBounds (Vector2 pos)
		{
				if (pos.x >= xyMin.x && pos.x <= xyMax.x) {
						if (pos.y >= xyMin.y && pos.y <= xyMax.y)
								return true;
				}
				return false;
		}
/*		public bool ScanPositionInBounds (Vector2 pos)
		{
				if (pos.x >= xyMin.x && pos.x <= xyMax.x) {
						if (pos.y >= xyMin.y && pos.y <= xyMax.y)
								return true;
				}
				return false;
		}*/
		public void SetPosition (Vector2 pos)
		{
				position = RoundVector (pos);
		}
		public void SetSize (Vector2 newSize)
		{
				size = RoundVector (newSize);
		}
		public void Draw ()
		{
				//draw the buffer
				Gizmos.color = Color.white;
				Gizmos.DrawWireCube (GetWorldPosition (), new Vector3 (size.x, 1.0f, size.y));
				//draw the useable space
				Gizmos.color = Color.red;
				Gizmos.DrawWireCube (GetWorldPosition (), new Vector3 (size.x - 2.0f, 1.0f, size.y - 2.0f));
				Gizmos.DrawIcon (GetWorldPosition (), "grid_icon.png", false);
				
		}
		public Vector2 RoundVector (Vector2 vecToRound)
		{
				Vector2 roundedVector;
				int x, y;
				x = (int)vecToRound.x;
				y = (int)vecToRound.y;
				roundedVector = new Vector2 (x, y);
				
				return roundedVector;
		}
		public Vector2 GetPosition ()
		{
				return position;
		}
		public Vector3 GetWorldPosition ()
		{
				Vector3 worldPos = new Vector3 ((int)size.x / 2.0f, 0.0f, (int)size.y / 2.0f);
				Vector3 offsetPos = new Vector3 ((int)position.x, 0.0f, (int)position.y);
				offsetPos -= new Vector3 (1.0f, 0.0f, 1.0f);
				worldPos += offsetPos;
				return worldPos;
		}
		public void ShowGridDataInConsole ()
		{
				string gridString = "";
		
				for (int ix = 0; ix < (int)size.x; ix+=1) {
						for (int iy = 0; iy < (int)size.y; iy+=1) {
								Vector2 pos = new Vector2 ((float)ix, (float)iy);
								switch (GetPoint (pos).GetPType ()) {
								case GPoint.PType.BUFFER:
										gridString += "B";
										break;
								case GPoint.PType.DEBUG:
										gridString += "G";
										break;
								default:
										break;
								}
								
						}
						Debug.Log (gridString);
						gridString = "";
				}
				
		}
		public Vector2 GetSize ()
		{
				return size - new Vector2 (2.0f, 2.0f);
		}
}
