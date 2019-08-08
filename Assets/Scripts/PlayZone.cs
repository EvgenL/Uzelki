using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class PlayZone : MonoBehaviour
{
    public RandomLetter[] RandomLetters;
    public Transform[] TargetLetters;

    [Header("Animation: Letters")]
    [SerializeField] private float _animationRowDelay = 0.1f;

    private List<Vector3> _positions = new List<Vector3>();

    public CanvasGroup CanvasGroup;

    void Awake()
    {
        CanvasGroup = GetComponent<CanvasGroup>();
        ShufflePositions();
        SetPositions();
    }

    private void SetPositions()
    {
        int i = TargetLetters.Length;
        foreach (var randLetter in RandomLetters)
        {
            if (i > _positions.Count)
            {
                break;
            }
            else
            {
                randLetter.transform.localPosition = _positions[i];
            }

            i++;
        }
    }

    public void ShufflePositions()
    {
        if (TargetLetters.Length == 0)
        {
            Debug.LogWarning("No TargetLetters was added.");
            return;
        }

        float letterWidth = (TargetLetters[0].transform as RectTransform).rect.width;
        List<Transform> leftTargetLetters = TargetLetters.ToList();
        _positions.Clear();

        var rect = (transform as RectTransform).rect;


        int tries = 0;

        while (leftTargetLetters.Count > 0)
        {
            float randX = Random.Range(rect.xMin, rect.xMax);
            float randY = Random.Range(rect.yMin, rect.yMax);
            Vector3 randomPos = new Vector3(randX, randY);

            // no letter overlapping this position
            bool ok = _positions.All(pos => Vector3.Distance(pos, randomPos) > letterWidth);

            if (ok)
            {
                leftTargetLetters[0].transform.localPosition = randomPos;
                leftTargetLetters.RemoveAt(0);
                _positions.Add(randomPos);
            }
            tries++;
            if (tries > 1000)
            {
                Debug.LogWarning("Can't position all targets. Play Zone is too small.");
            }
        }

        while (tries < 1000)
        {
            float randX = Random.Range(rect.xMin, rect.xMax);
            float randY = Random.Range(rect.yMin, rect.yMax);
            Vector3 randomPos = new Vector3(randX, randY);

            // no letter overlapping this position
            bool ok = _positions.All(pos => Vector3.Distance(pos, randomPos) > letterWidth);

            if (ok)
            {
                _positions.Add(randomPos);
            }
            tries++;
        }

    }

    public void AnimateMoveLetters()
    {
        int i = TargetLetters.Length;
        foreach (var randLetter in RandomLetters)
        {
            if (i > _positions.Count)
            {
                randLetter.AnimateHide();
            }
            else
            {
                randLetter.Move(_positions[i] + transform.position, i * _animationRowDelay);
            }

            i++;
        }
    }

    public void ShowLetters()
    {
        int i = TargetLetters.Length;
        foreach (var randLetter in RandomLetters)
        {
            if (i > _positions.Count)
            {
                randLetter.AnimateHide();
            }
            else
            {
                randLetter.AnimateApperar(i * _animationRowDelay);
            }

            i++;
        }
    }
}
