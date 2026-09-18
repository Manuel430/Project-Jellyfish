using UnityEngine;

public class UrkinNeedle : MonoBehaviour
{
    [Header("Needle Points")]
    [SerializeField] GameObject[] needlePoints;
    [SerializeField] GameObject needleProjectile;

    [Header("Cooldown")]
    [SerializeField] bool isOnCooldown;
    [SerializeField] float cooldownTime;
    float cooldownTimer;

    [Header("Animation")]
    [SerializeField] Animator animator;

    #region Public Methods
    public void TriggerAttack()
    {
        if(!isOnCooldown)
        {
            animator.SetTrigger("Attack");
        }
    }

    public void UrkinAttack()
    {
        FireNeedles();
    }

    #endregion

    private void Update()
    {
        if (isOnCooldown)
        {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0f)
            {
                isOnCooldown = false;
                
                animator.ResetTrigger("Attack");
            }
        }
    }

    private void FireNeedles()
    {
        if (!isOnCooldown)
        {
            foreach (GameObject point in needlePoints)
            {
                Instantiate(needleProjectile, point.transform.position, point.transform.rotation);
            }
            isOnCooldown = true;
            cooldownTimer = cooldownTime;
        }
    }

}
