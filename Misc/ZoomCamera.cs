using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomCamera : MonoBehaviour
{
    [SerializeField, Range(0, 179)] private float zoomFOVTarget = 40f;
    [SerializeField] private Camera zoomCamera = null;
    [SerializeField] private float zoomTime = 0.3f;

    private float startFOV = 60f;

    private bool isZoomed = false;
    // Start is called before the first frame update
    void Start()
    {
        if(zoomCamera == null) zoomCamera = Camera.main;
        startFOV = zoomCamera.fieldOfView;
    }

    // Update is called once per frame
    void Update()
    {
        if (UIManager.isPaused) return;
        HandleInput();
    }

    void HandleInput()
    {
        if (Input.GetMouseButtonDown(1))
        {
            ZoomIn();
        }
        else if (Input.GetMouseButtonUp(1))
        {
            ZoomOut();
        }

        if(isZoomed && !Input.GetMouseButton(1)) ZoomOut();
    }

    private void ZoomIn()
    {
        StopAllCoroutines();
        StartCoroutine(SetFOV(zoomFOVTarget));
        isZoomed = true;
    }

    private void ZoomOut()
    {
        StopAllCoroutines();
        StartCoroutine(SetFOV(startFOV));
        isZoomed = false;
    }

    IEnumerator SetFOV(float targetFOV)
    {
        float currentFOV = zoomCamera.fieldOfView;
        float elapsedTime = 0f;

        while(elapsedTime < zoomTime)
        {
            elapsedTime += Time.deltaTime;
            zoomCamera.fieldOfView = Mathf.SmoothStep(currentFOV, targetFOV, elapsedTime / zoomTime);
            yield return new WaitForEndOfFrame();
        }

        zoomCamera.fieldOfView = targetFOV;
    }
}
