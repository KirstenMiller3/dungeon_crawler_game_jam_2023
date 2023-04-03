using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PickUpType {
    Bucket,
    Apple,
    Beating_Heart
}

public class PickUpObj : MonoBehaviour {
    [SerializeField] private PickUpType _type;
    [SerializeField] private GameObject _prefab;
    [SerializeField] private GameObject _poof;

    public GameObject Prefab => _prefab;
    public PickUpType PickUpType => _type;

    public void PickUp() {
        if(_type == PickUpType.Beating_Heart) {
            AudioManager.instance.LockHeartPuzzle();
            AudioManager.instance.SetLowPassOn(false);
        }

        AudioManager.instance.Play("pickup");
        Instantiate(_poof, transform.position, Quaternion.identity);
    }

}
