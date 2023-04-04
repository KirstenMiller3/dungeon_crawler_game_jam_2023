using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class Water : MonoBehaviour {
    [SerializeField] private Material _mat;

    private float _offset = 1;

    private void Update() {
        _mat.mainTextureOffset = new Vector2(_offset += Time.deltaTime * 0.2f, 1);

        if(_offset > 10) {
            _offset = 1;
        }
    }
}
