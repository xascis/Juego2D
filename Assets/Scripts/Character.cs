using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.SceneManagement;

public class Character : MonoBehaviour
{
    public float Speed;
    public float lateralMovement = 2.0f;
    public float jumpMovement = 400.0f;
    private float myWidth, myHeight;

    public bool Grounded = true;
    private bool atTheDoor = false;

    public Transform myTransform;
    public LayerMask groundMask;
    private Animator myAnimator;
    private Rigidbody2D myBody;

    public AudioSource musicJump;
    public AudioSource musicCoin;
    public AudioSource musicKey;
    public AudioSource characterDamaged;

    private bool _damaged;
    private float _timer;
    private float _timerMax;

    void Start () {
        myAnimator = GetComponent<Animator>();
        myBody = GetComponent<Rigidbody2D>();
        myTransform = transform;
        SpriteRenderer mySpriteRenderer = GetComponent<SpriteRenderer>();
        myWidth = mySpriteRenderer.bounds.extents.x;
        myHeight = mySpriteRenderer.bounds.extents.y;

        // sonidos para el salto, moneda y llave
        var audioSource = GetComponents<AudioSource>();
        musicJump = audioSource[0];
        musicCoin = audioSource[1];
        musicKey = audioSource[2];
        characterDamaged = audioSource[3];
        _damaged = false;
	}
	
	void Update ()
	{
        print(Grounded);

	    Vector2 lineCastPosition = myTransform.position - Vector3.up * myHeight;
	    Debug.DrawLine(lineCastPosition, lineCastPosition + Vector2.down * 0.1f);
	    Grounded = Physics2D.Linecast(lineCastPosition, lineCastPosition + Vector2.down * 0.1f, groundMask);

        if (Grounded && Input.GetButtonDown("Jump") && !_damaged)
        {
            myBody.AddForce(Vector2.up * jumpMovement);

            // reproduce el audio cuando salta
            if (GameManager.musicSettings)
            {
                musicJump.Play();
            }
        }

	    // animación
	    if (Grounded)
	    {
	        myAnimator.SetTrigger("Grounded");
	    }
	    else
	    {
	        myAnimator.SetTrigger("Jump");
	    }
        // espera 2 segundos para dejar de estar dañado
        if(_damaged){
            if(Waited(2)) {
                _damaged = false;
                _timer = 0;
            }
        }

        // movimiento lateral con teclas
        Speed = lateralMovement * Input.GetAxis("Horizontal");
        transform.Translate(Vector2.right * Speed * Time.deltaTime);
        myAnimator.SetFloat("Speed", Mathf.Abs(Speed));

        if (Speed < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        } else
        {
            transform.localScale = new Vector3(1, 1, 1);
        }

        // reinicia el nivel si se cae por un barranco o al agua
        if (gameObject.transform.position.y < - 7)
        {
            GameManager.currentNumberHearth--;
            gameObject.GetComponent<Rigidbody2D>().transform.position = new Vector2(-8f, -3.75f);
        }

	    // check si está dentro de la puerta con la llave y pulsa la tecla arriba
	    if (GameManager.keyRedFound && Input.GetAxis("Vertical") > 0 && atTheDoor)
	    {
            DoorFinishLevel();
	    }
	}

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "CoinGold")
        {
            GameManager.currentNumberCoins++;

            // sonido al recoger la moneda
            if (GameManager.musicSettings)
            {
                musicCoin.volume = 0.5f;
                musicCoin.Play();
            }

            Destroy(collider.gameObject);
        }

        if (collider.tag == "Zoom")
            GameObject.Find("MainVirtual").GetComponent<CinemachineVirtualCamera>().enabled = false;

        // cuando el jugador encuentra la llave
        if (collider.tag == "Key")
        {
            // reproduce el audio cuando coge la llave
            if (GameManager.musicSettings)
            {
                musicKey.Play();
            }
            GameManager.keyRedFound = true;
            Destroy(collider.gameObject);
        }

        if (collider.tag == "DoorFinish")
        {
            atTheDoor = true;
        }

    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.tag == "Zoom")
            GameObject.Find("MainVirtual").GetComponent<CinemachineVirtualCamera>().enabled = true;

        if (collider.tag == "DoorFinish")
        {
            atTheDoor = false;
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "MobilePlatform") transform.SetParent(other.transform);

        // si un enemigo toca al personaje
        if (other.gameObject.tag == "Enemy")
        {
            _damaged = true;
            myAnimator.SetTrigger("Damaged");
            if (GameManager.musicSettings) characterDamaged.Play();
            // GameManager.currentNumberHearth--;
        }
    }
    void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.tag == "MobilePlatform") transform.SetParent(null);
    }

    void DoorFinishLevel()
    {
        // el jugador está en el nivel del tutorial, pasa a la pantalla de inicio
        if (GameManager.currentLevel == 0)
        {
            SceneManager.LoadScene("StartScreen");
        } else
        {
            // pasa al siguiente nivel
            GameManager.currentLevel++;
            if (GameManager.currentLevel > GameManager.maxLevel)
            {
                SceneManager.LoadScene("StartScreen");
            }
            string nextLevel = "Level" + GameManager.currentLevel;
            SceneManager.LoadScene(nextLevel);
        }
    }

    private bool Waited(float seconds)
    {
        _timerMax = seconds;
        _timer += Time.deltaTime;
        if (_timer >= _timerMax)
        {
            return true; //max reached - waited x - seconds
        }
        return false;
    }
}
