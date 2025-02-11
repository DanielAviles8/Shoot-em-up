using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public enum PowerUpType { Speed, Damage, Shield }
    public PowerUpType powerUpType;
    public float duration = 5f;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();

            switch (powerUpType)
            {
                case PowerUpType.Speed:
                    player.speed *= 1.5f;
                    Invoke("ResetSpeed", duration);
                    break;

                case PowerUpType.Damage:
                    player.fireRate /= 2;
                    Invoke("ResetDamage", duration);
                    break;

                case PowerUpType.Shield:
                    player.ActivateShield();
                    break;
            }

            gameObject.SetActive(false);
        }
    }

    void ResetSpeed()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().speed /= 1.5f;
    }

    void ResetDamage()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().fireRate *= 2;
    }
}
