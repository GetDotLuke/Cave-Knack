using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseToggle : MonoBehaviour {

	bool cursorLocked = true;

	void Update () {
		if (Input.GetKeyDown ("i") ){
			cursorLocked = !cursorLocked;
		}
		if (cursorLocked) {
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = true;
			cursorLocked = false;
		} else {
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
			cursorLocked = true;
		}

	}

}
