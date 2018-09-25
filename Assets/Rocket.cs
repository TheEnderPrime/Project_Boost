using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour {

	Rigidbody rigidBody;
	AudioSource audioSource;

	[SerializeField] float ThrustSpeed = .1f;
	[SerializeField] float RotationSpeed = 80f; 

	// Use this for initialization
	void Start () {
		rigidBody = GetComponent<Rigidbody>();
		audioSource = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		Thrust();
		Rotate();
	}

	 private void Thrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            rigidBody.AddRelativeForce(Vector3.up * ThrustSpeed);
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
        else
        {
            audioSource.Stop();
        }
    }

    void Rotate()
    {
		rigidBody.freezeRotation = true;

        if (Input.GetKey(KeyCode.A))
        {
            Quaternion deltaRotation = Quaternion.Euler(new Vector3(0, 0, RotationSpeed) * Time.deltaTime);
            rigidBody.MoveRotation(deltaRotation * rigidBody.rotation);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            Quaternion deltaRotation = Quaternion.Euler(new Vector3(0, 0, -RotationSpeed) * Time.deltaTime);
            rigidBody.MoveRotation(deltaRotation * rigidBody.rotation);
        }

		/*or float rotation = rotationspeed * time.delotaTime then transofrm.Rotate(Vector3.forward *rotationSpeed) */

		rigidBody.freezeRotation = false;
    }

	 void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
		{
			case "Friendly":
				print("Ok");
				break;
			case "Fuel":
				print("Fuel");
				break;
			case "Finish":
				print("Finished");
				break;
			default:
				print("Dead");
				break;
		}
    }
}
