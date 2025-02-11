using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider healthSlider;
    public Image fillImage;
    public Gradient healthGradient; // Cambia el color según la vida

    public void SetMaxHealth(int maxHealth)
    {
        healthSlider.maxValue = maxHealth;
        healthSlider.value = maxHealth;
        fillImage.color = healthGradient.Evaluate(1f);
    }

    public void SetHealth(int currentHealth)
    {
        healthSlider.value = currentHealth;
        fillImage.color = healthGradient.Evaluate(healthSlider.normalizedValue);
    }
}
