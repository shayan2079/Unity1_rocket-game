using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{

    private Rigidbody rigidBody;
    private AudioSource audioSource;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float movementSpeed;
    private bool isSceneChanging = false;
    [SerializeField] private ParticleSystem celebrationEffect;
    [SerializeField] private ParticleSystem deathEffect;
    [SerializeField] private ParticleSystem[] engineEffect = new ParticleSystem[3];

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        rotationSpeed = 200f;
        movementSpeed = 300f;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isSceneChanging)
        {
            ProcessInput();
        } 
        else
        {
            StopEnginesEffect();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isSceneChanging) return;

        switch (collision.gameObject.tag)
        {
            case "Friendly":
                break;
            case "Finish":
                celebrationEffect.Play();
                isSceneChanging = true;
                Invoke(nameof(LoadNextLevel), 2f);
                break;
            default:
                deathEffect.Play();
                isSceneChanging = true;
                Invoke(nameof(KillRocket), 2f);
                break;
        }
    }

    private void KillRocket()
    {
        SceneManager.LoadScene(0);
    }

    private void LoadNextLevel()
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        int totalScenes = SceneManager.sceneCountInBuildSettings;
        SceneManager.LoadScene((currentScene + 1) % totalScenes);
    }


    private void ProcessInput()
    {
        Thrusting();
        Rotating();
    }

    private void Thrusting()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            rigidBody.AddRelativeForce(movementSpeed * Time.deltaTime * Vector3.up);
            foreach (ParticleSystem effect in engineEffect)
            {
                effect.Play();
            }
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
        else
        {
            StopEnginesEffect();
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }
        }
    }

    private void StopEnginesEffect()
    {
        foreach (ParticleSystem effect in engineEffect)
        {
            if (effect.isPlaying)
            {
                effect.Stop();
            }
        }
    }

    private void Rotating()
    {
        rigidBody.freezeRotation = true;
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(rotationSpeed * Time.deltaTime * Vector3.forward, Space.World);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(rotationSpeed * Time.deltaTime * Vector3.back, Space.World);
        }
        rigidBody.freezeRotation = false;
    }
}
