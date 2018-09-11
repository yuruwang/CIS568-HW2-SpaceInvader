using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour {
    private CharacterController controller;
    GameObject flyingMissile = null;

    public int speed = 10;
    Vector3 initPos = new Vector3(-10, 0, -3);
    public GameObject missilePrefab;
    public GameObject gameManager;


    void Move(Vector3 dir, int speed, float deltaT) {
        controller.Move(dir * speed * deltaT);
    }


    void KeyboardEvents() {
        float moveX = Input.GetAxis("Horizontal");
        if (moveX != 0) {
            Move(new Vector3(moveX, 0, 0).normalized, speed, Time.deltaTime);
        }

        if (Input.GetKeyDown("space")) {
            this.Fire();
        }

    }

    // Use this for initialization
    void Start() {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update() {
        KeyboardEvents();

    }

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "missile") {
            Missile missile = other.gameObject.GetComponent<Missile>();
            if (missile.GetOrigin() == "alien") {
                this.Die();
            }
        }
    }

    void Die() {
        AudioClip clip = this.GetComponent<AudioSource>().clip;
        AudioSource.PlayClipAtPoint(clip, this.transform.position);

        GameManager gm = this.gameManager.GetComponent<GameManager>();
        gm.DeductLive(1);
        //Destroy(this.gameObject);

        this.Reset();
    }


    void Reset() {
        this.transform.position = this.initPos;
    }

    void Fire() {
        if (this.flyingMissile != null) {
            return;
        }
        GameObject missileObj = Instantiate(missilePrefab, transform.position, transform.rotation);
        this.flyingMissile = missileObj;
        Missile missile = missileObj.GetComponent <Missile> ();
        missile.ConfigMissile(new Vector3(0, 0, 1), "cannon", "straight");
    }
}
