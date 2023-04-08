using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Milo.Tools;
using UnityEngine.SceneManagement;
using System;

public class PlayerManager : Singleton<PlayerManager> {
    [SerializeField] private Transform _player;

    [SerializeField] private int _startCondition = 5;
    
    [SerializeField] private Transform _rain;

    private const int NumMirrors = 7;

    public bool GameFinished => _gameFinished;

    public int CompletedMirrors => _completedMirrors;
    public Transform PlayerTransform => _player;

    public Observable<int> ConditionLevel = new Observable<int>();

    public Mirror ActiveMirror => _activeMirror;

    private Mirror _activeMirror;

    private bool _gameFinished = false;
    private int _currConditionRemovalAmount;

    [SerializeField] private int _completedMirrors = 0;

    private Vector3 _homePos;
    private Quaternion _homeRot;

    protected override void Awake() {
        base.Awake();
        ConditionLevel.Value = _startCondition;
    }

    private void Start() {
        AudioManager.instance.SetFinal(false);
        AudioManager.instance.Play("ambience");
        AudioManager.instance.Play("soft-rain");
        _player = GameObject.Find("Player").transform;
        _homePos = _player.position;
        _homeRot = _player.rotation;
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.Alpha9)) {
            ESCAPE();
        }
    }

    //public void StartRemoveCondition(int amount) {
    //    if(_isRemovingCondition) {
    //        return;
    //    }

    //    _currConditionRemovalAmount = amount;
    //    _conditionTimer = 0f;
    //    _isRemovingCondition = true;
    //}

    //public void EndRemoveCondition() {
    //    _isRemovingCondition = false;
    //}

    public void ESCAPE() {
        PlayerTransform.GetComponent<AdvancedGridMovement>().Teleport(_homePos, _homeRot);
    }

    public void RemoveCondition(int amount) {
        ConditionLevel.Value -= amount;

        if (ConditionLevel.Value == 0) {
            if(_activeMirror.MirroredPerson != MirroredPerson.Health) {
                SceneManager.LoadScene("DeathScene");
            }
        }
    }

    public void AddCondition(int amount) {
        if(ConditionLevel.Value == 5)  {
            return;
        }

        AudioManager.instance.Play("heal");
        ConditionLevel.Value += amount;
    }

    public void SetActiveMirror(Mirror activeMirror) {
        _activeMirror = activeMirror;
    }


    [ContextMenu("Fully Heal")]
    public void FullHeal() {
        ConditionLevel.Value = 5;
    }

    public void ReduceConditionToOne() {
        ConditionLevel.Value = 1;
    }


    [ContextMenu("Complete Puzzle")]
    public void CompleteMirror() {
        SkyText.Instance.SetText("Keep going...", true);
        _completedMirrors++;

        _gameFinished = CompletedMirrors == NumMirrors;
    }

    [ContextMenu("Complete 6 Puzzles")]
    public void ForceComplete6Puzzles() {
        for(int i = 0; i < 6; i++) {
            CompleteMirror();
        }
    }

    public void EndGame() {
        SkyText.Instance.SetText("Write it on your heart that every day is the best day of the year.", false);
        StartCoroutine(EndGameRoutine());
    }

    private IEnumerator EndGameRoutine() {
        AudioManager.instance.SetFinal(true);
        PlayerTransform.GetComponentInChildren<Camera>().clearFlags = CameraClearFlags.SolidColor;
        yield return new WaitForSeconds(0.5f);
        UIManager.Instance.Transition(TeleportToEnd);
    }

    private void TeleportToEnd() {
        var rain = _player.GetComponentInChildren<ParticleSystem>();
        var rainEmission = rain.emission;
        rainEmission.rateOverTime = 500;


        var skybox = GameObject.Find("end_spawner");
        var pos = skybox.transform.position;
        PlayerTransform.GetComponent<AdvancedGridMovement>().Teleport(pos, skybox.transform.rotation);
        PlayerTransform.GetComponent<AdvancedGridMovement>().DisableMovement(false);
    }

}
