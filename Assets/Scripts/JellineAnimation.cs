using UnityEngine;

public class JellineAnimation : MonoBehaviour
{
    [Header("References")]
    [SerializeField] JellineHealth playerHealth;

    [Header("Audio")]
    [SerializeField] AudioSource musicManager;
    [SerializeField] AudioClip themeSong;
    [SerializeField] AudioClip victoryMusic;
    [SerializeField] AudioClip defeatMusic;

    [Header("UI")]
    [SerializeField] GameObject congratsUI;

    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void Congratulations()
    {
        congratsUI.SetActive(true);
    }

    public void PlayVictorySong()
    {
        musicManager.clip = victoryMusic;
        musicManager.loop = false;
        musicManager.Play();
    }

    public void PlayDefeatedSong()
    {
        musicManager.Stop();
        musicManager.clip = defeatMusic;
        musicManager.loop = false;
        musicManager.Play();
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
        musicManager.clip = themeSong;
        musicManager.loop = true;
        musicManager.Play();

        animator.SetTrigger("Respawn");
    }

    public void PlayAnimWin()
    {
        animator.SetTrigger("Win");
    }

    public void LoseLife()
    {
        playerHealth.LosingLife();
    }
}
