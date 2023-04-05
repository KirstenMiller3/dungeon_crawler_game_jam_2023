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

    public override void OnPressStartPuzzle() {
        base.OnPressStartPuzzle();
        if(IsComplete.Value) {
            return;
        }

        _repeatingWorld.Init();
        _repeatingWorld.Loops.Subscribe(OnLoopUpdate, true);
        _lastPlayerPosition = _player.position;
        _player.GetComponent<AdvancedGridMovement>().Teleport(_repeatingWorld.StartPosition, Quaternion.Euler(new Vector3(0, 90, 0)));

        AudioManager.instance.SetPatience(true);
    }

    private void OnLoopUpdate(int prev, int curr) {
        if(curr < _loopsNeeded) {
            return;
        }

        PickUpObj pickUpObj = _repeatingWorld.ShowPickUp(_endCorridorPickUp);
        pickUpObj.OnPickUp = CompleteMirror;
    }

    public override void CompleteMirror() {
        base.CompleteMirror();

        AudioManager.instance.SetPatience(false);
        _player.GetComponent<AdvancedGridMovement>().Teleport(_lastPlayerPosition, Quaternion.Euler(Vector3.zero));
        _repeatingWorld.Loops.Unsubscribe(OnLoopUpdate);
        _repeatingWorld.Complete();
    }
}
