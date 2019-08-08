using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class TargetLetter : Letter
{
    public Button ClickableTargetButton;
    [SerializeField] private GameObject _clickableTarget;
    [SerializeField] private Text _backText;
    [SerializeField] private Color _grayColor;
    [SerializeField] private Color _redColor;
    [SerializeField] private float _colorFadeDur = 2f;

    public char CurrentLetter { get; private set; }

    public void DrawLetter(char letter, Font font)
    {
        _displayText.text = letter.ToString();
        _displayText.font = font;

        _backText.text = letter.ToString();
        _backText.font = font;

        CurrentLetter = letter;
    }

    public void AnimateToGray()
    {
        _displayText.DOColor(_grayColor, _colorFadeDur);
    }

    public void AnimateToRed()
    {
        _displayText.DOColor(_redColor, _colorFadeDur);
    }

    public void OnClick()
    {
        ClickableTargetButton.interactable = false;
        // move _display to local zero position
        Move(transform.position);
        AnimateToRed();
        GameManager.Instance.OnCorrectClick(CurrentLetter);

    }
}
