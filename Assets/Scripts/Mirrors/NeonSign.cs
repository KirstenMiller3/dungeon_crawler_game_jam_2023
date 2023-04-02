using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeonSign : MonoBehaviour {
    [SerializeField] private Mirror _mirror;
    [SerializeField] private Animator _animator;

    private void Start() {
        _mirror.IsComplete.Subscribe(OnComplete, true);
    }

    private void OnDisable() {
        _mirror.IsComplete.Unsubscribe(OnComplete); 
    }

    private void OnComplete(bool prev, bool cur) {
        if(cur) {
            _animator.SetTrigger("Activate");
        }
    }
}
