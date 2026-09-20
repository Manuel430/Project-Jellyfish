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

    public bool GetRespawnTrigger()
    {
        return animator.GetBool("Respawn");
    }

    public void ResetHitTrigger()
    {
        animator.ResetTrigger("Hit");
    }

    public void ResetGameOverTrigger()
    {
        animator.ResetTrigger("GameOver");
    }

    public void ResetRespawnTrigger()
    {
        animator.ResetTrigger("Respawn");
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

    public void PlayAnimRespawn()
    {
        animator.SetTrigger("Respawn");
    }

    public void LoseLife()
    {
        playerHealth.LosingLife();
    }
}
