using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealMirror : Mirror {
    private bool _hasEnded;

    public override void StartFight() {
        base.StartFight();

        _hasEnded = false;
    }

    public override void OnPressStartPuzzle() {
        base.OnPressStartPuzzle();

        HealthZone.Instance.OnComplete = CompleteMirror;
        HealthZone.Instance.SpawnToArea();
    }

    protected override void Update() {
        base.Update();
        if(!_hasStarted) {
            return;
        }

        if(PlayerManager.Instance.ConditionLevel.Value == 0 && !_hasEnded) {
            _hasEnded = true;
            FightController.Instance.ForceEnd();
        }
    }


    public override void CancelPuzzle() {
        base.CancelPuzzle();
    }
}
