using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Milo.Tools;

public class PlayerInteract : Singleton<PlayerInteract> {
    public IInteractable CurrentInteractable => _currentInteractable;

    private IInteractable _currentInteractable;
    public Observable<float> Timer;
    public Observable<bool> Interacting;

    public delegate void OnPlayerInteract();
    public static event OnPlayerInteract OnInteract;
    public static event OnPlayerInteract OnComplete;
    public static event OnPlayerInteract OnCancel;

    private float _timer = 0;

    protected override void Awake() {
        base.Awake();
        Timer = new Observable<float>();
        Timer.Value = 0;
        Interacting = new Observable<bool>();
        Interacting.Value = false;
    }

    private void Update() {
        if(_currentInteractable == null) {
            return;
        }

        if(!_currentInteractable.CanInteract()) {
            return;
        }

        if(Input.GetKeyDown(_currentInteractable.KeyCode) && !Interacting.Value) {
            _currentInteractable.OnInteract();
            Interacting.Value = true;
            _timer = 0;
            OnInteract?.Invoke();
        }

        if(Input.GetKey(_currentInteractable.KeyCode)) {
            _timer += Time.deltaTime;
            Timer.Value = _timer / _currentInteractable.InteractionTime;
            if(_timer >= _currentInteractable.InteractionTime) {
                //Complete!
                _timer = 0;
                Interacting.Value = false;
                _currentInteractable.OnUsed();

                OnComplete?.Invoke();
            }
        }

        if(_currentInteractable != null && Input.GetKeyUp(_currentInteractable.KeyCode)) {
            //Cancel
            _currentInteractable.OnCancel();
            Interacting.Value = false;

            OnCancel?.Invoke();
        }
    }

    public void InVicinityOfInteractable(IInteractable interactable) {
       // UIManager.Instance.ShowButtonPrompt(interactable.KeyCode.ToString(), interactable.ObjectName, !interactable.CanInteract(), interactable.UnlockItem, interactable.ItemUnlockCount);
        _currentInteractable = interactable;
    }

    public void OutOfVicinity() {
       // UIManager.Instance.HideButtonPrompt();
       _currentInteractable = null;
    }
}
