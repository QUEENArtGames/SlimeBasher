using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPauseMenu : MonoBehaviour {

	// Use this for initialization
	public void QuitGame() {
        Application.Quit();
    }

    public void EnableCursor() {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}
