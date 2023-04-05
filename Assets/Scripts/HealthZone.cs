using Milo.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthZone : Singleton<HealthZone> {
    [SerializeField] private int _counter = 5;
    [SerializeField] private Transform _spawnPoint;

    private int _count;
    private AdvancedGridMovement _player;
    private Vector3 _originalPos;
    private Quaternion _originalRot;

    public System.Action OnComplete;

    public void SpawnToArea() {
        _player = PlayerManager.Instance.PlayerTransform.GetComponent<AdvancedGridMovement>();
        _originalPos = PlayerManager.Instance.PlayerTransform.position;
        _originalRot = PlayerManager.Instance.PlayerTransform.rotation;

        UIManager.Instance.Transition(() => _player.Teleport(_spawnPoint.position, _spawnPoint.rotation));
    }

    public void AddToCounter() {
        _count++;

        if(_count >= _counter) {
            OnComplete?.Invoke();

            UIManager.Instance.Transition(() => _player.Teleport(_originalPos, _originalRot));
        }
    }
}
