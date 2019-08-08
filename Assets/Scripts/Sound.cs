using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts
{
    [CreateAssetMenu(fileName = "Sound", menuName = "Uzelki/Sound")]
    public class Sound : ScriptableObject
    {
        public string Name;
        public AudioClip[] Clips;
        public float Volume = 0.7f;

        public AudioClip GetClip()
        {
            int randN = Clips.Length > 1 ? Random.Range(0, Clips.Length) : 0;
            return Clips[randN];
        }
    }
}
