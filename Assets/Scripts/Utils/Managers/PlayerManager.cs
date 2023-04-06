using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Milo.Tools;
using UnityEngine.SceneManagement;

public class PlayerManager : Singleton<PlayerManager> {
    [SerializeField] private Transform _player;

    [SerializeField] private int _startCondition = 5;
    [SerializeField] private float _conditionReductionTickRate = 3f;
    
    [SerializeField] private Transform _rain;

    private const int NumMirrors = 7;

    private bool first = true;


    public bool GameFinished = true;

    public int CompletedMirrors => _completedMirrors;
    public Transform PlayerTransform => _player;

    public Observable<int> ConditionLevel = new Observable<int>();

    public Mirror ActiveMirror => _activeMirror;

    private Mirror _activeMirror;

    private bool _isRemovingCondition = false;
    private float _conditionTimer = 0f;
    private int _currConditionRemovalAmount;

    private int _completedMirrors = 0;

    protected override void Awake() {
        base.Awake();
        ConditionLevel.Value = _startCondition;
    }

    private void Start() {
        _player = GameObject.Find("Player").transform;
    }

    private void Update() {
        if (GameFinished || CompletedMirrors == NumMirrors && first) {
            GameFinished = false;
            first = false;
            Debug.Log("Game Finished");
            var skybox = GameObject.Find("end_spawner");
            var pos = skybox.transform.position;
            PlayerTransform.GetComponent<AdvancedGridMovement>().Teleport(pos, skybox.transform.rotation);
            var rain = _player.GetComponentInChildren<ParticleSystem>();
        }

        if(_isRemovingCondition) {
            _conditionTimer += Time.deltaTime;
            // condition still removes even once mirror completed
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
        if (!AudioManager.instance.IsSoundPlaying("debuff")) {
            AudioManager.instance.Play("debuff");
        }

        ConditionLevel.Value -= amount;

        if (ConditionLevel.Value == 0) {
            if(_activeMirror.MirroredPerson != MirroredPerson.Health) {
                SceneManager.LoadScene("DeathScene");
            }
        }
    }

    public void AddCondition(int amount) {
        if(ConditionLevel.Value == 5)
        {
            return;
        }
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
    }

 
}
