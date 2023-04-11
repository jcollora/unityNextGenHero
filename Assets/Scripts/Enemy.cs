using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour {
    // enemy state
    public float health;
    private Vector3 position;
    
    // one manager of gamestate
    public static Manager mg;

    // Start is called before the first frame update
    void Start() {
        health = 4;
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
}
