using UnityEngine;
using System.Collections;

public class GameTile : MonoBehaviour
{

		// Use this for initialization
		public Vector2 position;
		public GPoint.PType type;
		public bool selected;
		bool isActive;
		//public Material selectedMaterial;
		//public Material transparentMaterial;
		public Color selectedColor;
		Color defaultColor;
		Color currentColor;
		Color activeColor;
		public int hp;
		bool isAlive;
		bool isDestroyable;
		public void SetData ()
		{
				//string  = gameObject.name;
				//Debug.Log ("setting data for point");
				char[] nameChars = gameObject.name.ToCharArray ();
				//Debug.Log (gameObject.name);
				for (int i = 0; i < nameChars.Length; i++) {
						char ch = nameChars [i];
						if (ch == '/') {
								int ii = i + 1;
								string st = "";
								while (ii<nameChars.Length) {
										if (nameChars [ii] == '$')
												break;
										st += nameChars [ii];
										ii++;
								}
								ScanForData (st);
						}
				}
				SetTileValues ();
				//NotificationCenter.DefaultCenter.RemoveObserver (this, "SetData");
		}
		void SetTileValues ()
		{
				switch (type) {
				case GPoint.PType.TILE_BUILDING:
						hp = 100;
						isDestroyable = true;
						break;
				case GPoint.PType.TILE_GRASS:
						hp = 0;
						isDestroyable = false;
						break;
				case GPoint.PType.TILE_HOUSE:
						hp = 50;
						isDestroyable = true;
						break;
				case GPoint.PType.TILE_ROADH:
						hp = 0;
						isDestroyable = false;
						break;
				case GPoint.PType.TILE_ROADI:
						hp = 0;
						isDestroyable = false;
						break;
				case GPoint.PType.TILE_ROADV:
						hp = 0;
						isDestroyable = false;
						break;
				case GPoint.PType.DEBUG:
						hp = 0;
						isDestroyable = true;
						break;
				case GPoint.PType.TILE_SAND:
						hp = 0;
						isDestroyable = false;
						break;
				case GPoint.PType.TILE_TEST:
						hp = 0;
						isDestroyable = false;
						break;
				default:
						hp = 0;
						isDestroyable = false;
						break;
				}
		}
		void ScanForData (string parse)
		{
				string key = new string (new char[]{parse [0], parse [1]});
				//Debug.Log ("data key: " + key);
				if (key.Contains (":")) {
						string data = parse.Substring (2);
						//Debug.Log ("data found" + data);
						switch (key) {
						case "x:":
								SetPositionX (ParseInt (data));
								break;
						case "y:":
								SetPositionY (ParseInt (data));
								break;
						case "t:":
								UpdateType (ParseType (data));
								break;
						default:
								break;
						}
				}
		}
		int ParseInt (string parse)
		{
				//Debug.Log (parse);
				return int.Parse (parse);
		}
		GPoint.PType ParseType (string parse)
		{
				GPoint.PType foundType;
				switch (parse) {
				case "//Debug":
						foundType = GPoint.PType.DEBUG;
						break;
				case "BUFFER":
						foundType = GPoint.PType.BUFFER;
						break;
				case "EMPTY":
						foundType = GPoint.PType.EMPTY;
						break;
				case "TILE_TEST":
						foundType = GPoint.PType.TILE_TEST;
						break;
				case "TILE_GRASS":
						foundType = GPoint.PType.TILE_GRASS;
						break;
				case "TILE_SAND":
						foundType = GPoint.PType.TILE_SAND;
						break;
				case "TILE_BUILDING":
						foundType = GPoint.PType.TILE_BUILDING;
						break;
				case "TILE_ROAD":
						foundType = GPoint.PType.TILE_ROAD;
						break;
				case "TILE_ROADH":
						foundType = GPoint.PType.TILE_ROADH;
						break;
				case "TILE_ROADV":
						foundType = GPoint.PType.TILE_ROADV;
						break;
				case "TILE_ROADI":
						foundType = GPoint.PType.TILE_ROADI;
						break;
				case "TILE_HOUSE":
						foundType = GPoint.PType.TILE_HOUSE;
						break;
				case "TILE_DESTROYED":
						foundType = GPoint.PType.TILE_DESTROYED;
						break;
				case "TILE_TREE":
						foundType = GPoint.PType.TILE_TREE;
						break;
				default:
						foundType = GPoint.PType.EMPTY;
						break;
				}
				//Debug.Log ("type found" + foundType);
				return foundType;
		}
		void UpdateType (GPoint.PType newType)
		{
				type = newType;
		}
		void SetPositionX (int newX)
		{
				position.x = newX - 1;
		}
		void SetPositionY (int newY)
		{
				position.y = newY - 1;
		}
		void Awake ()
		{
				
		}
		void Start ()
		{
				isAlive = true;
				isActive = false;
				NotificationCenter.DefaultCenter.AddObserver (this, "SetData");
				selectedColor = Color.blue;
				defaultColor = transform.GetChild (0).GetComponent<Renderer> ().material.color;
				currentColor = defaultColor;
				activeColor = Color.red;
		}
		// Update is called once per frame
		public void Select ()
		{
				selected = true;
				//transform.transform.Translate (transform.up);
				//SetColors (selectedColor);
				//Debug.Log ("selected " + gameObject.name);
		}
		public void DeSelect ()
		{
				selected = false;
				//transform.transform.Translate (-transform.up);
				//SetColors (defaultColor);
				//Debug.Log ("deselected " + gameObject.name);
		}
		public void ToggleActive ()
		{ 
				switch (isActive) {
				case true:
						Deactivate ();
						break;
				case false:
						Activate ();
						break;
				}
		}
		void SetColors (Color newColor)
		{
				foreach (Renderer r in transform.GetComponentsInChildren<Renderer>()) {
						r.material.color = newColor;
				}
		}
		void Deactivate ()
		{
				isActive = false;
				currentColor = defaultColor;
				SetColors (currentColor);
				transform.Translate (-Vector2.up);
		}
		void Activate ()
		{
				isActive = true;
				currentColor = activeColor;
				SetColors (currentColor);
				transform.Translate (Vector2.up);
		}
		void OnTriggerEnter (Collider other)
		{
				if (other.CompareTag ("PlayerLaserShot")) {
						Hit (10);
						Destroy (other.gameObject);
				} else if (other.CompareTag ("PlayerLightningBolt")) {
						Hit (1);
				}
				
		}
		public int GetHP ()
		{
				return hp;
		}
		void Hit (int damage)
		{
				if (isDestroyable) {
						hp -= damage;
						if (hp <= 0) {
								NotificationCenter.DefaultCenter.AddObserver (this, "Destroy");
								NotificationCenter.DefaultCenter.PostNotification (this, "DestroyTiles");
						}
				}
				
		}
		public bool IsAlive ()
		{
				return isAlive;
		}
		public void Destroy ()
		{
				//audio.PlayOneShot (audio.clip);
				//NotificationCenter.DefaultCenter.RemoveObserver (this, "Destroy");
				Destroy (gameObject);
		}
		void Update ()
		{

		}
}
