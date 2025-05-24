using UnityEngine;

namespace Client
{
    public class RobberyGame : MonoBehaviour
    {
        private RobberyDifficultySettingsSO _settings;

        public void SetDifficulty(RobberyDifficultySettingsSO settings)
        {
            _settings = settings;
        }
        public bool EvaluateAngle(float angle)
        {
            if (_settings == null)
            {
                Debug.LogError("[RobberyGame] No settings assigned.");
                return false;
            }

            bool success = angle >= _settings.SuccessMin && angle <= _settings.SuccessMax;
            bool fail = angle >= _settings.FailMin && angle <= _settings.FailMax;

            return success && !fail;
        }
    }
}