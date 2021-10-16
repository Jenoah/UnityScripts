using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotation : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform cameraTransform = null;

    [Header("Rotation speed")]
    [SerializeField] private float verticalRotationSpeed = 7.5f;
    [SerializeField] private float horizontalRotationSpeed = 7.5f;

    [Header("Limits")]
    [SerializeField, Range(0f, 89.99f)] private float upperRotationLimit = 89.99f;
    [SerializeField, Range(-89.99f, 0f)] private float lowerRotationLimit = -89.99f;

    private float currentVerticalAngle = 0;
    private float currentHorizontalAngle = 0;
    // Start is called before the first frame update
    void Start()
    {
        if(cameraTransform == null)
        {
            cameraTransform = Camera.main.transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        currentVerticalAngle -= Input.GetAxis("Mouse Y") * verticalRotationSpeed * Time.timeScale;
        currentHorizontalAngle += Input.GetAxis("Mouse X") * horizontalRotationSpeed * Time.timeScale;

        currentVerticalAngle = Mathf.Clamp(currentVerticalAngle, lowerRotationLimit, upperRotationLimit);

        cameraTransform.localRotation = Quaternion.Euler(currentVerticalAngle, 0, 0);
        transform.localRotation = Quaternion.Euler(0, currentHorizontalAngle, 0);

    }
}
