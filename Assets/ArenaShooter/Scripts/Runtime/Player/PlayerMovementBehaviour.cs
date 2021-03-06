using UnityEngine;
using System.Collections;
using System;

[RequireComponent (typeof(PlayerManager))]
[RequireComponent (typeof(CharacterController))]
[AddComponentMenu ("Scripts/Player/Player Movement Behaviour")]
public class PlayerMovementBehaviour : MonoBehaviour {

    // components
    private CharacterController controller;
    private PlayerManager manager;

    // settings
    public float moveSpeed = 3f;

    // Use this for initialization
    void Awake() {
        controller = GetComponent<CharacterController>();
        manager = GetComponent<PlayerManager>();
    }

    // Update is called once per frame
    void Update() {
        if (manager.Alive) {
            // move character
            Vector2 moveDirection = new Vector2(
                Input.GetAxis("Horizontal"),
                Input.GetAxis("Vertical"));

            moveDirection = Vector2.ClampMagnitude(moveDirection, 1f);

            Move((transform.forward * moveDirection.y +
                transform.right * moveDirection.x) * moveSpeed);
        }
    }

    // handle actually moving player
    private void Move(Vector3 moveDirection) {
        controller.SimpleMove(moveDirection);
    }
}
