/*
	This script is placed in public domain. The author takes no responsibility for any possible harm.
	Contributed by Jonathan Czeck
*/
using UnityEngine;
using System.Collections;
//THIS IS A MODIFIED VERSION OF THE
//UNITY PROCEDURAL FX SCRIPT OF THE SAME NAME
//AVAILABLE IN THE ASSET STORE
public class LightningBolt : MonoBehaviour
{
		public Transform target;
		public int zigs = 100;
		public float particleSpeed = 1f;
		public float scale = 1f;
		public Light startLight;
		public Light endLight;
		public float moveSpeed;
		public LayerMask raycastMask;
		Perlin noise;
		float oneOverZigs;
	
		private Particle[] particles;
	
		void Start ()
		{
				oneOverZigs = 1f / (float)zigs;
				GetComponent<ParticleEmitter> ().emit = false;
				
				GetComponent<ParticleEmitter> ().Emit (zigs);
				particles = GetComponent<ParticleEmitter> ().particles;
		}
		void OnEnable ()
		{
				endLight.transform.position = transform.position;
		}
		void Update ()
		{
				if (noise == null)
						noise = new Perlin ();
				
				float timex = Time.time * particleSpeed * 0.1365143f;
				float timey = Time.time * particleSpeed * 1.21688f;
				float timez = Time.time * particleSpeed * 2.5564f;
				RaycastHit rayHit;
				Vector3 hitPoint;
				Vector3 targetPoint;
				float step = moveSpeed * Time.deltaTime;
				if (Physics.Linecast (transform.position, target.position, out rayHit, raycastMask)) {
						Debug.DrawLine (transform.position, rayHit.point, Color.blue);
						hitPoint = rayHit.point;
						
				} else {
						hitPoint = target.position;
				}
				targetPoint = Vector3.MoveTowards (endLight.transform.position, hitPoint, step);
				for (int i=0; i < particles.Length; i++) {
						Vector3 position = Vector3.Lerp (transform.position, targetPoint, oneOverZigs * (float)i);
						Vector3 offset = new Vector3 (noise.Noise (timex + position.x, timex + position.y, timex + position.z),
										noise.Noise (timey + position.x, timey + position.y, timey + position.z),
										noise.Noise (timez + position.x, timez + position.y, timez + position.z));
						position += (offset * scale * ((float)i * oneOverZigs));
			
						particles [i].position = position;
						particles [i].color = Color.white;
						particles [i].energy = 1f;
				}
		
				GetComponent<ParticleEmitter> ().particles = particles;
		
				if (GetComponent<ParticleEmitter> ().particleCount >= 2) {
						if (startLight)
								startLight.transform.position = particles [0].position;
						if (endLight) {
								Vector3 onGround = new Vector3 (particles [particles.Length - 1].position.x, hitPoint.y, particles [particles.Length - 1].position.z);
								endLight.transform.position = onGround;
						}
				}
		}	
}