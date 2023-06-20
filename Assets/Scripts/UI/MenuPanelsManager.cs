using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;

namespace UI
{
    public class MenuPanelsManager : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI timeText;

        private float time;

        [SerializeField] private GameObject lvlSelection;
        [SerializeField] private GameObject menu;
        [SerializeField] private GameObject settings;
        [SerializeField] private GameObject sound;
        [SerializeField] private GameObject graphics;

        void Update()
        {
            //calculate time until player gets to finish
            if (timeText is not null && !timeText.IsActive())
            {
                time += Time.deltaTime;
                timeText.text = "Time: " + Math.Round(time,2) + " s";
                
            }
        }

        public void Retry()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void Next()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        
        public void Menu()
        {
            menu.SetActive(true);
            Time.timeScale = 0;
        
        }

        public void Play()
        {
            //SceneManager.LoadScene("Intro");
            menu.SetActive(false);
            lvlSelection.SetActive(true);
        }
    
    
        public void Resume()
        {
            Time.timeScale = 1;
            menu.SetActive(false);
        }

        public void MainMenu()
        {
            Time.timeScale = 1;
            SceneManager.LoadScene("MainMenu");
        }
    
        public void PlayBack()
        {
            menu.SetActive(true);
            lvlSelection.SetActive(false);
        }
        
        public void Settings()
        {
            menu.SetActive(false);
            settings.SetActive(true);
        }

        public void SettingsBack()
        {
            menu.SetActive(true);
            settings.SetActive(false);
        }

        public void Sound()
        {
            settings.SetActive(false);
            sound.SetActive(true);
        }
    
        public void SoundBack()
        {
            settings.SetActive(true);
            sound.SetActive(false);
        }
    
        public void Graphics()
        {
            settings.SetActive(false);
            graphics.SetActive(true);
        }
    
        public void GraphicsBack()
        {
            settings.SetActive(true);
            graphics.SetActive(false);
        }
        public void Quit()
        {
            Application.Quit();
        }
    }
}
