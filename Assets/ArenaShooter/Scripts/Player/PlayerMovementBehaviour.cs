﻿using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovementBehaviour : MonoBehaviour {

    // components
    private CharacterController controller;

    // settings
    public float moveSpeed = 3f;

    // Use this for initialization
    void Start() {
        // retrieve members from this object
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update() {
        // move character
        Vector2 moveDirection = new Vector2(
            Input.GetAxis("Horizontal"),
            Input.GetAxis("Vertical"));

        moveDirection = Vector2.ClampMagnitude(moveDirection, 1f);

        Move((transform.forward * moveDirection.y +
            transform.right * moveDirection.x) * moveSpeed);
    }

    // handle actually moving player
    private void Move(Vector3 moveDirection) {
        controller.SimpleMove(moveDirection);
    }
}
