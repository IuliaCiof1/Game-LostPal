using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace Audio
{
    public class AudioMaster : MonoBehaviour
    {
        [SerializeField] private AudioMixer mixer;
        [SerializeField] private Slider musicSlider;
        [SerializeField] private Slider SFXSlider;

        private void Start()
        {
            float value;
        
            //this is to keep the volume constant among all scenes
            mixer.GetFloat("MusicVol", out value);
            musicSlider.value = value;
            mixer.GetFloat("SFXVol", out value);
            SFXSlider.value = value;
        }

        public void Update()
        {
            //min, max values of sliders were changed to min, max of audio mixer
            //assign value from slider to the parameters defined in audio mixer
            mixer.SetFloat("MusicVol", musicSlider.value);
            mixer.SetFloat("SFXVol", SFXSlider.value);
        }
    }
}
