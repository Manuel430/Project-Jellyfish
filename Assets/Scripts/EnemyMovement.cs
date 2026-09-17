using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float moveSpeed;
    [SerializeField] GameObject[] wayPoints;
    float currentSpeed;
    float distanceToNextPoint;
    int nextPoint;

    [Header("Flip")]
    [SerializeField] bool movingHorizontal;
    [SerializeField] bool isFacingRight;
    [SerializeField] GameObject enemySprite;
    //Flip Enemy Sprite Here

    private void Awake()
    {
        currentSpeed = moveSpeed;

        if(isFacingRight)
        {
            enemySprite.transform.localScale = new Vector3(1f, 1f, 1f);
        }
        else
        {
            enemySprite.transform.localScale = new Vector3(-1f, 1f, 1f);
        }
    }

    private void Update()
    {
        Move();
    }

    private void OnEnable()
    {
        if (wayPoints.Length > 0)
        {
            transform.position = wayPoints[0].transform.position;
            nextPoint = 1;

            currentSpeed = moveSpeed;

            if (movingHorizontal)
            {
                enemySprite.transform.localScale = new Vector3(1f, 1f, 1f);
                isFacingRight = false;
            }
        }
        else
        {
            Debug.LogError("Waypoints not assigned for Enemy.");
        }
    }

    private void OnDisable()
    {
        currentSpeed = 0f;
    }

    private void Move()
    {
        distanceToNextPoint = Vector2.Distance(transform.position, wayPoints[nextPoint].transform.position);

        transform.position = Vector2.MoveTowards(transform.position, wayPoints[nextPoint].transform.position, currentSpeed * Time.deltaTime);

        NextPoint();
    }

    private void NextPoint()
    {
        if (distanceToNextPoint < 0.1f)
        {
            nextPoint++;
            if (nextPoint >= wayPoints.Length)
            {
                nextPoint = 0;
            }
        }
    }
}
