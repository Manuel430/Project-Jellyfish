using UnityEngine;

public class UrkinSentry : MonoBehaviour
{
    [Header("Needle Point")]
    [SerializeField] GameObject needlePoint;
    [SerializeField] GameObject needleProjectile;

    [Header("Animation")]
    [SerializeField] Animator animator;

    #region Public Method
    public void CanAttack()
    {
        animator.SetBool("Attack", true);
    }

    public void CannotAttack()
    {
        animator.SetBool("Attack", false);
    }

    public void UrkinAttack()
    {
        FireNeedle();
    }
    #endregion

    private void FireNeedle()
    {
        Instantiate(needleProjectile, needlePoint.transform.position, needlePoint.transform.rotation);
    }
}
