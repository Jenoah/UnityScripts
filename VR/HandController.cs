using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;


public class HandController : MonoBehaviour
{
    [Header("Controller settings")]
    [SerializeField] private XRNode controllerRole = XRNode.CenterEye;

    [Header("Hand settings")]
    [SerializeField] private HandAnimation handAnimation = null;

    private float triggerValue = 0f;
    private float gripValue = 0f;
    private Vector2 thumbstickValue = Vector2.zero;

    private InputDevice controller;

    private void Start()
    {
        controller = InputDevices.GetDeviceAtXRNode(controllerRole);
    }

    // Update is called once per frame
    void Update()
    {
        if (controller == null) return;

        GetControllerInput();

        handAnimation.SetTriggerValue(triggerValue);
        handAnimation.SetGripValue(gripValue);
    }

    private void GetControllerInput()
    {
        controller.TryGetFeatureValue(CommonUsages.trigger, out triggerValue);
        controller.TryGetFeatureValue(CommonUsages.grip, out gripValue);
        controller.TryGetFeatureValue(CommonUsages.primary2DAxis, out thumbstickValue);
    }
}
