using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour {
    // enemy state
    int waypointTarget;
    public int speed = 15;
    public float turnSpeed = 0.1f;
    public float health;
    public bool movingRandom;
    
    // one manager of gamestate
    public static Manager mg;

    // Start is called before the first frame update
    void Start() {
        movingRandom = false;
        waypointTarget = 0;
        health = 4;
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.J)) {
            //toggle sequential vs random plane movement on pressing J
            movingRandom = ! movingRandom;
        }
        move();
    }

    public void hit(int damage) {
        health -= damage;
        // check if dead, kill if so
        if (health <= 0) {
            mg.enemyCount--;
            mg.kills++;
            Destroy(gameObject);
        }
        // adjust the alpha of the enemy with renderer
        Renderer r = gameObject.GetComponent<Renderer>();
        Color c = r.material.color;
        c.a -= 1f / 4f; // decrease alpha by 1/4 of its maximum value
        c.a = Mathf.Clamp(c.a, 0f, 1f); // clamp alpha to be between 0 and its maximum value
        r.material.SetColor("_Color", c);
    }

    private void move() {
        if (movingRandom) {
            moveRandom();
        } else {
            moveSequential();
        }
    }

    private void moveRandom()
    {
        // check distance to target, if less than var change target and start rotation
        //           waypoint pos,     enemy pos
        if(reachedTarget(mg.wps[waypointTarget].transform.position, transform.position))
        {
            waypointTarget =  Random.Range(0, mg.wps.Length);
        }  
        turnAndMove(mg.wps[waypointTarget].transform.position);

    }

    private void moveSequential()
    {
        //condition ? consequent : alternative
        if(reachedTarget(mg.wps[waypointTarget].transform.position, transform.position))
        {
            waypointTarget = (waypointTarget >= 5 ? 0 : waypointTarget + 1);
        }        
        turnAndMove(mg.wps[waypointTarget].transform.position);
    }

    private void turnAndMove(Vector3 target) {
        var dir = (target - transform.position).normalized;
        var dirSmoothed = Vector2.Lerp(transform.up, dir, turnSpeed);
        transform.up = dirSmoothed;
        transform.position += (Vector3) dirSmoothed * speed * Time.deltaTime;
    }

    private bool reachedTarget(Vector3 waypointPOS, Vector3 enemyPOS, int RequiredProximity = 5)
    {
        //determine if you are near target 
        //(where required proximity says how close you need to be, default of 20)

        float xDiff = Mathf.Abs(waypointPOS.x - enemyPOS.x);
        float yDiff = Mathf.Abs(waypointPOS.y - enemyPOS.y);

        if((xDiff + yDiff) <= RequiredProximity)
        {
            //are close enough
            return true;
        }
        //too far apart for now
        return false;
    }
}
