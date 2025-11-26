using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public static float Battery = 100;
    public float battery
    {
        get { return Battery; }
        set { Battery = Mathf.Clamp(value, 0, 100); }
    }
    public bool thirdPerson = false;
    public Vector3 thirdPersonOffset = new Vector3(0, 2, -4);
    public float mouseSens = 100f;
    public Transform camera;
    float xRotation = 0f;

    public CharacterController controller;
    public float speed = 12f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;

    public GameObject teleporter1;
    public GameObject teleporter2;
    public GameObject playerModel;

    public Transform groundCheck;
    public float groundDistance = 0.7f;
    public LayerMask groundMask;

    public Vector3 velocity;
    public bool isGrounded;

    public AudioSource movementSounds; // assign your MovementSounds AudioSource in Inspector

    public float cameraRotateSpeed = 20f; // degrees/sec smoothing for camera rotation
    public float minPitch = -45f; // clamp down
    public float maxPitch = 45f;  // clamp up

    public static float Score;

    void Start()
    {
        Cursor.lockState = thirdPerson ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = thirdPerson;
    }

    void Update()
    {

        Battery -= Time.deltaTime * 0.5f; // Decrease battery over time

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded)
        {
            velocity.y = 0f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * speed * Time.deltaTime);
        if ((x != 0f || z != 0f) && movementSounds != null)
        {
            if (!movementSounds.isPlaying)
            {
                movementSounds.Play();
            }
        }
        else if (movementSounds != null && movementSounds.isPlaying)
        {
            movementSounds.Stop();
        }

        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        if (!isGrounded)
        {
            velocity.y += gravity * Time.deltaTime;
        }

        controller.Move(velocity * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.K))
        {
            thirdPerson = !thirdPerson;
            Cursor.lockState = thirdPerson ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = thirdPerson;
        }

        CameraMovement();

        if (battery <= 0)
        {
            // Handle player death or game over due to battery depletion
            Debug.Log("Battery Depleted! Game Over.");
            SceneManager.LoadScene("GameOverScene");
            // You can add additional logic here, such as restarting the level or showing a game over screen.
        }
    }

    void CameraMovement()
    {
        if (camera == null) return;

        float mouseX = Input.GetAxis("Mouse X") * mouseSens * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSens * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, minPitch, maxPitch);

        camera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

public void TeleportFromTeleporter1()
{
    controller.enabled = false;
    transform.position = teleporter2.transform.position + Vector3.up * 10f; // move forward 2 units
    controller.enabled = true;
}

public void TeleportFromTeleporter2()
{
    controller.enabled = false;
    transform.position = teleporter1.transform.position + Vector3.forward * 10f; // move forward 2 units
    controller.enabled = true;
}
void OnTriggerEnter(Collider other)
{
    if (other.gameObject == teleporter1)
    {
        TeleportFromTeleporter1();
    }
    else if (other.gameObject == teleporter2)
    {
        TeleportFromTeleporter2();
    }
}
}
//wsg code readers