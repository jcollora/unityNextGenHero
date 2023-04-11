using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour { 
    // projectile state  
    public  float velocity = 40;
    private Vector3 sb;
    private Vector3 position;

    // one manager of gamestate
    public static Manager mg;

    void Update() {
        // update screen bounds for obj deletion
        sb = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
        position = transform.position;
        // update projectile position *boilerplate from udPlayerPos in player.cs
        float rad = Mathf.Deg2Rad * transform.localEulerAngles.z;
        float delta = velocity * Time.deltaTime;
        position.y += Mathf.Cos(rad) * delta;
        position.x -= Mathf.Sin(rad) * delta;
        isDead();
        transform.position = position;
    }

    // does damage to enemy upon collision (it is the only case)
    private void OnTriggerEnter2D(Collider2D collider) {
        if (collider.gameObject.name.Substring(0, 5) == "Plane") {
            Enemy target = collider.gameObject.GetComponent<Enemy>();
            target.hit(1);
            // update the manager/ui state of projectriles to reflect -1 
            mg.projectileCount--;
            Destroy(gameObject);
        }
    }

    // deletes the projectile if it is out of sb.
    void isDead() {
        if (position.x < -sb.x || position.y < -sb.y || position.x > sb.x || position.y > sb.y) {
            // update the manager/ui state to reflect one less projectile
            mg.projectileCount--;
            Destroy(gameObject);
        }   
    }

}
