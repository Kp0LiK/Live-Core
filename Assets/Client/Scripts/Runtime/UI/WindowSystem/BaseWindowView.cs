using System;
using UnityEngine;
using UnityEngine.UI;

namespace Client
{
    public abstract class BaseWindowView : MonoBehaviour
    {
        public event Action<BaseWindowView> WindowOpened;
        public event Action<BaseWindowView> WindowClosed;

        
        public void Open()
        {
            gameObject.SetActive(true);

            WindowOpened?.Invoke(this);
        }

        public void Close()
        {
            WindowClosed?.Invoke(this);
            gameObject.SetActive(false);
        }

        public void OpenImmediately()
        {
            gameObject.SetActive(true);

            WindowOpened?.Invoke(this);
        }

        public void CloseImmediately()
        {

            WindowClosed?.Invoke(this);
            gameObject.SetActive(false);
        }
    }
}