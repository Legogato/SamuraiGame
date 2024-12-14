using UnityEngine;
using MilkShake;

public class CombatBehavior : MonoBehaviour
{
    [SerializeField]
    PlayerController playerControllerScript;

    public Shaker MyShaker;
    public ShakePreset punchShake;
    public ShakePreset parryShake;
    public ShakePreset blockShake;

    public float hitTimer = 0.3f;



    private void FixedUpdate()
    {

        if (playerControllerScript.statusScript.blocked)
        {
            BlockedAttack();
            MyShaker.Shake(blockShake);
            Instantiate(playerControllerScript.weaponScript.blockParticles, playerControllerScript.weaponScript.attackPoint.position, Quaternion.Euler(0, 90, 0));
            Debug.Log(gameObject + " ha bloqueado el ataque");
            playerControllerScript.statusScript.blocked = false;

        }
        if (playerControllerScript.statusScript.parryed)
        {
            ParryedAttack();
            MyShaker.Shake(parryShake);
            Instantiate(playerControllerScript.weaponScript.parryParticles, playerControllerScript.weaponScript.attackPoint.position,Quaternion.Euler(0, 90, 0));
            Debug.Log(gameObject + "ha hecho un parry");
            playerControllerScript.statusScript.parryed = false;
        }
        if (playerControllerScript.statusScript.damaged)
        {
            Damaged();
            playerControllerScript.statusScript.damaged = false;
            MyShaker.Shake(punchShake);
            Debug.Log(gameObject + " le hicieron daño");
            
        }


    }



    //Se bloquea el ataque del otro
    public void BlockedAttack()
    {

        if (playerControllerScript.movementScript.isFacingRight)
        {

            playerControllerScript.rb.AddForce(new Vector2(-2000, 0));
        }
        else
        {

            playerControllerScript.rb.AddForce(new Vector2(+2000, 0));
        }
    }
    //Se le hace un parry al otro
    public void ParryedAttack()
    {
        playerControllerScript.collisionsScript.otherPlayer.GetComponent<PlayerStatus>().DamagePosture(20);
        if (playerControllerScript.movementScript.isFacingRight)
        {

            playerControllerScript.rb.AddForce(new Vector2(-3000, 0f));
        }
        else
        {
            playerControllerScript.rb.AddForce(new Vector2(3000, 0f));
        }
    }

    //Se recibe daño sin block ni parry
    public void Damaged()
    {      
        playerControllerScript.animator.SetTrigger("damaged");

        if (playerControllerScript.movementScript.isFacingRight)
        {


            playerControllerScript.rb.AddForce(new Vector2(-2300, 0f));
        }
        else
        {
            playerControllerScript.rb.AddForce(new Vector2(2300, 0f));
        }
    }

    public void Finisher()
    {

        Debug.Log(gameObject + " le han hecho un finisher");
        if (playerControllerScript.movementScript.isFacingRight)
        {
            playerControllerScript.rb.AddForce(new Vector2(-5000, 0f));
        }
        else
        {
            playerControllerScript.rb.AddForce(new Vector2(5000, 0f));
        }

    }

}
