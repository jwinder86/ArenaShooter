using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class EnemyBehaviour : MonoBehaviour, DeathHandler {

    //public float maxSpeed;
    public float accelleration;

    // these settings may want to be abstracted/ defaulted for different classes of enemies
    // enemy's detection range
    public float detectionRange;
    //public float playerFollowAngle;
    // distance enemies pursue to (stop at)
    public float followRange;


    //private Vector3 startPos;
    private Transform playerLocation;

    private Rigidbody rb;
    private EnemyWeaponBehaviour wb;

    // Use this for initialization
    void Start () {
        playerLocation = FindObjectOfType<PlayerMovementBehaviour>().transform;
        rb = GetComponent<Rigidbody>();
        //startPos = transform.position;
        wb = GetComponent<EnemyWeaponBehaviour>();
    }
	
	// Update is called once per frame
	void Update () {
        //Debug.Log("enemy position: " + transform.position + " - Player Position: " + playerLocation.position +
        //    " - magnitude: " + (transform.position - playerLocation.position).magnitude + " - distance " + playerFollowDistance);

        // simplistic version, may want to adjust this to move away from player when too close, 
        //    rather than just not adding more force to get closer
        float distanceFromPlayer = (transform.position - playerLocation.position).magnitude;

        if (distanceFromPlayer < detectionRange) {
            wb.setCanFire(true);
            if (distanceFromPlayer > followRange)
            {

                // Debug.Log("follow player");
                FollowPlayer(accelleration);
            }
        }
        else
        {
            wb.setCanFire(false);
        }
    }


    void FollowPlayer(float accelleration)
    {
       
        Vector3 variance = Random.onUnitSphere * 0.8F;
        
        Vector3 direction = (playerLocation.position - transform.position + variance);
        direction.y = 0F;
        direction = direction.normalized;
        transform.LookAt(playerLocation);
        /* Debug.Log("playerLocation: " + playerLocation.position);
        Debug.Log("enemyLocation: " + transform.position);
        Debug.Log("variance: " + variance);
        Debug.Log("Direction: " + direction);*/

        /* if (Vector3.Angle(direction, transform.right) > playerFollowAngle)
         {
             return;
         }*/

        //float angle = Mathf.Rad2Deg * Mathf.Atan2(direction.y, direction.x);
        //transform.position + direction, Quaternion.Euler(new Vector3(0f, 0f, angle)));
        //rb.AddForce(new Vector3(0f, 0f, angle), ForceMode.Acceleration);
        // rb.AddRelativeForce(direction, ForceMode.Acceleration);

        rb.AddForce(direction * accelleration, ForceMode.Acceleration);
    }



    public void HandleDeath()
    {
        Destroy(gameObject);
    }
}
