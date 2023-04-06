using Milo.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMirrorDetection : MonoBehaviour {
    [SerializeField] private float _checkDist = 10f;
    [SerializeField] private float _checkDistToChange = 20f;
    [SerializeField] private LayerMask _layersToCheck;

    public Observable<bool> LookingAtMirror = new Observable<bool>();

    private Mirror _lookAtMirror;

    public Mirror LookAtMirror => _lookAtMirror;

    public void OnPressInteract() {
        if(_lookAtMirror != null && _lookAtMirror.IsInteractable && !_lookAtMirror.IsComplete.Value) {
            if(!LookingAtMirror.Value) {
                return;
            }
            _lookAtMirror.StartFight();
        }
    }

    private void FixedUpdate() {
        Vector3 fwd = transform.TransformDirection(Vector3.forward);
        RaycastHit hit;
        Debug.DrawRay(transform.position, fwd * _checkDist);
        if(Physics.Raycast(transform.position, fwd, out hit, _checkDist, _layersToCheck)) {
            if(hit.transform.gameObject.layer == LayerMask.NameToLayer("Level")) {
                return;
            }

            _lookAtMirror = hit.transform.GetComponent<Mirror>();
            _lookAtMirror.Interact();

            LookingAtMirror.Value = true;
        }
        else {
            if(_lookAtMirror) {
                _lookAtMirror.StopInteract();
            }

            _lookAtMirror = null;

            LookingAtMirror.Value = false;
        }

        if(Physics.Raycast(transform.position, fwd, out hit, _checkDistToChange, _layersToCheck)) {
            if(hit.transform.gameObject.layer == LayerMask.NameToLayer("Level")) {
                return;
            }

            _lookAtMirror = hit.transform.GetComponent<Mirror>();
            _lookAtMirror.ShowPerson();
        }
    }
}
