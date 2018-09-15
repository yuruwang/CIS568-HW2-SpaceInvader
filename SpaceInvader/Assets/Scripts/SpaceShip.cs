using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceShip : MonoBehaviour {
    int score;
    int speed = 10;
    Vector3 movingDir = new Vector3(-1, 0, 0);
    GameManager gm;

    static int bricksX = 3;
    static int bricksY = 3;
    static int bricksZ = 3;
    GameObject[] bricks = new GameObject[SpaceShip.bricksX * SpaceShip.bricksY * SpaceShip.bricksZ];

    public GameObject SpaceShipBrickPrefab;
    public Material HeartMaterial;


    static float explosionPower = 10.0f;
    static float explosionRadius = 4.0f;
    static float explosionUpforce = 0.0f;

    // Use this for initialization
    void Start () {
        System.Random rnd = new System.Random();
        this.score = rnd.Next(0, 100);

        this.gm = GameObject.Find("GameManager").GetComponent<GameManager>();

        this.InitSpaceShip();
    }
	
	// Update is called once per frame
	void Update () {
        this.Move(this.movingDir, this.speed, Time.deltaTime);

    }

    void InitSpaceShip() {
        //this.transform.position = this.initPos;
        Vector3 center = this.transform.position;
        float offsetX = 1.0f;
        float offsetY = 1.0f;
        float offsetZ = 1.0f;

        
        System.Random rnd= new System.Random();
        Vector3Int heartPos = new Vector3Int(0, 0, 0);

        int count = 0;
        for (int i = -SpaceShip.bricksX / 2; i <= SpaceShip.bricksX / 2; ++i) {
            for (int j = -SpaceShip.bricksY / 2; j <= SpaceShip.bricksY / 2; ++j) {
                for (int k = -SpaceShip.bricksZ / 2; k <= SpaceShip.bricksZ / 2; ++k) {
                    GameObject brickObj = Instantiate(SpaceShipBrickPrefab, center + new Vector3(i * offsetX, j * offsetY, k * offsetZ), this.transform.rotation);
                    brickObj.transform.parent = this.transform;
                    this.bricks[count] = brickObj;
                    count++;

                    SpaceShipBrick brick = brickObj.GetComponent<SpaceShipBrick>();
                    if (i == heartPos.x && j == heartPos.y && k == heartPos.z) {
                        brick.ConfigBrick("heart", rnd.Next(1, 3));

                        // change material
                        brickObj.GetComponent<MeshRenderer>().material = HeartMaterial;

                    } else {
                        brick.ConfigBrick("brick", 0);
                    }
                }
            }
        }
    }

    void Move(Vector3 dir, int speed, float deltaT) {
        transform.Translate(dir * speed * deltaT);
    }

    private void OnTriggerEnter(Collider other) {


    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.collider.tag == "missile") {
            this.Die();
            gm.SetNextSpaceShipTime();
        } else if (collision.collider.tag == "wall") {
            Destroy(this.gameObject);
            gm.SetNextSpaceShipTime();
        }


    }

    void Explode() {
        for (int i = 0; i < this.bricks.Length; ++i) {
            GameObject brickObj = this.bricks[i];
            brickObj.transform.parent = null;
            Rigidbody rb = brickObj.AddComponent<Rigidbody>() as Rigidbody;
            rb.constraints = RigidbodyConstraints.None;
            rb.constraints = RigidbodyConstraints.FreezePositionZ;
            rb.useGravity = true;
            rb.AddExplosionForce(SpaceShip.explosionPower, this.transform.position, SpaceShip.explosionRadius, SpaceShip.explosionUpforce, ForceMode.Impulse);

        }

    }

    void Die () {
        AudioClip clip = this.GetComponent<AudioSource>().clip;
        AudioSource.PlayClipAtPoint(clip, this.transform.position);

        gm.AddScore(this.score);
        gm.ShowReminder("+ " + this.score + " pts");

        this.GetComponent<Rigidbody>().useGravity = true;
        Rigidbody rb = this.GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.None;

        //// change color
        //Renderer rend = this.GetComponent<Renderer>();
        //rend.material.shader = Shader.Find("_Color");
        //rend.material.SetColor("_Color", Color.green);

        this.Explode();
    }
}
