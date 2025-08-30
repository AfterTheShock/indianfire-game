using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float acceleration;
    [SerializeField] private float deceleration;
    [SerializeField] private float maxSpeed;
    
    private float horizontalInput;
    private Vector2 direction;
    
    private Rigidbody2D rb2d;

    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        float absHorizontalInput = Mathf.Abs(horizontalInput);
        bool isMoving = absHorizontalInput > 0;
        direction = new Vector2(horizontalInput, 0).normalized;
        
        // El player se esta moviendo
        if (isMoving)
        {
            rb2d.linearVelocity = direction * new Vector2(
                Mathf.Clamp(absHorizontalInput * acceleration, -maxSpeed, maxSpeed), rb2d.linearVelocity.y) * Time.fixedDeltaTime;
        }
        else if(Mathf.Abs(rb2d.linearVelocity.x) > 0.2f)
        {
            Vector2 dDir = -rb2d.linearVelocity.normalized;
            rb2d.linearVelocity = dDir * (deceleration * Time.fixedDeltaTime);
        }
        else
        {
            rb2d.linearVelocity = new Vector2(0, rb2d.linearVelocity.y);
        }
    }
}