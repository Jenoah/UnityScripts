using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MonoBehaviour
{
    [SerializeField] private float jumpHeight = 300f;
    [SerializeField] private float jumpCooldown = 0.25f;
    [SerializeField] private float jumpDetectionHeight = .15f;
    [Space(20)]
    [SerializeField] private LayerMask jumpableLayers;
    [SerializeField] private Rigidbody playerRigidbody = null;

    private float timeBeforeNextJump = 0f;


    private void Start()
    {
        if (playerRigidbody == null) playerRigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > timeBeforeNextJump)
        {
            if (Physics.Raycast(transform.position + Vector3.up * 0.2f, Vector3.down, jumpDetectionHeight + 0.2f, jumpableLayers))
            {
                timeBeforeNextJump = Time.time + jumpCooldown;
                playerRigidbody.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);
            }
        }
    }
}
