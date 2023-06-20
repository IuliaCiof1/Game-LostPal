using UnityEngine;

namespace Audio
{
    public class BackgroundMusicManager : MonoBehaviour
    {
        [SerializeField] private AudioClip loopClip;
        [SerializeField] private AudioClip endingClip;
    
        private AudioSource audioSource;
        void Start()
        {
            audioSource = GetComponent<AudioSource>();
            audioSource.clip = loopClip;
            audioSource.Play();
            audioSource.loop = true;
        }

        private void OnEnable()
        {
            PlayerController.OnPlayerWins += TriggerEnding;
        }

        private void OnDisable()
        {
            PlayerController.OnPlayerWins -= TriggerEnding;
        }

        public void TriggerEnding()
        {
            audioSource.clip = endingClip;
            audioSource.Play();
            audioSource.loop = false;
            PlayerController.OnPlayerWins -= TriggerEnding;
        }
    
        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
