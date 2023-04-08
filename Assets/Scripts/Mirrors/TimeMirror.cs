using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeMirror : Mirror {
    [SerializeField] private PickUpObj _endCorridorPickUp;
    [SerializeField] private int _loopsNeeded;

    private RepeatingWorld _repeatingWorld;
    private Transform _player;
    private Vector3 _lastPlayerPosition;
    private Quaternion _lastPlayerRotation;

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
        _lastPlayerRotation = _player.rotation;
        UIManager.Instance.Transition(() => _player.GetComponent<AdvancedGridMovement>().Teleport(_repeatingWorld.StartPosition, Quaternion.Euler(new Vector3(0, 90, 0))));

        AudioManager.instance.SetPatience(true);
    }

    private PickUpObj pickUpObj;
    private void OnLoopUpdate(int prev, int curr) {
        if(curr < _loopsNeeded) {
            return;
        }

        pickUpObj = _repeatingWorld.ShowPickUp(_endCorridorPickUp);
        pickUpObj.OnPickUp = CompletedThisGuy;
    }

    private bool _doubleCheck = false;
    private void CompletedThisGuy() {
        if(_doubleCheck) {
            return;
        }

        _doubleCheck = true;

        CompleteMirror();

        pickUpObj.OnPickUp = null;
        AudioManager.instance.SetPatience(false);
        UIManager.Instance.Transition(() => _player.GetComponent<AdvancedGridMovement>().Teleport(_lastPlayerPosition, _lastPlayerRotation));
        _repeatingWorld.Loops.Unsubscribe(OnLoopUpdate);
        _repeatingWorld.Complete();
    }

    public override void CompleteMirror() {
        base.CompleteMirror();
    }
}
