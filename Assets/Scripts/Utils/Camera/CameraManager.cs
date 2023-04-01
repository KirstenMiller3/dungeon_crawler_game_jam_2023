using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour {
    [SerializeField] private GameObject _gameCam;
    [SerializeField] private GameObject _battleCam;

    private float _targetFOV = 60;
    private Transform _target;

    private Camera _gameCamera;
    private Quaternion _camBaseRot;


    private void Update() {
        _gameCamera.fieldOfView = Mathf.Lerp(_gameCamera.fieldOfView, _targetFOV, Time.deltaTime * 5f);
        if(_target != null) {
            var targetRotation = Quaternion.LookRotation(_target.position - _gameCam.transform.position);
            _gameCam.transform.rotation = Quaternion.Slerp(_gameCam.transform.rotation, targetRotation, 5f * Time.deltaTime);
        }
    }

    public void SetBattleCam() {
        _battleCam.SetActive(true);
        _gameCam.SetActive(false);
        _target = null;

        _gameCam.transform.rotation = _camBaseRot;
    }

    public void SetGameCam() {
        if(_gameCamera == null) {
            _gameCamera = _gameCam.GetComponent<Camera>();
            _camBaseRot = _gameCam.transform.rotation;
        }
        _targetFOV = 60f;
        _battleCam.SetActive(false);
        _gameCam.SetActive(true);
    }

    public void Zoom(Transform target) {
        _targetFOV = 20f;
        _target = target;
    }
}
