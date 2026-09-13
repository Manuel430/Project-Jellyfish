using UnityEngine;
using UnityEngine.InputSystem;

public class JellineMovement : MonoBehaviour
{
    [Header("References")]
    PlayerControlsScript playerControls;
    Rigidbody2D rb;

    // Set Animations Here

    [Header("Stats")]
    [SerializeField] float moveSpeed;
    [SerializeField] float jumpForce;
    float horizontal;

    [Header("Twirl")]
    [SerializeField] bool isTwirling;
    [SerializeField] float twirlPower;
    [SerializeField] float twirlTime;
    float twirlTimer;

    [Header ("Knockback")]
    [SerializeField] bool isKnockback;
    //Work out Knockback logic later

    #region Public Properties
    public void StartMoving()
    {
        playerControls.Enable();

        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    public void StopMoving()
    {
        playerControls.Disable();

        rb.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    public void HitBouncyObject()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }
    #endregion

    private void Awake()
    {
        playerControls = new PlayerControlsScript();
        rb = GetComponent<Rigidbody2D>();

        StartMoving();

        playerControls.Player.Move.performed += Move;
        playerControls.Player.Move.canceled += Move;

        playerControls.Player.Jump.performed += Jump;
        playerControls.Player.Jump.canceled += Jump;

        playerControls.Player.Twirl.performed += Twirl;
    }

    private void FixedUpdate()
    {
        if (isTwirling) { return; }

        rb.linearVelocity = new Vector2(horizontal * moveSpeed, rb.linearVelocity.y);
    }

    private void Move(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            horizontal = context.ReadValue<Vector2>().x;
        }
        else if(context.canceled)
        {
            horizontal = 0f;
        }
    }

    private void Jump(InputAction.CallbackContext context)
    {
        if(isTwirling) { return; }

        if (context.performed)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
        if(context.canceled && rb.linearVelocity.y > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y);
        }
    }

    private void Twirl(InputAction.CallbackContext context)
    {
        if(isTwirling) { return; }

        if (context.performed)
        {
            Debug.Log("Twirl activated");
            StartCoroutine(TwirlCoroutine());
        }
    }

    private System.Collections.IEnumerator TwirlCoroutine()
    {
        isTwirling = true;
        twirlTimer = 0f;
        while (twirlTimer < twirlTime)
        {
            if (isKnockback)
            {
                rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            }
            else
            {
                rb.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionY;
            }
            rb.linearVelocityX = twirlPower;
            twirlTimer += Time.deltaTime;
            yield return null;
        }
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        isTwirling = false;
    }

    // Debugging
    private void OnDrawGizmosSelected()
    {
        // Draw a line to represent the movement direction
    }
}
