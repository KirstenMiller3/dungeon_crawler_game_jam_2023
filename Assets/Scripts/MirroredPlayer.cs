using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirroredPlayer : MonoBehaviour {
    [SerializeField] private Transform _player;
    [SerializeField] private Vector3 _offset;

    private void Update() {
        transform.position = _player.position + _offset;
    }
}
