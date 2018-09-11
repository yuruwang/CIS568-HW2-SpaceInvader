using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aliens : MonoBehaviour {
    Vector3 movingDir = new Vector3(1, 0, 0);
    GameManager gm;

	// Use this for initialization
	void Start () {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update () {
        this.Move(this.movingDir, this.gm.GetAliensSpeed(), Time.deltaTime);
    }

    void Move(Vector3 dir, int speed, float deltaT) {
        transform.Translate(dir * speed * deltaT);
    }

    private void OnTriggerEnter(Collider other) {
        // change moving direction
        if (other.name == "LeftWall") {
            this.movingDir = new Vector3(1, 0, 0);
            transform.Translate(gm.GetAdventStep());

        } else if (other.name == "RightWall") {
            this.movingDir = new Vector3(-1, 0, 0);
            transform.Translate(gm.GetAdventStep());
        } 
    }


}
