using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    [SerializeField] private Renderer _renderer;
    [SerializeField] private Material[] _personMaterial = new Material[7];
    [SerializeField] private ParticleSystem _transformParticles;
    [SerializeField] private Animator _light;

    private MirroredPerson _currentMirroredPerson;

    private void Update() {
        transform.position = _player.position + _offset;
    }

    public void ShowMirroredPerson(MirroredPerson mirroredPerson) {
        _currentMirroredPerson = mirroredPerson;
        _renderer.material = _personMaterial[(int)_currentMirroredPerson];
    }

    public void TransformPlayer() {
        StartCoroutine(DoTransform());
    }

    private IEnumerator DoTransform() {
        _light.SetTrigger("Play");
        _transformParticles.Play();
        yield return new WaitForSeconds(2f);
        ShowMirroredPerson(0);
    }
}
