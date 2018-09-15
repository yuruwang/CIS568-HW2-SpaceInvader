 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alien : MonoBehaviour {
    static float restOriSpeed = 50.0f;
    static int maxFlyingMissiles = 3;
    static Vector3 fireDir = new Vector3(0, -1, 0);

    int score;
    string type;
    Vector3Int idx;
    bool died = false;

    int flyingMissileCount = 0;
    bool isStriker = false;

    GameManager gm;
    public GameObject StraightMissilePrefab;
    public GameObject WiggyMissilePrefab;
    public Material DeadMaterial;


    // Use this for initialization
    void Start () {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
	
	// Update is called once per frame
	void Update () {
        if (this.isStriker && !this.died) {
            float randomNum = Random.Range(0.0f, 1.0f);
            if (randomNum < gm.GetFiringProb()) {
                this.Fire();
            }
        }
    }

    private void OnCollisionEnter(Collision collision) {
        if (this.died) {
            if (collision.collider.name == "BottomWall") {
                Destroy(this.gameObject);
            }
            return;
        }
        if (collision.collider.tag == "missile") {
            GameObject missileObj = collision.collider.gameObject;
            Missile missile = missileObj.GetComponent<Missile>();
            if (missile.GetOrigin() == "alien" || !missile.IsAlive()) {
                return;
            }
            this.Die();
            missile.SetAlive(false);

        }

        if (collision.collider.name == "DeadLine") {
            gm.Lose();
            this.gameObject.SetActive(false);

        }

        this.ResetOrientation();


    }

    public  void ResetOrientation() {
        //Quaternion origOrientation = this.transform.parent.rotation;
        //Rigidbody rb = this.GetComponent<Rigidbody>();
        //print("original orientation: " + origOrientation);
        //rb.MoveRotation(origOrientation);
    }

    public void ConfigAlien(string type, int score, Vector3Int idx) {
        this.type = type;
        this.score = score;
        this.idx = idx;
    }

    void Die() {
        this.died = true;
        AudioClip clip = this.GetComponent<AudioSource>().clip;
        AudioSource.PlayClipAtPoint(clip, this.transform.position);
        gm.KillAlien(this.idx);
        //Destroy(this.gameObject);
        this.transform.parent = null;
        this.GetComponent<Rigidbody>().useGravity = true;

        // change color
        //Renderer rend = this.GetComponent<Renderer>();
        //rend.material.shader = Shader.Find("_Color");
        //rend.material.SetColor("_Color", Color.green);
        this.GetComponent<MeshRenderer>().material = DeadMaterial;

        Rigidbody rb = this.GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.None;
        rb.constraints = RigidbodyConstraints.FreezePositionZ;
    }

    void Fire() {
        if (this.flyingMissileCount >= Alien.maxFlyingMissiles) {
            return;
        }

        // determine type of missile
        Vector3 offset = new Vector3(0, -0.5f, 0);
        float randomNum = Random.Range(0.0f, 1.0f);
        if (randomNum < gm.GetStraightMissileProb()) {
            GameObject missileObj = Instantiate(StraightMissilePrefab, transform.position + offset, transform.rotation);
            Missile missile = missileObj.GetComponent<Missile>();
            missile.ConfigMissile(new Vector3(0, -1, 0), this.gameObject, "straight");
        } else {
            GameObject missileObj = Instantiate(WiggyMissilePrefab, transform.position + offset, transform.rotation);
            Missile missile = missileObj.GetComponent<Missile>();
            missile.ConfigMissile(new Vector3(0, -1, 0), this.gameObject, "wiggy");
        }
        this.flyingMissileCount++;


    }
    // getters
    public int GetScore() {
        return this.score;
    }

    // setters
    public void SetStriker(bool isStriker) {
        this.isStriker = isStriker;
    }

    public void DeductFlyingMissiles() {
        this.flyingMissileCount--;
        if (this.flyingMissileCount < 0) {
            this.flyingMissileCount = 0;
        }
    }
        
}
