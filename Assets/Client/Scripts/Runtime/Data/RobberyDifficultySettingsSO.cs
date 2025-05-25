using UnityEngine;

namespace Client
{
    [CreateAssetMenu(fileName = "RobberyDifficultySettings", menuName = "Robbery/Settings", order = 0)]
    public class RobberyDifficultySettingsSO : ScriptableObject
    {
        [Header("General")]
        [SerializeField] private RobberyDifficulty _difficulty;
        public RobberyDifficulty Difficulty => _difficulty;
        
        private float _speed = 2f;
        private float _successMin = 0.45f;
        private float _successMax = 0.55f;
        private float _failMin = 0.7f;
        private float _failMax = 0.9f;

        [Header("Game Rules")]
        [SerializeField] private int _totalRounds = 3;
        [SerializeField] private float _timeLimitSeconds = 90f;

        [Header("Reward")]
        [SerializeField] private int _rewardAmount = 100;
        [SerializeField] private Color _rewardColor = Color.green;

        [Header("Dynamic Scaling")]
        [SerializeField] private float _baseSpeed = 50f;
        [SerializeField] private float _speedBoost = 15f;
        [SerializeField] private float _baseSuccessZoneSize = 45f;
        [SerializeField] private float _successZoneMinClamp = 15f;

        // Public accessors
        public float Speed
        {
            get => _speed;
            set => _speed = value;
        }

        public float SuccessMin
        {
            get => _successMin;
            set => _successMin = value;
        }

        public float SuccessMax
        {
            get => _successMax;
            set => _successMax = value;
        }

        public float FailMin => _failMin;
        public float FailMax => _failMax;

        public int TotalRounds => _totalRounds;
        public float TimeLimitSeconds => _timeLimitSeconds;

        public int RewardAmount
        {
            get => _rewardAmount;
            set => _rewardAmount = value;
        }

        public Color RewardColor => _rewardColor;

        public float BaseSpeed => _baseSpeed;
        public float SpeedBoost => _speedBoost;
        public float BaseSuccessZoneSize => _baseSuccessZoneSize;
        public float SuccessZoneMinClamp => _successZoneMinClamp;
    }
}