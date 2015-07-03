using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Pathfinding;

public class ShipController : MonoBehaviour
{

		// Use this for initialization

		public float nextWaypoint;
		public float newPathThreshold;
		public Slider energySlider;
		public Slider healthSlider;
		public GameObject prefab;
		public float normalSpeed;
		private float shipSpeed;
		public float boostSpeed;
		public GameObject laserShot;
		public float fireSpeed;
		public int health;
		public int maxHealth;
		public float maxEnergy;
		public float energy;
		public float laserEnergyCost = 10.0f;
		public float lightningEnergyCost = 20f;
		public float tractorBeamEnergyCost = 5.0f;
		public float boostEnergyCost = 10.0f;
		public float energyRegenSpeed;
		public float energyRegenStep;
		public float lightningSpeed;
		public float lightningLength;
		public GameObject laserIcon;
		public GameObject lightningIcon;
		public GameObject lightningObject;
		public GameObject tractorBeamObject;
		//public  GameObject lightningEndLight;
		private Seeker seeker;
		private Path path;
		private int currentWaypoint;
		private Vector3 lastTargetPos;
		public LayerMask raycastMask;
		public Transform targetGameobject;
		private Vector3 moveTarget;
		private bool tractorBeamOn = false;
		private bool isFiring = false;
		private bool isSafeToPlay = false;
		private int selectedWeapon = 0;
		private bool mouseDown;
		private bool isMoving = false;
		private bool boostIsOn = false;

		void Start ()
		{
				StartCoroutine ("RegenerateEnergy");
				NotificationCenter.DefaultCenter.AddObserver (this, "SafeToPlay");
				energySlider.maxValue = maxEnergy;
				healthSlider.maxValue = maxHealth;
				ActivateWeaponIcon ();
				seeker = GetComponent<Seeker> ();
				shipSpeed = normalSpeed / 10;
				//lightningEndLight = lightningObject.transform.GetChild (0).gameObject;
		}
		// Update is called once per frame
		void Update ()
		{
				if (isSafeToPlay) {
						HandleInput ();
						healthSlider.value = health;
						energySlider.value = energy;
				}
				
		}
		void FixedUpdate ()
		{
				if (isSafeToPlay) {
						Move ();
						Vector3 newTarget = ScriptUtilities.Vector3Utils.SetY (targetGameobject.transform.position, 5.0f);
						Debug.DrawLine (moveTarget, newTarget, Color.yellow);
				}
		}
		void SafeToPlay ()
		{
				isSafeToPlay = true;
				NewPath ();
				StartCoroutine ("UpdatePath");
				ActivateWeaponIcon ();
		}

		void HandleInput ()
		{
				if (Input.GetKeyDown (KeyCode.Alpha1)) {
						selectedWeapon = 0;
						if (mouseDown) {
								StopLightning ();
								StartLasers ();
						}
						ActivateWeaponIcon ();
				}
				if (Input.GetKeyDown (KeyCode.Alpha2)) {
						selectedWeapon = 1;
						if (mouseDown) {
								StopLasers ();
								StartLightning ();
						}
						ActivateWeaponIcon ();
			
				}
				if (Input.GetKeyDown (KeyCode.LeftShift)) {
						StartBoost ();
				}
				if (Input.GetKeyUp (KeyCode.LeftShift)) {
						StopBoost ();
				}
				if (Input.GetKeyDown (KeyCode.LeftControl)) {
						StartTractorBeam ();
				}
				if (Input.GetKeyUp (KeyCode.LeftControl)) {
						StopTractorBeam ();
				}
				
				if (Input.GetMouseButtonDown (0)) {
						Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
						RaycastHit hit;
						if (Physics.Raycast (ray, out hit, 100.0f, raycastMask)) {
								NewPath ();
						}
						
				}
				if (Input.GetMouseButtonDown (1)) {
						mouseDown = true;
						switch (selectedWeapon) {
						case 0:
								StartLasers ();
								return;
						case 1:
								StartLightning ();
								return;
						default:
						//none
								return;
						}
				}
				if (!Input.GetMouseButton (1)) {
						mouseDown = false;
						isFiring = false;
						switch (selectedWeapon) {
						case 0:
								StopLasers ();
								return;
						case 1:
								StopLightning ();
								return;
						default:
								return;
						}
				}
		}

