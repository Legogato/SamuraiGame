using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWeapon : MonoBehaviour
{

    [SerializeField]
    PlayerController playerControllerScript;

    //attack
    internal bool canAttack;
    internal bool attacking;
    public Transform attackPoint;
    public float attackRange = 0.5f;
    private float frameCount = 0;
    private float attackTimer = 0.4f;
    private float attackTimerStart;

    //block
    internal bool blocking;
    public GameObject blockZone;
    public GameObject blockParticles;

    //parry
    public GameObject parryZone;
    public GameObject parryParticles;
    internal bool parrying;

    private void Start()
    {

        canAttack = true;
        attackTimerStart = attackTimer;

    }
    private void Update()
    {
        if (parryZone.activeInHierarchy)
        {
            frameCount += 1f * Time.deltaTime;
            if (frameCount >= 0.2 || !blocking)
            {
                parryZone.SetActive(false);
                parrying = false;
                frameCount = 0;
            }
        }
        if (attacking)
        {
            attackTimer -= Time.deltaTime;
            if(attackTimer <= 0)
            {
                canAttack = true;
                attacking = false;
                attackTimer = attackTimerStart;
            }
        }
        
    }
    //Puede bloquear, se usa en el animation actions para que al atacar bloqueando vuelva a la velocidad de bloqueo
    public void EnableBlock()
    {
        blockZone.SetActive(true);
        blocking = true;
        playerControllerScript.movementSpeed = playerControllerScript.movementScript.moveSpeedBlocking;
    }

    //Block
    public void Block(InputAction.CallbackContext context)
    {
        parrying = true;
        parryZone.SetActive(true);
        canAttack = true;

        blocking = true;
        playerControllerScript.animator.SetBool("block", true);

        if (context.canceled)
        {
            blocking = false;
            blockZone.SetActive(false);
            playerControllerScript.animator.SetBool("block", false);
            playerControllerScript.movementSpeed = playerControllerScript.movementScript.moveSpeedStart;
        }

    }

    // performs the attack animation and the attack radio
    public void Attack(InputAction.CallbackContext context)
    {

        if (canAttack && context.started)
        {
            attacking = true;
            blocking = false;
            parrying = false;
            blockZone.SetActive(false);
            parryZone.SetActive(false);
            canAttack = false;
            playerControllerScript.animator.SetTrigger("attack");
            playerControllerScript.movementSpeed = 0;

        }

    }

    //Start the attack with a little impulse
    public void StartAttack() 
    {
        playerControllerScript.movementSpeed = playerControllerScript.movementScript.moveSpeedStart;
        
        if (playerControllerScript.movementScript.isFacingRight)
        {
            playerControllerScript.rb.AddForce(new Vector2(2000, 0));
        }
        else
        {
            playerControllerScript.rb.AddForce(new Vector2(-2000, 0));
        }
    }

    private void OnDrawGizmosSelected()
    {
        if(attackPoint == null)
        {
            return;
        }
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);

    }

}
