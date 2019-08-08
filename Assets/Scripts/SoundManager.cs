using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class SoundManager : MonoBehaviour
{
    #region Singleton

    public static SoundManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        _source = GetComponent<AudioSource>();
    }

    #endregion

    [SerializeField] private Sound[] _sounds;
    [SerializeField] private float _delayBetweenSounds = 1.5f;

    private AudioSource _source;

    public float PlaySound(string _name)
    {
        foreach (var s in _sounds)
        {
            if (s.Name == _name)
            {
                _source.volume = s.Volume;
                AudioClip clip = s.GetClip();
                _source.clip = clip;
                _source.Play();
                return clip.length;
            }
        }

        return 0f;

        Debug.LogWarning("No sound with name: " + _name);
    }
}
