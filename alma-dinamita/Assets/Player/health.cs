using UnityEngine.UI;
using UnityEngine;

public class health : MonoBehaviour
{
    private bool customHealthAllowed = false;
    private float maxHealth;
    private float customMaxHealth;
    private float playerHealth;
    private short gamemodeHealthType;
    [SerializeField] Text healthTextUI;

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
    public void GetDamageOnHealth(float damage)
    {
        playerHealth = playerHealth - damage;
    }

    public void GetHealed()
    {
        if (playerHealth >= maxHealth)
        {
            playerHealth = maxHealth;
            Debug.Log("Health is at it's max!");
        } else
        {
            playerHealth = playerHealth + 5.0f;
        }
    }

    private void GetDamageByFall()
    {

    }

    private void Start()
    {
        gamemodeHealthType = 0; // Classic health: 100
        UpdateGamemodeHealthType(true);
    }

    private void Update()
    {
        healthTextUI.text = playerHealth.ToString();
    }
}
