using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HungerMonster : MonoBehaviour {
    [SerializeField] private int _numberOfApplesNeeded = 3;
    [SerializeField] private GameObject[] _states;
    [SerializeField] private Renderer _renderer;

    public System.Action OnComplete;

    private int _currentApples;

    private void Start() {
        _states[0].SetActive(true);
        _states[1].SetActive(false);
        _states[2].SetActive(false);
    }

    public void Feed() {
        _states[0].SetActive(false);
        _states[1].SetActive(false);
        _states[2].SetActive(false);

        _currentApples++;
        if(_currentApples == 1) {
            AudioManager.instance.Play("munching");
        }
        else {
            AudioManager.instance.Play("munching_2");
        }

        _states[_currentApples].SetActive(true);

        if(_currentApples >= _numberOfApplesNeeded) {
            UIManager.Instance.ShowMoreFood(false);
            StartCoroutine(CountdownToDone());
        }
    }

    public void CheckDialogue() {
        if(_currentApples == 0) {
            UIManager.Instance.ShowMoreFood(false);
            UIManager.Instance.ShowHungry(true);
        }
        else if(_currentApples == 1) {
            UIManager.Instance.ShowMoreFood(true);
            UIManager.Instance.ShowHungry(false);
        }
        else {
            UIManager.Instance.ShowMoreFood(false);
            UIManager.Instance.ShowHungry(false);
        }
    }

    public void HideDialogue() {
        UIManager.Instance.ShowMoreFood(false);
        UIManager.Instance.ShowHungry(false);
    }

    private IEnumerator CountdownToDone() {
        PlayerManager.Instance.PlayerTransform.GetComponent<AdvancedGridMovement>().DisableMovement(true);
        yield return new WaitForSeconds(2f);
        PlayerManager.Instance.PlayerTransform.GetComponent<AdvancedGridMovement>().DisableMovement(false);
        gameObject.SetActive(false);
        OnComplete?.Invoke();
    }

    public void Reset() {
        _currentApples = 0;
        _states[0].SetActive(true);
    }
}
