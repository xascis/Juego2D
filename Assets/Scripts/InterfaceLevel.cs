using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InterfaceLevel : MonoBehaviour {

    // imagenes de la UI que cambian
    public Image imageUnitNumberCoins;
    public Image imageDecimalNumberCoins;
    public Image imageKeyRedFound;
    public Image imageGemYellow;
    public Image imageHearth1;
    public Image imageHearth2;
    public Image imageHearth3;

    public AudioSource musicFile;

	void Start () {
        GameManager.currentNumberHearth = 3;
        GameManager.currentNumberCoins = 0;
        GameManager.keyRedFound = false;
	    GameManager.fireballSkill = false;
        imageDecimalNumberCoins.GetComponent<Image>().enabled = false;

        // se ejecuta la música
        musicFile = GetComponent<AudioSource>();
        if (GameManager.musicSettings)
        {
            musicFile.volume = 0.7f;
            musicFile.Play();
        }

        // relleno corazones
        imageHearth1.GetComponent<Image>().sprite = Resources.Load<Sprite>("hud_heartFull");
        imageHearth2.GetComponent<Image>().sprite = Resources.Load<Sprite>("hud_heartFull");
        imageHearth3.GetComponent<Image>().sprite = Resources.Load<Sprite>("hud_heartFull");
    }
	
	void Update () {

        UpdateCoinNumber(GameManager.currentNumberCoins);

        if (GameManager.keyRedFound)
        {
            imageKeyRedFound.GetComponent<Image>().sprite = Resources.Load<Sprite>("hud_keyRed");
        }

	    if (GameManager.fireballSkill)
	    {
	        imageGemYellow.enabled = true;
	    }
	    else
	    {
	        imageGemYellow.enabled = false;
	    }

        // corazones
        switch (GameManager.currentNumberHearth)
        {
            case 2:
                imageHearth3.GetComponent<Image>().sprite = Resources.Load<Sprite>("hud_heartEmpty");
                break;
            case 1:
                imageHearth2.GetComponent<Image>().sprite = Resources.Load<Sprite>("hud_heartEmpty");
                break;
            case 0:
                imageHearth1.GetComponent<Image>().sprite = Resources.Load<Sprite>("hud_heartEmpty");

                // volver a la pantalla de inicio
                GameManager.currentLevel = 0;
                SceneManager.LoadScene("StartScreen");
                break;
        }

	}

    void UpdateCoinNumber (int number)
    {
        string imageFileName;
        string CoinNumber = number.ToString();

        if (CoinNumber.Length > 1)
        {
            imageDecimalNumberCoins.GetComponent<Image>().enabled = true;
            imageFileName = "hud_" + CoinNumber[0];
            imageDecimalNumberCoins.GetComponent<Image>().sprite = Resources.Load<Sprite>(imageFileName);
            imageFileName = "hud_" + CoinNumber[1];
            imageUnitNumberCoins.GetComponent<Image>().sprite = Resources.Load<Sprite>(imageFileName);
        } else
        {
            imageFileName = "hud_" + CoinNumber;
            imageUnitNumberCoins.GetComponent<Image>().sprite = Resources.Load<Sprite>(imageFileName);
        }

    }
}
