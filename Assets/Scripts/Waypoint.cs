using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Waypoint : MonoBehaviour {
    // enemy state
    public float health;
    private Vector3 position;
    private Vector3 initialPosition;
    
    // one manager of gamestate
    public static Manager mg;

    // Start is called before the first frame update
    void Start() {
        health = 4;
        position = transform.position;
        initialPosition = position;
    }

    public void hit(int damage) {
        health -= damage;
        // check if dead, kill if so
        if (health <= 0) {
            // randomize transform based on position + random offset
            resurrect();
        }
        // adjust the alpha of the enemy with renderer
        Renderer r = gameObject.GetComponent<Renderer>();
        Color c = r.material.color;
        c.a -= 1f / 4f; // decrease alpha by 1/4 of its maximum value
        c.a = Mathf.Clamp(c.a, 0f, 1f); // clamp alpha to be between 0 and its maximum value
        r.material.SetColor("_Color", c);
    }

    private void resurrect() {
        //relocate enemy (it had died and will now reappear in a similar location)
        int dif = Random.Range(-15,15);
        position.x = initialPosition.x + dif;
        position.y = initialPosition.y + dif;
        transform.position = position;     

        Renderer r = gameObject.GetComponent<Renderer>();
        Color c = r.material.color;
        c.a = 1; // decrease alpha by 1/4 of its maximum value
        r.material.SetColor("_Color", c);  
        health = 4;
    }
}
