using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PickUpType {
    Bucket,
    Apple,
    Beating_Heart,
    Pebble,
    Acceptance
}

public class PickUpObj : MonoBehaviour {
    [SerializeField] private PickUpType _type;
    [SerializeField] private GameObject _prefab;
    [SerializeField] private GameObject _poof;
    [SerializeField] protected bool _dontAddToHand;

    public bool DontAddToHand => _dontAddToHand;

    public GameObject Prefab => _prefab;
    public PickUpType PickUpType => _type;

    public System.Action OnPickUp;

    protected virtual void Start() {
    }

    public void PickUp() {
        AudioManager.instance.Play("pickup");
        Instantiate(_poof, transform.position, Quaternion.identity);
        OnPickUp?.Invoke();
    }

}
