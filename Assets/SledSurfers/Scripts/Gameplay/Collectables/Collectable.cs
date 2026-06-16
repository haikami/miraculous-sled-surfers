using SledSurfers.Scripts.Core;
using SledSurfers.Scripts.Data.Models;
using UnityEngine;

namespace SledSurfers.Scripts.Gameplay.Collectables
{
    public class Collectable : MonoBehaviour
    {
        public CurrencyType CurrencyType { get; private set; }

        private ICollectableOwner _owner;
        private CurrencyData _currencyData;

        public void Initialize(CollectableData data)
        {
            _currencyData = data.currencyData;
            CurrencyType  = data.currencyData.currencyType;
            transform.position = data.spawnPosition;
            gameObject.SetActive(true);
        }

        public void Reactivate() => gameObject.SetActive(true);
        public void Deactivate() => gameObject.SetActive(false);

        public void InjectOwner(ICollectableOwner owner) => _owner = owner;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;

            // ServiceLocator.Get<CurrencyManager>().Add(_currencyData);
            _owner.Return(this);
        }
    }
}