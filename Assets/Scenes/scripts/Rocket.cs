using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{

    Rigidbody rigidBody;
    AudioSource audioSource;
    [SerializeField] float rotationSpeed;
    [SerializeField] float movementSpeed;
    private bool inputsActive = true;

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
        if (inputsActive)
        {
            ProcessInput();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Friendly":
                break;
            case "Finish":
                inputsActive = false;
                Invoke(nameof(LoadNextLevel), 2f);
                break;
            default:
                inputsActive = false;
                Invoke(nameof(killRocket), 2f);
                break;
        }
    }

    private void killRocket()
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
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
        else
        {
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
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
