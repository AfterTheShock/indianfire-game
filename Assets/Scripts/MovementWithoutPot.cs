using UnityEngine;

public class MovementWithoutPot : MonoBehaviour
{
    [SerializeField] private Vector2 acceleration;
    [SerializeField] private Vector2 deceleration;
    [SerializeField] private Vector2 maxSpeed;
    [SerializeField] private LayerMask climbLayerMask;

    [SerializeField] Transform playerVisuals;

    [SerializeField] Animator animator;

    private PotOnHandManager potOnHandManager;
    
    private bool canClimb;
    private bool canExtinguishFire;
    private bool isClimbing;
    private bool canGrabPot;
    
    private Rigidbody2D rb2d;

    private Vector2 movementInput;
    
    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        potOnHandManager = GetComponent<PotOnHandManager>();
    }

    private void Update()
    {
        Debug.Log(canGrabPot);
        
        if (canExtinguishFire)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("Extinguish Fire Hut");
            }
        }

        if (canGrabPot)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                potOnHandManager.GrabPot();
                canGrabPot = false;
            }
        }

        ManagePlayerAnimations();
    }

    private void FixedUpdate()
    {
        movementInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        Vector2 movementAbsInput = new Vector2(Mathf.Abs(movementInput.x), Mathf.Abs(movementInput.y));

        Vector2 direction = movementInput.normalized;
        
        if (canClimb)
        {
            if (movementAbsInput.y > 0f) isClimbing = true;
        }
        
        if (!isClimbing)
        {
            rb2d.gravityScale = 1f;
            
            rb2d.linearVelocity = direction * new Vector2(
                Mathf.Clamp(movementAbsInput.x * acceleration.x, -maxSpeed.x, maxSpeed.x) * Time.fixedDeltaTime, 
                0f) + new Vector2(0f, rb2d.linearVelocity.y);
        }
        else
        {
            rb2d.gravityScale = 0f;
            
            rb2d.linearVelocity = direction * new Vector2(
                Mathf.Clamp(movementAbsInput.x * acceleration.x, -maxSpeed.x, maxSpeed.x), 
                Mathf.Clamp(movementAbsInput.y * acceleration.y, -maxSpeed.y, maxSpeed.y)) * Time.fixedDeltaTime;
        }
    }
    
    private void ManagePlayerAnimations()
    {
        if (movementInput.x != 0) animator.Play("WalkWithoutPotPlayer");
        else animator.Play("IdleWithoutPotPlayer 1");
        if(movementInput.x > 0) playerVisuals.localScale = new Vector3(1,1,1);
        if(movementInput.x < 0) playerVisuals.localScale = new Vector3(-1,1,1);
    }
    
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Climb"))
        {
            canClimb = true;
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Hut"))
        {
            canExtinguishFire = true;
        }
        
        if (other.gameObject.layer == LayerMask.NameToLayer("Pot"))
        {
            canGrabPot = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Climb"))
        {
            isClimbing = false;
            canClimb = false;
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Hut"))
        {
            canExtinguishFire = false;
        }
        
        if (other.gameObject.layer == LayerMask.NameToLayer("Pot"))
        {
            canGrabPot = false;
        }
    }
}