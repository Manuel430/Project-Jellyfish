using UnityEngine;

public class EnemyExplosion : MonoBehaviour
{
    public void StopExploding()
    {
        Destroy(gameObject);
    }
}
