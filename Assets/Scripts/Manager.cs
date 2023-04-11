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

    // movment mode
    public bool mouseMode;

    // enemy prefab objects for runtime creation
    public Enemy enemy;

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
    }

    // Update is used to quit application
    void Update() {
        if (Input.GetKeyDown(KeyCode.Q)) {
            Application.Quit();
        }
        spawnEnemies();
        updateStateStatus();
    }

    // returns a position within inner 90* of the sb
    private Vector3 getEnemySpawnPos() {
        Vector3 sb = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
        // spawns enemies within 90% of sb
        sb.x *= 0.9f;
        sb.y *= 0.9f;
        return new Vector3(Random.Range(-sb.x, sb.x), Random.Range(-sb.y, sb.y), 0);
    }

    // spawns enemies until there are 10 on the screen
    private void spawnEnemies() {
        while (enemyCount < 10) {
            Instantiate(enemy, getEnemySpawnPos(), Quaternion.Euler(0f, 0f, 0f));
            enemyCount++;
        }
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
