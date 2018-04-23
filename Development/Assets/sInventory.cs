using UnityEngine;
using System.Collections;

public class sInventory : MonoBehaviour {

	public GameObject Menu; // Assign in inspector
	public GameObject Crosshair;
	public GameObject TimerText;
	private bool isShowing;

	void Start(){

		isShowing = false;
		Menu.SetActive(isShowing);
	}



	void Update() {
		
		if (Input.GetKeyDown("i")) {
			isShowing = !isShowing;
			Menu.SetActive(isShowing);

			isShowing = !isShowing;
			Crosshair.SetActive(isShowing);

			TimerText.SetActive (isShowing);
			isShowing = !isShowing;


		}
	}
}
