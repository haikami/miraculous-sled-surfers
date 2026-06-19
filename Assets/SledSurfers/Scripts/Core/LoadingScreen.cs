using UnityEngine;

namespace SledSurfers.Scripts.Core
{
    public class LoadingScreen : MonoBehaviour
    {
        [SerializeField] private GameObject _visual;
        
        private void OnDestroy()
        {
            ServiceLocator.Unregister<LoadingScreen>();
        }

        public void Show() => _visual.SetActive(true);
        public void Hide() => _visual.SetActive(false);
    }
}