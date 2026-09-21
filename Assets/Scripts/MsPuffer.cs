using UnityEngine;

public class MsPuffer : MonoBehaviour
{
    [Header("Camera")]
    [SerializeField] GameObject previousRoomCam;

    [Header("Movement")]
    [SerializeField] float moveSpeed;
    [SerializeField] GameObject[] wayPoints;
    float currentSpeed;
    float distanceToNextPoint;
    int nextPoint;

    [Header("Sprite")]
    [SerializeField] GameObject bossSprite;

    [Header("Needles")]
    [SerializeField] GameObject[] needlePoints;
    [SerializeField] GameObject needleProjectiles;

    [Header("Cooldown")]
    [SerializeField] bool isOnCooldown;
    [SerializeField] float cooldownTime;
    bool startAttacking;
    float cooldownTimer;

    #region Public Methods
    public void StartMoving()
    {
        currentSpeed = moveSpeed;

        startAttacking = true;
        isOnCooldown = true;

        previousRoomCam.SetActive(false);
    }

    public void StopMoving()
    {
        currentSpeed = 0;

        startAttacking = false;
    }
    #endregion

    private void OnEnable()
    {
        if(wayPoints.Length > 0)
        {
            transform.position = wayPoints[0].transform.position;
            nextPoint = 1;

            StopMoving();
        }
    }

    private void OnDisable()
    {
        StopMoving();

        previousRoomCam.SetActive(true);

        bossSprite.transform.localScale = new Vector3(1f, 1f, 1f);
    }

    private void Update()
    {
        Move();
        if (startAttacking)
        {
            if (isOnCooldown)
            {
                cooldownTimer -= Time.deltaTime;
                if (cooldownTimer <= 0f)
                {
                    isOnCooldown = false;
                    FireNeedles();
                }
            }
        }
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

            if(nextPoint == 1)
            {
                bossSprite.transform.localScale = new Vector3(1f, 1f, 1f);
            }
            else if(nextPoint == 3)
            {
                bossSprite.transform.localScale = new Vector3(-1f, 1f, 1f);
            }
        }
    }

    private void FireNeedles()
    {
        if(!isOnCooldown)
        {
            foreach(GameObject point in needlePoints)
            {
                Instantiate(needleProjectiles, point.transform.position, point.transform.rotation);
            }
            isOnCooldown = true;
            cooldownTimer = cooldownTime;
        }
    }

}
