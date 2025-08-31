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
    private bool isClimbing;
    private bool canGrabPot;
    
    private Rigidbody2D rb2d;

    private bool isFallen = false;

    [SerializeField] Transform visualsHolder;

    [SerializeField] GameObject normalVisuals;
    [SerializeField] GameObject fallenVisuals;
    [SerializeField] float timeFalling = 1.5f;

    [SerializeField] Vector2 fallenRotationLeftAndRight = new Vector2(90, -90);

    Vector2 movementInput;

    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        potOnHandManager = GetComponent<PotOnHandManager>();
    }

    private void Update()
    {
        if (isFallen) return;
<<<<<<< HEAD
        Debug.Log(canGrabPot);
=======
        
        if (canExtinguishFire)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("Extinguish Fire Hut");
            }
        }
>>>>>>> 837be72514961d120263cda1e30c97365be5570d

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
        if (isFallen) return;

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

    public void MakePlayerFall(bool isFallingFromRight = false)
    {
        this.transform.position += new Vector3(0, .25f, 0);
        visualsHolder.localScale = new Vector3(1, 1, 1);
        if (!isFallingFromRight) visualsHolder.localEulerAngles = new Vector3(0, 0, fallenRotationLeftAndRight.x);
        if (isFallingFromRight) visualsHolder.localEulerAngles = new Vector3(0, 0, fallenRotationLeftAndRight.y);
        isFallen = true;
        fallenVisuals.SetActive(true);
        normalVisuals.SetActive(false);
        Invoke("ResetPlayerFalling", timeFalling);
    }

    private void ResetPlayerFalling()
    {
        visualsHolder.localEulerAngles = Vector3.zero;
        isFallen = false;
        fallenVisuals.SetActive(false);
        normalVisuals.SetActive(true);
    }

    private void ManagePlayerAnimations()
    {
        if(movementInput.x > 0) playerVisuals.localScale = new Vector3(1,1,1);
        if(movementInput.x < 0) playerVisuals.localScale = new Vector3(-1,1,1);

        if (!animator.gameObject.activeSelf || !playerVisuals.gameObject.activeSelf) return;

        if (isClimbing)
        {
            if (movementInput.x != 0 || movementInput.y != 0)
            {
                animator.Play("ClimingAnimation");
                animator.speed = 1;
            }
            else
            {
                animator.Play("ClimingAnimation");
                animator.speed = 0;
            }
        }
        else if (movementInput.x != 0)
        {
            animator.speed = 1;
            animator.Play("WalkWithoutPotPlayer");
        }
        else
        {
            animator.speed = 1;
            animator.Play("IdleWithoutPotPlayer");
        }
    }
    
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Climb"))
        {
            canClimb = true;
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
        
        if (other.gameObject.layer == LayerMask.NameToLayer("Pot"))
        {
            canGrabPot = false;
        }
    }
}