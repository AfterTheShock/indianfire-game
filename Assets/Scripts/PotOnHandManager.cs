using UnityEngine;

public class PotOnHandManager : MonoBehaviour
{
    public bool hasPotGrabbed = true;
    public bool hasWaterOnPot = false;

    [SerializeField] GameObject[] ObjectsToBeOnWithWater = new GameObject[0];

    [SerializeField] GameObject potOnGroundPrefab;

    [SerializeField] Transform pointToDropPot;

    [SerializeField] GameObject playerWithPotVisuals;

    private MovementWithPot movementWithPot;
    private MovementWithoutPot movementWithoutPot;

    private InputSystem_Actions inputs;

    private void Start()
    {
        inputs = new InputSystem_Actions();
        inputs.Player.Enable();
        movementWithPot = this.gameObject.GetComponent<MovementWithPot>();
        movementWithoutPot = this.gameObject.GetComponent<MovementWithoutPot>();
    }

    private void Update()
    {
        if (inputs.Player.Drop.ReadValue<float>() != 0 && hasPotGrabbed)
        {
            DropPot();
        }
    }

    private void DropPot()
    {
        if (!hasPotGrabbed) return;

        GameObject go = Instantiate(potOnGroundPrefab);
        go.transform.position = pointToDropPot.position;

        hasPotGrabbed = false;
        movementWithPot.enabled = false;
        movementWithoutPot.enabled = true;

        playerWithPotVisuals.transform.rotation = Quaternion.identity;
    }

    public void GrabPot()
    {
        if (hasPotGrabbed) return;

        hasPotGrabbed = false;
        movementWithPot.enabled = false;
        movementWithoutPot.enabled = true;
    }

    private void SetWaterOnPot(bool hasWater)
    {
        foreach(GameObject go in ObjectsToBeOnWithWater)
        {
            go.SetActive(hasWater);
        }
        hasWaterOnPot = hasWater;
    }
}
