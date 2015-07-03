using UnityEngine;
using System.Collections;

public class RecursiveMaterialReplace : MonoBehaviour
{

		/*this class will start with a gameobject
		that has multiple children with possibly multiple grandchildren
		on start it will detect any material on any renderer
		and change it to a user defined material*/
		public Material newMaterial;
		// Use this for initialization
		void Start ()
		{
				ScanChildren (transform, newMaterial);
		}
		private static void RecursiveReplace (Transform t, Material m)
		{
				ScanChildren (t, m);
		}
		private static void ScanChildren (Transform t, Material m)
		{
				if (t.childCount != 0) {
						int c = t.childCount;
						for (int i = 0; i < c; i++) {
								ReplaceMaterial (t, i, m);
								ScanChildren (t.GetChild (i), m);
						}
				}
		}
		
		private static void ReplaceMaterial (Transform t, int i, Material m)
		{
				Renderer r = t.GetChild (i).GetComponent<Renderer> () as Renderer;
				if (r != null) {
						r.material = m;
				}
		}
}
