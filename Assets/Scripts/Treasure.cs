using UnityEngine;

public class Treasure : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] GameObject fallingStop;
    [SerializeField] float itemSpeed;
    float distanceToPoint;

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    private void Update()
    {
        MoveDown();
    }

    private void MoveDown()
    {
        distanceToPoint = Vector2.Distance(transform.position, fallingStop.transform.position);

        transform.position = Vector2.MoveTowards(transform.position, fallingStop.transform.position, itemSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            JellineMovement playerMovement = collision.GetComponent<JellineMovement>();
            if (playerMovement != null)
            {
                playerMovement.EndGame();
            }

            Destroy(gameObject);
        }
    }
}
