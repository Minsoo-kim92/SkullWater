﻿using UnityEngine;
using System.Collections;
using Vuforia;

public class Gyro : MonoBehaviour {

    // Use this for initialization
    void Start()
    {
        VuforiaBehaviour.Instance.RegisterVuforiaStartedCallback(OnVuforiaStarted);
        VuforiaBehaviour.Instance.RegisterOnPauseCallback(OnPaused); // 카메라 초점모드
        Input.gyro.enabled = true;
        Input.gyro.updateInterval = 0.01f;
    }
    private void OnVuforiaStarted()
    {
        bool focusModeSet = CameraDevice.Instance.SetFocusMode(
     CameraDevice.FocusMode.FOCUS_MODE_CONTINUOUSAUTO);

        if (!focusModeSet)
        {
            Debug.Log("Failed to set focus mode (unsupported mode).");
        }
    }

    private void OnPaused(bool paused)
    {
        if (!paused) // resumed
        {
            // Set again autofocus mode when app is resumed
            CameraDevice.Instance.SetFocusMode(
                CameraDevice.FocusMode.FOCUS_MODE_CONTINUOUSAUTO);
        }
    }

    void Update()
    {
        transform.Rotate(-Input.gyro.rotationRateUnbiased.x,
                          -Input.gyro.rotationRateUnbiased.y,
                          0);
    }
}