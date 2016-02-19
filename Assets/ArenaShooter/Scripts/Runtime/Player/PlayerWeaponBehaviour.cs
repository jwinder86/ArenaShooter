using UnityEngine;
using System.Collections;

[RequireComponent (typeof(PlayerManager))]
[RequireComponent (typeof(CameraAimBehaviour))]
[AddComponentMenu ("Scripts/Player/Player Weapon Behaviour")]
public class PlayerWeaponBehaviour : MonoBehaviour {

    public Vector3 fireOriginOffset;
    public float fireDistanceOffset;

    public WeaponConfig config;

    // components
    private CameraAimBehaviour aim;
    private PlayerManager manager;

    // state
    private Weapon weapon;

	// Use this configure references
	void Awake () {
        aim = GetComponent<CameraAimBehaviour>();
        manager = GetComponent<PlayerManager>();
	}

    // Use this for initialization
    void Start() {
        if (config.Validate()) {
            weapon = new Weapon(config);
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (manager.Alive) {
            // update weapon's internal timer
            weapon.UpdateTimer(Time.deltaTime);

            // try to fire weapon
            if (Input.GetButton("Fire1") && weapon.CanFire) {

                // find position to fire gun from
                Vector3 firePosition = transform.position + transform.TransformDirection(fireOriginOffset);

                // find direction to fire gun
                Vector3 fireDirection, target;
                if (aim.RaycastAim(out target)) {
                    fireDirection = (target - firePosition).normalized;
                } else {
                    fireDirection = aim.AimDirection;
                }

                // move fire position forward to avoid self-shooting
                firePosition += fireDirection * fireDistanceOffset;

                // fire weapon
                weapon.FireWeapon(firePosition, fireDirection, DamageDealer.Player);
            }
        }
	}
}