		void ActivateWeaponIcon ()
		{
				switch (selectedWeapon) {
				case 0:
						laserIcon.SetActive (true);
						lightningIcon.SetActive (false);
						return;
				case 1:
						laserIcon.SetActive (false);
						lightningIcon.SetActive (true);
						return;
				default:
						return;
				}
		}
		void FindPath ()
		{
				moveTarget = ScriptUtilities.Vector3Utils.SetY (targetGameobject.transform.position, 5.0f);
				int layerMask = 1 << 0;
				seeker.StartPath (transform.position, moveTarget, OnPathComplete, layerMask);
		}
		void NewPath ()
		{
				//seeker.ReleaseClaimedPath ();
				lastTargetPos = moveTarget;
				FindPath ();
				//currentWaypoint = 0;
				/*FindPath ();*/
		}

		void OnPathComplete (Path p)
		{
				if (!p.error) {
						path = p;
						currentWaypoint = 0;
				} else {
						Debug.Log (p.error);
				}
		}
		void Move ()
		{
				if (path == null) {
						return;
				} else {
						isMoving = true;
				}
				if (currentWaypoint < path.vectorPath.Count) {
						Vector3 lookPos = ScriptUtilities.Vector3Utils.Subtract (path.vectorPath [currentWaypoint], transform.position);
						lookPos = lookPos.normalized * shipSpeed;
						transform.Translate (lookPos * Time.fixedDeltaTime);
						if (Vector3.Distance (transform.position, path.vectorPath [currentWaypoint]) < nextWaypoint) {
								currentWaypoint++;
								//transform.LookAt (path.vectorPath [currentWaypoint]);
						}
				} else {
						isMoving = false;
				}
		}
		void StartBoost ()
		{
				StopBoost ();
				StartCoroutine ("CoBoost");
		}
		void StopBoost ()
		{
				StopCoroutine ("CoBoost");
				boostIsOn = false;
				shipSpeed = (normalSpeed / 10);
		}
		void StartLightning ()
		{
				StopLightning ();
				StartCoroutine ("CoFireLightning");
		}

		void StopLightning ()
		{
				StopCoroutine ("CoFireLightning");
				lightningObject.SetActive (false);
		}

		void StartLasers ()
		{
				StopLasers ();
				StartCoroutine ("CoFireLasers");
		}

		void StopLasers ()
		{
				StopCoroutine ("CoFireLasers");
		}
		void StartTractorBeam ()
		{
				tractorBeamOn = true;
				shipSpeed = (normalSpeed / 10) / 2;
				StartCoroutine ("CoTractorBeam");
		}
		void StopTractorBeam ()
		{
				StopCoroutine ("CoTractorBeams");
				tractorBeamOn = false;
				shipSpeed = (normalSpeed / 10);
		}
		void OnTriggerEnter (Collider other)
		{
				if (other.CompareTag ("EnemyShot")) {
						TakeDamage ();
				}
		}

