
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    [SerializeField]
    internal PlayerCollisions collisionsScript;

    [SerializeField]
    internal PlayerMovement movementScript;

    [SerializeField]
    internal PlayerWeapon weaponScript;

    [SerializeField]
    internal PlayerStatus statusScript;

    [SerializeField]
    internal UIVisuals uiScript;

    [SerializeField]
    internal CombatBehavior combatBehavior;


    //Player GameObject
    public Animator animator;
    internal Rigidbody2D rb;
    public GameObject finisherArea;
    private PlayerInput playerInput;
    private PlayerInput playerInputOtherPlayer;
    public GameObject pauseMenu;


    //Player health and posture

    public float currentHealth;
    public float currentPosture;

    internal float postureResetTimer = 0.5f;
    internal float timerStart;

    internal float postureResertVelocity;
    public int lives = 3;

    //Jump

    public float jumpForce;

    //Movement and inputs
    public float movementSpeed;
  
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();
        playerInputOtherPlayer = collisionsScript.otherPlayer.GetComponent<PlayerInput>();
        timerStart = postureResetTimer;
        QualitySettings.vSyncCount = 1;
        playerInput.actions.FindActionMap("UI").Enable();
        playerInputOtherPlayer.actions.FindActionMap("UI").Enable();
        playerInput.actions.FindActionMap("Player").Enable();
        playerInputOtherPlayer.actions.FindActionMap("Player").Enable();

    }

    private void OnEnable()
    {
        playerInput.actions["PauseMenu"].performed += PauseGame;
        playerInputOtherPlayer.actions["PauseMenu"].performed += PauseGame;

        playerInput.actions["Resume"].performed += ResumeGame;
        playerInputOtherPlayer.actions["Resume"].performed += ResumeGame;

    }

    private void OnDisable()
    {
        playerInput.actions["PauseMenu"].performed -= PauseGame;
        playerInput.actions["Resume"].performed -= ResumeGame;
    

    }

    private void PauseGame(InputAction.CallbackContext context)
    {
        if (pauseMenu != null)
        {
            pauseMenu.SetActive(true);
        }
        Time.timeScale = 0;
    }

    private void ResumeGame(InputAction.CallbackContext context)
    {
        if (pauseMenu != null)
        {
            pauseMenu.SetActive(false);
        }
        
        Time.timeScale = 1;
        playerInput.SwitchCurrentActionMap("Player");
        playerInputOtherPlayer.SwitchCurrentActionMap("Player");
        playerInput.actions.FindActionMap("UI").Disable();
        playerInputOtherPlayer.actions.FindActionMap("UI").Disable();
        playerInput.actions.FindActionMap("Player").Enable();
        playerInputOtherPlayer.actions.FindActionMap("Player").Enable();
    }

    public void ResumeGameButton()
    {
        Time.timeScale = 1;
    }


    //Sistema de subida y bajada de postura
    private void Update()
    {
        if(pauseMenu.activeInHierarchy)
        {
            playerInput.SwitchCurrentActionMap("UI");
            playerInputOtherPlayer.SwitchCurrentActionMap("UI");
            playerInputOtherPlayer.actions.FindActionMap("UI").Enable();
            playerInput.actions.FindActionMap("Player").Disable();
            playerInputOtherPlayer.actions.FindActionMap("Player").Disable();
        }

  
        if (currentPosture >= 0)
        {
            postureResetTimer -= Time.deltaTime;

            if (postureResetTimer <= 0)
            {
                if(weaponScript.blocking || !movementScript.isGrounded || weaponScript.attacking)
                {
                    postureResertVelocity = 2;
                }
                else if (movementSpeed != 0)
                {
                    postureResertVelocity = 3;
                }
                else
                {
                    postureResertVelocity = 5;
                }
                //Si la postura está bajo 25%
                if(currentPosture < 25)
                {
                    currentPosture -= 5f * Time.deltaTime * postureResertVelocity;
                }
                //Si la postura está sobre 25% y bajo 50%
                if (currentPosture >= 25 && currentPosture < 50)
                {
                    currentPosture -= 4f * Time.deltaTime * postureResertVelocity;
                }
                //si la postura está Sobre 50% y bajo 75%
                if (currentPosture >= 50 && currentPosture < 75)
                {
                    currentPosture -= 3f * Time.deltaTime * postureResertVelocity;
                }
                //si La postura está sobre 75%
                if (currentPosture >= 75)
                {
                    currentPosture -= 2f * Time.deltaTime * postureResertVelocity;
                }
            }         
        }
        if (currentPosture < 0)
        {
            currentPosture = 0;
        }
    }

    




}

