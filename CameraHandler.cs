using UnityEngine;
using System.Collections;

public class CameraHandler : MonoBehaviour
{
    [SerializeField]
    private Transform player = null;

    [Header("Camera Settings")]
    [SerializeField]
    private float cameraSmoothFactor = 0.125f;
    [SerializeField]
    private Vector3 cameraLookatOffset = new Vector3(0, 0.5f, 0);
    [SerializeField]
    private float mouseSensitivity = 10f;
    [SerializeField]
    private Vector3 cameraOffset = Vector3.zero;
    private Vector3 defaultOffset = Vector3.zero;

    private Vector3 movementVector = Vector3.zero;
    private bool isSmoothing = false;


    // Use this for initialization
    void Start()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }

        defaultOffset = cameraOffset;

    }

    // Update is called once per frame
    private void Update()
    {

        movementVector = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
    }

    void LateUpdate()
    {
        Quaternion camAngle;
        if (Input.GetMouseButton(1) || movementVector == Vector3.zero)
        {
            camAngle = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * mouseSensitivity, Vector3.up);
            cameraOffset = camAngle * cameraOffset;
        }
        else if(movementVector != Vector3.zero)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                StartCoroutine(SmoothRotatePlayer());
                //SetPlayerUpright();
                //SnapToPlayerForward();
            }
            else
            {
                SnapBack();
            }
        }

        gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, player.position + cameraOffset, cameraSmoothFactor);


        gameObject.transform.LookAt(player.position + cameraLookatOffset);
    }

    void SnapBack()
    {
        if (!isSmoothing)
        {
            cameraOffset = player.transform.TransformDirection(defaultOffset);
        }
    }

    void SnapToPlayerForward()
    {
        player.transform.rotation = Quaternion.LookRotation(transform.forward, Vector3.up);
    }

    IEnumerator SmoothRotatePlayer()
    {
        isSmoothing = true;
        float elapsedTime = 0f;
        float smoothTime = 0.3f;
        while(elapsedTime < smoothTime)
        {
            elapsedTime += Time.deltaTime;
            player.transform.rotation = Quaternion.Lerp(player.transform.rotation, Quaternion.LookRotation(transform.forward, Vector3.up), elapsedTime / smoothTime);
            SetPlayerUpright();
            yield return new WaitForEndOfFrame();
        }
        isSmoothing = false;
    }

    void SetPlayerUpright(){
        player.transform.eulerAngles = new Vector3(0f, player.transform.eulerAngles.y, 0f);    }
}
