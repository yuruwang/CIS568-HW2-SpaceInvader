using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceShip : MonoBehaviour {
    int score;
    int speed = 10;
    Vector3 movingDir = new Vector3(-1, 0, 0);
    GameManager gm;
    Canvas canvas;

	// Use this for initialization
	void Start () {
        System.Random rnd = new System.Random();
        this.score = rnd.Next(0, 100);

        this.gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        this.canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
    }
	
	// Update is called once per frame
	void Update () {
        this.Move(this.movingDir, this.speed, Time.deltaTime);

    }

    void Move(Vector3 dir, int speed, float deltaT) {
        transform.Translate(dir * speed * deltaT);
    }

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "missile") {
            this.Die();
        }
        Destroy(this.gameObject);
        gm.SetNextSpaceShipTime();

    }

    void Die () {
        AudioClip clip = this.GetComponent<AudioSource>().clip;
        AudioSource.PlayClipAtPoint(clip, this.transform.position);

        gm.AddScore(this.score);
        this.canvas.SetSpaceShipScore(this.score);
    }
}
