using Milo.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepeatingWorld : MonoBehaviour {
    [SerializeField] private Transform _corridorStart;
    [SerializeField] private Transform _pickUpLocation;
    [SerializeField] private Collider[] _triggers;

    public Vector3 StartPosition => _corridorStart.position;

    private bool _triggerIndex;
    
    public Observable<int> Loops = new Observable<int>();

    public void Init() {
        SetTriggers();
    }

    public PickUpObj ShowPickUp(PickUpObj pickUpObj) {
        return Instantiate(pickUpObj, _pickUpLocation.position, Quaternion.identity);
    }

    public void Complete() {
        _triggers[0].enabled = false;
        _triggers[1].enabled = false;
    }

    private void OnTriggerEnter(Collider other) {
        if(other.transform.tag != "Player") {
            return;
        }

        _triggerIndex = !_triggerIndex;

        if(!_triggerIndex) {
            Loops.Value++;
        }
    }

    private void SetTriggers() {
        _triggers[0].enabled = _triggerIndex;
        _triggers[1].enabled = !_triggerIndex;
    }
}
