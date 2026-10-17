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
While the player's health is fairly easy to do, connecting it to the game's UI is what makes it important to talk about. First, when it comes to visualizing the health, there has to be an image array for the amount of health the player will have, alongside two image types to show that the player has either full health or empty health.
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



### Enemy Types

### Ms. Puffer - Boss Mechanics

### Misc Code

#### Healing Pickups

#### Bubble Hazards

