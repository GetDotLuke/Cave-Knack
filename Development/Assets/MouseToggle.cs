using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets.Utility;
using Random = UnityEngine.Random;

public class MouseToggle : MonoBehaviour {

	bool cursorLocked = true;

	void Update () {
		
		if (Input.GetKeyDown ("i")) {
			cursorLocked = !cursorLocked;
		}


			if (cursorLocked) {
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = false;

			} 

			else {
			Cursor.lockState = CursorLockMode.Confined;
				Cursor.visible = true;

			var go = GameObject.Find("PlayerCharacter");
			var gameObject = GameObject.Find("Main Camera");
							
		}







	}

}
