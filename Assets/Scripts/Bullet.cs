using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bullet : MonoBehaviour {

    public float speed = 20f;
    public Rigidbody2D rb2D;
    public int bulletDamage=100;
    public AudioClip shootSound;

	// Use this for initialization
	void Start () {
        rb2D.velocity = transform.right * speed;
        SoundManager.instance.RandomizeSfx(shootSound, shootSound);
	}

    private void OnTriggerEnter2D(Collider2D hitInfo)
    {

        EnemyController enemy = hitInfo.GetComponent<EnemyController>();
        if(enemy != null)
        {
            enemy.TakeDamage(bulletDamage);
        }
        Destroy(gameObject);
    }
}
