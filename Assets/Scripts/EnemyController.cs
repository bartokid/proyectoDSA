using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

    public float maxSpeed = 2f;
    public float speed = 2f;
    public int damage = 15;
    public GameObject player;
    public GameObject HollyWater;
    public Transform playerToFollow=null;
    public Animator animator;
    public int health = 100;
    public AudioClip attackSound;




    private Rigidbody2D rb2D;
	// Use this for initialization
	void Start () {
        rb2D = GetComponent<Rigidbody2D>();
        player = GetComponent<GameObject>();
        HollyWater = GetComponent<GameObject>();
        playerToFollow = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        animator = GetComponent<Animator>();

    }
	
	// Update is called once per frame
	void FixedUpdate () {
        rb2D.AddForce(Vector2.right * speed);
        if (rb2D.velocity.x > maxSpeed)
            rb2D.velocity = new Vector2(maxSpeed, rb2D.velocity.y);
        else if (rb2D.velocity.x < -maxSpeed)
            rb2D.velocity = new Vector2(-maxSpeed, rb2D.velocity.y);

        transform.LookAt(transform.position);
        
        transform.position = Vector2.MoveTowards(transform.position, playerToFollow.transform.position, speed*0.03f);
        //transform.up = playerToFollow.position - transform.position;

    }
    private void Update()
    {
       
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
        else
        {
            animator.SetTrigger("EnemyAttack");
        }
            
    }

    void Die()
    {
        animator.SetTrigger("EnemyDeath 0");
        Destroy(gameObject,0.8f);
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController player = other.GetComponent<PlayerController>();
        if (player != null)
        {
            player.TakeDamage(damage);
            SoundManager.instance.RandomizeSfx(attackSound, attackSound);
            animator.SetTrigger("EnemyAttack");
        }

        if (other.tag == "Player")
        {
            animator.SetTrigger("EnemyAttack");
        }
    }
}
