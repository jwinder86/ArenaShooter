using UnityEngine;
using System.Collections;

[AddComponentMenu("Scripts/Player/Player Aim Behaviour")]
public class PlayerAimBehaviour : MonoBehaviour {

    // settings
    [Header("Movement")]
    public float horizontalSpeed = 5f;
    public float verticalSpeed = 5f;

    // private members
    private Vector3 aimDirection;
    public Vector3 AimDirection {
        get { return aimDirection; }
    }

    // rotation around Y axis
    private float yRot;
    
    // Use this for initialization
    void Start() {
        aimDirection = transform.forward;
        yRot = 0f;
    }

    // Update is called once per frame
    void Update() {
        float xRot = Input.GetAxis("Mouse X") * horizontalSpeed;
        yRot -= Input.GetAxis("Mouse Y") * verticalSpeed;
        yRot = Mathf.Clamp(yRot, -85f, 85f);

        // rotate transform first, them adjust aim up/down
        transform.Rotate(Vector3.up, xRot);
        aimDirection = transform.forward;
        aimDirection = Quaternion.AngleAxis(yRot, transform.right) * aimDirection;
    }
}
