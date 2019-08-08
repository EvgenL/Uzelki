using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    #region Singleton

    public static GameManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    #endregion

    public char TargetLetter;

    public string Alphavet = "АБВГДЕЖЗИЙКЛМНПРСТУ";

    public Font[] Fonts;

    [SerializeField] private PlayZone _playZone;
    [SerializeField] private TargetLetter[] _targetLetters;
    [SerializeField] private GameObject _restartButton;

    private int _score;

    private void Start()
    {
        GenerateTopLetters();
        // ask button
        StartCoroutine("StartAnimation");
    }

    private IEnumerator StartAnimation()
    {
        SetInteractible(false);

        float phraseLen = SoundManager.Instance.PlaySound("Привет");
        yield return new WaitForSeconds(phraseLen + 2f);
        MoveTopToPlayZone();
        // delay 2 sec
        yield return new WaitForSeconds(2f);
        _playZone.ShowLetters();
        yield return new WaitForSeconds(3f);
        _playZone.ShufflePositions();
        _playZone.AnimateMoveLetters();
        MoveTopToPlayZone();

        yield return new WaitForSeconds(3f);
        SetInteractible(true);
    }

    private void MoveTopToPlayZone()
    {
        if (_targetLetters.Length != _playZone.TargetLetters.Length)
        {
            // error
            return;
        }

        for (int i = 0; i < _targetLetters.Length; i++)
        {
            _targetLetters[i].Move(_playZone.TargetLetters[i].position, i * 0.1f);
            _targetLetters[i].AnimateToGray();
        }
    }

    private void GenerateTopLetters()
    {
        // select random letter
        char randLetter = GetRandomLetter();
        TargetLetter = randLetter;
        // by pairs
        List<Font> notUsedFonts = Fonts.ToList();
        for (int i = 0; i < _targetLetters.Length - 1; i+=2)
        {
            int randN = notUsedFonts.Count > 1 ? Random.Range(0, notUsedFonts.Count) : 0;
            Font randFont = notUsedFonts[randN];
            notUsedFonts.Remove(randFont);
            _targetLetters[i].DrawLetter(char.ToUpper(randLetter), randFont);
            _targetLetters[i+1].DrawLetter(char.ToLower(randLetter), randFont);
        }
    }

    public static char GetRandomLetter()
    {
        var alphavet = GameManager.Instance.Alphavet.ToLower();
        if (string.IsNullOrEmpty(alphavet))
        {
            Debug.LogWarning("Alphavet is empty.");
            return 'A';
        }
        int randomN = Random.Range(0, alphavet.Length);
        return alphavet[randomN];
    }

    public void OnWrongClick()
    {
        SoundManager.Instance.PlaySound("Неправильно");
        SetInteractible(false);
        foreach (var tg in _targetLetters)
        {
            tg.AnimateToRed();
        }

        Invoke("Restart", 3f);
    }

    private void Restart()
    {
        _score = 0;
        print("Restart");
        _playZone.ShufflePositions();
        _playZone.AnimateMoveLetters();
        MoveTopToPlayZone();
        SetInteractible(true);
    }

    public void OnCorrectClick(char letter)
    {
        _score++;
        if (_score == 1)
        {
            SoundManager.Instance.PlaySound("Правильно первый");
        }
        else if (_score >= _targetLetters.Length)
        {
            Win();
        }

        else if (Random.Range(0, 2) == 0)
        {
            SoundManager.Instance.PlaySound(letter.ToString().ToLower());
            SoundManager.Instance.PlaySound("Правильно", 2);
        }
    }

    private void Win()
    {
        print("Done");
        SoundManager.Instance.PlaySound("Ай");
        
        SetInteractible(false);

        _restartButton.gameObject.SetActive(true);
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void SetInteractible(bool value)
    {
        _playZone.CanvasGroup.interactable = value;
        foreach (var tg in _targetLetters)
        {
            tg.ClickableTargetButton.interactable = value;
        }
    }
}
