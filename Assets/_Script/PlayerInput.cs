using BogBog.Utility;
using System;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public Action OnDown;
    public Action OnTap;
    public Action OnRelease;

    private Vector2 _previousTouchPosition;
    public bool holding;

    private float _elapsedHoldTime;

    public Vector2 touchDelta;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            OnDown?.Invoke();

            _previousTouchPosition = new Vector2(Input.mousePosition.x.Remap(0, Screen.width, 0, 1080), Input.mousePosition.y.Remap(0, Screen.height, 0, 1080));
            holding = true;
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (_elapsedHoldTime < 0.15f) 
            {
                OnTap?.Invoke();
            }

            OnRelease?.Invoke();
            holding = false;
            _elapsedHoldTime = 0;
        }

        if (!holding)
        {
            touchDelta = Vector2.zero;
        }
        else
        {
            _elapsedHoldTime += Time.deltaTime;

            touchDelta = new Vector2(Input.mousePosition.x.Remap(0, Screen.width, 0, 1080), Input.mousePosition.y.Remap(0, Screen.height, 0, 1080)) - _previousTouchPosition;

            _previousTouchPosition = new Vector2(Input.mousePosition.x.Remap(0, Screen.width, 0, 1080), Input.mousePosition.y.Remap(0, Screen.height, 0, 1080));
        }
    }
}