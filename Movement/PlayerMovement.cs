﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    private enum MoveState
    {
        walking, running, crouching, fastCrouching 
    }

    [Header("Speed")]
    [SerializeField] private float walkSpeed = 2.5f;
    [SerializeField] private float runSpeed = 3.5f;
    [SerializeField] private float crouchSpeed = 1.5f;
    [SerializeField] private float fastCrouchSpeed = 2f;

    [Header("Generic")]
    [SerializeField, Range(0f, 10f)] private float moveStateTransitionSmoothing = 1.5f;
    [SerializeField, Range(0f, 10f)] private float crouchTransitionSmoothing = 5f;
    [SerializeField] private Transform cameraHolder = null;
    private Rigidbody playerRigidbody = null;

    [Header("Collision")]
    [SerializeField] private CapsuleCollider playerCollider = null;
    [SerializeField] private float standingHeight = 1.8f;
    [SerializeField] private float crouchHeight = 1.1f;
    [SerializeField] private float fastCrouchHeight = 1.3f;

    private MoveState currentMoveState = MoveState.walking;
    private float currentMoveSpeed = 0;
    private float targetMoveSpeed = 0;

    private float currentColliderHeight = 1.8f;
    private float targetColliderHeight = 1.8f;

    private Vector3 moveVector = Vector3.zero;
    private float cameraHeight = 0f;

    private void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        cameraHeight = (cameraHolder.position.y - transform.position.y) - standingHeight;
        currentColliderHeight = standingHeight;
        SetMoveState(MoveState.walking);
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift)){
            if (Input.GetKey(KeyCode.LeftControl))
            {
                SetMoveState(MoveState.fastCrouching);
            }
            else
            {
                SetMoveState(MoveState.running);
            }
        }else if (Input.GetKey(KeyCode.LeftControl))
        {
            SetMoveState(MoveState.crouching);
        }
        else
        {
            SetMoveState(MoveState.walking);
        }

        currentMoveSpeed = Mathf.Lerp(currentMoveSpeed, targetMoveSpeed, moveStateTransitionSmoothing * Time.deltaTime);
        currentColliderHeight = Mathf.Lerp(currentColliderHeight, targetColliderHeight, crouchTransitionSmoothing * Time.deltaTime);

        playerCollider.height = currentColliderHeight;

        cameraHolder.transform.localPosition = new Vector3(0, currentColliderHeight + cameraHeight, 0);
    }

    void FixedUpdate()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        moveVector = new Vector3(horizontal, 0, vertical);
        moveVector = moveVector.magnitude > 1 ? moveVector.normalized : moveVector;
        moveVector *= Time.fixedDeltaTime * currentMoveSpeed;
        moveVector = transform.TransformDirection(moveVector);

        playerRigidbody.MovePosition(playerRigidbody.position + moveVector);
    }

    private void SetMoveState(MoveState newMoveState)
    {
        currentMoveState = newMoveState;

        switch (currentMoveState)
        {
            case MoveState.walking:
                targetMoveSpeed = walkSpeed;
                targetColliderHeight = standingHeight;
                break;
            case MoveState.running:
                targetMoveSpeed = runSpeed;
                targetColliderHeight = standingHeight;
                break;
            case MoveState.crouching:
                targetMoveSpeed = crouchSpeed;
                targetColliderHeight = crouchHeight;
                break;
            case MoveState.fastCrouching:
                targetMoveSpeed = fastCrouchSpeed;
                targetColliderHeight = fastCrouchHeight;
                break;
            default:
                targetMoveSpeed = walkSpeed;
                targetColliderHeight = standingHeight;
                break;
        }
    }
}