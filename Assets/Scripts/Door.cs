using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour {
    [SerializeField] private GameObject _blocker;
    [SerializeField] private Animator _animator;
    [SerializeField] private GameObject[] _lights;
    [SerializeField] private Mirror[] _mirrors;

    private int _completeMirrors = 0;

    private void Start() {
        for(int i = 0; i < _mirrors.Length; i++) {
            _mirrors[i].IsComplete.Subscribe(OnMirrorIsComplete, true);
        }

        for(int i = 0; i < _lights.Length; i++) {
            _lights[i].gameObject.SetActive(false);
        }
        
        _blocker.SetActive(true);
    }

    private void OnMirrorIsComplete(bool prev, bool cur) {
        if(cur) {
            _completeMirrors++;
        }

        if(_completeMirrors == _mirrors.Length) {
            OpenDoor();
        }
    }

    private void OpenDoor() {
        for(int i = 0; i < _lights.Length; i++) {
            _lights[i].gameObject.SetActive(true);
        }

        _blocker.SetActive(false);

        _animator.SetTrigger("Open");
    }
}
