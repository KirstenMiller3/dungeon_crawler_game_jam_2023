using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuplicateWorld : MonoBehaviour {
    [SerializeField] private GameObject _environment;
    [SerializeField] private Vector3 _offset = new Vector3(0, 0, 90);

    public void Start() {
        GameObject mirrorWorld = Instantiate(_environment);
        mirrorWorld.transform.position = _offset;
    }
}
