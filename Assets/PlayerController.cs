using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {


    Rigidbody body;
    public float torqueAir;
    public float torqueGround;
    public float driveForce;
    bool isGrounded = false;
    public LayerMask driveableSurfaces;
    public float disToGround;
    float tireSpeed;
    public float maxTireSpeed = 10;


	// Use this for initialization
	void Start () {
        body = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        bool b = Input.GetButton("Handbrake");
        bool j = Input.GetButton("Jump");

        CheckGround();

        body.AddRelativeForce(new Vector3(0, 0, v * driveForce));
        
        //body.velocity = transform.forward * body.velocity.magnitude;

        if (isGrounded)
        { //on the ground
            //Vector3 torque = new Vector3(0,h,0);
            //body.AddRelativeTorque(torque * torqueGround);
            //
            //Quaternion newQ = Quaternion.AngleAxis(h * (torqueGround *(tireSpeed * .15f))  * Time.deltaTime, transform.up) * transform.rotation;

            //body.MoveRotation(newQ);

            tireSpeed += v * driveForce * Time.fixedDeltaTime;
            tireSpeed = Mathf.Clamp(tireSpeed, -maxTireSpeed, maxTireSpeed);

            float speedPercent = tireSpeed / maxTireSpeed;
            body.velocity = tireSpeed * transform.forward;



            if(v == 0)
            {
                if(tireSpeed > 0)
                {
                    tireSpeed -= driveForce * Time.fixedDeltaTime * .2f;
                    if (tireSpeed < 0) tireSpeed = 0;
                } else if (tireSpeed < 0)
                {
                    tireSpeed += driveForce * Time.fixedDeltaTime * .2f;
                    if (tireSpeed > 0) tireSpeed = 0;
                }
            }

            Quaternion newQ = Quaternion.AngleAxis(h * (torqueGround * speedPercent) * Time.deltaTime, transform.up) * transform.rotation;
            body.MoveRotation(newQ);

        } else
        { //in the air
            //spin
            Vector3 torque = new Vector3();
            if (b)
            {
                torque.z = -h * torqueAir;
            } else
            {
                torque.y = h * torqueAir;
            }
            torque.x = v * torqueAir;
            

            body.AddRelativeTorque(torque);
        } 



	}

    void CheckGround()
    {
        Ray ray = new Ray(transform.position, transform.up * -1);
        RaycastHit hit;

        //Debug.DrawRay(ray.origin, ray.direction * disToGround);

        if (Physics.Raycast(ray, out hit, disToGround, driveableSurfaces))
        {
            isGrounded = true;
        }
        else isGrounded = false;
    }
}
