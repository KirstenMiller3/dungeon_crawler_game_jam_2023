using DG.Tweening;
using Milo.Tools;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FightController : Singleton<FightController> {
    [SerializeField] private Transform _root;
    [SerializeField] private Transform _centerTarget;
    [SerializeField] private AngrySoul _soul;
    [SerializeField] private Transform[] _soulSpawns;
    [SerializeField] private TextMeshProUGUI _countdown;
    
    public System.Action OnComplete;
    public System.Action OnFail;

    private int _soulCounter = 0;
    private int _soulAmount = 0;
    private float _speed = 3f;

    private AngrySoul _lastSoul;

    private void Start() {
        _root.gameObject.SetActive(false);
    }

    [ContextMenu("Start Fight")]
    public void Test() {
        StartFight();
    }

    public void StartFight(int soulsAmount = 3, float speed = 3f) {
        _soulAmount = soulsAmount;
        _soulCounter = 0;
        _speed = speed;
        _root.gameObject.SetActive(true);
        PlayerManager.Instance.PlayerTransform.GetComponent<AdvancedGridMovement>().DisableMovement(true);

        StartCoroutine(StartCount());
    }

    private IEnumerator StartCount() {
        _countdown.transform.localScale = Vector3.zero;
        _countdown.transform.DOScale(Vector3.one, 0.8f);
        _countdown.text = "3";
        yield return new WaitForSeconds(1);
        _countdown.transform.localScale = Vector3.zero;
        _countdown.transform.DOScale(Vector3.one, 0.8f);
        _countdown.text = "2";
        yield return new WaitForSeconds(1);
        _countdown.transform.localScale = Vector3.zero;
        _countdown.transform.DOScale(Vector3.one, 0.8f);
        _countdown.text = "1";
        yield return new WaitForSeconds(1);
        AudioManager.instance.Play("game_start");
        _countdown.transform.localScale = Vector3.zero;
        _countdown.transform.DOScale(Vector3.one, 0.8f);
        _countdown.text = "Protect Yourself";
        yield return new WaitForSeconds(1);
        _countdown.gameObject.SetActive(false);
        SpawnSoul();
    }

    private void OnSoulDestroyed() {
        CheckEndGame();
    }

    private void OnSoulHit() {
        if(PlayerManager.Instance.ConditionLevel.Value == 0) {
            return;
        }
        AudioManager.instance.Play("damage");
        PlayerManager.Instance.RemoveCondition(1);

        CheckEndGame();
    }

    private void SpawnSoul() {
        _soulCounter++;

        GameObject soul = Instantiate(_soul.gameObject, transform);
        _lastSoul = soul.GetComponent<AngrySoul>();
        soul.transform.position = _soulSpawns[Random.Range(0, _soulSpawns.Length)].position;
        _lastSoul.OnDestroyed = OnSoulDestroyed;
        _lastSoul.OnHitTarget = OnSoulHit;
        _lastSoul.SetUp(_centerTarget, _speed);
    }

    private void CheckEndGame() {
        if(_soulCounter >= _soulAmount) {
            StartCoroutine(EndGame());
        }
        else {
            SpawnSoul();
        }
    }

    public void ForceEnd() {
        Debug.Log("FORCE END");
        _soulCounter = 100;
        _lastSoul.Explode();
        StartCoroutine(EndGame());
    }

    private IEnumerator EndGame() {
        _countdown.transform.gameObject.SetActive(true);
        _countdown.transform.localScale = Vector3.zero;
        _countdown.transform.DOScale(Vector3.one, 0.8f);
        _countdown.text = "Reflection Weakened";
        yield return new WaitForSeconds(2f);
        _countdown.text = "Now, restore them";
        yield return new WaitForSeconds(2f);
        OnComplete?.Invoke();
        _root.gameObject.SetActive(false);
        PlayerManager.Instance.PlayerTransform.GetComponent<AdvancedGridMovement>().DisableMovement(false);
    }
}
