using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour {

    static int POWER_STRAIGHT = 1;
    static int POWER_WIGGY = 5;
    static int POWER_CANNON = 1;
    static int INIT_SPEED = 20;

    Vector3 dir;
    GameObject origin; // alien or cannon
    int power;
    int speed;
    bool isAlive = true;

    public Material DeadMaterial;


    // Use this for initialization
    void Start () {

		
	}
	
	// Update is called once per frame
	void Update () {
        if (this.dir != null && this.isAlive) {
            this.Move(this.dir, this.speed, Time.deltaTime);
        }
    }

    void Move (Vector3 dir, int speed, float deltaT) {
        transform.Translate(dir * speed * deltaT, Space.World);
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.collider.name == "FrontWall") {
            this.isAlive = false;
        }

        if (collision.collider.name == "BottomWall") {
            Destroy(this.gameObject);
        }

        switch (this.origin.tag) {
            case "alien":
                if (collision.collider.tag == "alien" ||
                    collision.collider.tag == "aliensGroup" ||
                    collision.collider.name == "DeadLine") {
                    return;
                } else {
                    Alien alien = this.origin.GetComponent<Alien>();
                    alien.DeductFlyingMissiles();
                    this.Die();
                }

                break;

            case "cannon":
                if (collision.collider.tag == "cannon" ||
                    collision.collider.tag == "cannonBrick" ||
                    collision.collider.tag == "aliensGroup" ||
                    collision.collider.name == "DeadLine") {
                    return;
                } else if (collision.collider.name == "BackWall") {
                    this.dir.y = this.dir.y * (-1);
                } else {
                    Cannon cannon = this.origin.GetComponent<Cannon>();
                    cannon.ClearFlyingMissile();
                    this.Die();

                }

                break;

        }
    }

    void Die() {
        this.GetComponent<Rigidbody>().useGravity = true;

        Rigidbody rb = this.GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.None;
        rb.constraints = RigidbodyConstraints.FreezePositionZ;

        // change material
        this.GetComponent<MeshRenderer>().material = DeadMaterial;


    }


    public void ConfigMissile(Vector3 dir, GameObject origin, string type) {
        this.dir = dir;
        this.origin = origin;
        this.speed = Missile.INIT_SPEED;
        switch (type) {
            case "straight":
                this.power = Missile.POWER_STRAIGHT;
                break;
            case "wiggy":
                this.power = Missile.POWER_WIGGY;
                break;
        }
    }

    // setter
    void SetSpeed (int speed) {
        this.speed = speed;
    }

    public void SetAlive(bool isAlive) {
        this.isAlive = isAlive;
    }


    // getter
    public string GetOrigin() {
        return this.origin.tag;
    }

    public bool IsAlive() {
        return this.isAlive;
    }

}
