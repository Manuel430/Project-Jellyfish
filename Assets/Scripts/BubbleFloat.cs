using UnityEngine;

public class BubbleFloat : MonoBehaviour
{
    [Header("Projectile")]
    [SerializeField] float floatingSpeed;
    Rigidbody2D rb;

    [Header("Animation")]
    [SerializeField] Animator animator;


    public void PopBubble()
    {
        Destroy(gameObject);
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = transform.up * floatingSpeed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            floatingSpeed = 0;
            rb.constraints = RigidbodyConstraints2D.FreezeAll;

            animator.SetTrigger("Player");
        }
        else
        {
            animator.SetTrigger("Sky");
        }
    }
}
