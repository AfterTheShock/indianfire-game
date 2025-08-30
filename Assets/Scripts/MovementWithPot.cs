using UnityEngine;

public class MovementWithPot : MonoBehaviour
{
    [SerializeField] float movementSpeed = 25f;
    [SerializeField] float rotationSpeed = 25f;
    [SerializeField] float velocityToStopRotation = 15f;
    [SerializeField] Vector2 rotationLimits = new Vector2 (45, 315);
    [SerializeField] float velToStartRotating = 15f;
    [SerializeField] float rotationDificultySpeed = 15f;
    [SerializeField] Transform rotationTransform;
    [SerializeField] Animator animator;
    [SerializeField] LayerMask groundMask;
    [SerializeField] float playerHeight = 3f;
    [SerializeField] float RotationToSlopSpeed = 2f;

    [SerializeField] GameObject normalVisuals;
    [SerializeField] GameObject fallenVisuals;

    private PotOnHandManager potOnHandManager;
    private MovementWithoutPot movementWithoutPot;

    private InputSystem_Actions inputs;

    private float arrowsInputs;
    private float wasdInputs;
    private Rigidbody2D rb;

    private void Start()
    {
        inputs = new InputSystem_Actions();
        inputs.Player.Enable();
        rb = GetComponent<Rigidbody2D>();
        potOnHandManager = GetComponent<PotOnHandManager>();
        movementWithoutPot = GetComponent<MovementWithoutPot>();
    }
    private void OnEnable()
    {
        rotationTransform.localEulerAngles = Vector3.zero;
    }

    private void Update()
    {
        RotationManager();
        GetPlayerInputs();
        RotatePlayerBySpeed();
        ManagePlayerAnimations();
        //ManageWorldRotationRelativeToTheGround();
    }

    private void ManageWorldRotationRelativeToTheGround()
    {
        RaycastHit2D hit;
        hit = Physics2D.Raycast(this.transform.position, -this.transform.up, playerHeight, groundMask);
        if (Physics2D.Raycast(this.transform.position,-this.transform.up, playerHeight, groundMask))
        {
            float angle = Mathf.Atan2(hit.normal.y, hit.normal.x) * Mathf.Rad2Deg;
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.AngleAxis(angle -90, Vector3.forward), RotationToSlopSpeed * Time.deltaTime);
        }
    }

    private void ManagePlayerAnimations()
    {
        if (wasdInputs != 0 && animator.gameObject.activeSelf) animator.Play("WalkWithPotPlayer");
        if(wasdInputs > 0) rotationTransform.localScale = new Vector3(1,1,1);
        if(wasdInputs < 0) rotationTransform.localScale = new Vector3(-1,1,1);
    }

    private void GetPlayerInputs()
    {
        if (inputs.Player.Arrows.ReadValue<Vector2>().x != 0) arrowsInputs = inputs.Player.Arrows.ReadValue<Vector2>().x;
        else arrowsInputs = Mathf.Lerp(arrowsInputs, 0, velocityToStopRotation * Time.deltaTime);

        wasdInputs = inputs.Player.Move.ReadValue<Vector2>().x;
    }
    private void RotatePlayerBySpeed()
    {
        float rotationDificulty = rotationDificultySpeed;

        if (arrowsInputs != 0) rotationDificulty = rotationDificultySpeed / 2;

        if (rb.linearVelocity.x > velToStartRotating)
        {
            TryToRotatePlayer(rotationDificulty, 1 * Mathf.Abs(rb.linearVelocity.x));
        }
        if (rb.linearVelocity.x < -velToStartRotating)
        {
            TryToRotatePlayer(rotationDificulty, -1 * Mathf.Abs(rb.linearVelocity.x));
        }
    }

    private void FixedUpdate()
    {
        ManagePlayerMovement ();
    }
    private void ManagePlayerMovement()
    {
        if(wasdInputs != 0) rb.linearVelocity = new Vector2(wasdInputs * movementSpeed * Time.fixedDeltaTime, rb.linearVelocity.y);
    }

    private void RotationManager()
    {

        TryToRotatePlayer(rotationSpeed, arrowsInputs);
    }

    private void TryToRotatePlayer(float rotationSpeed, float input)
    {
        Vector3 oldAngles = rotationTransform.localEulerAngles;

        rotationTransform.localEulerAngles += (new Vector3(0, 0, -input * rotationSpeed * Time.deltaTime));

        if (rotationTransform.localEulerAngles.z < rotationLimits.y && rotationTransform.localEulerAngles.z > rotationLimits.x)
        {
            bool isFallingFromRight = false;
            if(rotationTransform.localEulerAngles.z < rotationLimits.y && rotationTransform.localEulerAngles.z > 180) isFallingFromRight = true;
            rotationTransform.localEulerAngles = oldAngles;
            FallOverWithPot(isFallingFromRight);
        }
    }

    private void FallOverWithPot(bool isFallingFromRight = false)
    {

        movementWithoutPot.MakePlayerFall(isFallingFromRight);
        potOnHandManager.DropPot();


        if (isFallingFromRight) rotationTransform.localEulerAngles = new Vector3 (0, 0, 90);
        if(isFallingFromRight) rotationTransform.localEulerAngles = new Vector3 (0, 0, -90);
    }

}
