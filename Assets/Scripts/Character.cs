using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.SceneManagement;

public class Character : MonoBehaviour {
    public float Speed = 0.0f;
    public float lateralMovement = 2.0f;
    public float jumpMovement = 400.0f;

    public Transform groundCheck;

    private Animator animator;
    private Rigidbody2D rigidbody2D;

    public bool grounded = true;

    public AudioSource musicJump;
    public AudioSource musicCoin;
    public AudioSource musicKey;

    void Start () {
        animator = GetComponent<Animator>();
        rigidbody2D = GetComponent<Rigidbody2D>();

        // sonidos para el salto, moneda y llave
        var audioSource = GetComponents<AudioSource>();
        musicJump = audioSource[0];
        musicCoin = audioSource[1];
        musicKey = audioSource[2];
	}
	
	void Update () {
        grounded = Physics2D.Linecast(
            transform.position,
            groundCheck.position,
            1 << LayerMask.NameToLayer("Ground"));

        if (grounded && Input.GetButtonDown("Jump"))
        {
            rigidbody2D.AddForce(Vector2.up * jumpMovement);

            // reproduce el audio cuando salta
            if (GameManager.musicSettings)
            {
                musicJump.Play();
            }
        }

        Speed = lateralMovement * Input.GetAxis("Horizontal");
        transform.Translate(Vector2.right * Speed * Time.deltaTime);
        animator.SetFloat("Speed", Mathf.Abs(Speed));

        if (Speed < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        } else
        {
            transform.localScale = new Vector3(1, 1, 1);
        }

        // restart level si se cae por un barranco o al agua
        if (gameObject.transform.position.y < - 7)
        {
            GameManager.currentNumberHearth--;
            gameObject.GetComponent<Rigidbody2D>().transform.position = new Vector2(-8f, -3.75f);
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

        
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.tag == "Zoom")
            GameObject.Find("MainVirtual").GetComponent<CinemachineVirtualCamera>().enabled = true;
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        // fin de nivel, jugador entra por la puerta
        if (collision.gameObject.tag == "DoorFinish"
            && GameManager.keyRedFound)
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
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "MobilePlatform") transform.SetParent(other.transform);
    }
    void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.tag == "MobilePlatform") transform.SetParent(null);
    }

}
