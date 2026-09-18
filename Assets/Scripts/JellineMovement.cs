using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

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
    [SerializeField] float knockbackForce;
    [SerializeField] float knockbackTimer;
    [SerializeField] float knockbackTime;
    [SerializeField] bool isKnockbackFromRight;
    [SerializeField] bool isKnockbackFromTop;

    Transform teleportLocation;
    bool canEnter;

    #region Public Methods
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

    public bool IsTwirling()
    {
        return isTwirling;
    }

    public void SetKBTimer()
    {
        knockbackTimer = knockbackTime;
    }

    public void SetKnockbackFromRight(bool value)
    {
        isKnockbackFromRight = value;
    }

    public void SetKnockbackFromTop(bool value)
    {
        isKnockbackFromTop = value;
    }

    public void SetTeleportLocation(Transform transform)
    {
        teleportLocation = transform;
    }

    public void SetEnterState(bool doorCheck)
    {
        canEnter = doorCheck;
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

        playerControls.Player.Enter.performed += Enter;
    }

    private void FixedUpdate()
    {
        if(knockbackTimer <= 0)
        {
            if(isTwirling) { return; }

            rb.linearVelocity = new Vector2(horizontal * moveSpeed, rb.linearVelocity.y);
        }
        else
        {
            if (isKnockbackFromRight)
            {
                if (isKnockbackFromTop)
                {
                    rb.linearVelocity = new Vector2(-knockbackForce, knockbackForce);
                }
                else
                {
                    rb.linearVelocity = new Vector2(-knockbackForce, knockbackForce);
                }
            }
            else
            {
                if (isKnockbackFromTop)
                {
                    rb.linearVelocity = new Vector2(knockbackForce, knockbackForce);
                }
                else
                {
                    rb.linearVelocity = new Vector2(knockbackForce, knockbackForce);
                }
            }

            knockbackTimer -= Time.fixedDeltaTime;
        }
    }

    private void Flip() { transform.localScale = new Vector3(Mathf.Sign(horizontal), 1f, 1f); }

    private void Move(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            horizontal = context.ReadValue<Vector2>().x;
            Flip();
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
            StartCoroutine(TwirlCoroutine());
        }
    }

    private void Enter(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if(!isTwirling)
            {
                if (canEnter)
                {
                    Debug.Log("Door located! Teleporting...");
                    gameObject.transform.position = teleportLocation.transform.position;
                }
                else
                {
                    Debug.LogWarning("You are not near a door");
                }
            }
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
            
            rb.linearVelocityX = twirlPower * transform.localScale.x;
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
