using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StareMirror : Mirror {
    [SerializeField] private float _stareTime = 5f;

    private float _timer;


    public override void Interact() {
        base.Interact();

        if(IsComplete.Value) {
            return;
        }

        _timer += Time.deltaTime;

        if(_timer >= _stareTime) {
            CompleteMirror();
        }
    }

    public override void StopInteract() {
        base.StopInteract();

        _timer = 0f;
    }
}
