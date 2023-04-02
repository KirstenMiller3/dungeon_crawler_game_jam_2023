using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Milo.Tools;

public class PlayerManager : Singleton<PlayerManager> {
    [SerializeField] private Transform _player;

    [SerializeField] private int _startCondition = 100;
    [SerializeField] private float _conditionReductionTickRate = 1f;

    public int CompletedMirrors => _completedMirrors;
    public Transform PlayerTransform => _player;

    public Observable<int> ConditionLevel = new Observable<int>();

    private bool _isRemovingCondition = false;
    private float _conditionTimer = 0f;
    private int _currConditionRemovalAmount;

    private int _completedMirrors = 0;

    protected override void Awake() {
        base.Awake();
        ConditionLevel.Value = _startCondition;
    }

    private void Update() {
        if(_isRemovingCondition) {
            _conditionTimer += Time.deltaTime;

            if(_conditionTimer >= _conditionReductionTickRate) {
                RemoveCondition(_currConditionRemovalAmount);
                _conditionTimer = 0f;
            }
        }
    }

    public void StartRemoveCondition(int amount) {
        if(_isRemovingCondition) {
            return;
        }

        _currConditionRemovalAmount = amount;
        _conditionTimer = 0f;
        _isRemovingCondition = true;
    }

    public void EndRemoveCondition() {
        _isRemovingCondition = false;
    }

    public void RemoveCondition(int amount) {
        ConditionLevel.Value -= amount;
    }

    public void AddCondition(int amount) {
        ConditionLevel.Value += amount;
    }

    [ContextMenu("Complete Puzzle")]
    public void CompleteMirror() {
        _completedMirrors++;
    }

    public virtual void TeleportPlayer(Vector3 newPos) {
        _player.position = newPos;
        Debug.Log($"Telporting Player to {newPos}: is at {_player.position}");
    }
}
