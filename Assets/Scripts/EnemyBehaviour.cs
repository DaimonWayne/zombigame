using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    public float enemySpeed = 5f;
    private Transform target;
    public float enemyHealth = 100f;

    private void Update()
    {
        FollowPlayer();
    }

    void FollowPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            target = player.transform;

            transform.LookAt(target.position);
            transform.Translate(Vector3.forward * enemySpeed * Time.deltaTime);
        }
    }

    public void TakeDamage(float amount)
    {
        enemyHealth -= amount;
        if (enemyHealth < 0)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
