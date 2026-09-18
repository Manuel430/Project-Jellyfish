using UnityEngine;

public class BubbleSpawner : MonoBehaviour
{
    [Header("Bubble")]
    [SerializeField] GameObject bubble;

    [Header("Cooldown")]
    [SerializeField] float cooldownTime;
    float cooldownTimer;

    private void Awake()
    {
        cooldownTimer = cooldownTime;
    }

    private void OnEnable()
    {
        SpawnBubble();
    }

    private void Update()
    {
        cooldownTimer -= Time.deltaTime;
        if (cooldownTimer < 0)
        {
            cooldownTimer = 0;

            cooldownTimer = cooldownTime;

            SpawnBubble();
        }
    }

    private void SpawnBubble()
    {
        Instantiate(bubble, gameObject.transform.position, gameObject.transform.rotation);
    }
}
