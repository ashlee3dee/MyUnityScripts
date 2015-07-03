using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ScriptUtilities
{
		//this namespace wil contain many useful utilites for vectors, floats, math, and otherwise
		//add as you go, create multiple subclasses for each type of utility
		public static class Vector3Utils
		{
				//designated class in the namespace for vector3
				public static Vector3 Add (Vector3 a, Vector3 b)
				{
						return new Vector3 (a.x + b.x, a.y + b.y, a.z + b.z);
				}
				public static Vector3 Subtract (Vector3 a, Vector3 b)
				{
						return new Vector3 (a.x - b.x, a.y - b.y, a.z - b.z);
				}
				public static Vector3 Multiply (Vector3 a, Vector3 b)
				{
						return new Vector3 (a.x * b.x, a.y * b.y, a.z * b.z);
				}
				public static Vector3 Divide (Vector3 a, Vector3 b)
				{
						return new Vector3 (a.x / b.x, a.y / b.y, a.z / b.z);
				}
				public static Vector3 Divide (Vector3 a, float b)
				{
						return new Vector3 (a.x / b, a.y / b, a.z / b);
				}
				public static Vector3 Divide (Vector3 a, int b)
				{
						return new Vector3 (a.x / b, a.y / b, a.z / b);
				}
				public static Vector3 SetY (Vector3 a, float y)
				{
						return new Vector3 (a.x, y, a.z);
				}
		}
		public static class CircleUtils
		{
				public static Vector2 CalculatePointOnCircle (Vector2 origin, float angle, float radius)
				{
						/*
								where
								x,y are the destination coordinates
								cx,cy are the origin coordinates
								r is the radius
								a is the degrees in radians clockwise from the top
								x = cx + r * cos(a)
								y = cy + r * sin(a)
						*/
						Vector2 position = Vector2.zero;
						position.x = origin.x + (radius * Mathf.Cos (angle * Mathf.Deg2Rad));
						position.y = origin.y + (radius * Mathf.Sin (angle * Mathf.Deg2Rad));
						return position;
				}
				public static Vector3 CalculatePointOnCircle (Vector3 origin, float angle, float radius)
				{
						/*
								where
								x,y are the destination coordinates
								cx,cy are the origin coordinates
								r is the radius
								a is the degrees in radians clockwise from the top
								x = cx + r * cos(a)
								y = cy + r * sin(a)
						*/
						Vector3 position = Vector3.zero;
						position.x = origin.x + (radius * Mathf.Cos (angle * Mathf.Deg2Rad));
						position.y = origin.y;
						position.z = origin.z + (radius * Mathf.Sin (angle * Mathf.Deg2Rad));
						return position;
				}
				public static List<Vector3> CalculatePointsOnCircle (int n, float r, Vector3 o)
				{
						List<Vector3> posList = new List<Vector3> ();
						float aiDeg = (360 / n);
						for (int i = 0; i < n; i++) {
								posList.Add (CalculatePointOnCircle (o, aiDeg * i, r));
						}
						return posList;
				}
		}
}

