		void TakeDamage ()
		{
				if (health > 0) {
						health -= 1;
				} else {
						Application.LoadLevel (Application.loadedLevel);
				}
		}
		IEnumerator CoBoost ()
		{
				yield return new WaitForSeconds (0.125f);
				while (Input.GetKey (KeyCode.LeftShift)) {
						if (energy >= boostEnergyCost && isMoving && !tractorBeamOn) {
								boostIsOn = true;
								shipSpeed = boostSpeed / 10;
								energy -= boostEnergyCost;
						} else {
								boostIsOn = false;
								shipSpeed = (normalSpeed / 10);
						}
						yield return new WaitForSeconds (0.125f);
				}
				yield return null;
		}
		IEnumerator UpdatePath ()
		{
				while (true) {
						if (Input.GetMouseButton (0)) {
								Vector3 newTarget = ScriptUtilities.Vector3Utils.SetY (targetGameobject.transform.position, 5.0f);
								if (Vector3.Distance (lastTargetPos, newTarget) >= newPathThreshold) {
										//Debug.Log ("getting new path");
										Debug.DrawLine (lastTargetPos, newTarget, Color.green);
										NewPath ();
										//FindPath ();
								}
						}
						yield return new WaitForSeconds (0.125f);
				}
		}
		IEnumerator RegenerateEnergy ()
		{
				while (true) {
						//dont regen while attacking
						if (!isFiring && !tractorBeamOn && !boostIsOn) {
								//make sure that our energy plus an interation of regeneration wont overflow
								//float tEnergy = ;
								if (energy == maxEnergy) {
								} else if (energy + energyRegenStep <= maxEnergy) {
										energy += energyRegenStep;
								} else {
										//otherwise, we add the difference, negating the loop
										energy += maxEnergy - energy;
								}
						}
						yield return new WaitForSeconds (energyRegenSpeed);
				}
		}
		IEnumerator CoTractorBeam ()
		{
				Debug.Log ("tractor beam enabled");
				yield return new WaitForSeconds (0.125f);
				while (Input.GetKey(KeyCode.LeftControl)) {
						if (energy >= tractorBeamEnergyCost) {
								tractorBeamOn = true;
								energy -= tractorBeamEnergyCost;
								tractorBeamObject.SetActive (true);
						} else {
								tractorBeamObject.SetActive (false);
								tractorBeamOn = false;
						}
						yield return new WaitForSeconds (0.125f);
				}
				tractorBeamObject.SetActive (false);
				tractorBeamOn = false;
				yield return null;
		}
		IEnumerator CoFireLightning ()
		{
				Debug.Log ("laser coroutine started");
				yield return new WaitForSeconds (lightningSpeed / 2);
				while (mouseDown) {
						if (energy >= lightningEnergyCost) {
								isFiring = true;
								energy -= lightningEnergyCost;
								lightningObject.SetActive (true);
								yield return new WaitForSeconds (lightningLength);
								lightningObject.SetActive (false);
						} else {
								isFiring = false;
						}
						yield return new WaitForSeconds (lightningSpeed);
				}
				isFiring = false;
				yield return null;
		}

		IEnumerator CoFireLasers ()
		{
				Debug.Log ("fire coroutine runnig");
				//StopCoroutine ("CoFireLasers");
				yield return new WaitForSeconds (fireSpeed);
				while (mouseDown) {
						//Debug.Log ("firing a laser!");
						if (energy >= laserEnergyCost) {
								Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
								RaycastHit hit;
								if (Physics.Raycast (ray, out hit, 100.0f, raycastMask)) {
										isFiring = true;
										GameObject newLaserShot = (GameObject)Instantiate (laserShot, transform.position, Quaternion.identity);
										newLaserShot.transform.LookAt (RandomAround (hit.point, 0.0f, 1.0f));
										energy -= laserEnergyCost;
								} else {
										isFiring = false;
										//break;
								}
						} else {
								isFiring = false;
								//break;
						}
						yield return new WaitForSeconds (fireSpeed);
				}
				isFiring = false;
				yield return null;
				//yield return null;
		}

		Vector3 RandomAround (Vector3 center, float minDist, float maxDist)
		{
				var v3 = Quaternion.AngleAxis (Random.Range (0.0f, 360.0f), Vector3.up) * Vector3.forward;
				v3 = v3 * Random.Range (minDist, maxDist);
				return center + v3; 
		}
}
