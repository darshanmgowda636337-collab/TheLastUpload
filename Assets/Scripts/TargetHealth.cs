using UnityEngine;

public class TargetHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;

    private float currentHealth;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        Debug.Log(
            name + " health: " +
            Mathf.Max(0f, currentHealth)
        );

        if (currentHealth <= 0f)
        {
            Destroy(gameObject);
        }
    }
}