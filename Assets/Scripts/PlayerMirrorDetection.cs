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
            _lookAtMirror.OnPressStartPuzzle();
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

            //if(_lookAtMirror.IsInteractable && !_lookAtMirror.IsComplete.Value) {
            //    UIManager.Instance.ShowMirrorInteractPrompt(true);
            //}

            LookingAtMirror.Value = true;

            if (_lookAtMirror.IsComplete.Value) {
                PlayerManager.Instance.EndRemoveCondition();
            } else {
                PlayerManager.Instance.StartRemoveCondition(1);
            }
        }
        else {
            if(_lookAtMirror) {
                _lookAtMirror.StopInteract();
                //if(_lookAtMirror.IsInteractable) {
                //    UIManager.Instance.ShowMirrorInteractPrompt(false);
                //}
            }

            _lookAtMirror = null;

            LookingAtMirror.Value = false;

            if (PlayerManager.Instance != null) {
                PlayerManager.Instance.EndRemoveCondition();
            }
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
