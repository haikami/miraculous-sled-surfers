using System.Collections.Generic;
using SledSurfers.Scripts.Core;
using SledSurfers.Scripts.Managers;
using UnityEngine;

namespace SledSurfers.Scripts.UI.Upgrades
{
    public class UpgradesBarView : MonoBehaviour
    {
        [SerializeField] private List<UpgradeCardView> _upgradeCards;
        
        private void Awake()
        {
            SetupCards();
        }

        private void SetupCards()
        {
            var numCards = _upgradeCards.Count;
            var manager = ServiceLocator.Get<UpgradesManager>();
            var availableUpgrades = manager.GetUpgradeAvailableTypes();
            var currentCardIndex = 0;
            foreach (var upgradeType in availableUpgrades)
            {
                if (currentCardIndex > numCards)
                {
                    Debug.LogWarning($"There are more upgrade types than supported cards ({numCards})");
                    break;
                }
                
                var card = _upgradeCards[currentCardIndex];
                card.gameObject.SetActive(true);
                card.Setup(upgradeType, manager);
                currentCardIndex++;
            }

            for (; currentCardIndex < numCards; currentCardIndex++)
            {
                _upgradeCards[currentCardIndex].gameObject.SetActive(false);
            }
        }
        
        public void Hide() => gameObject.SetActive(false);
        public void Show() => gameObject.SetActive(true);
    }
}