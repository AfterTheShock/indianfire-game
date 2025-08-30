using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float horizontalInput;
    private Vector2 direction;
    
    private Rigidbody2D rb2d;
    
    private void FixedUpdate()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        direction = new Vector2(horizontalInput, 0).normalized;
        
        // El player se esta moviendo
        if (Mathf.Abs(horizontalInput)> 0)
        {
            Debug.Log(direction);
        }
    }
}