using UnityEngine;
using System.Collections;

public class texturescroll : MonoBehaviour
{

		// Use this for initialization
		public Vector2 scrollSpeed;
		private Renderer rend;
		public bool MainTex;
		public bool DetailTex;
		public string customTexture;
		public bool customTex;
		void Start ()
		{
				rend = GetComponent<Renderer> ();
		}
	
		// Update is called once per frame
		void Update ()
		{
				Vector2 offset = Time.time * scrollSpeed;
				if (MainTex)
						rend.material.SetTextureOffset ("_MainTex", offset);
				if (DetailTex)
						rend.material.SetTextureOffset ("_DetailAlbedoMap", offset);
				if (customTex)
						rend.material.SetTextureOffset (customTexture, offset);
		}
}