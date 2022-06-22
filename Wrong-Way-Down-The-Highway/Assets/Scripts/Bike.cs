using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bike : MonoBehaviour
{
    [SerializeField] WheelCollider frontWheel;
    [SerializeField] WheelCollider rearWheel;

    [SerializeField] Transform frontTransform;
    [SerializeField] Transform rearTransform;
    [SerializeField] Transform bikeHandleTransform;

    public float acceleration = 500f;
    public float brakingForce = 300f;
    public float maxTurnAngle = 15f;
    public float maxSpeed = 200f;

    private float currentAcceleration = 0f;
    private float currentBrakeForce = 0f;
    private float currentTurnAngle = 0f;

    public delegate void CrashedDelegate();
    public event CrashedDelegate crashEvent;

    private void FixedUpdate()
    {

       //currentAcceleration = acceleration * Input.GetAxis("Vertical"); 

        //If pressing space, assign brake force value
        if (Input.GetKey(KeyCode.Space))
        {
            currentBrakeForce = brakingForce;
        }
        else
        {
            currentBrakeForce = 0f;
        }

        
        rearWheel.motorTorque = acceleration; //rear wheel acceleration
        

        //braking force applied to wheels
        frontWheel.brakeTorque = currentBrakeForce; 
        rearWheel.brakeTorque = currentBrakeForce;

        //steering
        currentTurnAngle = maxTurnAngle * Input.GetAxis("Horizontal");
        frontWheel.steerAngle = currentTurnAngle; //steer angle method built in from unity Wheel Colliders
        

        UpdateWheel(frontWheel, frontTransform, false);
        UpdateWheel(rearWheel, rearTransform, false);

        //UpdateHandle(bikeHandleTransform, currentTurnAngle, 90);
        Rigidbody rb = transform.GetComponent<Rigidbody>();
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);

        if(transform.position.y < 0)
        {
            crashEvent();
        }

    }

    void UpdateHandle(Transform trans, float currentTurnAngle, float maxRotation)
    {
        if(trans.eulerAngles.y < maxRotation)
        {
            trans.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, currentTurnAngle, transform.rotation.eulerAngles.z);

        }

    }

    void OnCollisionEnter(Collision targetObj)
    {
        if (targetObj.gameObject.tag == "Vehicle")
        {
            //call delegate
            if(crashEvent != null)
            {
                crashEvent();
            }
        }
    }
    
    void UpdateWheel(WheelCollider col, Transform trans, bool onlyRotateY)
    {
        //get wheel collider state
        Vector3 position;
        Quaternion rotation;
        

        //getworldpose built in wheelcollider method
        col.GetWorldPose(out position, out rotation);//out paras initialise and set the variables

        if (onlyRotateY)
        {
            //rotates only on Y axis for front wheel and handle and light objects etc
            rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
            trans.rotation = rotation;
            return;
        }
        //set wheel transform

        trans.position = position;
        trans.rotation = rotation;
    }
}
