using UnityEngine;
using System.Collections;

public class PlayerAimBehaviour : MonoBehaviour {

    // settings
    [Header("Movement")]
    public float horizontalSpeed = 5f;
    public float verticalSpeed = 5f;

    // offset of player's eyes from transform center
    public Vector3 playerViewOffset = new Vector3(0f, 0.5f, 0f);
    public float cameraRise = 1.5f;
    public float cameraOffset = 5f;
    public float maxAimDistance = 100f;

    [Header("Rendering")]
    public Renderer charRenderer;
    public Material transparencyMaterial;
    public float minFadeDistance = 0.5f;
    public float maxFadeDistance = 1f;

    // external components
    private Transform camera;

    // private members
    private Vector3 aimDirection;
    public Vector3 AimDirection {
        get { return aimDirection; }
    }

    private Material defaultMaterial;

    // rotation around Y axis
    private float yRot;

    // distance between camera and player
    private float cameraDist;

    // speed that distance between camera and player changes, used to provide smooth movement when camera collides
    private float cameraDistSpeed;

    // Use this for initialization
    void Start() {
        camera = Camera.main.transform;
        aimDirection = transform.forward;
        yRot = 0f;
        cameraDist = cameraOffset;
        cameraDistSpeed = 0f;

        // Copy texture to transparency material
        defaultMaterial = charRenderer.sharedMaterial;
        transparencyMaterial.SetTexture("_MainTex", defaultMaterial.GetTexture("_MainTex"));
        transparencyMaterial.color = defaultMaterial.color;
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

    // LateUpdate is called once per frame, after all Updates complete
    void LateUpdate() {
        // get camera lift
        Vector3 rise = Vector3.Cross(aimDirection, transform.right).normalized * cameraRise;
        Vector3 startPos = transform.position + playerViewOffset;
        Vector3 toTarget = rise - aimDirection * cameraOffset;

        // find distance before hitting something
        float goalDist = toTarget.magnitude;
        int layerMask = ~(1 << LayerMask.NameToLayer("Player"));
        RaycastHit hit;
        if (Physics.SphereCast(startPos, 0.35f, toTarget, out hit, goalDist, layerMask)) {
            goalDist = hit.distance;
        }

        // smooth distance
        cameraDist = Mathf.SmoothDamp(cameraDist, goalDist, ref cameraDistSpeed, 0.1f);

        // adjust character renderer
        FadeCameraDistance(cameraDist);

        // move camera into position
        camera.position = startPos + toTarget.normalized * cameraDist;
        camera.rotation = Quaternion.LookRotation(aimDirection, Vector3.up);
    }

    // Fade out player depending on camera distance
    private void FadeCameraDistance(float distance) {
        if (distance >= maxFadeDistance) {
            charRenderer.enabled = true;
            charRenderer.material = defaultMaterial;
        } else if (distance <= minFadeDistance) {
            charRenderer.enabled = false;
        } else {
            charRenderer.enabled = true;
            charRenderer.material = transparencyMaterial;
            Color color = transparencyMaterial.color;
            color.a = (distance - minFadeDistance) / (maxFadeDistance - minFadeDistance);
            transparencyMaterial.color = color;
        }
    }

    // Determine if player is aiming at something, and where it's aim is intersecting
    public bool AimTarget(out Vector3 target) {
        int layerMask = ~(1 << LayerMask.NameToLayer("Player"));
        RaycastHit hit;
        if (Physics.Raycast(camera.transform.position, aimDirection, out hit, maxAimDistance, layerMask)) {
            target = hit.point;
            return true;
        } else {
            target = camera.transform.position + aimDirection * maxAimDistance;
            return true;
        }
    }
}
