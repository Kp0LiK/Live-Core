using UnityEngine;

namespace Client
{
    public class WelcomeWindowView : BaseWindowView
    {
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q))
                WindowsManager.Instance.CloseCurrentWindow();
        }
    }
}