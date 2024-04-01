using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Enemy : MonoBehaviour
{
    public int health;
    public int damageToPlayer;
    public int moneyOnDeath;
    public float moveSpeed;

    // Path
    private Transform[] path;
    private int curPathWaypoint;

    public GameObject healthBarPrefab;

    public static event UnityAction OnDestroyed;

    void Start ()
    {
        path = GameManager.instance.enemyPath.waypoints;

        // create the health bar
        Canvas canvas = FindObjectOfType<Canvas>();
        GameObject healthBar = Instantiate(healthBarPrefab, canvas.transform);
        healthBar.GetComponent<EnemyHealthBar>().Initialize(this);
    }

    void Update ()
    {
        MoveAlongPath();
    }

    // called every frame to move the enemy towards the end of the path
    void MoveAlongPath ()
    {
        if(curPathWaypoint < path.Length)
        {
            transform.position = Vector3.MoveTowards(transform.position, path[curPathWaypoint].position, moveSpeed * Time.deltaTime);

            if(transform.position == path[curPathWaypoint].position)
                curPathWaypoint++;
        }
        // if we're at the end of the path
        else
        {
            GameManager.instance.TakeDamage(damageToPlayer);
            OnDestroyed.Invoke();
            Destroy(gameObject);
        }
    }

    // called when a tower deals damage to the enemy
    public void TakeDamage (int amount)
    {
        health -= amount;

        if(health <= 0)
        {
            GameManager.instance.AddMoney(moneyOnDeath);
            OnDestroyed.Invoke();
            Destroy(gameObject);
        }
    }
}