using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialText : MonoBehaviour {
    public Text textTutorial;
    public bool coinObject = false;
    public bool keyObject = false;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

        if (GameManager.currentNumberCoins > 0 
            && GameManager.currentNumberCoins < 2 
            && !coinObject)
        {
            textTutorial.text = "You should collect coins";
        }
        else if (GameManager.currentNumberCoins > 1
            && GameManager.currentNumberCoins < 4
            && !coinObject)
        {
            textTutorial.text = "You can move yellow boxes";
        }
        else if (GameManager.currentNumberCoins > 3
            && GameManager.currentNumberCoins < 5
            && !coinObject)
        {
            textTutorial.text = "Well done!";
        }
        else if (GameManager.currentNumberCoins > 6 
            && !coinObject)
        {
            coinObject = true;
        }

        if (coinObject 
            && !GameManager.keyRedFound
            && !keyObject)
        {
            textTutorial.text = "You need a key to finish the level";
        }
        else if (GameManager.keyRedFound
            && !keyObject)
        {
            textTutorial.text = "Well done! Now open the door";
            keyObject = true;
        }

    }

}
