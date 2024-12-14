using UnityEngine;

public class PlayerCollisions : MonoBehaviour
{
    [SerializeField]
    PlayerController playerControllerScript;

    public LayerMask enemyLayer;
    public LayerMask blockLayer;
    public LayerMask parryLayer;
    public LayerMask finisherLayer;
    public GameObject otherPlayer;


    internal bool otherPlayerBlocked;
    internal bool otherPlayerDamaged;
    internal bool otherPlayerParryed;
    internal bool finisher = false;



    public void DetectAttack()
    {

        bool enemyHit = Physics2D.OverlapCircle(playerControllerScript.weaponScript.attackPoint.position, playerControllerScript.weaponScript.attackRange, enemyLayer);
        bool blockHit = Physics2D.OverlapCircle(playerControllerScript.weaponScript.attackPoint.position, playerControllerScript.weaponScript.attackRange, blockLayer);   
        bool parryHit = Physics2D.OverlapCircle(playerControllerScript.weaponScript.attackPoint.position, playerControllerScript.weaponScript.attackRange, parryLayer);
        bool finisherHit = Physics2D.OverlapCircle(playerControllerScript.weaponScript.attackPoint.position, playerControllerScript.weaponScript.attackRange, finisherLayer);

        //Si el otro hace parry
        if (finisherHit)
        {        
            otherPlayer.GetComponent<PlayerStatus>().finished = true;
        }
  
        if (parryHit)
        {

            //Object
            otherPlayer.GetComponent<PlayerStatus>().DamagePosture(5);
            //Damage
            otherPlayer.GetComponent<UIVisuals>().DamagePosture(0.05f);
            otherPlayerParryed = true;
            otherPlayer.GetComponent<PlayerStatus>().parryed = true;
        }
        //Si el otro está bloqueando
        if (blockHit && !parryHit) 
        {

            //UI
            otherPlayer.GetComponent<UIVisuals>().DamagePosture(0.2f);
            otherPlayer.GetComponent<UIVisuals>().DamageHealth(0.02f);
            //Object
            otherPlayer.GetComponent<PlayerStatus>().TakeDamage(2);
            otherPlayer.GetComponent<PlayerStatus>().DamagePosture(20);
            otherPlayerBlocked = true;
            otherPlayer.GetComponent<PlayerStatus>().blocked = true;
       
        }
        //Si el otro está descubierto
        if(enemyHit && !parryHit && !blockHit && !finisher)
        {
            //UI
            otherPlayer.GetComponent<UIVisuals>().DamageHealth(0.2f);
            otherPlayer.GetComponent<UIVisuals>().DamagePosture(0.1f);
            //Object
            otherPlayer.GetComponent<PlayerStatus>().TakeDamage(20);
            otherPlayer.GetComponent<PlayerStatus>().DamagePosture(10);
            otherPlayerDamaged = true;
            otherPlayer.GetComponent<PlayerStatus>().damaged = true;
        }
        

    }

   

}
