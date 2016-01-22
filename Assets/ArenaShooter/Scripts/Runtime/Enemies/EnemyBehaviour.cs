using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class EnemyBehaviour : MonoBehaviour {

    public float maxSpeed;
    public float accelleration;

    public float playerFollowDistance;
    public float playerFollowAngle;
    private Vector3 startPos;
    private Transform playerLocation;

    public Rigidbody rb;


    // Use this for initialization
    void Start () {
        playerLocation = FindObjectOfType<PlayerMovementBehaviour>().GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();
        startPos = transform.position;
    }
	
	// Update is called once per frame
	void Update () {
        Debug.Log("enemy position: " + transform.position + " - Player Position: " + playerLocation.position);
        if ((transform.position - playerLocation.position).magnitude < playerFollowDistance)
        {
            FollowPlayer();
        }
    }


    void FollowPlayer()
    {
        Vector3 direction = (playerLocation.position - transform.position).normalized;
        
        if (Vector3.Angle(direction, transform.right) > playerFollowAngle)
        {
            return;
        }

        float angle = Mathf.Rad2Deg * Mathf.Atan2(direction.y, direction.x);
        //transform.position + direction, Quaternion.Euler(new Vector3(0f, 0f, angle)));




        rb.AddForce(new Vector3(0f, 0f, angle), ForceMode.Acceleration);
    }
}
