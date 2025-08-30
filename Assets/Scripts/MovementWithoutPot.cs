using UnityEngine;

public class MovementWithoutPot : MonoBehaviour
{
    [SerializeField] private Vector2 acceleration;
    [SerializeField] private Vector2 deceleration;
    [SerializeField] private Vector2 maxSpeed;
    [SerializeField] private LayerMask climbLayerMask;
    [SerializeField] private bool canClimb;
    
    private bool isClimbing;
    
    private Rigidbody2D rb2d;

    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        //Debug.Log(isClimbing);
        
        Vector2 movementInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
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
    
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Climb")) canClimb = true;

        if (other.gameObject.layer == LayerMask.NameToLayer("Hut"))
        {
            
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
            
        }
    }
}