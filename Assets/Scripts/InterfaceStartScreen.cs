using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InterfaceStartScreen : MonoBehaviour {
    // declaración de las variables
    public Text musicSetting;
    public AudioSource musicFile;

    void Start()
    {
        GameManager.currentLevel = 0;

        // se ejecuta la música de la pantalla inicial
        musicFile = GetComponent<AudioSource>();
        if (GameManager.musicSettings)
        {
            musicFile.Play();
        }
    }

    // botón Play para iniciar el juego y pasar al primer nivel
    public void PlayClick()
    {
        GameManager.currentLevel = 1;
        SceneManager.LoadScene("Level1");
    }

    // configuración de la música y los efectos de sonido
    public void MusicSettingsClick()
    {
        if (GameManager.musicSettings == true)
        {
            // pone la música en pausa para no reproducir 
            // desde el inicio si el usuario camibia la configuración
            musicFile.Pause(); 
            GameManager.musicSettings = false;
            musicSetting.text = "Music OFF";

        } else
        {
            musicFile.Play();
            GameManager.musicSettings = true;
            musicSetting.text = "Music ON";
        }
    }

    // botón Tutorial
    public void TutorialClick()
    {
        SceneManager.LoadScene("Tutorial");
    }
}
