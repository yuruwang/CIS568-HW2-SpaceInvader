using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour {

    static int POWER_STRAIGHT = 1;
    static int POWER_WIGGY = 5;
    static int POWER_CANNON = 1;
    static int INIT_SPEED = 20;

    Vector3 dir;
    string origin; // alien or cannon
    int power;
    int speed;

	// Use this for initialization
	void Start () {

		
	}
	
	// Update is called once per frame
	void Update () {
        if (this.dir != null) {
            this.Move(this.dir, this.speed, Time.deltaTime);
        }



    }

    void Move (Vector3 dir, int speed, float deltaT) {
        transform.Translate(dir * speed * deltaT);
    }

    private void OnTriggerEnter(Collider other) {
        switch (this.origin) {
            case "alien":
                if (other.tag == "alien" || other.tag == "aliensGroup" || other.gameObject.name == "DeadLine") {
                    return;
                } else {
                    Destroy(this.gameObject);
                }

                break;

            case "cannon":
                if (other.tag == "cannon" || other.tag == "aliensGroup" || other.name == "DeadLine") {
                    return;
                } else {
                    Destroy(this.gameObject);
                }

                break;

        }
    }

    public void ConfigMissile(Vector3 dir, string origin, string type) {
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

    // getter
    public string GetOrigin() {
        return this.origin;
    }
}
