using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public abstract class Letter : MonoBehaviour
    {
        [SerializeField] protected float _animationMoveDur = 1.8f;
        [SerializeField] protected Text _displayText;

        public void Move(Vector3 pos, float delay = 0f)
        {
            _displayText.transform.DOMove(pos, _animationMoveDur).SetDelay(delay);
        }
    }
}
