using Milo.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PickUpDetection : MonoBehaviour {
    [SerializeField] private float _checkDist = 10f;
    [SerializeField] private LayerMask _layersToCheck;
    [SerializeField] private Transform _heldObjPos;
    [SerializeField] protected PlayerMirrorDetection _mirrorDetection;

    public Observable<bool> LookingAtObj = new Observable<bool>();
    public Observable<string> LookAtObjName = new Observable<string>();

    public bool HasHeldObj => _heldObj;

    private PickUpObj _heldObj;
    private PickUpObj _lookAtObj;
    private GameObject _heldInstance;

    private void FixedUpdate() {
        Vector3 fwd = transform.TransformDirection(Vector3.forward);
        RaycastHit hit;
        Debug.DrawRay(transform.position, fwd * _checkDist);
        if(Physics.Raycast(transform.position, fwd, out hit, _checkDist, _layersToCheck)) {
            _lookAtObj = hit.transform.GetComponent<PickUpObj>();
            LookingAtObj.Value = true;
            LookAtObjName.Value = _lookAtObj.PickUpType.ToString();
        }
        else {
            _lookAtObj = null;
            LookingAtObj.Value = false;
            LookAtObjName.Value = string.Empty;
        }
    }

    public void PickUp() {
        if(_lookAtObj == null) {
            return;
        }

        if(_heldInstance != null) {
            return;
        }

        _lookAtObj.PickUp();
        _lookAtObj.gameObject.SetActive(false);

        if(_lookAtObj.DontAddToHand) {
            return;
        }

        _heldObj = _lookAtObj;
        _heldInstance = Instantiate(_heldObj.Prefab, _heldObjPos);
    }

    public void DropObj() {
        if(_heldObj == null) {
            return;
        }

        _heldObj.gameObject.SetActive(true);
        Destroy(_heldInstance);

        _heldObj = null;
        _heldInstance = null;
    }

    public void PresentObj() {
        if(_heldObj == null) {
            return;
        }

        if(_mirrorDetection.LookAtMirror != null) {
            if(_mirrorDetection.LookAtMirror is PresentMirror) {
                PresentMirror mirror = (PresentMirror)_mirrorDetection.LookAtMirror;
                if(!_mirrorDetection.LookingAtMirror.Value) {
                    return;
                }
                if(mirror.Solution == _heldObj.PickUpType) {
                    Destroy(_heldInstance);
                    _heldObj = null;
                    _heldInstance = null;
                    mirror.CompleteMirror();
                }
            }
        }
    }
}
