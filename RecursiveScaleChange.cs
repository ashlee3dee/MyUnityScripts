using UnityEngine;
using System.Collections;

public class RecursiveScaleChange : MonoBehaviour
{
	
		/*this class will start with a gameobject
		that has multiple children with possibly multiple grandchildren
		on start it will detect any material on any renderer
		and change it to a user defined material*/
		public float minScale;
		public float maxScale;
		public float newScale;
		public bool randomScale;
		public int maxIterations;
		public int iterationCount;
		// Use this for initialization
		void Start ()
		{
		}
		public void Go ()
		{
				RecursiveReplace (transform, randomScale);
		}
		private void RecursiveReplace (Transform t, bool r)
		{
				ScanChildren (t, r, iterationCount);
		}
		private void ScanChildren (Transform t, bool r, int ilvl)
		{
				if (iterationCount < maxIterations) {
						if (t.childCount != 0) {
								int c = t.childCount;
								for (int i = 0; i < c; i++) {
										iterationCount++;
										ChangeScale (t.GetChild (i), r);
										ScanChildren (t.GetChild (i), r, iterationCount);
								}
								iterationCount--;
						} else {
								iterationCount--;
						}
				} else {
						iterationCount--;
				}
		}
	
		private void ChangeScale (Transform t, bool r)
		{
				float s = newScale;
				if (r) {
						s = Random.Range (minScale, maxScale);
				}
				t.localScale = t.localScale * s;
		}
}
