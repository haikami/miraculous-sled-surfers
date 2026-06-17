using System;
using System.Collections.Generic;
using SledSurfers.Scripts.Core;
using SledSurfers.Scripts.Data.Models;
using SledSurfers.Scripts.Gameplay.Level;
using SledSurfers.Scripts.Managers;
using UnityEngine;

namespace SledSurfers.Scripts.Gameplay.Collectables
{
    public class CollectablePool : MonoBehaviour, ICollectableOwner
    {
        [SerializeField] private Collectable _coinPrefab;
        [SerializeField] private Collectable _gemPrefab;

        [SerializeField] private int _initialCoinPool = 30;
        [SerializeField] private int _initialGemPool = 5;

        private readonly List<Collectable> _active = new();
        private readonly Queue<Collectable> _coinPool = new();
        private readonly Queue<Collectable> _gemPool = new();
        
        private void Awake()
        {
            ServiceLocator.Register(this);
            InitializePools();
            ServiceLocator.Get<LevelManager>().OnLevelLoaded += PlaceCollectables;
        }

        private void PlaceCollectables()
        {
            ReleaseAll();
            
            if (!ServiceLocator.TryGet(out LevelDefinition levelDefinition))
            {
                Debug.LogError("No level definition found, add one to level scene");
                return;
            }
            
            var collectables = levelDefinition.GetCollectableData();

            foreach (var data in collectables)
            {
                if (!TryGetPool(data.currencyData.currencyType, out var pool))
                {
                    Debug.LogWarning($"[CollectablePool] Unsupported currency type: {data.currencyData.currencyType}");
                    continue;
                }

                var collectable = Rent(pool, GetPrefab(data.currencyData.currencyType));
                collectable.Initialize(data);
                _active.Add(collectable);
            }
        }

        private void OnDestroy()
        {
            ServiceLocator.Unregister<CollectablePool>();
        }

        private void InitializePools()
        {
            InitializePool(CurrencyType.Coins, _initialCoinPool);
            InitializePool(CurrencyType.Gems, _initialGemPool);
        }

        private void InitializePool(CurrencyType currencyType, int amount)
        {
            if (amount > 0 && TryGetPool(currencyType, out var pool))
            {
                var prefab = GetPrefab(currencyType);
                for (var i = 0; i < amount; i++)
                {
                    var collectable = Rent(pool, prefab);
                    collectable.Deactivate();
                }
            }
        }

        public void Reset()
        {
            foreach (var collectable in _active)
                collectable.Reactivate();
        }

        public void Return(Collectable collectable)
        {
            _active.Remove(collectable);

            if (!TryGetPool(collectable.CurrencyType, out var pool))
                return;

            collectable.Deactivate();
            pool.Enqueue(collectable);
        }

        private void ReleaseAll()
        {
            foreach (var collectable in _active)
            {
                if (!TryGetPool(collectable.CurrencyType, out var pool))
                    continue;

                collectable.Deactivate();
                pool.Enqueue(collectable);
            }

            _active.Clear();
        }

        private Collectable Rent(Queue<Collectable> pool, Collectable prefab)
        {
            if (pool.Count > 0)
                return pool.Dequeue();

            var collectable = Instantiate(prefab, transform);
            collectable.InjectOwner(this);
            return collectable;
        }

        private bool TryGetPool(CurrencyType type, out Queue<Collectable> pool)
        {
            pool = type switch
            {
                CurrencyType.Coins => _coinPool,
                CurrencyType.Gems  => _gemPool,
                _                  => null
            };

            return pool != null;
        }

        private Collectable GetPrefab(CurrencyType type) => type switch
        {
            CurrencyType.Coins => _coinPrefab,
            CurrencyType.Gems  => _gemPrefab,
            _                  => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
    }
}