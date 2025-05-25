using UnityEngine;
using UnityEngine.SceneManagement;

namespace Client
{
    public class LoseWindowView : BaseWindowView
    {
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q))
                WindowsManager.Instance.CloseCurrentWindow();
            
            if (Input.GetKeyDown(KeyCode.R))
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}