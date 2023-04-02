using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMirrorDetection : MonoBehaviour {
    [SerializeField] private float _checkDist = 10f;
    [SerializeField] private LayerMask _layersToCheck;


    private void FixedUpdate() {
        Vector3 fwd = transform.TransformDirection(Vector3.forward);
        RaycastHit hit;
        Debug.DrawRay(transform.position, fwd * _checkDist);
        if(Physics.Raycast(transform.position, fwd, out hit, _checkDist, _layersToCheck)) {
            Mirror mirror = hit.transform.GetComponent<Mirror>();
            mirror.Interact();

            PlayerManager.Instance.StartRemoveCondition(1);
        }
        else {
            PlayerManager.Instance.EndRemoveCondition();
        }
    }
}
