using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class ContinuousLocomotion : MonoBehaviour
{
    [SerializeField] private XRNode controllerListener = XRNode.LeftHand;
    [SerializeField] private Transform forwardReference = null;
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private Rigidbody rigRigidbody = null;

    private InputDevice controller;

    private void Start()
    {
        controller = InputDevices.GetDeviceAtXRNode(controllerListener);
    }

    void FixedUpdate()
    {
        if (controller == null) return;
        Move();
    }

    private void Move()
    {
        controller.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 controllerThumbpad);
        if (controllerThumbpad.magnitude > 1) controllerThumbpad.Normalize();
        Vector3 moveDirection = forwardReference.TransformDirection(new Vector3(controllerThumbpad.x, 0, controllerThumbpad.y) * moveSpeed * Time.deltaTime);

        moveDirection.y = 0;

        rigRigidbody.MovePosition(rigRigidbody.position + moveDirection);
    }
}
