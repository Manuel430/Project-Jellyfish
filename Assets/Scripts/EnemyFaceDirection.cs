using UnityEngine;

public class EnemyFaceDirection : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] EnemyMovement enemyMovement;

    [Header("Placement")]
    [SerializeField] bool onTheLeft;

    [Header("Flip")]
    [SerializeField] GameObject enemy;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if(onTheLeft)
            {
                if (!enemyMovement.IsFacingRight())
                {
                    Debug.Log("Already Facing Player, do nothing - L");
                }
                else
                {
                    if(enemyMovement == null)
                    {
                        Debug.Log("Flip in this Script");
                    }
                    else
                    {
                        enemyMovement.CallToFlip();
                    }
                }
            }
            else
            {
                if (enemyMovement.IsFacingRight())
                {
                    Debug.Log("Already Facing Player, do nothing - R");
                }
                else
                {
                    if (enemyMovement == null)
                    {
                        Debug.Log("Flip in this Script");
                    }
                    else
                    {
                        enemyMovement.CallToFlip();
                    }
                }
            }
        }
    }
}
