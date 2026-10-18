# Jelline
Play the game here: [Jelline](https://primalknight430.itch.io/jelline)

## About
Jelline is a Game Jam project done for GBJAM 14, a game jam that had the main theme of creating a game inspired by old Game Boy games. Working alongside a musician, the theme of the jam was "Old Gold", which led to the game being an underwater-themed 2D Platformer game where the player has to defeat a boss to retrieve an item of significant importance to the player character, Jelline.

### Credits
Programmer, Artist, Animator, Level Designer, and Concept Art - Manuel Rodriguez

Composer and SFX - [Phantom Axis](https://phantomaxisstudios.itch.io/)

## Coding
### Player Mechanics
#### Movement and Twirling
The character that the player is during the game, her name being Jelline, is fairly simple, mechanics-wise. While she shares basic movement like previous projects, her jumping amount is unlimited, letting her jump above obstacles and enemies as much as she wants to. This part of her is inspired by Kirby, since the first game was what inspired the mechanics of this project.

Jelline's main mechanic that makes her unique is her twirl ability, which allows her to move horizontally faster than if she walked. Not only is the twirl good for movement, but also for damaging enemies in the way.

Similar to how the movement and jumping inputs work, the twirling ability is first connected to the Input System as a function with an InputAction in the parentheses.
```cs
private void Twirl(InputAction.CallbackContext context)
    {
        if(isTwirling) { return; }

        if (context.performed)
        {
            playerAnim.PlayAnimTwirling(true);
            StartCoroutine(TwirlCoroutine());
        }
    }
```

When the input is performed by pressing a button, a bool that confirms that Jelline is twirling turns on, setting the twirlTimer to zero and extending the duration of the twirl until the timer is equal to the twirlTime. While the timer is going up, the constraints of Jelline's Rigidbody change to freeze the y position, allowing Jelline to move only straight ahead when changing their linear velocity. The only time that the y position is no longer frozen is if she gets knocked back from enemies.
```cs
    private System.Collections.IEnumerator TwirlCoroutine()
    {
        isTwirling = true;
        twirlTimer = 0f;
        while (twirlTimer < twirlTime)
        {
            if (isKnockback)
            {
                rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            }
            else
            {
                rb.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionY;
            }

            rb.linearVelocityX = twirlPower * transform.localScale.x;
            twirlTimer += Time.deltaTime;
            yield return null;
        }
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        playerAnim.PlayAnimTwirling(false);
        isTwirling = false;
    }
```

One thing to keep in mind is the bool that determines if Jelline is twirling or not. Not only does this stop the player from jumping, but it helps enemies that make contact with Jelline to determine who gets the damage amount. To determine who gets damaged first, the script has a public function that tells whoever's hitting her if she is twirling or not.
```cs
    public bool IsTwirling()
    {
        return isTwirling;
    }
```

Now that the script is set, on the enemy's side, they have a script called EnemyCollision, which is triggered when the collision is entered by Jelline, who has the tag "Player", and contacts the box collision. If Jelline is twirling, the enemy will take damage. If she isn't, then she gets damaged. After the damage is dealt, Jelline will be pushed back by a knockback timer so she doesn't get stuck taking constant damage once she stops twirling.
```cs
public class EnemyCollision : MonoBehaviour
{
    [Header("Damage")]
    [SerializeField] int damageAmount;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            JellineMovement playerMovement = collision.gameObject.GetComponent<JellineMovement>();

            if(playerMovement != null)
            {
                if(playerMovement.IsTwirling())
                {
                    Debug.Log("Player is twirling, so the enemy takes damage instead.");

                    EnemyHealth enemyHealth = GetComponent<EnemyHealth>();
                    if(enemyHealth != null)
                    {
                        Debug.Log("Enemy takes damage: " + damageAmount);
                        enemyHealth.TakeDamage(damageAmount);
                    }
                }
                else
                {
                    Debug.Log("Player is not twirling, so the player takes damage.");

                    JellineHealth playerHealth = collision.gameObject.GetComponent<JellineHealth>();
                    if(playerHealth != null)
                    {
                        Debug.Log("Player takes damage: " + damageAmount);
                        playerHealth.TakeDamage(damageAmount);
                    }
                }

                playerMovement.SetKBTimer();
                if (collision.transform.position.x < transform.position.x)
                {
                    playerMovement.SetKnockbackFromRight(true);
                }
                else
                {
                    playerMovement.SetKnockbackFromRight(false);
                }

                if(collision.transform.position.y < transform.position.y)
                {
                    playerMovement.SetKnockbackFromTop(true);
                }
                else
                {
                    playerMovement.SetKnockbackFromTop(false);
                }
            }
        }
    }
}
```

Finally, some extra code that shows how knockback is dealt with to Jelline.
```cs
 private void FixedUpdate()
    {
        if(knockbackTimer <= 0)
        {
            if(isTwirling) { return; }

            rb.linearVelocity = new Vector2(horizontal * moveSpeed, rb.linearVelocity.y);
        }
        else
        {
            if (isKnockbackFromRight)
            {
                if (isKnockbackFromTop)
                {
                    rb.linearVelocity = new Vector2(-knockbackForce, knockbackForce);
                }
                else
                {
                    rb.linearVelocity = new Vector2(-knockbackForce, knockbackForce);
                }
            }
            else
            {
                if (isKnockbackFromTop)
                {
                    rb.linearVelocity = new Vector2(knockbackForce, knockbackForce);
                }
                else
                {
                    rb.linearVelocity = new Vector2(knockbackForce, knockbackForce);
                }
            }

            knockbackTimer -= Time.fixedDeltaTime;
        }
    }
```

#### Player Health and UI
While the player's health is fairly easy to implement, connecting it to the game's UI is what makes it worth discussing. First, to visualize health, you need an image array for the player's health, plus two image types to show full health and empty health.
```cs
    [Header("Visualizer")]
    [SerializeField] SpriteRenderer visualSprite;
    [SerializeField] Color defaultColor;
    [SerializeField] Color damageColor;
```

From here, it is fairly easy to update the health whenever the player gets damaged or heals, being called in one function whenever the situation occurs. For this game, that will be the UpdateUI() Function.
```cs
    private void UpdateUI()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < health)
            {
                hearts[i].sprite = heartFull;
            }
            else
            {
                hearts[i].sprite = heartEmpty;
            }
        }
    }
```

This is also used for the lives counter, using images rather than a text font. It makes the UI look cleaner in a pixel art game rather than using text and struggling to get it to look the same. To get the number of lives, the function UpdateLivesUI() has an if statement that either updates both sprites to match, or keeps the first sprite at zero and the second sprite at the number of lives.
```cs
    private void UpdateLivesUI()
    {
        if(currentLives < 10)
        {
            livesNumberFront.GetComponent<Image>().sprite = livesNumber[0];
            livesNumberBack.GetComponent<Image>().sprite = livesNumber[currentLives];
        }
        else
        {
            int firstDigit = currentLives / 10;
            int secondDigit = currentLives % 10;

            livesNumberFront.GetComponent<Image>().sprite = livesNumber[firstDigit];
            livesNumberBack.GetComponent<Image>().sprite = livesNumber[secondDigit];
        }
    }
```

Lastly, when the player has extra lives, instead of restarting the level, it resets the player's health and puts them back in the starting position, making it look like the game reset rather than just loading the level all over again. This was intentionally made so that the player can keep their progress in the next level, but the time limit forced only one level to be playable.
```cs
private void LoseLife()
    {
        currentLives--;

        if(currentLives < 0)
        {
            Debug.Log("GameOver");
            gameOverUI.SetActive(true);
        }
        else
        {
            UpdateLivesUI();

            playerMovement.TurnOnTransition();

            gameObject.transform.position = respawnPoint.position;
            Heal(maxHealth);
            UpdateUI();
            playerAnim.ResetGameOverTrigger();
            playerAnim.PlayAnimRespawn();

            playerMovement.StartMoving();
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }
```

### Enemy Types
There are two types of scripts that enemies follow: either basic enemy scripts or scripts specific to the enemy. All enemies share the same scripts for collisions, health, movement, and whether or not they can face left or right. 

#### Shared Scripts
The important scripts that are shared are health and movement. If an enemy needs to move, this script will help, as long as they have waypoints that determine their movement. It doesn't matter how many waypoints there are for the enemy; as long as they have the next waypoint set, they will be moving.
```cs
    [Header("Movement")]
    [SerializeField] float moveSpeed;
    [SerializeField] GameObject[] wayPoints;
    float currentSpeed;
    float distanceToNextPoint;
    int nextPoint;
```

When the enemy is enabled, whether it be at the start of the game or when the level restarts after the player dies, the enemy will be placed on the first waypoint, facing the direction that it is moving, but only if it is moving horizontally. If not, the enemy will just face their default direction.
```cs
 private void OnEnable()
    {
        if (wayPoints.Length > 0)
        {
            transform.position = wayPoints[0].transform.position;
            nextPoint = 1;

            currentSpeed = moveSpeed;

            if (movingHorizontal)
            {
                enemySprite.transform.localScale = new Vector3(-1f, 1f, 1f);
                isFacingRight = true;
            }
        }
        else
        {
            Debug.LogError("Waypoints not assigned for Enemy.");
        }
    }
```

Once the enemy begins to move, it will determine the distance to the next waypoint and move ahead, only going as fast as the speed they are set to. When they finally reach the point, if the distanceToNextPoint < 0.1f, they will determine what their next point is and be ready to move there, with the last waypoint telling the enemy to move to the first waypoint again. If the enemy is moving horizontally, then the enemy will be flipped to face the other direction.
```cs
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

            if (movingHorizontal)
            {
                FlipEnemy();
            }
        }
    }

    private void FlipEnemy()
    {
        isFacingRight = !isFacingRight;

        if (isFacingRight)
        {
            enemySprite.transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        else
        {
            enemySprite.transform.localScale = new Vector3(1f, 1f, 1f);
        }
```

For any enemy that does not move horizontally, there is another way to keep the enemy facing the player: a whole new shared script that uses trigger collisions to determine if the player is behind or in front of the enemy.
```cs
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
```

The enemy's health is similar to the player's, but instead of messing with the UI, it only disappears the sprite for a small second.
```cs
    public void TakeDamage(int damage)
    {
        StartCoroutine(FlashDamageColor());

        currentHealth -= damage;

        if(currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
    }
```

```cs
   IEnumerator FlashDamageColor()
    {
        if(visualSprite != null)
        {
            visualSprite.color = damageColor;
            yield return new WaitForSeconds(0.1f);
            visualSprite.color = defaultColor;
        }
    }
```

If the enemy is killed, a small explosion will play out as the enemy turns off. The enemy must be using SetActive(false) rather than Destroy.GameObject, because if the enemy is destroyed, then it cannot reappear when the player re-enters the room; the enemies are unable to respawn. So, when they are disabled, when the player re-enters the room, the enemies will be fully healed and set to enabled, letting them walk around again and attack the player if possible.
```cs
    private void Die()
    {
        if(deathEffectPrefab != null)
        {
            Instantiate(deathEffectPrefab, transform.position,transform.rotation);
        }

        gameObject.SetActive(false);
    }
```

The script below is the RoomManagement script, allowing the enemies to spawn when the player enters the room and despawn when the player leaves the room.
```cs
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            virtualCamera.SetActive(true);

            if (enemiesInRoom.Length != 0)
            {
                foreach (GameObject enemy in enemiesInRoom)
                {
                    int enemyIndex = System.Array.IndexOf(enemiesInRoom, enemy);
                    enemiesInRoom[enemyIndex].SetActive(true);
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            virtualCamera.SetActive(false);

            if (enemiesInRoom.Length != 0)
            {
                foreach (GameObject enemy in enemiesInRoom)
                {
                    int enemyIndex = System.Array.IndexOf(enemiesInRoom, enemy);
                    enemiesInRoom[enemyIndex].SetActive(false);
                }
            }
        }
    }
}
```

#### Specific Scripts
This is part of cut content from the game, but it is important to look at two enemies that have unique projectile attacks that the other two enemies didn't have. These enemies were known as Urkin Needle and Urkin Sentry. What made them interesting enough to talk about is their mechanic to shoot out needle projectiles. Whenever they detect the player, the script can determine if the enemy has the specific script that'll do the enemy's specific attack.
```cs
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
```

If the Urkin Needle is attacking, they have multiple projectile points that shoot outwards, leaving the enemy defenseless as they have to deal with a cooldown before attacking again, letting the player either attack or leave before they attack again.
```cs
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
```

The Urkin Sentry, on the other hand, is more animation-based, shooting out needles at specific points of the animation, then waiting to see if the player is still within their range, repeating the animation until either dead or the player has left their firing range.
```cs
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
```

### Ms. Puffer - Boss Mechanics
Originally, there were meant to be different bosses for the game, hence why there was the same split between a shared bosses script and a specific script for the only boss that was finished, Ms. Puffer. The boss health was a combination of enemy and player health, where they have a flashing transparency when damaged but also have their health bar shown in the UI, even before the player enters the boss room.
```cs
    public void TakeDamage(int damage)
    {
        StartCoroutine(FlashDamageColor());

        currentHealth -= damage;

        UpdateUI();

        if(currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
    }
```

```cs
    private void UpdateUI()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if(i < currentHealth)
            {
                hearts[i].sprite = heartFull;
            }
            else
            {
                hearts[i].sprite = heartEmpty;
            }
        }
    }
```

Damage and collision are the same as enemies, but what made Ms. Puffer dangerous is her movement, with her massive size taking a large part of the screen while moving in four specific points. The player will have to be careful with twirling, as if they stop, the player will get hurt more than by any other enemy.
```cs
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
```

While the boss wouldn't be super hard, there is one more attack up her sleeve that could be a challenge: her projectile attacks. Similar to the cut Urkin Needle, she has a cooldown that can be determined by a float variable. Once the timer goes down to zero, she will fire out projectiles in 8 directions, then back to another cooldown until she can shoot projectiles again.
```cs
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
```

```cs
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
```

Finally, if the player defeats Ms. Puffer, she has an original death animation rather than an explosion like previous enemies, in which the goal of the game is set to active, doing a small transition down so the player can grab, finally able to finish the game and let the player win.
```cs
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
```

### Misc Code
#### Healing Pickups
Picking up healing items is a very simple script, as it is a simple int that determines how much the player will heal from the item, all determined in a switch statement, with 0-2 giving more health to the player, and 3 giving the player an extra life. Unfortunately, the healing items only appear once, so be wise with what you collect.
```cs
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
```

#### Bubble Platforms
The bubble platforms are unique as they come in two components: the bubble spawner and the bubble itself. This is important because if the bubble were destroyed a few seconds after contact, there would be no way for the bubble to come back, leaving a space. So, the bubble spawner is its own separate object that'll keep spawning more bubbles without a worry about getting destroyed.
```cs
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
```

This allows the bubble to spawn continuously, forever moving upwards by linear velocity, until either the player touches it and pops after a specific amount of time, or it pops immediately if anything else touches it.
```cs
    public void PopBubble()
    {
        Destroy(gameObject);
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = transform.up * floatingSpeed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            floatingSpeed = 0;
            rb.constraints = RigidbodyConstraints2D.FreezeAll;

            animator.SetTrigger("Player");
        }
        else
        {
            animator.SetTrigger("Sky");
        }
    }
}
```
