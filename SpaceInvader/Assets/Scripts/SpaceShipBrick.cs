using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceShipBrick : MonoBehaviour {

    string type;
    int value;

    GameManager gm;



    // Use this for initialization
    void Start () {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ConfigBrick(string type, int value) {
        this.type = type;
        this.value = value;
    }

    private void OnTriggerEnter(Collider other) {
        if (this.type == "heart") {
            if (other.tag == "cannon") {
                gm.AddLives(this.value);
                gm.ShowReminder("+ " + this.value + " lives");
                other.GetComponent<Cannon>().PlayObtainLivesSound();
            }
        }

        if (other.name == "BottomWall") {
            Destroy(this.gameObject);
        }

    }



    // getters
    public int getValue() {
        return this.value;
    }

    public string getType() {
        return this.type;
    }
}
