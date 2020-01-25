using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Positioning")]
    [SerializeField]
    private Vector3 offset = Vector3.zero;
    [SerializeField]
    private Transform target = null;
    [SerializeField, Range(0.005f, 1f)]
    private float smoothing = 0.125f;

    [Header("Look at")]
    [SerializeField]
    private bool lookAtTarget = true;
    [SerializeField]
    private Vector3 lookAtOffset = Vector3.zero;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = target.position + offset;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetPos = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, targetPos, smoothing);
        if (lookAtTarget)
        {
            transform.LookAt(target.position + lookAtOffset);
        }
    }
}
