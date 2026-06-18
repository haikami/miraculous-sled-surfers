using System;
using UnityEngine;

namespace SledSurfers.Scripts.Gameplay.UI
{
    public class ResultScreen : MonoBehaviour
    {
        private Action _onClosed;
        
        public void Show(Action onClose)
        {
            _onClosed = onClose;
            gameObject.SetActive(true);
        }
        
        public void Close()
        {
            _onClosed?.Invoke();
            _onClosed = null;
            gameObject.SetActive(false);
        }
    }
}