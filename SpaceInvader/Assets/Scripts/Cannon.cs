using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour {
    GameObject flyingMissile = null;
    public GameObject CannonBrickPrefab;
    public GameObject missilePrefab;
    public GameObject platform;

    static float explosionPower = 20.0f;
    static float explosionRadius = 2.0f;
    static float explosionUpforce = 0.0f;

    static int bricksX = 5;
    static int bricksY = 5;
    static int bricksZ = 5;

    GameObject[] bricks = new GameObject[Cannon.bricksX * Cannon.bricksY * Cannon.bricksZ];

    public int speed = 10;
    Vector3 initPos = new Vector3(-10, 0, 0);
    GameManager gm;
    bool died = false;




    // Use this for initialization
    void Start() {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        this.InitCannon();
    }

    // Update is called once per frame
    void Update() {

        KeyboardEvents();

        if (this.died) {
            StartCoroutine(this.Explode());
        }



    }

    void InitCannon() {
        this.transform.position = this.initPos;
        Vector3 center = this.transform.position;
        float offsetX = 0.2f;
        float offsetY = 0.2f;
        float offsetZ = 0.2f;

        int count = 0;
        for (int i = -Cannon.bricksX / 2; i <= Cannon.bricksX / 2; ++i) {
            for (int j = -Cannon.bricksY / 2; j <= Cannon.bricksY / 2; ++j) {
                for (int k = -Cannon.bricksZ / 2; k <= Cannon.bricksZ / 2; ++k) {
                    GameObject brickObj = Instantiate(CannonBrickPrefab, center + new Vector3(i * offsetX, j * offsetY, k * offsetZ), this.transform.rotation);
                    brickObj.transform.parent = this.transform;
                    this.bricks[count] = brickObj;
                    count++;
                }
            }
        }
        this.died = false;

    }

    void DestroyBricks() {
        for (int i = 0; i < this.bricks.Length; ++i) {
            Destroy(this.bricks[i]);
        }
    }

    void Move(Vector3 dir, int speed, float deltaT) {
        if (this.died) {
            return;
        }

        this.transform.Translate(dir * speed * deltaT);
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

    private void OnCollisionEnter(Collision collision) {
        if (collision.collider.tag == "missile") {
            Missile missile = collision.collider.gameObject.GetComponent<Missile>();
            if (missile.GetOrigin() == "alien" && missile.IsAlive()) {
                missile.SetAlive(false);
                this.Die();
            }
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.name == "LeftBlocker") {
            this.Move(new Vector3(1, 0, 0), speed * 3, Time.deltaTime);
        }
        if (other.name == "RightBlocker") {

            this.Move(new Vector3(-1, 0, 0), speed * 3, Time.deltaTime);
        }
    }


    void Die() {
        AudioClip clip = this.GetComponent<AudioSource>().clip;
        AudioSource.PlayClipAtPoint(clip, this.transform.position);
        gm.DeductLive(1);
        this.died = true;
       
    }

    public void PlayObtainLivesSound() {
        GameObject RewardSound = GameObject.Find("RewardSound");
        AudioClip clip = RewardSound.GetComponent<AudioSource>().clip;
        AudioSource.PlayClipAtPoint(clip, this.transform.position);

    }

    IEnumerator Explode() {
        for (int i = 0; i < this.bricks.Length; ++i) {
            Rigidbody rb = this.bricks[i].AddComponent<Rigidbody>() as Rigidbody;

            if (rb != null) {
                rb.mass = 0.5f;
                rb.AddExplosionForce(Cannon.explosionPower, this.transform.position, Cannon.explosionRadius, Cannon.explosionUpforce, ForceMode.Impulse);
            }
        }
        yield return new WaitForSeconds(3);
        this.DestroyBricks();
        this.InitCannon();

    }



    void Fire() {
        Vector3 offSet = new Vector3(0, 2, 0);
        if (this.flyingMissile != null) {
            return;
        }
        GameObject missileObj = Instantiate(missilePrefab, transform.position + offSet, transform.rotation);
        this.flyingMissile = missileObj;
        Missile missile = missileObj.GetComponent <Missile> ();
        missile.ConfigMissile(new Vector3(0, 1, 0), this.gameObject, "straight");
    }

    public void ClearFlyingMissile() {
        this.flyingMissile = null;
    }
}
