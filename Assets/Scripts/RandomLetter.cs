using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class RandomLetter : Letter
{
    [Header("Animation: Apperar")]
    [SerializeField] private Color _targetColor;
    [SerializeField] private float _apperarScaleDur = 0.7f;
    [SerializeField] private float _apperarPunchForce = 1.1f;
    [SerializeField] private float _apperarPunchDuration = 1f;
    [SerializeField] private int _apperarPunchVibrato = 10;
    [SerializeField] private float _apperarPunchElastic = 1f;

    public bool Hid { get; private set; }

    void Awake()
    {
        Hid = false;
        ShowRandomLetter(GameManager.Instance.TargetLetter);
        AnimateHide();
    }

    public void ShowRandomLetter(char butNot)
    {
        // select random letter
        char randomLetter = butNot;
        int tries = 0;
        while (randomLetter == butNot)
        {
            randomLetter = GameManager.GetRandomLetter();
            tries++;
            if (tries > 1000)
            {
                Debug.LogWarning("Error trying find random letter. Maybe alphavet is empty.");
            }
        }

        // if 50% random case
        if (Random.Range(0, 2) == 0)
        {
            _displayText.text = randomLetter.ToString().ToUpper();
        }
        else
        {
            _displayText.text = randomLetter.ToString().ToLower();
        }

        // choose random font
        _displayText.font = GameManager.Instance.Fonts[Random.Range(0, GameManager.Instance.Fonts.Length)];
    }

    public void AnimateApperar(float delay = 0f)
    {
        if (!Hid) return;

        Hid = false;
        gameObject.SetActive(true);

        // Scale from zero
        _displayText.transform.localScale = Vector3.zero;
        _displayText.transform.DOScale(Vector3.one, _apperarScaleDur)
            .SetDelay(delay);

        // Fancy scale anim
        _displayText.transform.DOPunchScale(Vector3.one * _apperarPunchForce,
            _apperarPunchDuration, _apperarPunchVibrato, _apperarPunchElastic)
            .SetDelay(_apperarScaleDur
                      + delay);
    }

    public void AnimateHide(float delay = 0f)
    {
        if (Hid) return; 

        Hid = true;
        gameObject.SetActive(false);

        _displayText.transform.DOScale(Vector3.zero, _apperarScaleDur * 2f)
            .SetDelay(delay);
    }

    public void OnClick()
    {
        print("Wrong letter click");
        AnimateWrong();
        GameManager.Instance.OnWrongClick();
    }

    private void AnimateWrong()
    {
        if (Hid) return; // this shouldn't happen usually

        Sequence seq = DOTween.Sequence();
        seq.Append(_displayText.DOColor(Color.red, 0.5f));
        seq.Append(_displayText.DOColor(_targetColor, 0.5f));
        seq.SetLoops(10);
    }

}
