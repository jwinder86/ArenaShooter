using UnityEngine;
using System.Collections;

public class EnemyWeaponBehaviour : MonoBehaviour {
    public Vector3 fireOriginOffset;
    public float fireDistanceOffset;

    public WeaponConfig config;

    // components
   // private CameraAimBehaviour aim;

    private Vector3 aimDirection;


    // state
    private Weapon weapon;


    // Use this for initialization
    void Start () {
        if (config.Validate())
        {
            weapon = new Weapon(config);
        }
        aimDirection = transform.forward;

    }

    // Update is called once per frame
    void Update () {
        weapon.UpdateTimer(Time.deltaTime);
        aimDirection = transform.forward;

        // try to fire weapon

        // find position to fire gun from
        Vector3 firePosition = transform.position + transform.TransformDirection(fireOriginOffset);

        // find direction to fire gun
        Vector3 fireDirection;

        fireDirection = aimDirection;
       

        // move fire position forward to avoid self-shooting
        firePosition += fireDirection * fireDistanceOffset;

        // fire weapon
        weapon.FireWeapon(firePosition, fireDirection, DamageDealer.Enemy);
        
    }
}
