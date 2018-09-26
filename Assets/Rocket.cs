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

	bool isTransitioning = false;

	bool collisionsDisabled = false;

	// Use this for initialization
	void Start () {
		rigidBody = GetComponent<Rigidbody>();
		audioSource = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		if(!isTransitioning)
		{
			RespondToThrustInput();
			RespondToRotateInput();
		}
		if( Debug.isDebugBuild ) // allows only in development builds
		{
			RespondToDebugKeys();
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
            StopApplyingThrust();
        }
    }

    private void StopApplyingThrust()
    {
        audioSource.Stop();
        mainEngineParticles.Stop();
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
		rigidBody.angularVelocity = Vector3.zero;

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
    }

	void RespondToDebugKeys() 
	{
		if(Input.GetKey(KeyCode.L)) 
		{
			LoadNextLevel(); 
		}

		if(Input.GetKey(KeyCode.C))
		{
			collisionsDisabled = !collisionsDisabled;
		}
	}

	 void OnCollisionEnter(Collision collision)
    {
		if(isTransitioning || collisionsDisabled)
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
        isTransitioning = true;
        audioSource.Stop();
        audioSource.PlayOneShot(finishChime);
		mainEngineParticles.Stop();
		finishChimeParticles.Play();
        Invoke("LoadNextLevel", levelLoadDelay);
    }

    private void StartDeathSequence()
    {
        isTransitioning = true;
        audioSource.Stop();
        audioSource.PlayOneShot(deathExplosion);
		mainEngineParticles.Stop();
		deathExplosionParticles.Play();
        Invoke("LoadFirstLevel", levelLoadDelay);
    }

    private void LoadNextLevel()
    {
		int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        if(currentSceneIndex + 1 < SceneManager.sceneCountInBuildSettings)
		{
			SceneManager.LoadScene(currentSceneIndex + 1);
		}
		else 
		{
			LoadFirstLevel(); // todo build finish screen or victory screen
		}
    }

	private void LoadFirstLevel()
    {
        SceneManager.LoadScene(0);
    }
}