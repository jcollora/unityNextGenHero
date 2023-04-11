using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
    // managed gamestate variables
    public int projectileCount;
    public int collideCount;
    public int enemyCount;
    public int kills;

    //bools for game control
    public bool planesMoveSequential = true;
    public bool showWaypoints = true;

    // movment mode
    public bool mouseMode;

    // enemy prefab objects for runtime creation
    public Enemy enemy;
    
    // waypoint prefabs and list
    public Waypoint[] wps;
    public Waypoint wp1;
    public Waypoint wp2;
    public Waypoint wp3;
    public Waypoint wp4;
    public Waypoint wp5;
    public Waypoint wp6;
    
    // text out to ui
    public Text stateStatus;

    void Start() {
        // handing manager off
        Player.mg = this;
        Enemy.mg = this;
        Projectile.mg = this;
        // initital values
        collideCount = 0;
        projectileCount = 0;
        kills = 0;
        enemyCount = 0;
        mouseMode = true;
        wps = new Waypoint[6];
        spawnWaypoints();
    }

    // Update is used to quit application
    void Update() {
        if (Input.GetKeyDown(KeyCode.Q)) {
            Application.Quit();
        }
        if (Input.GetKeyDown(KeyCode.J)) {
            //toggle sequential vs random plane movement on pressing J
            planesMoveSequential = ! planesMoveSequential;
        }
        if (Input.GetKeyDown(KeyCode.H)) {
            //toggle showing waypoints vs hidden (and largely inactive waypoints) on pressing H
            showWaypoints = ! showWaypoints;
        }

        spawnEnemies();
        updateStateStatus();
    }

    // returns a position within inner 90* of the sb
    private Vector3 getSpawnPos() {
        Vector3 sb = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
        // spawns enemies within 90% of sb
        sb.x *= 0.9f;
        sb.y *= 0.9f;
        return new Vector3(Random.Range(-sb.x, sb.x), Random.Range(-sb.y, sb.y), 0);
    }

    // spawns enemies until there are 10 on the screen
    private void spawnEnemies() {
        while (enemyCount < 10) {
            Instantiate(enemy, getSpawnPos(), Quaternion.Euler(0f, 0f, 0f));
            enemyCount++;
        }
    }

    // spawn waypoints
    //will spawn waypoints. When waypoints are 'killed', not destroyed, just resurected (relocated)
    private void spawnWaypoints() {
        //change enemy to the waypoint object/prefab...(https://answers.unity.com/questions/237806/instantiate-not-returning-anything.html)
        var a = Instantiate(wp1, getSpawnPos(), Quaternion.Euler(0f, 0f, 0f)) as Waypoint;
        var b = Instantiate(wp2, getSpawnPos(), Quaternion.Euler(0f, 0f, 0f)) as Waypoint;
        var c = Instantiate(wp3, getSpawnPos(), Quaternion.Euler(0f, 0f, 0f)) as Waypoint;
        var d = Instantiate(wp4, getSpawnPos(), Quaternion.Euler(0f, 0f, 0f)) as Waypoint;
        var e = Instantiate(wp5, getSpawnPos(), Quaternion.Euler(0f, 0f, 0f)) as Waypoint;
        var f = Instantiate(wp6, getSpawnPos(), Quaternion.Euler(0f, 0f, 0f)) as Waypoint;
        wps[0] = a.GetComponent<Waypoint>();
        wps[1] = b.GetComponent<Waypoint>();
        wps[2] = c.GetComponent<Waypoint>();
        wps[3] = d.GetComponent<Waypoint>();
        wps[4] = e.GetComponent<Waypoint>();
        wps[5] = f.GetComponent<Waypoint>();
    } 

    // updates the status ui for gamestate
    private void updateStateStatus() {
        stateStatus.text = "HERO: Drive(";
        stateStatus.text += mouseMode ? "Mouse) " : "Key) ";
        stateStatus.text += "TouchedEnemy(" + collideCount + ") ";
        stateStatus.text += "EGG: OnScreen(" + projectileCount + ") ";
        stateStatus.text += "ENEMY: Count(" + enemyCount + ") Destroyed( " + kills + ")";
    } 
}
