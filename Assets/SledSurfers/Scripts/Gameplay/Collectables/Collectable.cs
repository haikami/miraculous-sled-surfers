using System;
using SledSurfers.Scripts.Core;
using SledSurfers.Scripts.Data.Models;
using SledSurfers.Scripts.Managers;
using UnityEngine;

namespace SledSurfers.Scripts.Gameplay.Collectables
{
    public class Collectable : MonoBehaviour
    {
        public CurrencyType CurrencyType { get; private set; }

        private ICollectableOwner _owner;
        private CurrencyData _currencyData;

        private CurrencyManager _currencyManager;

        private void Awake()
        {
            _currencyManager = ServiceLocator.Get<CurrencyManager>();
        }

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
            
            _owner.Return(this);
            _currencyManager?.Add(_currencyData);
        }
    }
}