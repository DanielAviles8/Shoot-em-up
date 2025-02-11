using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    public int damage = 1;
    public GameObject impactEffect;
    public AudioClip impactSound;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        Destroy(gameObject, 3f); // Destruir después de 3 segundos si no impacta
    }

    void Update()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<Enemy>().TakeDamage(damage);
            ShowImpactEffect();
            Destroy(gameObject);
        }
        else if (other.CompareTag("Wall")) // La bala se destruye si choca con una pared
        {
            ShowImpactEffect();
            Destroy(gameObject);
        }
    }

    void ShowImpactEffect()
    {
        if (impactEffect != null)
            Instantiate(impactEffect, transform.position, Quaternion.identity);

        if (impactSound != null && audioSource != null)
            audioSource.PlayOneShot(impactSound);
    }
}
