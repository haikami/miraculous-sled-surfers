using System.Collections.Generic;
using SledSurfers.Scripts.Core;
using SledSurfers.Scripts.Data.Models;
using SledSurfers.Scripts.Gameplay.Collectables;
using UnityEngine;

namespace SledSurfers.Scripts.Gameplay.Level
{
    public class LevelDefinition : MonoBehaviour
    {
        [SerializeField] private Transform _playerSpawnPoint;
        [SerializeField] private GameObject _collectableMarkersParent;
        
        public Transform PlayerSpawnPoint => _playerSpawnPoint;

        public List<CollectableData> GetCollectableData()
        {
            var collectablesData = new List<CollectableData>();
            var collectableMarkers =  _collectableMarkersParent.GetComponentsInChildren<CollectableMarker>(true);
            foreach (var collectableMarker in collectableMarkers)
            {
                collectablesData.Add(collectableMarker.GetCollectableData());
            }
            
            return collectablesData;
        }
        
        private void Awake()
        {
            ServiceLocator.Register(this);
        }

        private void OnDestroy()
        {
            ServiceLocator.Unregister<LevelDefinition>();
        }
    }
}