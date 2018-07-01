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

    public GameObject fireball;
    private String fireballDirection = "right";

    public bool Grounded = true;
    private bool atTheDoor = false;

    public Transform myTransform;
    public LayerMask groundMask;
    private Animator myAnimator;
    private Rigidbody2D myBody;
    private SpriteRenderer mySpriteRenderer;

    public AudioSource musicJump;
    public AudioSource musicCoin;
    public AudioSource musicKey;
    public AudioSource characterDamaged;
    public AudioSource launchFireball;
    public AudioSource mushroomRed;

    private bool _damaged;
    private bool _coroutineColorCalled;
    // variables función Wait
    private float _timer;
    private float _timerMax;

    private Vector3 _lastGroundedPosition;

    void Start () {
        myAnimator = GetComponent<Animator>();
        myBody = GetComponent<Rigidbody2D>();
        myTransform = GetComponent<Transform>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        myWidth = mySpriteRenderer.bounds.extents.x;
        myHeight = mySpriteRenderer.bounds.extents.y;

        // sonidos para el salto, moneda y llave
        var audioSource = GetComponents<AudioSource>();
        musicJump = audioSource[0];
        musicCoin = audioSource[1];
        musicKey = audioSource[2];
        characterDamaged = audioSource[3];
        launchFireball = audioSource[4];
        mushroomRed = audioSource[5];
        _damaged = false;
        _coroutineColorCalled = false;
    }
	
	void Update ()
	{
	    Vector2 lineCastPosition = myTransform.position - Vector3.up * myHeight;
	    Debug.DrawLine(lineCastPosition, lineCastPosition + Vector2.down * 0.1f);
	    Grounded = Physics2D.Linecast(lineCastPosition, lineCastPosition + Vector2.down * 0.05f, groundMask);

        if (Grounded && Input.GetButtonDown("Jump"))
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
            _lastGroundedPosition = myTransform.position;
	        myAnimator.SetTrigger("Grounded");
	    }
	    else if (!_damaged)
	    {
	        myAnimator.SetTrigger("Jump");
	    }

        // movimiento lateral con teclas
        Speed = lateralMovement * Input.GetAxis("Horizontal");
        transform.Translate(Vector2.right * Speed * Time.deltaTime);
        myAnimator.SetFloat("Speed", Mathf.Abs(Speed));

        if (Speed < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            fireballDirection = "left";
        } else
        {
            transform.localScale = new Vector3(1, 1, 1);
            if (Speed != 0) fireballDirection = "right";
        }

	    if (Input.GetButtonDown("Fire1")) {
	        if (Fireball.fireballNumber < 3 && GameManager.fireballSkill)
	        {
	            if (GameManager.musicSettings){launchFireball.Play();}

	            GameObject fireballClone;
	            fireballClone = Instantiate(GameObject.FindGameObjectWithTag("Fireball"), transform.position, transform.rotation);
	            fireballClone.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
	            Vector2 vector2 = Vector2.right * 200f;
	            if (fireballDirection == "left") vector2 = Vector2.left * 200f;
	            fireballClone.GetComponent<Rigidbody2D>().AddForce(Vector2.up * 250f);
	            fireballClone.GetComponent<Rigidbody2D>().AddForce(vector2);
	        }
	    }

        // reinicia el nivel si se cae por un barranco o al agua
//        if (gameObject.transform.position.y < - 7)
//        {
//            GameManager.currentNumberHearth--;
//            if(GameManager.currentNumberHearth != 0) {
//                myTransform.position = _lastGroundedPosition;
//            }
//        }

	    // check si está dentro de la puerta con la llave y pulsa la tecla arriba
	    if (GameManager.keyRedFound && Input.GetAxis("Vertical") > 0 && atTheDoor)
	    {
            DoorFinishLevel();
	    }

	    // cambia de color al estar dañado y es inmune
	    if (_damaged)
	    {
	        if (!_coroutineColorCalled)
	        {
                StartCoroutine("color");
	        }

	        if (Waited(3)) {
	            _damaged = false;
	            _timer = 0;
	        }
	    }
//	    else
//	    {
//	        mySpriteRenderer.material.SetColor("_Color", Color.white);
//	    }

	    // espera 3 segundos para dejar de estar dañado
//	    if (_damaged) {
//	        if(Waited(3)) {
//	            _damaged = false;
//	            _timer = 0;
//	        }
//	    }
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
        {
            GameObject.Find("MainVirtual").GetComponent<CinemachineVirtualCamera>().enabled = false;
        }


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

        if (collider.tag == "MushroomRed")
        {
            if(GameManager.musicSettings){mushroomRed.Play();}
            GameManager.fireballSkill = true;
        }

        if (collider.tag == "DoorFinish")
        {
            atTheDoor = true;
        }
        // reinicia el nivel si se cae por un barranco o al agua
        if (collider.tag == "Void")
        {
            GameManager.currentNumberHearth--;
            if(GameManager.currentNumberHearth != 0) {
                myTransform.position = _lastGroundedPosition;
            }
        }

    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.tag == "Zoom")
        {
            GameObject.Find("MainVirtual").GetComponent<CinemachineVirtualCamera>().enabled = true;
        }

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
            if (!_damaged)
            {
                if (GameManager.musicSettings) characterDamaged.Play();
                _damaged = true;
                if (GameManager.fireballSkill)
                {
                    GameManager.fireballSkill = false;
                }
                else
                {
                    GameManager.currentNumberHearth--;
                }
            }
        }
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.tag == "MobilePlatform") transform.SetParent(null);
    }

    // función finaliza nivel
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

    // funciona para esperar x segundos
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

    // cambia de color después de recibir daño
    IEnumerator color(){
        while(_damaged)
        {
            _coroutineColorCalled = true;
            mySpriteRenderer.material.SetColor("_Color", new Color(0.9811321f, 0.5507298f, 0.5507298f));
            yield return new WaitForSeconds(0.3f);
            mySpriteRenderer.material.SetColor("_Color", Color.white);
            yield return new WaitForSeconds(0.3f);
        }
        _coroutineColorCalled = false;
    }
}
