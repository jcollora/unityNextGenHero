using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    // hero state 
    public bool mouseMode;
    public float velocity;
    private Vector3 position;
    private Vector3 mouse;
    
    // hero constraints
    public float maxAcceleration = 13;
    public float maxVelocity = 100;

    // shooting 
    public float shootCooldown = 0.2f;
    private float lastShootTime = 0;
    
    // one manager of gamestate, instantiated by user
    public static Manager mg;
    
    // projectile prefab
    public Projectile egg;

    // cooldown bar
    public GameObject cooldownBar;  

    void Start() {
        velocity = 20;
        mouseMode = true;
    }

    void Update() {
        // update mouse pos
        mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // controlls for mousemode
        if (Input.GetKeyDown(KeyCode.M)) {
            mouseMode = !mouseMode;
            mg.mouseMode = !mg.mouseMode;
            velocity = mouseMode ? 0 : 20;
        }

        udPlayerDir();
        udPlayerVel();
        udPlayerPos();
        checkShoot();
    }

    // instantly kills enemeny and updates manager state
    private void OnTriggerEnter2D(Collider2D collider) {
        if(collider.gameObject.name.Substring(0, 5) == "Plane") {
            Enemy target = collider.gameObject.GetComponent<Enemy>();
            target.hit(4);
            // update the manager/ui state of projectriles to reflect -1 
            mg.collideCount++;
        }
    }

    // updates player direction
    private void udPlayerDir() {
        float delta = 0;
        if (Input.GetKey(KeyCode.D)) {
            delta -= Time.deltaTime * 45;
        }
        if (Input.GetKey(KeyCode.A)) {
            delta += Time.deltaTime * 45;
        }
        transform.Rotate(0, 0, delta);
    }

    // updates player velocity 
    private void udPlayerVel() {
        if(mouseMode) {
            velocity = 0;
        } else {
            if (Input.GetKey(KeyCode.W))
                velocity += maxAcceleration * (1 - velocity / maxVelocity) * Time.deltaTime;

            if (Input.GetKey(KeyCode.S))
                velocity -= maxAcceleration * (1 + velocity / maxVelocity) * Time.deltaTime;
        }
    }

    // updates player position based on mouseMode state
    private void udPlayerPos() {
        if (mouseMode) {
            // uses the mouse position, when onscreen, to determine position
            position.x = mouse.x;
            position.y = mouse.y;
        } else {
            // keyboard based updates
            // use updated velocity to find delta
            float rad = Mathf.Deg2Rad * transform.localEulerAngles.z;
            float delta = velocity * Time.deltaTime;
            position.y += Mathf.Cos(rad) * delta;
            position.x -= Mathf.Sin(rad) * delta;
        }
        transform.position = position;
    }

    // shoots an egg prefab if spacebar is pressed
    private void checkShoot() {
        if (Input.GetKey("space") && Time.time > lastShootTime) {
            // last shoot time = next time we are allowed to shoot an egg
            lastShootTime = Time.time + shootCooldown;
            Instantiate(egg, position, transform.rotation);
            mg.projectileCount++;
        }

        ///////////// Shoot Cooldown UI ///////////////////
        // next valid shoot time < current timeXX

        //next shoot time is more than current time (so we can't shoot yet)
        if(Time.time - lastShootTime <= 0)
        {
            cooldownBar.transform.localScale = new Vector3(3, 0.28f, 0); 
            //have shot recently, need a cooldown bar
            float timeRemaining = lastShootTime - Time.time;
            float percentageOfBar = timeRemaining / shootCooldown;
            Vector3 scaled = Vector3.Scale(cooldownBar.transform.localScale, new Vector3((percentageOfBar), 1, 1));
            cooldownBar.transform.localScale = scaled;


            Debug.Log(percentageOfBar * 100);
            

        } else {

            //reducing amount
            cooldownBar.transform.localScale = new Vector3(0, 0, 0); //0? 1?


            //have not shot recently, no cooldown bar
            //float cooldown = last - Time.time;
            //float fillamt = cooldownLeft
        }
        ///////////// Shoot Cooldown UI ///////////////////


    }
}
