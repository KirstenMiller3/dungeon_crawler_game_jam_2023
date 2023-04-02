using Milo.Tools;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : Singleton<CameraController> {
    [SerializeField] private float _tilt;
    [SerializeField] private float _tiltSpeed = 2f;

    private Vector3 _targetRot;
    private Vector3 _defaultRot;

    public Observable<bool> IsLookingUp = new Observable<bool>();

    private void Start() {
        _defaultRot = transform.localRotation.eulerAngles;
        _targetRot = new Vector3(_defaultRot.x, transform.localRotation.eulerAngles.y, transform.localRotation.eulerAngles.z);
    }

    public void Update() {
        transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(_targetRot), Time.deltaTime * _tiltSpeed);
    }

    public void StartLookUp() {
        _targetRot = new Vector3(_tilt, transform.localRotation.eulerAngles.y, transform.localRotation.eulerAngles.z);
        IsLookingUp.Value = true;
    }

    public void EndLookUp() {
        _targetRot = new Vector3(_defaultRot.x, transform.localRotation.eulerAngles.y, transform.localRotation.eulerAngles.z);
        IsLookingUp.Value = false;
    }
}
