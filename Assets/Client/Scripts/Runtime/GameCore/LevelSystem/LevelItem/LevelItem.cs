using UnityEngine;

namespace Client
{
    public class LevelItem : MonoBehaviour
    {
        [SerializeField] private RobberyDifficultySettingsSO _robberyDifficultySettings;
        public RobberyDifficultySettingsSO RobberyDifficultySettings => _robberyDifficultySettings;

        private bool _playerInside = false;

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out PlayerBehaviour player))
            {
                _playerInside = true;
                LevelSystem.Instance.NotifyPlayerEntered(this);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out PlayerBehaviour player))
            {
                _playerInside = false;
                LevelSystem.Instance.NotifyPlayerExited(this);
            }
        }

        public bool IsPlayerInside => _playerInside;
    }
}