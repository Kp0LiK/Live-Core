using System;
using TMPro;
using UnityEngine;

namespace Client
{
    public class WinWindowView : BaseWindowView
    {
        [SerializeField] private TextMeshProUGUI _moneyLabel;

        private void OnEnable()
        {
            _moneyLabel.text = $"Вы получили: {RobberySystem.Instance.BaseSettings.RewardAmount}$";
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q))
                WindowsManager.Instance.CloseCurrentWindow();
        }
    }
}