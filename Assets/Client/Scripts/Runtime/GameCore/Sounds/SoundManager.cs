using System.Threading.Tasks;
using UnityEngine;

namespace Client
{
    public class SoundManager : MonoBehaviour
    {
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioSource _backgroundSource;

        [SerializeField] private AudioClip _backgroundClip;
        [SerializeField] private AudioClip _backgroundLoseClip;
        [SerializeField] private AudioClip _clickSound;
        [SerializeField] private AudioClip _loseClickSound;
        [SerializeField] private AudioClip _winSound;
        [SerializeField] private AudioClip _missionFailed;
        [SerializeField] private AudioClip _loseSound;

        public static SoundManager Instance;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            
            _backgroundSource.PlayOneShot(_backgroundClip);
        }

        public void PlayClick()
        {
            _audioSource.PlayOneShot(_clickSound);
        }
        
        public void LoseClick()
        {
            _audioSource.PlayOneShot(_loseClickSound);
        }

        public void PlayWin()
        {
            _audioSource.PlayOneShot(_winSound);
        }

        public async void PlayLose()
        {
            _audioSource.PlayOneShot(_missionFailed);
            await Task.Delay(2000);
            _audioSource.PlayOneShot(_loseSound);
            await Task.Delay(2000);
            _backgroundSource.Stop();
            
            _backgroundSource.PlayOneShot(_backgroundLoseClip);
        }
    }

}