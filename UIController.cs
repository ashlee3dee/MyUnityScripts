using UnityEngine;
using System.Collections;

public class UIController : MonoBehaviour
{

		// Use this for initialization
		private bool isPaused = false;
		public GameObject InGame;
		public GameObject OnPause;
		private bool isSafeToPlay = false;
		public GameObject gfxConfigWindow;
		void Start ()
		{
				NotificationCenter.DefaultCenter.AddObserver (this, "SafeToPlay");
		}
	
		// Update is called once per frame
		void Update ()
		{
				HandleInput ();
		}
		void SafeToPlay ()
		{
				isSafeToPlay = true;
				InGame.SetActive (true);
				OnPause.SetActive (false);
		}
		void HandleInput ()
		{
				if (isSafeToPlay) {
						if (Input.GetKeyDown (KeyCode.P)) {
								if (isPaused) {
										UnPause ();
								} else {
										Pause ();
								}
						}
				}
		}
		public void ToggleGFXWindow ()
		{
				if (gfxConfigWindow.activeSelf) {
						gfxConfigWindow.SetActive (false);
				} else {
						gfxConfigWindow.SetActive (true);
				}
		}
		void Pause ()
		{
				isPaused = true;
				Time.timeScale = 0.0f;
				InGame.SetActive (false);
				OnPause.SetActive (true);
		}
		void UnPause ()
		{
				isPaused = false;
				Time.timeScale = 1.0f;
				InGame.SetActive (true);
				OnPause.SetActive (false);
		}
}
