using UnityEngine;
using System.Collections;

public class CursorBehaviour : MonoBehaviour {

    private bool cursorLocked;

    // Use this for initialization
    void Start() {
        cursorLocked = true;
        ApplyCursorLock();
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.Mouse0)) {
            ApplyCursorLock();
        }

        if (Input.GetKeyDown(KeyCode.Escape)) {
            ToggleCursorLock();
        }
    }

    private void ToggleCursorLock() {
        cursorLocked = !cursorLocked;
        ApplyCursorLock();
    }

    private void ApplyCursorLock() {
        if (cursorLocked) {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        } else {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
