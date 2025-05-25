using UnityEngine;

namespace Client
{
    public class LevelSystem : MonoBehaviour
    {
        public static LevelSystem Instance { get; private set; }

        [SerializeField] private RobberySystem _robberySystem;

        private LevelItem _currentLevelItem;

        private void Awake()
        {
            Instance = this;
        }

        private void Update()
        {
            if (_currentLevelItem != null && Input.GetKeyDown(KeyCode.E))
            {
                _robberySystem.StartGame(_currentLevelItem.RobberyDifficultySettings);
                HideInteractionUI();
                _currentLevelItem = null;
            }
        }

        public void NotifyPlayerEntered(LevelItem item)
        {
            _currentLevelItem = item;
            ShowInteractionUI();
        }

        public void NotifyPlayerExited(LevelItem item)
        {
            if (_currentLevelItem == item)
            {
                _currentLevelItem = null;
                HideInteractionUI();
            }
        }

        private void ShowInteractionUI()
        {
            Debug.Log("[LevelSystem] PRESS E to Rob!");
            // Здесь можно показать надпись или иконку
        }

        private void HideInteractionUI()
        {
            Debug.Log("[LevelSystem] Exit trigger zone");
            // Скрыть UI
        }
    }
}