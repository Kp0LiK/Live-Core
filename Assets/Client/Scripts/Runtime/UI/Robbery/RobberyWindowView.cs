using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

namespace Client
{
    public class RobberyWindowView : MonoBehaviour
    {
        [Header("UI Elements")] [SerializeField]
        private RectTransform _pointerTransform;

        [SerializeField] private GameObject _windowRoot;
        [SerializeField] private Button _crackButton;
        [SerializeField] private TextMeshProUGUI _rewardText;

        [SerializeField] private Image _successZoneImage;
        [SerializeField] private Image _failZoneImage;
        [SerializeField] private RectTransform _dialRoot;

        [Header("UI Game")] [SerializeField] private TextMeshProUGUI _timerText;
        [SerializeField] private TextMeshProUGUI _roundText;
        [SerializeField] private TextMeshProUGUI _yourChanceText;
        [SerializeField] private Slider _timerSlider;
        [SerializeField] private Color _startColor;
        [SerializeField] private Color _warningColor;
        [SerializeField] private Color _criticalColor;


        private RobberyDifficultySettingsSO _settings;
        private float _currentAngle;
        private bool _isPlaying;

        private float _timeRemaining;
        private bool _timerRunning;

        private float _currentWinMinAngle;
        private float _currentWinMaxAngle;

        public float CurrentWinMinAngle => _currentWinMinAngle;
        public float CurrentWinMaxAngle => _currentWinMaxAngle;

        private Image _timerFillImage;
        private float _initialTime;
        private float _pulseSpeed = 4f;

        public event Action<float> OnCrackPressed;

        public void Init(RobberyDifficultySettingsSO settings, float timer, int currentRound, int totalRounds,
            int attemptsLeft)
        {
            _settings = settings;

            _rewardText.text = $"Win and get +{settings.RewardAmount}$";
            _roundText.text = $"Round {currentRound}/{totalRounds}";
            _yourChanceText.text = $"Attempts: {attemptsLeft}";

            _timeRemaining = timer;
            _timerRunning = true;

            _timerSlider.maxValue = _timeRemaining;
            _timerSlider.value = _timeRemaining;
            _timerFillImage = _timerSlider.fillRect.GetComponent<Image>();
            _initialTime = _timeRemaining;

            Show(true);
            ResetPointer();
            SetupZones();
        }

        public void Show(bool show)
        {
            _windowRoot.SetActive(show);
        }

        private void OnEnable()
        {
            _crackButton.onClick.AddListener(HandleCrackPressed);
        }

        private void OnDisable()
        {
            _crackButton.onClick.RemoveListener(HandleCrackPressed);
        }

        private void Update()
        {
            if (!_isPlaying || _settings == null)
                return;

            _currentAngle += _settings.Speed * Time.deltaTime;
            _currentAngle %= 360f;

            if (_timerRunning)
            {
                _timeRemaining -= Time.deltaTime;
                _timeRemaining = Mathf.Max(0f, _timeRemaining);

                _timerText.text = Mathf.CeilToInt(_timeRemaining).ToString();

                float t = _timeRemaining;

                _timerSlider.value = t;

                if (_timerFillImage != null)
                {
                    float progress = _timeRemaining / _initialTime;

                    if (progress > 0.3f)
                    {
                        _timerFillImage.color = Color.Lerp(_warningColor, _startColor, (progress - 0.5f) * 2f);
                    }
                    else
                    {
                        float pulse = Mathf.PingPong(Time.time * _pulseSpeed, 1f);
                        _timerFillImage.color = Color.Lerp(_warningColor, _criticalColor, pulse);
                    }
                }


                if (_timeRemaining <= 0f)
                {
                    _timerRunning = false;
                    RobberySystem.Instance.LoseGame();
                }
            }

            if (_pointerTransform != null)
                _pointerTransform.rotation = Quaternion.Euler(0f, 0f, -_currentAngle);

            if (Input.GetKeyDown(KeyCode.Space))
                HandleCrackPressed();
        }

        public void UpdateAttemptsLeft(int attemptsLeft)
        {
            _yourChanceText.text = $"Attempts: {attemptsLeft}";
        }

        public void StartMiniGame()
        {
            _isPlaying = true;
            ResetPointer();
        }

        private void SetupZones()
        {
            SetupFailZoneFull();
            SetupWinZoneRandomly();
        }

        private void SetupWinZoneRandomly()
        {
            float angleSize = _settings.SuccessMax - _settings.SuccessMin;
            float startAngle = UnityEngine.Random.Range(0f, 360f - angleSize);

            float fillAmount = angleSize / 360f;

            _successZoneImage.fillMethod = Image.FillMethod.Radial360;
            _successZoneImage.fillOrigin = (int)Image.Origin360.Left;
            _successZoneImage.fillClockwise = true;
            _successZoneImage.fillAmount = fillAmount;
            _successZoneImage.color = new Color(0f, 1f, 0f, 0.7f);

            _successZoneImage.rectTransform.localRotation = Quaternion.Euler(0f, 0f, -startAngle);

            _currentWinMinAngle = startAngle;
            _currentWinMaxAngle = startAngle + angleSize;
        }

        private void SetupFailZoneFull()
        {
            _failZoneImage.fillMethod = Image.FillMethod.Radial360;
            _failZoneImage.fillOrigin = (int)Image.Origin360.Right;
            _failZoneImage.fillClockwise = true;
            _failZoneImage.fillAmount = 1f;
            _failZoneImage.color = new Color(1f, 0f, 0f, 0.4f);
        }


        public void Stop()
        {
            _isPlaying = false;
            _timerRunning = false;
        }

        private void ResetPointer()
        {
            _currentAngle = 0f;
            _pointerTransform.rotation = Quaternion.identity;
        }

        private void HandleCrackPressed()
        {
            _isPlaying = false;
            Show(false);
            OnCrackPressed?.Invoke(_currentAngle);
        }
    }
}