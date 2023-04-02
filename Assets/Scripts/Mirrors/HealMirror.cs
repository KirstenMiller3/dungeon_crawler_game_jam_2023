using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealMirror : Mirror {
    [SerializeField] private int _conditionCheck = 101;

    public override void Interact() {
        base.Interact();

        if(IsComplete.Value) {
            return;
        }

        if(PlayerManager.Instance.ConditionLevel.Value > _conditionCheck) {
            CompleteMirror();
        }
    }
}
