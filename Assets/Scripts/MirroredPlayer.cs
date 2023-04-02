using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public enum MirroredPerson {
    None,
    Calm = 1,
    Perfection = 2,
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

    private MirroredPerson _currentMirroredPerson;

    private void Start() {
        _player = GameObject.Find("Player").transform;
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
        _currentMirroredPerson = mirroredPerson;

        switch(mirroredPerson) 
        {
            case MirroredPerson.Calm:
                AudioManager.instance.Play("fire");
                break;
            case MirroredPerson.Health:
                AudioManager.instance.Play("health");
                break;
            case MirroredPerson.Peace:
                AudioManager.instance.Play("peace");
                break;
            case MirroredPerson.Perfection:
                AudioManager.instance.Play("perfection");
                break;
            case MirroredPerson.Presence:
                AudioManager.instance.Play("presence");
                break;
            case MirroredPerson.Restore:
                AudioManager.instance.Play("restore");
                break;
            case MirroredPerson.Time:
                AudioManager.instance.Play("time");
                break;
        }

        for(int i = 0; i < _personModel.Length; i++) {
            if(_personModel[i] != null) {
                _personModel[i].SetActive(false);
            }
        }

        Debug.Log($"Person: {(int)_currentMirroredPerson}");

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
        _light.SetTrigger("Play");
        _transformParticles.Play();
        yield return new WaitForSeconds(2f);
        ShowMirroredPerson(0);
    }
}
