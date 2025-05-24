using UnityEngine;

namespace Client
{
    [CreateAssetMenu(fileName = "RobberyDifficultySettings", menuName = "Robbery/Settings", order = 0)]
    public class RobberyDifficultySettingsSO : ScriptableObject
    {
        [Header("General")]
        [SerializeField] private RobberyDifficulty _difficulty;
        public RobberyDifficulty Difficulty => _difficulty;

        [Header("Pointer Timing")]
        [Tooltip("Speed of pointer rotation in degrees per second")]
        [SerializeField] private float _speed = 2f;

        [Tooltip("Minimum angle of success zone (degrees)")]
        [SerializeField] private float _successMin = 0.45f;

        [Tooltip("Maximum angle of success zone (degrees)")]
        [SerializeField] private float _successMax = 0.55f;

        [Tooltip("Minimum angle of fail zone (degrees)")]
        [SerializeField] private float _failMin = 0.7f;

        [Tooltip("Maximum angle of fail zone (degrees)")]
        [SerializeField] private float _failMax = 0.9f;

        [Header("Reward")]
        [SerializeField] private int _rewardAmount = 100;

        [Tooltip("Optional color of reward UI / particles")]
        [SerializeField] private Color _rewardColor = Color.green;

        public float Speed => _speed;
        public float SuccessMin => _successMin;

        public float SuccessMax => _successMax;
        public float FailMin => _failMin;
        public float FailMax => _failMax;
        public int RewardAmount => _rewardAmount;
        public Color RewardColor => _rewardColor;
    }
}