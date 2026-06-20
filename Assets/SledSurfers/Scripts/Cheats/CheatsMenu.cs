using System;
using SledSurfers.Scripts.Core;
using SledSurfers.Scripts.Data.Models;
using SledSurfers.Scripts.Managers;
using UnityEngine;

namespace SledSurfers.Scripts.Cheats
{
    public class CheatsMenu : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform _cheatsContainer;
        [SerializeField] private GameObject _openCheatsButton;
        [SerializeField] private GameObject _cheatsMenu;
         
        [Header("Instances")]
        [SerializeField] private CheatButton _cheatInstance;
        private void Awake()
        {
            _openCheatsButton.SetActive(true);
            _cheatsMenu.SetActive(false);
            AddCheats();
        }
        
        public void AddCheat(string text, Action callback)
        {
            var cheat = Instantiate(_cheatInstance, _cheatsContainer);
            cheat.Setup(text, callback);
        }

        private void AddCheats()
        {
            AddCheat("Add 10000 coins", () => ServiceLocator.Get<CurrencyManager>()?.Add(CurrencyType.Coins, 10000));
            AddCheat("Add 10 gems", () => ServiceLocator.Get<CurrencyManager>()?.Add(CurrencyType.Gems, 10));
            AddCheat("Remove currencies", ()=>
            {
                ServiceLocator.Get<CurrencyManager>()?.Reset(CurrencyType.Coins);
                ServiceLocator.Get<CurrencyManager>()?.Reset(CurrencyType.Gems);
            });
            
            AddCheat("Reset upgrades", () => ServiceLocator.Get<UpgradesManager>().ResetUpgrades());
        }

    }
}