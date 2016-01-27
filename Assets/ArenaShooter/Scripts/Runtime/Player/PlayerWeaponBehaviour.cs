using UnityEngine;
using System.Collections;

[RequireComponent (typeof(CameraAimBehaviour))]
[AddComponentMenu("Scripts/Player/Player Weapon Behaviour")]
public class PlayerWeaponBehaviour : MonoBehaviour {

    public Vector3 fireOriginOffset;
    public float fireDistanceOffset;

    // components
    private WeaponBehaviour weapon;
    private CameraAimBehaviour aim;

	// Use this for initialization
	void Awake () {
        this.weapon = GetComponent<WeaponBehaviour>();
        this.aim = GetComponent<CameraAimBehaviour>();
	}
	
	// Update is called once per frame
	void Update () {
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
