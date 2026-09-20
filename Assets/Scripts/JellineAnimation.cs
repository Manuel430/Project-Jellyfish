using UnityEngine;

public class JellineAnimation : MonoBehaviour
{
    [Header("References")]
    [SerializeField] JellineHealth playerHealth;

    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void ResetHitTrigger()
    {
        animator.ResetTrigger("Hit");
    }

    public void ResetGameOverTrigger()
    {
        animator.ResetTrigger("GameOver");
    }

    public void PlayAnimMove(bool isMoving)
    {
        animator.SetBool("isMoving", isMoving);
    }

    public void PlayAnimGrounded(bool isGrounded)
    {
        animator.SetBool("isGrounded", isGrounded);
    }

    public void PlayAnimTwirling(bool isTwirling)
    {
        animator.SetBool("isTwirling", isTwirling);
    }

    public void PlayAnimHit()
    {
        animator.SetTrigger("Hit");
    }

    public void PlayAnimGameOver()
    {
        animator.SetTrigger("GameOver");
    }

    public void LoseLife()
    {
        playerHealth.LosingLife();
    }
}
