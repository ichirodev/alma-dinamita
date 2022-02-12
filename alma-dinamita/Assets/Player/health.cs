using UnityEngine.UI;
using UnityEngine;

public class health : MonoBehaviour
{
    private bool customHealthAllowed = false;
    private float maxHealth;
    private float customMaxHealth;
    private float playerHealth;
    private short gamemodeHealthType;

    public void UpdateGamemodeHealthType(bool restorePlayerHealth)
    {
        if (customHealthAllowed)
        {
            maxHealth = customMaxHealth;
            if (restorePlayerHealth) playerHealth = maxHealth;
            return;
        }
        switch(gamemodeHealthType)
        {
            // 0: Classic gamemode
            // Max health: 100
            case 0:
                {
                    maxHealth = 100;
                    break;
                }
            // 1: Realism gamemode
            // Max health: 60
            case 1:
                {
                    maxHealth = 60;
                    break;
                }
            // 2: Competitive gamemode
            // Max health: 150
            case 2:
                {
                    maxHealth = 150;
                    break;
                }
            default:
                {
                    maxHealth = 100;
                    break;
                }
        }
        if (restorePlayerHealth) playerHealth = maxHealth;
    }
    public void SetDamageOnHealth(float damage)
    {
        var healthAfterDamage = playerHealth - damage;
        if (healthAfterDamage <= 0)
        {
            if (transform.name == "Player")
            {
                Debug.Log("You lost!");
            }
            else
            {
                fuckingDie();
                Debug.Log("xd die");
            }
            return;
        }
        playerHealth = healthAfterDamage;
    }
    public float GetPlayerHealth()
    {
        return playerHealth;
    }
    public void GetHealed(float healingPoints)
    {
        if (playerHealth >= maxHealth)
        {
            playerHealth = maxHealth;
            Debug.Log("Health is at it's max!");
        } else
        {
            playerHealth = playerHealth + healingPoints;
        }
    }

    private void fuckingDie()
    {
        Destroy(gameObject);
    }

    private void Start()
    {
        gamemodeHealthType = 0; // Classic health: 100
        UpdateGamemodeHealthType(true);
    }
}
