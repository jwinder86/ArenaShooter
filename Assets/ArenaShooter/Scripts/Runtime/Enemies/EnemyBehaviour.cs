using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class EnemyBehaviour : MonoBehaviour {

    //public float maxSpeed;
    public float accelleration;

    public float playerFollowDistance;
    public float playerFollowAngle;
    private Vector3 startPos;
    private Transform playerLocation;

    public Rigidbody rb;


    // Use this for initialization
    void Start () {
        playerLocation = FindObjectOfType<PlayerMovementBehaviour>().transform;
        rb = GetComponent<Rigidbody>();
        startPos = transform.position;
    }
	
	// Update is called once per frame
	void Update () {
        //Debug.Log("enemy position: " + transform.position + " - Player Position: " + playerLocation.position +
        //    " - magnitude: " + (transform.position - playerLocation.position).magnitude + " - distance " + playerFollowDistance);
        if ((transform.position - playerLocation.position).magnitude < playerFollowDistance)
        {
           // Debug.Log("follow player");
            FollowPlayer(accelleration);
        }
    }


    void FollowPlayer(float accelleration)
    {
       
        Vector3 variance = Random.onUnitSphere * 0.8F;
        
        Vector3 direction = (playerLocation.position - transform.position + variance);
        direction.y = 0F;
        direction = direction.normalized;
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
}
