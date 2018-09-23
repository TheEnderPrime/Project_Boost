using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour {

	Rigidbody rigidBody;

	// Use this for initialization
	void Start () {
		rigidBody = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
		ProcessUpdate();
	}

    void ProcessUpdate()
    {
        if (Input.GetKey(KeyCode.Space))
		{
			rigidBody.AddRelativeForce(Vector3.up);
		}

		if (Input.GetKey(KeyCode.A))
		{
			Quaternion deltaRotation = Quaternion.Euler(new Vector3(0,0,80) * Time.deltaTime);
			rigidBody.MoveRotation(deltaRotation * rigidBody.rotation);
		}
		else if (Input.GetKey(KeyCode.D))
		{
			Quaternion deltaRotation = Quaternion.Euler(new Vector3(0,0, -80) * Time.deltaTime);
			rigidBody.MoveRotation(deltaRotation * rigidBody.rotation);
		}
    }
}
