using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PickUpDetection : MonoBehaviour {
    [SerializeField] private float _checkDist = 10f;
    [SerializeField] private LayerMask _layersToCheck;
    [SerializeField] private Transform _heldObjPos;
    [SerializeField] protected PlayerMirrorDetection _mirrorDetection;

    private PickUpObj _heldObj;
    private PickUpObj _lookAtObj;
    private GameObject _heldInstance;

    private void FixedUpdate() {
        Vector3 fwd = transform.TransformDirection(Vector3.forward);
        RaycastHit hit;
        Debug.DrawRay(transform.position, fwd * _checkDist);
        if(Physics.Raycast(transform.position, fwd, out hit, _checkDist, _layersToCheck)) {
            _lookAtObj = hit.transform.GetComponent<PickUpObj>();
        }
        else {
            _lookAtObj = null;
        }
    }

    public void PickUp() {
        if(_lookAtObj == null) {
            return;
        }

        if(_heldInstance != null) {
            return;
        }

        _lookAtObj.gameObject.SetActive(false);
        _heldObj = _lookAtObj;
        _heldInstance = Instantiate(_heldObj.Prefab, _heldObjPos);
    }

    public void PresentObj() {
        if(_mirrorDetection.LookAtMirror != null) {
            if(_mirrorDetection.LookAtMirror is PresentMirror) {
                PresentMirror mirror = (PresentMirror)_mirrorDetection.LookAtMirror;
                if(mirror.Solution == _heldObj.PickUpType) {
                    Destroy(_heldInstance);
                    _heldInstance = null;
                    mirror.CompleteMirror();
                }
            }
        }
    }
}
