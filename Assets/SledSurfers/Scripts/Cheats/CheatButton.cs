using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SledSurfers.Scripts.Cheats
{
    public class CheatButton : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private Button _button;

        public void Setup(string text, Action callback)
        {
            _text.text = text;
            _button.onClick.AddListener(()=>callback?.Invoke());
        }
    }
}