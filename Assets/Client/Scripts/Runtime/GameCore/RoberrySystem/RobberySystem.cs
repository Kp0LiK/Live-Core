using UnityEngine;
using UnityEngine.Events;

namespace Client
{
    public class RobberySystem : MonoBehaviour
    {
        public static RobberySystem Instance;

        [Header("References")]
        [SerializeField] private RobberyGame _game;
        [SerializeField] private RobberyWindowView _view;

        [Header("Difficulty Preset")]
        [SerializeField] private RobberyDifficultySettingsSO _baseSettings;
        
        private int _fails = 0;
        [SerializeField] private int _maxFails = 2;

        public event UnityAction OnGameStart;
        public event UnityAction OnGameSuccess;
        public event UnityAction OnGameFail;
        public event UnityAction OnWinGame;
        public event UnityAction OnLoseGame;

        private int _currentRound = 0;
        private int _totalRounds;
        private float _timer;
        private bool _timing;

        public int MaxFails => _maxFails;

        private void Awake()
        {
            Instance = this;
        }

        private void OnEnable()
        {
            _view.OnCrackPressed += HandleCrack;
        }

        private void OnDisable()
        {
            _view.OnCrackPressed -= HandleCrack;
        }

        public void StartGame(RobberyDifficultySettingsSO settings)
        {
            _baseSettings = settings;
                
            _currentRound = 0;
            _fails = 0;
            _totalRounds = _baseSettings.TotalRounds;
            _timer = _baseSettings.TimeLimitSeconds;
            _timing = true;

            StartRound(_currentRound);
            OnGameStart?.Invoke();
        }


        private void StartRound(int round)
        {
            var runtimeSettings = GenerateRuntimeSettings(round);
            _game.SetDifficulty(runtimeSettings);
            _view.Init(runtimeSettings, _timer, round + 1, _totalRounds, _maxFails - _fails);
            _view.StartMiniGame();
        }

        private RobberyDifficultySettingsSO GenerateRuntimeSettings(int round)
        {
            var settings = ScriptableObject.CreateInstance<RobberyDifficultySettingsSO>();

            float progress = Mathf.Clamp01((float)round / (_baseSettings.TotalRounds - 1));
            
            settings.Speed = _baseSettings.BaseSpeed + round * _baseSettings.SpeedBoost;
            Debug.Log($"[RobberySystem] Current speed is {settings.Speed}");

            float zoneSize = Mathf.Lerp(_baseSettings.BaseSuccessZoneSize, _baseSettings.SuccessZoneMinClamp, progress);
            float center = Random.Range(0f, 360f);
            settings.SuccessMin = center - zoneSize / 2f;
            settings.SuccessMax = center + zoneSize / 2f;
            settings.RewardAmount = _baseSettings.RewardAmount;

            return settings;
        }

        private void HandleCrack(float angle)
        {
            float min = Mathf.Repeat(_view.CurrentWinMinAngle + 90f, 360f);
            float max = Mathf.Repeat(_view.CurrentWinMaxAngle + 90f, 360f);
            float pointerAngle = Mathf.Repeat(angle, 360f);

            bool success = IsAngleInRange(pointerAngle, min, max);

            Debug.Log($"[RobberySystem] Angle: {angle:0.0}° — Win Zone: {min:0.0}° to {max:0.0}° → Result: {(success ? "SUCCESS" : "FAIL")}");

            if (success)
            {
                OnGameSuccess?.Invoke();
                _currentRound++;
                SoundManager.Instance.PlayClick();

                if (_currentRound >= _totalRounds)
                {
                    _timing = false;
                    SoundManager.Instance.PlayWin();
                    OnWinGame?.Invoke();
                }
                else
                {
                    StartRound(_currentRound);
                }
            }
            else
            {
                _fails++;
                _view.UpdateAttemptsLeft(_maxFails - _fails);
                OnGameFail?.Invoke();
                SoundManager.Instance.LoseClick();

                if (_fails >= _maxFails)
                {
                    _timing = false;
                    LoseGame();
                }
                else
                {
                    StartRound(_currentRound);
                }
            }
        }


        public void LoseGame()
        {
            SoundManager.Instance.PlayLose();
            OnLoseGame?.Invoke();
        }

        private bool IsAngleInRange(float angle, float min, float max)
        {
            angle = Mathf.Repeat(angle, 360f);
            min = Mathf.Repeat(min, 360f);
            max = Mathf.Repeat(max, 360f);

            return min < max
                ? angle >= min && angle <= max
                : angle >= min || angle <= max;
        }
    }
}
