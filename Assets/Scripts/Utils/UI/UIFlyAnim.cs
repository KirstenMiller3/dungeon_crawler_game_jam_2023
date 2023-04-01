using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFlyAnim : MonoBehaviour {
    [SerializeField] private Transform _target;
    [SerializeField] private float _speed = 6f;

    private Vector3 _initPos;
    private float _waitTime = 1f;
    private float _waitTimer = 0;

    private void Start() {
        _initPos = transform.position;
    }

    void Update(){
        _waitTimer += Time.deltaTime;
        if(_waitTimer < _waitTime) {
            return;
        }

        transform.position = Vector3.Lerp(transform.position, _target.position, Time.deltaTime * _speed);
    }

    public void MoveToTarget() {
        _waitTimer = 0f;
        transform.position = _initPos;
    }
}
