using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider))]
public class CameraTrackedCollider : MonoBehaviour
{
    [SerializeField] private Transform cameraObject = null;
    [SerializeField] private float minimalHeight = 0.8f;
    [SerializeField] private float maximumHeight = 1.95f;

    private CapsuleCollider capsuleCollider = null;

    // Start is called before the first frame update
    void Start()
    {
        capsuleCollider = GetComponent<CapsuleCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        float capsuleHeight = Mathf.Clamp(cameraObject.transform.localPosition.y, minimalHeight, maximumHeight);
        Vector3 capsuleCenter = cameraObject.transform.localPosition;

        capsuleCenter.y = capsuleHeight / 2;

        capsuleCollider.center = capsuleCenter;
        capsuleCollider.height = capsuleHeight;
    }
}
