using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMirrorDetection : MonoBehaviour {
    [SerializeField] private float _checkDist = 10f;
    [SerializeField] private LayerMask _layersToCheck;

    private Mirror _lookAtMirror;

    public Mirror LookAtMirror => _lookAtMirror;

    private void FixedUpdate() {
        Vector3 fwd = transform.TransformDirection(Vector3.forward);
        RaycastHit hit;
        Debug.DrawRay(transform.position, fwd * _checkDist);
        if(Physics.Raycast(transform.position, fwd, out hit, _checkDist, _layersToCheck)) {
            _lookAtMirror = hit.transform.GetComponent<Mirror>();
            _lookAtMirror.Interact();

            PlayerManager.Instance.StartRemoveCondition(1);
        }
        else {
            _lookAtMirror = null;
            PlayerManager.Instance.EndRemoveCondition();
        }
    }
}
