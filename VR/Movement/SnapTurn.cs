using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class SnapTurn : MonoBehaviour
{
    [SerializeField] private XRNode controllerListener = XRNode.LeftHand;
    [SerializeField] private float turnAmount = 45f;
    [SerializeField] private float cooldown = 0.5f;
    [SerializeField] private bool enableTurnAround = true;

    private float nextRotateTime = 0f;

    private InputDevice controller;

    private void Start()
    {
        controller = InputDevices.GetDeviceAtXRNode(controllerListener);
    }

    void Update()
    {
        if (controller == null) return;
        Turn();
    }

    private void Turn()
    {
        if (Time.time > nextRotateTime)
        {
            controller.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 controllerThumbpad);
            int horizontalAxis = Mathf.RoundToInt(controllerThumbpad.x);
            int verticalAxis = Mathf.RoundToInt(controllerThumbpad.y);

            if(horizontalAxis != 0)
            {
                transform.localEulerAngles += new Vector3(0, turnAmount * horizontalAxis, 0);
                nextRotateTime = Time.time + cooldown;
            }else if(enableTurnAround && verticalAxis != 0)
            {
                transform.localEulerAngles += new Vector3(0, 180, 0);
                nextRotateTime = Time.time + cooldown;
            }
        }
    }
}
