using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    [Header("Propiedades Generales")]
    public float speed = 2f;
    public int health = 3;
    public int damage = 1;
    public float attackCooldown = 1f;
    public bool isBoss = false;

    [Header("Efectos y Sonidos")]
    public GameObject explosionEffect;
    public AudioClip hitSound;
    public AudioClip deathSound;
    private AudioSource audioSource;
    
    [Header("Esquivar Balas")]
    public float dodgeCooldown = 3f;
    private float lastDodgeTime;

    [Header("Disparo del Jefe")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletSpeed = 5f;
    public float fireRate = 2f;
    private float lastFireTime;

    private Transform player;
    private SpriteRenderer spriteRenderer;
    private float lastAttackTime;
     public int maxHealth = 5;
    private int currentHealth;
    public HealthBar healthBar;


    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    void Update()
    {
        if (player != null)
        {
            // Movimiento hacia el jugador
            transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);

            // Rotación hacia el jugador
            Vector2 direction = (player.position - transform.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, angle);

            // Esquiva de balas
            if (Time.time > lastDodgeTime + dodgeCooldown)
            {
                Dodge();
                lastDodgeTime = Time.time;
            }

            // Ataque del jefe
            if (isBoss && Time.time > lastFireTime + fireRate)
            {
                Shoot();
                lastFireTime = Time.time;
            }
        }
    }

    public void TakeDamage(int damageAmount)
    {
        health -= damageAmount;
        StartCoroutine(FlashRed());

        if (audioSource && hitSound)
        {
            audioSource.PlayOneShot(hitSound);
        }

        // Aumenta velocidad al recibir daño (desesperación)
        speed += 0.2f;
        currentHealth -= damageAmount;
        healthBar.SetHealth(currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
        }

        if (audioSource && deathSound)
        {
            audioSource.PlayOneShot(deathSound);
        }

        if (isBoss)
        {
            // Explosión en área (daño a todo alrededor)
            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, 2f);
            foreach (Collider2D collider in hitColliders)
            {
                if (collider.CompareTag("Player"))
                {
                    collider.GetComponent<Player>().TakeDamage(2);
                }
            }
        }

        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet"))
        {
            TakeDamage(1);
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("Player") && Time.time >= lastAttackTime + attackCooldown)
        {
            other.GetComponent<Player>().TakeDamage(damage);
            lastAttackTime = Time.time;
        }
    }

    void Dodge()
    {
        Vector2 dodgeDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
        transform.position += (Vector3)dodgeDirection * 1.5f;
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.velocity = firePoint.right * bulletSpeed;
        Destroy(bullet, 3f);
    }

    IEnumerator FlashRed()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = Color.white;
    }
}
