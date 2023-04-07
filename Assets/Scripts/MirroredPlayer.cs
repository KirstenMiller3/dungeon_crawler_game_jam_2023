using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum MirroredPerson {
    None,
    Calm = 1,
    Acceptance = 2,
    Presence = 3,
    Peace = 4,
    Health = 5,
    Restore = 6,
    Time = 7
}

public class MirroredPlayer : MonoBehaviour {
    [SerializeField] private Transform _player;
    [SerializeField] private Vector3 _offset;
    [SerializeField] private GameObject[] _personModel = new GameObject[8];
    [SerializeField] private ParticleSystem _transformParticles;
    [SerializeField] private Animator _light;

    public System.Action OnCompleteTransform;
    public bool IsTransforming => _isTransforming;

    public Vector3 Offset => _offset;

    private MirroredPerson _currentMirroredPerson;
    private bool _isTransforming;

    private void Start() {
        _player = GameObject.Find("Player").transform;
        _currentMirroredPerson = MirroredPerson.Health;
        ShowMirroredPerson(0);
    }

    private void Update() {
        transform.position = _player.position + _offset;
        transform.rotation = _player.rotation;
    }

    public void ShowMirroredPerson(MirroredPerson mirroredPerson) {
        if (_currentMirroredPerson == mirroredPerson) {
            return;
        }

        if(_isTransforming) {
            return;
        }

        _currentMirroredPerson = mirroredPerson;
        for(int i = 0; i < _personModel.Length; i++) {
            if(_personModel[i] != null) {
                _personModel[i].SetActive(false);
            }
        }

        if(_personModel[(int)_currentMirroredPerson] == null) {
            return;
        }

        _personModel[(int)_currentMirroredPerson].SetActive(true);
    }

    public void TransformPlayer() {
        StopCoroutine(DoTransform());
        StartCoroutine(DoTransform());
    }

    private IEnumerator DoTransform() {
        _isTransforming = true;
        _light.SetTrigger("Play");
        _transformParticles.Play();
        yield return new WaitForSeconds(2f);
        _isTransforming = false;
        ShowMirroredPerson(0);
        OnCompleteTransform?.Invoke();
    }
}
