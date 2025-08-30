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
        withPotVisuals.SetActive(false);
        movementWithoutPot.enabled = true;
        withoutPotVisuals.SetActive(true);

        playerWithPotVisuals.transform.rotation = Quaternion.identity;
        SetWaterOnPot(false);
    }

    public void GrabPot()
    {
        if (hasPotGrabbed) return;

        hasPotGrabbed = false;
        movementWithPot.enabled = false;
        withoutPotVisuals.SetActive(false);
        movementWithoutPot.enabled = true;
        withPotVisuals.SetActive(true);
        //Do OnlyIfPotHasWater
        if (true) SetWaterOnPot(true);
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
