using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Client
{
    public class RobberySystem : MonoBehaviour
    {
        public static RobberySystem Instance;

        [Header("References")] [SerializeField]
        private RobberyGame _game;

        [SerializeField] private RobberyWindowView _view;

        [Header("Difficulties")] [SerializeField]
        private List<RobberyDifficultySettingsSO> _difficultySettings;

        public event UnityAction OnGameStart;
        public event UnityAction OnGameSuccess;
        public event UnityAction OnGameFail;

        public RobberyDifficulty CurrentDifficulty { get; private set; } = RobberyDifficulty.Normal;

        private RobberyDifficultySettingsSO _activeSettings;

        private void Awake()
        {
            Instance = this;
            StartGame();
        }

        private void OnEnable()
        {
            _view.OnCrackPressed += HandleCrack;
        }

        private void OnDisable()
        {
            _view.OnCrackPressed -= HandleCrack;
        }

        public void SetDifficulty(RobberyDifficulty difficulty)
        {
            CurrentDifficulty = difficulty;
            _activeSettings = _difficultySettings.Find(x => x.Difficulty == difficulty);

            if (_activeSettings == null)
                Debug.LogWarning($"[RobberySystem] Difficulty {difficulty} not found!");
        }

        public void StartGame()
        {
            if (_activeSettings == null)
                SetDifficulty(CurrentDifficulty);

            _game.SetDifficulty(_activeSettings);
            _view.Init(_activeSettings);

            _view.StartMiniGame();
            OnGameStart?.Invoke();
        }

        private void HandleCrack(float angle)
        {
            float min = Mathf.Repeat(_view.CurrentWinMinAngle + 90f, 360f);
            float max = Mathf.Repeat(_view.CurrentWinMaxAngle + 90f, 360f);
            float pointerAngle = Mathf.Repeat(angle, 360f);

            bool success = IsAngleInRange(pointerAngle, min, max);

            Debug.Log(
                $"[RobberySystem] Angle: {angle:0.0}° — Win Zone: {min:0.0}° to {max:0.0}° → Result: {(success ? "SUCCESS" : "FAIL")}");

            if (success)
                OnGameSuccess?.Invoke();
            else
                OnGameFail?.Invoke();
        }

        private bool IsAngleInRange(float angle, float min, float max)
        {
            angle = Mathf.Repeat(angle, 360f);
            min = Mathf.Repeat(min, 360f);
            max = Mathf.Repeat(max, 360f);

            if (min < max)
                return angle >= min && angle <= max;
            else
                return angle >= min || angle <= max;
        }
    }
}