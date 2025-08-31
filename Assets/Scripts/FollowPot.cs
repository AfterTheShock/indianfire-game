using UnityEngine;

public class FollowPot : MonoBehaviour
{
    public PotOnGround potReference;

    private void Start()
    {
        potReference = this.transform.parent.GetComponent<PotOnGround>();
        this.transform.SetParent(null);
    }

    private void Update()
    {
        this.transform.position = potReference.transform.position;
        this.transform.rotation = potReference.transform.rotation;
    }
}
