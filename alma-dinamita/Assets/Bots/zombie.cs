using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;
public class zombie : MonoBehaviour
{
    private Transform player;
    private health playerHealth;
    private Vector3 randomGoToPosition;
    public NavMeshAgent zAgent;
    private bool playerInsideChaseRange = false;
    private bool playerInsideAttackRange = false;
    private bool walking = false;
    private bool lookingForPlayer = false;
    private bool shouldWalk = false;
    private float chaseRadius = 8f;
    private const float attackRadius = 1f;
    private float attackTime = 0.0001f;
    public bool standingStill = false;
    public float standTilTime = 0.0001f;
    private IEnumerator Start()
    {
        player = GameObject.Find("Player").GetComponent<Transform>();
        playerHealth = GameObject.Find("Player").GetComponent<health>();
        yield return new WaitForSeconds(2f);
        zAgent = GetComponent<NavMeshAgent>();
        if (Random.Range(0, 101) > 50)
        {
            shouldWalk = true;
        }
    }

    private void Update()
    {
        standingStill = standTilTime > Time.time;
        
        if (!zAgent || standingStill) return;
        
        
        if (playerInsideChaseRange)
        {
            chaseRadius = 24.0003f;
            ChasePlayer();
            AttackPlayer(playerInsideAttackRange);
            return;
        }
        
        if (shouldWalk)
        {
            if (!playerInsideChaseRange && !walking)
            {
                WalkAround();
            }
            
            walking = ((float)System.Math.Round(transform.position.x, 2) != (float) System.Math.Round(randomGoToPosition.x, 2)) &&
                      ((float)System.Math.Round(transform.position.z, 2) != (float) System.Math.Round(randomGoToPosition.z, 2));
        }
        else
        {
            if (!lookingForPlayer) {
                var lookForPlayer = Random.Range(0, 10) < 5;
                if (lookForPlayer) LookForPlayer();
                else StandStill();
            }
            else
            {
                lookingForPlayer = ((float)System.Math.Round(transform.position.x, 2) != (float) System.Math.Round(randomGoToPosition.x, 2)) &&
                                   ((float)System.Math.Round(transform.position.z, 2) != (float) System.Math.Round(randomGoToPosition.z, 2));
            }
        }
    }

    private void FixedUpdate()
    {
        Collider[] sphereCollidersForChase = Physics.OverlapSphere(this.transform.position, chaseRadius);
        Collider[] sphereCollidersForAttack = Physics.OverlapSphere(this.transform.position, attackRadius);
        foreach (Collider coll in sphereCollidersForChase) playerInsideChaseRange = coll.name == "Player";
        foreach (Collider coll in sphereCollidersForAttack) playerInsideAttackRange = coll.name == "Player";
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(this.transform.position, chaseRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position, attackRadius);
    }

    private void ChasePlayer()
    {
        // Set the zombie speed as fast
        zAgent.speed = 6.59f;
        zAgent.destination = player.position;
        // Restart status variables
        shouldWalk = false;
        walking = false;
        lookingForPlayer = false;
    }

    private void LookForPlayer()
    {
        // Set the zombie speed as medium to make it walk
        zAgent.speed = 1.2f;

        var transformPositionX = transform.position.x;
        var transformPositionZ = transform.position.z;
        
        // Make the zombie go to a certain place on an extended range
        randomGoToPosition = new Vector3(
            Random.Range(transformPositionX - 23.0f, transformPositionX + 22.0f),
            0.0f,
            Random.Range(transformPositionZ - 22.0f, transformPositionZ + 22.0f));
        zAgent.destination = randomGoToPosition;
        
        // Restart status variables
        lookingForPlayer = true;
        walking = false;
        shouldWalk = false;
    }

    private void WalkAround()
    {
        // Set the zombie speed as slow as possible but enough to see the model moving
        zAgent.speed = 0.1f;
        
        var transformPositionX = transform.position.x;
        var transformPositionZ = transform.position.z;
        
        // Make the zombie go to a certain place on a medium range range
        randomGoToPosition = new Vector3(
            Random.Range(transformPositionX - 12.0f, transformPositionX + 12.0f),
            0.0f,
            Random.Range(transformPositionZ - 12.0f, transformPositionZ + 12.0f));
        zAgent.destination = randomGoToPosition;
        
        // Restart status variables
        lookingForPlayer = false;
    }

    private void StandStill()
    {
        standTilTime = Time.time + Random.Range(0.3f, 15.8f);
    }
    
    
    private void AttackPlayer(bool attackRange)
    {
        // If the player is in the attack range and the attack cooldown has passed attack and add a cooldown to the attack
        if (attackRange && (Time.time > attackTime))
        {
            attackTime = Time.time + 1.8f;
            playerHealth.SetDamageOnHealth(10f);
        }
    }
    
}
