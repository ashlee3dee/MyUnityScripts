using UnityEngine;
using System.Collections;

public class GPoint
{
		public enum PType
		{
				DEBUG,
				BUFFER,
				EMPTY,
				TILE_TEST,
				TILE_GRASS,
				TILE_TREE,
				TILE_SAND,
				TILE_BUILDING,
				TILE_ROAD,
				TILE_ROADH,
				TILE_ROADV,
				TILE_ROADI,
				TILE_HOUSE,
				TILE_DESTROYED
		}
		;
		public enum Direction
		{
				NORTH,
				SOUTH,
				EAST,
				WEST
		}
		;
		private Vector2 position;
		private PType type;
		private Direction direction;
		public GPoint (Vector2 setPosition, PType setType)
		{
				position = setPosition;
				UpdateType (setType);
		}
		public Vector2 GetPosition ()
		{
				return position;
		}
		public Vector3 GetWorldPosition ()
		{
				return new Vector3 (position.x - 0.5f, 0.0f, position.y - 0.5f);
		}
		public PType GetPType ()
		{
				return type;
		}
		public Direction GetDirection ()
		{
				//Debug.Log ("returning " + direction);
				return direction;
		}
		public void SetDirection (Direction dir)
		{
				//Debug.Log ("set direction" + dir);
				direction = dir;
		}
		public bool IsBackgroundTile ()
		{
				switch (type) {
				case PType.TILE_GRASS:
						return true;
				case PType.TILE_SAND:
						return true;
				case PType.BUFFER:
						return true;
				case PType.EMPTY:
						return true;
				default:
						return false;
				}
		}
		public void UpdateType (PType newType)
		{
				if (type != PType.BUFFER)
						type = newType;
		}
}
