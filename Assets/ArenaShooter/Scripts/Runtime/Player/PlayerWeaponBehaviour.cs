using UnityEngine;
using System.Collections;

[RequireComponent (typeof(PlayerAimBehaviour))]
[AddComponentMenu("Scripts/Player/Player Weapon Behaviour")]
public class PlayerWeaponBehaviour : MonoBehaviour {

    public Vector3 fireOriginOffset;
    public float fireDistanceOffset;

    // components
    private WeaponBehaviour weapon;
    private PlayerAimBehaviour aim;

	// Use this for initialization
	void Start () {
        this.weapon = GetComponent<WeaponBehaviour>();
        this.aim = GetComponent<PlayerAimBehaviour>();
	}
	
	// Update is called once per frame
	void Update () {
	    if (Input.GetButton("Fire1") && weapon.CanFire) {

            // find position to fire gun from
            Vector3 firePosition = transform.position + transform.TransformDirection(fireOriginOffset);

            // find direction to fire gun
            Vector3 fireDirection, target;
            if (aim.AimTarget(out target)) {
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
