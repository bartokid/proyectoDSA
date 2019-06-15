using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class PlayerController : MonoBehaviour {

    public int difficulty = 4;
    public float speed = 12f;
    public float maxSpeed = 10f;
    public float jumpPower =6.5f;
    public double food=100;
    public int hollyWater=100;
    public int wallDamage = 1;
    public int pointsPerFood = 15;
    public float restartLevelDelay = 1f;
    public int  enemyDamage =10;
    public Text foodText;
    public Text hollyWaterText;
    public AudioClip eatSound;
    public AudioClip waterDropSound;
    public Joystick joystick;


    private bool wand = false;
    private bool jump = false;
    private Rigidbody2D rb2D;
    private Animator animator;
    private bool change = false;
    private Vector2 touchOrigin = -Vector2.one;
    private float horizontal;
    private float vertical;
    private float timeLeft=1000f;


    
    // Use this for initialization
    void Start () {

        food = GameManager.instance.playerFoodPoints;
        hollyWater = GameManager.instance.playerHollyWaterPoints;

        animator = GetComponent<Animator>();
        rb2D = GetComponent<Rigidbody2D>();

        foodText.text = "Food: " + food;
        hollyWaterText.text = "HollyWater: " + hollyWater;

       
       

    }

    private void OnDisable()
    {
        GameManager.instance.playerFoodPoints = (int)food;
        GameManager.instance.playerHollyWaterPoints = hollyWater;
    }


    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown(KeyCode.UpArrow) || joystick.Vertical>0 ) // both conditions can be in the same branch
        {
            food=food-0.01* difficulty;
            CheckIfGameOver();
            foodText.text = "Food: " + (int)food;
            jump = true;
           
        }

        if (Input.GetButtonDown("Fire1")&& wand)
        {
            animator.SetTrigger("Shoot");

        }
    }

     void FixedUpdate()
    {

        horizontal = Input.GetAxis("Horizontal");
        horizontal = joystick.Horizontal;


        rb2D.AddForce(Vector2.right * speed * horizontal);

        if (rb2D.velocity.x > maxSpeed)
            rb2D.velocity = new Vector2(maxSpeed, rb2D.velocity.y);
        else if (rb2D.velocity.x < -maxSpeed)
            rb2D.velocity = new Vector2(-maxSpeed, rb2D.velocity.y);
        
        if(horizontal < 0f && !change)
        {
            transform.Rotate(0f, 180f, 0f);
            change = true;
            
        }else if (horizontal > 0f && change)
        {
            change = false;
            transform.Rotate(0f,180f,0f);
            
        }

        if (jump)
        {
            rb2D.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            jump = false;
        }
    }

    //private void Restart()//aquí se cargaria la nueva escena
    //{
    //    //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
    //    Application.LoadLevel(Application.loadedLevel);
    //}

    public void LoseFood(int loss)
    {
        animator.SetTrigger("PlayerHit");
        food -= loss;
        foodText.text = "Food: " + (int)food;
        CheckIfGameOver();
    }

    private void CheckIfGameOver()
    {
        if (food <= 0)
        {
            GameManager.instance.GameOver();
            SoundManager.instance.musicSource.Stop();
            //Destroy(gameObject);
        }
    }

    public void TakeDamage(int damage)
    {
        food -= damage;
        animator.SetTrigger("Hit");
        if (food <= 0)
        {
            CheckIfGameOver();
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Exit")
        {
            Invoke("Restart", restartLevelDelay);
            enabled = false;
        }
        else if (other.tag == "Food")
        {
            food += pointsPerFood;
            foodText.text = "Food: " + (int)food;
            SoundManager.instance.RandomizeSfx(eatSound, eatSound);
            other.gameObject.SetActive(false);
        }
        else if(other.tag == "HollyWater")
        {
            hollyWater++;
            hollyWaterText.text = "HollyWater: " + hollyWater;
            SoundManager.instance.RandomizeSfx(waterDropSound, waterDropSound);
            other.gameObject.SetActive(false); 
        }
        else if (other.tag == "Varita")
        {
            wand = true;
            other.gameObject.SetActive(false);
        }

         else if (other.tag == "Enemy")
        {
            food -= enemyDamage;
            foodText.text = "Food: " + (int)food;
            animator.SetTrigger("Hit");
            
        }

    }

    private void OnBecameInvisible()
    {
        food -= 20;
        transform.position = new Vector3(0,5, 0);
    }
}
