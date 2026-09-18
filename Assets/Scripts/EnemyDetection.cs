using UnityEngine;

public class EnemyDetection : MonoBehaviour
{
    [Header("Enemy Type")]
    [SerializeField] UrkinNeedle urkinNeedle;
    [SerializeField] UrkinSentry urkinSentry;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Player detected!");

            if (urkinNeedle != null)
            {
                urkinNeedle.TriggerAttack();
            }
            else if (urkinSentry != null)
            {
                urkinSentry.CanAttack();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if(urkinSentry != null)
            {
                urkinSentry.CannotAttack();
            }
        }
    }
}
