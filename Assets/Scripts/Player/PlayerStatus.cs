using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStatus : MonoBehaviour
{
    [SerializeField]
    PlayerController playerControllerScript;

   
    public int maxHeatlh;
    private int maxPosture = 100;

    public bool damaged;
    public bool parryed;
    public bool blocked;
    public bool finished;
    public bool weakened;
    public bool isDead;
    internal PlayerInput inputs;
    


    public float standTime = 4f;
    private float standTimeStart;




    private void Start()
    {
        playerControllerScript.currentHealth = maxHeatlh;
        playerControllerScript.currentPosture = 0;
        inputs = GetComponent<PlayerInput>();

        standTimeStart = standTime;

    }

    private void Update()
    {
        BrokePosture();
        LifeOver();

        if(weakened)
        {         
            standTime -= Time.deltaTime;
            if (standTime <= 0)
            {
                playerControllerScript.animator.SetBool("weakened", false);
                playerControllerScript.finisherArea.SetActive(false);           
                standTime = standTimeStart;
                inputs.actions.Enable();
                weakened = false;
            }
        }
        if (finished)
        {
            inputs.actions.Enable();
            playerControllerScript.animator.SetBool("weakened", false);
            playerControllerScript.combatBehavior.Finisher();
            playerControllerScript.finisherArea.SetActive(false);
            playerControllerScript.currentHealth = maxHeatlh;
            playerControllerScript.currentPosture = 0;
            playerControllerScript.lives -= 1;
            playerControllerScript.uiScript.RemoveLives(playerControllerScript.lives);
            finished = false;
           
        }
        if (playerControllerScript.lives == 0)
        {
            Death();
        }
    }

    public void TakeDamage(int damage)
    {
        playerControllerScript.currentHealth -= damage;
        playerControllerScript.postureResetTimer = playerControllerScript.timerStart;

    }
    public void DamagePosture(int damage)
    {
        playerControllerScript.currentPosture += damage;
        playerControllerScript.postureResetTimer = playerControllerScript.timerStart;
    }

    public void Death()
    {
        isDead = true;
        playerControllerScript.animator.SetBool("death", true);
        inputs.actions.Disable();
        FindObjectOfType<GameManager>().EndGame();
    }

    public void Weakened()
    {
        inputs.actions.Disable();
        playerControllerScript.animator.SetBool("weakened",true);
        weakened = true;
        playerControllerScript.finisherArea.SetActive(true);

    }

    void BrokePosture()
    {
       
        if(playerControllerScript.currentPosture >= maxPosture)
        {
            Weakened();
            playerControllerScript.currentPosture = 0;
        }        
        //si es que hace un parry con la postura al maximo
        if (playerControllerScript.currentPosture >= maxPosture && playerControllerScript.weaponScript.parrying)
        {
            playerControllerScript.currentPosture = maxPosture - 5;
        }
    }

    public void LifeOver()
    {      
  
        if(playerControllerScript.currentHealth <= 0)
        {
            Weakened();
            playerControllerScript.currentHealth = 0.1f;
        }
        
    }

}
