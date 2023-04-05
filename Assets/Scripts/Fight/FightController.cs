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
    private bool _isRunning = false;

    private void Start() {
        _root.gameObject.SetActive(false);
    }

    public void Update() {
        if(!_isRunning) {
            return;
        }
    }

    [ContextMenu("Start Fight")]
    public void Test() {
        StartFight();
    }

    public void StartFight(int soulsAmount = 3) {
        _soulAmount = soulsAmount;
        _soulCounter = 0;
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
        PlayerManager.Instance.RemoveCondition(1);

        CheckEndGame();
    }

    private void SpawnSoul() {
        _soulCounter++;

        GameObject soul = Instantiate(_soul.gameObject, transform);
        soul.transform.position = _soulSpawns[Random.Range(0, _soulSpawns.Length)].position;
        soul.GetComponent<AngrySoul>().OnDestroyed = OnSoulDestroyed;
        soul.GetComponent<AngrySoul>().OnHitTarget = OnSoulHit;
        soul.GetComponent<AngrySoul>().SetCenterTarget(_centerTarget);
    }

    private void CheckEndGame() {
        if(_soulCounter >= _soulAmount) {
            StartCoroutine(EndGame());
        }
        else {
            SpawnSoul();
        }
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
