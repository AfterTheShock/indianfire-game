using UnityEngine;

public class PotOnHandManager : MonoBehaviour
{
    public bool hasPotGrabbed = true;
    public bool hasWaterOnPot = false;

    [SerializeField] GameObject[] ObjectsToBeOnWithWater = new GameObject[0];

    [SerializeField] GameObject withPotVisuals;
    [SerializeField] GameObject withoutPotVisuals;

    [SerializeField] GameObject potOnGroundPrefab;

    [SerializeField] Transform pointToDropPot;

    [SerializeField] GameObject playerWithPotVisuals;

    private MovementWithPot movementWithPot;
    private MovementWithoutPot movementWithoutPot;

    private PotOnGround grabbedPotScript;

    private bool isOnFuente = false;

    private InputSystem_Actions inputs;

    private static PotOnHandManager _instance;

    public static PotOnHandManager Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    private void Start()
    {
        if (inputs == null) inputs = new InputSystem_Actions();
        inputs.Player.Enable();
        movementWithPot = this.gameObject.GetComponent<MovementWithPot>();
        movementWithoutPot = this.gameObject.GetComponent<MovementWithoutPot>();

        if (hasPotGrabbed) 
        { 
            SetWaterOnPot(hasWaterOnPot);

        }

    }

    private void OnEnable()
    {
        if (inputs == null) inputs = new InputSystem_Actions();
        inputs.Player.Enable();
    }

    private void OnDisable()
    {
        inputs.Player.Disable();
    }


    private void Update()
    {
        if (inputs.Player.Drop.ReadValue<float>() != 0 && hasPotGrabbed)
        {
            DropPot();
        }

        if (inputs.Player.Interact.ReadValue<float>() != 0 && hasPotGrabbed && isOnFuente)
        {
            GrabWater();
        }
    }

    private void GrabWater()
    {
        SetWaterOnPot(true);
    }

    public void DropWater()
    {
        SetWaterOnPot(false);
    }

    public void DropPot(bool shoudDropWater = false)
    {
        if (!hasPotGrabbed) return;

        GameObject go = Instantiate(potOnGroundPrefab);
        go.transform.position = pointToDropPot.position;

        go.GetComponent<PotOnGround>().hasWater = hasWaterOnPot;

        if(shoudDropWater) go.GetComponent<PotOnGround>().hasWater = false;

        hasPotGrabbed = false;
        movementWithPot.enabled = false;
        withPotVisuals.SetActive(false);
        movementWithoutPot.enabled = true;
        withoutPotVisuals.SetActive(true);

        playerWithPotVisuals.transform.rotation = Quaternion.identity;
        SetWaterOnPot(false);
        grabbedPotScript = null;
    }

    public void GrabPot(GameObject pot, GameObject potTrigger)
    {
        if (hasPotGrabbed) return;

        this.GetComponent<MovementWithPot>().potGrabbed = pot.GetComponent<PotOnGround>();

        grabbedPotScript = pot.GetComponent<PotOnGround>();
        hasWaterOnPot = pot.GetComponent<PotOnGround>().hasWater;

        Destroy(pot);
        Destroy(potTrigger);

        hasPotGrabbed = true;
        movementWithPot.enabled = true;
        withoutPotVisuals.SetActive(false);
        movementWithoutPot.enabled = false;
        withPotVisuals.SetActive(true);
        //Do OnlyIfPotHasWater
        SetWaterOnPot(hasWaterOnPot);
    }

    public void SetWaterOnPot(bool hasWater)
    {
        foreach(GameObject go in ObjectsToBeOnWithWater)
        {
            go.SetActive(hasWater);
        }
        hasWaterOnPot = hasWater;
        if(grabbedPotScript != null) grabbedPotScript.hasWater = hasWater;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Fuente"))
        {
            isOnFuente = true;
        }

    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Fuente"))
        {
            isOnFuente = false;
        }

    }
}
