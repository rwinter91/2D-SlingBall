using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour {

    public float camZoomPercent = 25.0f;
    public float zoomSensitivity = 20.0f;
    public bool isCameraLocked = false;

    public static CameraController instance = null;

    private float zoomMax = 10.0f;
    private float zoomMin = 2.0f;
    //private bool isZooming = false;
    //private Camera mainCamera = null;
    private CinemachineVirtualCamera mainCamera = null;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        mainCamera = GetComponent<CinemachineVirtualCamera>();
        if (mainCamera == null)
            Debug.LogError("Missing Camera for Controller!");

        QuickZoom(camZoomPercent);
    }

    public void QuickZoom (float zoomPercent)
    {
        //isZooming = true;

        camZoomPercent = Mathf.Clamp(zoomPercent, 0.0f, 100.0f);
        mainCamera.m_Lens.OrthographicSize = ((camZoomPercent / 100.0f) * (zoomMax - zoomMin)) + zoomMin;

        //isZooming = false;
    }

    // TODO: Add in smoothing for more polished zoom in/out
    private void UpdateScroll(float scrollAmount)
    {
        scrollAmount = -(zoomSensitivity * scrollAmount);
        camZoomPercent += scrollAmount;
        QuickZoom(camZoomPercent);
    }
	
	void Update ()
    {
        if (Input.GetAxis("Mouse ScrollWheel") != 0.0f)
        {
            UpdateScroll (Input.GetAxis("Mouse ScrollWheel"));
        }
	}
}
