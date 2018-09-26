using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour {

	Rigidbody rigidBody;
	AudioSource audioSource;

	[SerializeField] float ThrustSpeed = .1f;
	[SerializeField] float RotationSpeed = 80f; 
	[SerializeField] float levelLoadDelay = 1f;

	[SerializeField] AudioClip mainEngine;
	[SerializeField] AudioClip finishChime;
	[SerializeField] AudioClip deathExplosion;

	[SerializeField] ParticleSystem mainEngineParticles;
	[SerializeField] ParticleSystem finishChimeParticles;
	[SerializeField] ParticleSystem deathExplosionParticles;

	enum State {Alive, Dying, Transending}
	State state = State.Alive;
	// Use this for initialization
	void Start () {
		rigidBody = GetComponent<Rigidbody>();
		audioSource = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		if(state == State.Alive)
		{
			RespondToThrustInput();
			RespondToRotateInput();
		}
	}
	
	private void RespondToThrustInput()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            ApplyThrust();
        }
        else
        {
            audioSource.Stop();
			mainEngineParticles.Stop();
        }
    }

    private void ApplyThrust()
    {
        rigidBody.AddRelativeForce(Vector3.up * ThrustSpeed);
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(mainEngine);
        }
		mainEngineParticles.Play();
    }

    void RespondToRotateInput()
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

		/*or float rotation = rotationspeed * time.delotaTime then transofrm.RespondToRotateInput(Vector3.forward *rotationSpeed) */

		rigidBody.freezeRotation = false;
    }

	 void OnCollisionEnter(Collision collision)
    {
		if(state != State.Alive)
		{
			return;
		}
        switch (collision.gameObject.tag)
		{
			case "Friendly":
				break;
			case "Fuel":
				break;
            case "Finish":
                StartSuccessSequence();
                break;
            default:
                StartDeathSequence();
                break;
        }
    }

	private void StartSuccessSequence()
    {
        state = State.Transending;
        audioSource.Stop();
        audioSource.PlayOneShot(finishChime);
		mainEngineParticles.Stop();
		finishChimeParticles.Play();
        Invoke("LoadNextLevel", levelLoadDelay);
    }

    private void StartDeathSequence()
    {
        state = State.Dying;
        audioSource.Stop();
        audioSource.PlayOneShot(deathExplosion);
		mainEngineParticles.Stop();
		deathExplosionParticles.Play();
        Invoke("LoadFirstLevel", levelLoadDelay);
    }

    private void LoadNextLevel()
    {
        SceneManager.LoadScene(1);
    }

	private void LoadFirstLevel()
    {
        SceneManager.LoadScene(0);
    }
}
