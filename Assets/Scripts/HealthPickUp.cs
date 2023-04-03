using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickUp : MonoBehaviour {
    [SerializeField] private GameObject _pickUpParticles;
    [SerializeField] private Collider _triggerBox;
    [SerializeField] private GameObject _particles;
    [SerializeField] private float _respawnTime = 30f;

    private float _timer;
    private bool _isDisabled;

    private void Update() {
        if(!_isDisabled) {
            return;
        }

        _timer += Time.deltaTime;
        if(_timer >= _respawnTime) {
            _timer = 0;
            SetIsActived(true);
        }
    }

    private void OnTriggerEnter(Collider other) {
        if(other.tag != "Player") {
            return;
        }

        Instantiate(_pickUpParticles, transform.position, Quaternion.identity);
        PlayerManager.Instance.AddCondition(1);

        SetIsActived(false);
    }

    private void SetIsActived(bool isActivated) {
        _isDisabled = !isActivated;
        _triggerBox.enabled = isActivated;
        _particles.SetActive(isActivated);
    }
}
