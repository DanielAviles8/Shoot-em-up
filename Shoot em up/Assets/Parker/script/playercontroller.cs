using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Movimiento")]
    public float speed = 5f;
    public float acceleration = 2f;
    private Vector2 movement;
    private Rigidbody2D rb;

    [Header("Disparo")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletSpeed = 10f;
    public float fireRate = 0.5f;
    private float lastShotTime;
    

    [Header("Dash")]
    public float dashSpeed = 10f;
    public float dashCooldown = 2f;
    private float lastDashTime;

    [Header("Vida")]
    public int maxHealth = 5;
    private int currentHealth;
    public HealthBar healthBar;

    [Header("Power-ups")]
    public bool isShielded = false;
    public float powerUpDuration = 5f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    void Update()
    {
        // Movimiento
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        movement = new Vector2(moveX, moveY).normalized * speed;

        // Disparo con cooldown
        if (Input.GetKey(KeyCode.Space) && Time.time > lastShotTime + fireRate)
        {
            Shoot();
            lastShotTime = Time.time;
        }

        // Dash (Esquiva rápida)
        if (Input.GetKeyDown(KeyCode.LeftShift) && Time.time > lastDashTime + dashCooldown)
        {
            Dash();
            lastDashTime = Time.time;
        }
    }

    void FixedUpdate()
    {
        rb.velocity = Vector2.Lerp(rb.velocity, movement, acceleration * Time.fixedDeltaTime);
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rbBullet = bullet.GetComponent<Rigidbody2D>();
        rbBullet.velocity = firePoint.right * bulletSpeed;
        Destroy(bullet, 2f);
    }

    void Dash()
    {
        rb.velocity *= dashSpeed;
    }

    public void TakeDamage(int damage)
    {
        if (isShielded) return;

        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Jugador Muerto");
        gameObject.SetActive(false);
    }

    public void ActivateShield()
    {
        isShielded = true;
        Invoke("DeactivateShield", powerUpDuration);
    }

    void DeactivateShield()
    {
        isShielded = false;
    }
}
