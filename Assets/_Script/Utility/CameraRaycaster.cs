using BogBog.Utility;
using System;
using UnityEngine;

public class CameraRaycaster : Singleton<CameraRaycaster>
{
    [SerializeField] private LayerMask raycastMask;

    [SerializeField] private Camera cam;

    private void Start()
    {
        cam = GetComponent<Camera>();
    }

    public RaycastHit[] CastRay()
    {
        if (cam == null)
        {
            Debug.LogWarning("Camera is null.");
            return null;
        }

        Ray camRay = cam.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(camRay.origin, camRay.direction * 1000f, Color.red);

        RaycastHit[] hits = Physics.RaycastAll(camRay, 1000f, raycastMask);

        if (hits == null)
        {
            Debug.LogWarning("Failed to return hits");
            return null;
        }
        else
        {
            return hits;
        }
    }
}