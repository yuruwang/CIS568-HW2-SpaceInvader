 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alien : MonoBehaviour {
    int score;
    string type;
    Vector3Int idx;

    List<GameObject> flyingMissiles = new List<GameObject>();
    bool isStriker = false;

    GameManager gm;
    public GameObject StraightMissilePrefab;
    public GameObject WiggyMissilePrefab;


    // Use this for initialization
    void Start () {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
	
	// Update is called once per frame
	void Update () {
        if (this.isStriker) {
            float randomNum = Random.Range(0.0f, 1.0f);
            if (randomNum < gm.GetFiringProb()) {
                this.Fire();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "missile") {
            GameObject missileObj = other.gameObject;
            Missile missile = missileObj.GetComponent<Missile>();
            if (missile.GetOrigin() == "alien") {
                return;
            }
            this.Die();

        } else if (other.name == "DeadLine") {
            gm.Lose();
            this.gameObject.SetActive(false);
        }
    }

    public void ConfigAlien(string type, int score, Vector3Int idx) {
        this.type = type;
        this.score = score;
        this.idx = idx;
    }

    void Die () {
        AudioClip clip = this.GetComponent<AudioSource>().clip;
        AudioSource.PlayClipAtPoint(clip, this.transform.position);
        gm.KillAlien(this.idx);
        Destroy(this.gameObject);
    }

    void Fire() {
        bool firable = false;
        if (this.flyingMissiles.Count > 0) {
            for (int i = 0; i < this.flyingMissiles.Count; i++) {
                if (this.flyingMissiles[i] == null) {
                    firable = true;
                    this.flyingMissiles.RemoveAt(i);
                }
            }
        } else {
            firable = true;
        }
        if (!firable) {
            return;
        }

        // determine type of missile
        float randomNum = Random.Range(0.0f, 1.0f);
        if (randomNum < gm.GetStraightMissileProb()) {
            GameObject missileObj = Instantiate(StraightMissilePrefab, transform.position, transform.rotation);
            Missile missile = missileObj.GetComponent<Missile>();
            missile.ConfigMissile(new Vector3(0, 0, -1), "alien", "straight");
            this.flyingMissiles.Add(missileObj);

        } else {
            GameObject missileObj = Instantiate(WiggyMissilePrefab, transform.position, transform.rotation);
            Missile missile = missileObj.GetComponent<Missile>();
            missile.ConfigMissile(new Vector3(0, 0, -1), "alien", "wiggy");
            this.flyingMissiles.Add(missileObj);

        }

    }
    // getters
    public int GetScore() {
        return this.score;
    }

    // setters
    public void SetStriker(bool isStriker) {
        this.isStriker = isStriker;
    }
        
}
