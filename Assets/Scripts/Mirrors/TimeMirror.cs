using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeMirror : Mirror {
    [SerializeField] private int _checkCount = 6;

    public override void Interact() {
        base.Interact();

        if(IsComplete.Value) {
            return;
        }

        if(PlayerManager.Instance.CompletedMirrors == _checkCount) {
            CompleteMirror();
        } 
    }
}
