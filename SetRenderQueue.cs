using UnityEngine;
using System.Collections;

[AddComponentMenu("Effects/SetRenderQueue")]
[RequireComponent(typeof(Renderer))]

public class SetRenderQueue : MonoBehaviour
{
		public int queue = 1;
	
		public int[] queues;
		private Renderer rend;
		protected void Start ()
		{
				rend = GetComponent<Renderer> () as Renderer;
				if (!rend || !rend.sharedMaterial || queues == null)
						return;
				rend.sharedMaterial.renderQueue = queue;
				for (int i = 0; i < queues.Length && i < rend.sharedMaterials.Length; i++)
						rend.sharedMaterials [i].renderQueue = queues [i];
		}
	
}