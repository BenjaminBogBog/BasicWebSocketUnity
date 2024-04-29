using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerController : MonoBehaviour
{
    public bool initializeInput = true;

    public PlayerInput Input { get; private set; }

    private Vector3 _targetMove;

    [Header("Move Settings")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float stopDistance;

    [Header("Camera Settings")]
    [SerializeField] private CinemachineFreeLook freeLookCam;
    [SerializeField] private Vector2 rotateSpeed;
    void Awake()
    {
        if (initializeInput)
        {
            Input = gameObject.AddComponent<PlayerInput>();

            Input.OnTap += OnPlayerClick;
        }

        _targetMove = transform.position;
    }

    private void OnPlayerClick()
    {
        Debug.Log("Click");
        RaycastHit[] hits = CameraRaycaster.Instance.CastRay();

        if (hits == null) return;

        foreach (RaycastHit hit in hits)
        {
            if (hit.collider == null) continue;

            _targetMove = hit.point;
        }
    }

    private void MoveTowardsTarget()
    {
        if (Vector3.Distance(_targetMove, transform.position) < stopDistance) return;

        Vector3 moveDirection = _targetMove - transform.position;
        transform.position += moveDirection.normalized * moveSpeed * Time.deltaTime;
    }
    private void RotateCamera()
    {
        if (!Input.holding) return;
        freeLookCam.m_XAxis.Value += Input.touchDelta.normalized.x * rotateSpeed.x * Time.deltaTime;
        freeLookCam.m_YAxis.Value += Input.touchDelta.normalized.y * -rotateSpeed.y * Time.deltaTime;
    }

    void Update()
    {
        MoveTowardsTarget();
        RotateCamera();
    }
}
