using Blocks.Mechanics;
using UnityEngine;
using UnityEngine.UI;

namespace Audio
{
    public class UiSfxManager : MonoBehaviour
    {
        private AudioSource audioSource;
        [SerializeField] private AudioClip normalButtonSound;
        [SerializeField] private AudioClip runButtonSound;
        [SerializeField] private AudioClip blockDropOnNormal;
        [SerializeField] private AudioClip blockDropOnExpandBlock;
        private DragObject[] blocks;
        void OnEnable()
        {
            audioSource = GetComponent<AudioSource>();
        
            Button[] buttons = FindObjectsOfType<Button>(true);

            foreach (var button in buttons)
            {
                if(button.GetComponent<RunCode>())
                    button.onClick.AddListener(OnClickRunButton);
                else
                    button.onClick.AddListener(OnClickNormalButton);
            }

            blocks = FindObjectsOfType<DragObject>();
            foreach (DragObject block in blocks)
            {
                DragObject.OnDropOnObjectSound += OnDropOnObjectSoundHandler;
            }
        }

        private void OnDisable()
        {
        
            foreach (DragObject block in blocks)
            {
                DragObject.OnDropOnObjectSound -= OnDropOnObjectSoundHandler;
            }
        }

        void OnClickNormalButton()
        {
            audioSource.PlayOneShot(normalButtonSound);
        }
    
        void OnClickRunButton()
        {
            audioSource.PlayOneShot(runButtonSound);
        }
    
        //Handle blocks audio
        public void OnDropOnObjectSoundHandler(int mode)
        {
            if(mode==0)
                audioSource.PlayOneShot(blockDropOnNormal);
            else
                audioSource.PlayOneShot(blockDropOnExpandBlock);
        }
    }
}
