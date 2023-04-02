using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvoidMirror : Mirror {
    [SerializeField] private float _avoidTime = 5f;

    private float _timer;

    private bool _hasBeenLookedAt = false;
    private bool _startTimer = false;

    private void Update() {
        if(IsComplete.Value) {
            return;
        }

        if(_startTimer && _hasBeenLookedAt) {
            _timer += Time.deltaTime;

            if(_timer >= _avoidTime) {
                CompleteMirror();
                _timer = 0f;
            } 
        }
    }

    public override void Interact() {
        base.Interact();

        if(IsComplete.Value) {
            return;
        }

        _hasBeenLookedAt = true;
        _startTimer = false;
        _timer = 0f;
    }

    public override void StopInteract() {
        base.StopInteract();
        _startTimer = true;
    }
}
