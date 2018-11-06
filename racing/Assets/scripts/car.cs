using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class car : MonoBehaviour {

public float topSpeed = 150;
private float currentSpeed;
public float decelerationTorque = 30;
public Transform WheelTransformFL;
public Transform WheelTransformFR;
public Transform WheelTransformBR;
public Transform WheelTransformBL;

public float maxTurnAngle = 10;
public float maxTorque = 10;
public WheelCollider wheelFL;
public WheelCollider wheelBL;
public WheelCollider wheelFR;
public WheelCollider wheelBR;

public float spoilerRatio = 0.1f;

public Vector3 centerOfMassAdjustment = new Vector3(0f,-0.9f,0f);
private Rigidbody body;

	// Use this for initialization
	void Start () {
		body = GetComponent<Rigidbody>();
		body.centerOfMass += centerOfMassAdjustment;
	}
	
	// Update is called once per frame
	void Update () {
		UpdateWheelPositions();
		float rotationThisFrame = 360*Time.deltaTime;
		WheelTransformFL.Rotate(0,-wheelFL.rpm/rotationThisFrame,0);
		WheelTransformFR.Rotate(0,-wheelFR.rpm/rotationThisFrame,0);
        WheelTransformBL.Rotate(0,-wheelBL.rpm/rotationThisFrame,0);
    	WheelTransformBR.Rotate(0,-wheelBR.rpm/rotationThisFrame,0);

		
	}

	void FixedUpdate () {
currentSpeed = wheelBL.radius*wheelBL.rpm*Mathf.PI*0.12f;
    if(currentSpeed < topSpeed)
    {
      //rear wheel drive.
      wheelBL.motorTorque = Input.GetAxis("Vertical") * maxTorque;
      wheelBR.motorTorque = Input.GetAxis("Vertical") * maxTorque;
    }
    else
    {
      //can't go faster, already at top speed that engine produces.
      wheelBL.motorTorque = 0;
      wheelBR.motorTorque = 0;
    }


		Vector3 localVelocity = transform.InverseTransformDirection(body.velocity);
		body.AddForce(-transform.up*(localVelocity.z*spoilerRatio),ForceMode.Impulse);

		wheelFL.steerAngle = Input.GetAxis("Horizontal") * maxTurnAngle;
		wheelFR.steerAngle = Input.GetAxis("Horizontal") * maxTurnAngle;

		wheelBL.motorTorque = Input.GetAxis("Vertical") * maxTorque;
		wheelBR.motorTorque = Input.GetAxis("Vertical") * maxTorque;
		if(Input.GetAxis("Vertical") <= -0.5f && localVelocity.z > 0){
			wheelBL.brakeTorque = decelerationTorque + maxTorque;
			wheelBR.brakeTorque = decelerationTorque + maxTorque;
		}
		else if (Input.GetAxis("Vertical") == 0) {
			wheelBL.brakeTorque = decelerationTorque;
			wheelBR.brakeTorque = decelerationTorque;
		}
		else {
			wheelBL.brakeTorque = 0;
			wheelBR.brakeTorque = 0;
		}
	}

	void UpdateWheelPositions(){
		WheelHit contact = new WheelHit();

		if (wheelFL.GetGroundHit(out contact))
		{
			Vector3 temp = wheelFL.transform.position;
			temp.y = (contact.point + (wheelFL.transform.right * wheelFL.radius)).y;
			temp.y += 0.5f;
			WheelTransformFL.position = temp;
		}
		if(wheelFR.GetGroundHit(out contact))
    {
      Vector3 temp = wheelFR.transform.position;
      temp.y = (contact.point + (wheelFR.transform.up * wheelFR.radius)).y;
	  temp.y += 0.5f;
      WheelTransformFR.position = temp;
    }
    if(wheelBL.GetGroundHit(out contact))
    {
      Vector3 temp = wheelBL.transform.position;
      temp.y = (contact.point + (wheelBL.transform.right * wheelBL.radius)).y;
      temp.y+=0.5f;
	  WheelTransformBL.position = temp;
    }
    if(wheelBR.GetGroundHit(out contact))
    {
      Vector3 temp = wheelBR.transform.position;
      temp.y = (contact.point + (wheelBR.transform.up * wheelBR.radius)).y;
      temp.y+=0.5f;
	  WheelTransformBR.position = temp;
    }

	}
}
