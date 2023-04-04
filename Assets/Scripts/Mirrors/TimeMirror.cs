using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeMirror : Mirror {
    [SerializeField] private PickUpObj _endCorridorPickUp;
    [SerializeField] private int _loopsNeeded;

    private RepeatingWorld _repeatingWorld;
    private Transform _player;
    private Vector3 _lastPlayerPosition;

    protected override void Start() {
        base.Start();
        _player = GameObject.Find("Player").transform;
        _repeatingWorld = FindObjectOfType<RepeatingWorld>();
    }

    public override void Interact() {
        base.Interact();

        if(IsComplete.Value) {
            return;
        }

        _repeatingWorld.Init();
        _repeatingWorld.Loops.Subscribe(OnLoopUpdate, true);
        _lastPlayerPosition = _player.position;
        _player.position = _repeatingWorld.StartPosition;
    }

    private void OnLoopUpdate(int prev, int curr) {
        if(curr < _loopsNeeded) {
            return;
        }

        _player.position = _lastPlayerPosition;
        _repeatingWorld.Loops.Unsubscribe(OnLoopUpdate);
        _repeatingWorld.Complete();
        PickUpObj pickUpObj = _repeatingWorld.ShowPickUp(_endCorridorPickUp);
        pickUpObj.OnPickUp = CompleteMirror;
    }
}
