using UnityEngine;

public class HealingPickups : MonoBehaviour
{
    [Header("Type")]
    [Range(0, 3)][SerializeField] int healingType;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            JellineHealth health = collision.GetComponent<JellineHealth>();
            if(health != null )
            {
                switch (healingType)
                {
                    case 0:
                        health.TakeHeal(1);
                        break;
                    case 1:
                        health.TakeHeal(5);
                        break;
                    case 2:
                        health.TakeHeal(10);
                        break;
                    case 3:
                        health.GivingLife();
                        break;
                    default:
                        Debug.LogWarning("Item has no Healing Type");
                        break;
                }

                Destroy(gameObject);
            }
        }
    }
}
