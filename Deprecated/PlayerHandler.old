using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerHandler : MonoBehaviour
{

    [Header("Movement")]
    public float mouseLookSpeed = 50;
    [SerializeField]
    private float walkSpeed = 3f;
    [SerializeField]
    private float runSpeed = 5f;
    [SerializeField]
    private float jumpHeight = 0.4f;
    [SerializeField]
    private float gravity = 2f;

    private GameObject cam;
    private float mouseY;
    private float mouseX;
    private CharacterController characterController;
    private Vector3 moveDir;
    private float moveSpeed;
    private bool isPaused;
    private float verticalSpeed;


    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main.gameObject;
        moveSpeed = walkSpeed;
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    void Move()
    {
        if (!isPaused)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                moveSpeed = runSpeed;
            }
            else
            {
                moveSpeed = walkSpeed;
            }
            if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
            {
                float x = Input.GetAxis("Horizontal") * Time.deltaTime;
                float z = Input.GetAxis("Vertical") * Time.deltaTime;

                moveDir = new Vector3(x, 0, z);
                moveDir = transform.TransformDirection(moveDir);
                moveDir *= moveSpeed;

            }
            else
            {
                moveDir.x = 0;
                moveDir.z = 0;
            }
            if (Input.GetAxis("Mouse X") != 0)
            {
                float mouseX = Input.GetAxis("Mouse X") * Time.deltaTime * mouseLookSpeed;
                transform.Rotate(0, mouseX, 0);
            }
            if (Input.GetAxis("Mouse Y") != 0)
            {
                mouseY += Input.GetAxis("Mouse Y") * Time.deltaTime * mouseLookSpeed;
                mouseY = Mathf.Clamp(mouseY, -60, 90);
                cam.gameObject.transform.localRotation = Quaternion.Euler(-mouseY, 0f, 0f);
            }
            if (characterController.isGrounded)
            {
                verticalSpeed = -1f;
                if (Input.GetButtonDown("Jump"))
                {
                    verticalSpeed = jumpHeight;
                }
            }
            verticalSpeed -= gravity * Time.deltaTime;
            moveDir.y = verticalSpeed;
            characterController.Move(moveDir);
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isPaused = !isPaused;
        }
        if (isPaused)
        {
            //Unpause
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            //Pause
            //miniMap.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
