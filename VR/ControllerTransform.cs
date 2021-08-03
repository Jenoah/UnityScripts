using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class ControllerTransform : MonoBehaviour
{
    [Header("Controller settings")]
    [SerializeField] private XRNode controllerRole = XRNode.CenterEye;
    [SerializeField] private Vector3 positionalOffset = Vector3.zero;
    [SerializeField] private Quaternion rotationalOffset = Quaternion.identity;

    private InputDevice controller;

    private void Start()
    {
        controller = InputDevices.GetDeviceAtXRNode(controllerRole);
    }

    // Update is called once per frame
    void Update()
    {
        if (controller == null) return;
        SetDevicePositionAndRotation();
    }

    private void SetDevicePositionAndRotation()
    {
        controller.TryGetFeatureValue(CommonUsages.devicePosition, out Vector3 controllerPosition);
        controller.TryGetFeatureValue(CommonUsages.deviceRotation, out Quaternion controllerRotation);

        transform.localPosition = controllerPosition + positionalOffset;
        transform.localRotation = controllerRotation * rotationalOffset;
    }

    public Vector3 GetVelocity()
    {
        controller.TryGetFeatureValue(CommonUsages.deviceVelocity, out Vector3 controllerVelocity);

        return controllerVelocity;
    }

    public Vector3 GetAngularVelocity()
    {
        controller.TryGetFeatureValue(CommonUsages.deviceAngularVelocity, out Vector3 controllerAngularVelocity);

        return controllerAngularVelocity;
    }
}
