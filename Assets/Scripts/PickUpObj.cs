using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PickUpType {
    Bucket
}

public class PickUpObj : MonoBehaviour {
    [SerializeField] private PickUpType _type;
    [SerializeField] private GameObject _prefab;

    public GameObject Prefab => _prefab;
    public PickUpType PickUpType => _type;

}
