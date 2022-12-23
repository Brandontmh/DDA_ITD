using UnityEngine;
public class Target : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public int bossHealth = 1000;

    public HealthBar healthBar;
    private void Start()
    {   
        if (GameObject.FindGameObjectWithTag("Boss"))
        {
            currentHealth = bossHealth;
            healthBar.SetMaxHealth(bossHealth);
        }

        else
        {
            currentHealth = maxHealth;
            healthBar.SetMaxHealth(maxHealth);
        }
    }
    public void TakeDamage(int amount)
    {
        currentHealth -= amount;

        healthBar.SetHealth(currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }
}

    