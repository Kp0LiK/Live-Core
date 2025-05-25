using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Client
{
    public class WindowsManager : MonoBehaviour
    {
        public static WindowsManager Instance { get; private set; }

        [SerializeField] private List<BaseWindowView> _windowViews;

        private BaseWindowView _currentWindowView;
        private BaseWindowView _previousWindowView;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            _currentWindowView = _windowViews[0];
            _currentWindowView.OpenImmediately();
        }

        public void OpenWindow<T>() where T : BaseWindowView
        {
            var window = _windowViews.FirstOrDefault(w => w is T);

            if (ReferenceEquals(window, null))
            {
                Debug.LogError("[Windows Manager] Window hasn't initialized " + typeof(T).Name);
                if (_windowViews.Count <= 0) return;

                _currentWindowView = _windowViews[0];
                _currentWindowView.Open();
                return;
            }

            if (!ReferenceEquals(_currentWindowView, null))
            {
                _previousWindowView = _currentWindowView;
                _currentWindowView.Close();
            }

            window.Open();
            _currentWindowView = window;
        }
        
        public void CloseCurrentWindow()
        {
            _currentWindowView.Close();
            _currentWindowView = _windowViews[0];
        }

        public void CloseAlLWindows()
        {
            foreach (var panel in _windowViews) panel.CloseImmediately();
        }
    }
}