using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Positioning")]
    [SerializeField]
    private Vector3 offset = Vector3.zero;
    [SerializeField]
    private Transform target = null;
    [SerializeField, Range(1f, 60f)]
    private float moveSmoothing = 20f;

    [Header("Look at")]
    [SerializeField]
    private bool lookAtTarget = true;
    [SerializeField]
    private Vector3 lookAtOffset = Vector3.zero;
    [SerializeField, Range(1f, 60f)]
    private float lookAtSmoothing = 20f;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = target.position + offset;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetPos = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, targetPos, moveSmoothing * Time.deltaTime);
    }

    private void FixedUpdate()
    {
        if (lookAtTarget)
        {
            Quaternion lookAtPos = Quaternion.LookRotation(target.transform.position - transform.position + lookAtOffset);
            transform.rotation = Quaternion.Lerp(transform.rotation, lookAtPos, lookAtSmoothing * Time.deltaTime);
        }
    }
}
