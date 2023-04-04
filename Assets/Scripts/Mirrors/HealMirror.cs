using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealMirror : Mirror {
    [SerializeField] private int _conditionCheck = 101;

    public override void OnPressStartPuzzle() {
        base.OnPressStartPuzzle();

        if(PlayerManager.Instance.ConditionLevel.Value >= _conditionCheck) {
            CompleteMirror();
        }
    }

    public override void CancelPuzzle() {
        base.CancelPuzzle();
    }
}
