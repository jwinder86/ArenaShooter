using UnityEngine;
using System.Collections;

[RequireComponent (typeof(PlayerAimBehaviour))]
[AddComponentMenu("Scripts/Player/Camera Aim Behaviour")]
public class CameraAimBehaviour : MonoBehaviour {

    private int layerMask;

    [Header("Positioning")]
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

    public Vector3 AimDirection {
        get { return aim.AimDirection; }
    }

    new private Transform camera;
    private PlayerAimBehaviour aim;
    private PlayerManager manager;

    // distance between camera and player
    private float cameraDist;

    // speed that distance between camera and player changes, used to provide smooth movement when camera collides
    private float cameraDistSpeed;

    // default material for renderer
    private Material defaultMaterial;

    private Vector3 aimDirection;

    // called when object instantiated
    void Awake() {
        layerMask = ~(1 << LayerMask.NameToLayer("Player"));
        camera = Camera.main.transform;

        aim = GetComponent<PlayerAimBehaviour>();
        manager = GetComponent<PlayerManager>();
    }

    // called before first Update
    void Start() {
        cameraDist = cameraOffset;
        cameraDistSpeed = 0f;

        // get current material
        defaultMaterial = charRenderer.sharedMaterial;

        // clone editor's material
        transparencyMaterial = new Material(transparencyMaterial);

        // Copy texture to transparency material
        transparencyMaterial.SetTexture("_MainTex", defaultMaterial.GetTexture("_MainTex"));
        transparencyMaterial.color = defaultMaterial.color;

        aimDirection = Vector3.forward;
    }

    // LateUpdate is called once per frame, after all Updates complete
    void LateUpdate() {
        if (manager.Alive) {
            aimDirection = aim.AimDirection;
        }

        // get camera lift
        Vector3 rise = Vector3.Cross(aimDirection, transform.right).normalized * cameraRise;
        Vector3 startPos = transform.position + playerViewOffset;
        Vector3 toTarget = rise - aimDirection * cameraOffset;

        // find distance before hitting something
        float goalDist = toTarget.magnitude;
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
    public bool RaycastAim(out Vector3 target) {
        if (manager.Alive) {
            aimDirection = aim.AimDirection;
        }

        RaycastHit hit;
        if (Physics.Raycast(camera.transform.position, aimDirection, out hit, maxAimDistance, layerMask)) {
            target = hit.point;
            Debug.Log("Aiming at: " + hit.collider);
            return true;
        } else {
            target = camera.transform.position + aimDirection * maxAimDistance;
            Debug.Log("Aiming at nothing");
            return false;
        }
    }
}
