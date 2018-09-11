using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    public static GameManager instance;
    static float initFiringProb = 0.0005f;
    static float initStraightMissileProb = 0.9f;
    static float initWiggyMissileProb = 0.1f;
    static int initAliensSpeed = 1;
    static int totalLevels = 1;
    static int totalLives = 3;

    static Vector3 spaceShipStartPos = new Vector3(18.7f, 0, 26.8f);
    static Vector3 aliensGroupStartPos = new Vector3(-10, 0, 11);
    static Vector3 aliensAdventStep = new Vector3(0, 0, -1);
    static int rows = 5;
    static int cols = 11;

    public GameObject smallAllienPrefab;
    public GameObject mediumAllienPrefab;
    public GameObject largeAllienPrefab;
    public GameObject spaceShipPrefab;
    public GameObject[] cameras = new GameObject[2];


    int livesRemaining = GameManager.totalLives;
    int scores = 0;
    int currLevel = 1;  // current game level
    float nextSpaceShipTime;

    float firingProb;
    float straightMissileProb;
    float wiggyMissileProb;
    int aliensSpeed;

    bool lose = false;

    GameObject[,] aliens;

    int aliensRemaining;

    GameObject flyingSpaceShip;
    GameObject canvas;
    GameObject aliensGroup;
    int currCameraIdx;

    private void Awake()
    {
        //if (instance == null)
        //{
        //    instance = this;
        //}
        //else
        //{
        //    Destroy(this.gameObject);
        //}

        //DontDestroyOnLoad(this.gameObject);
    }


    // Use this for initialization
    void Start () {
        this.canvas = GameObject.Find("Canvas");  // may need to change
        this.aliensGroup = GameObject.Find("Aliens");
        // initialize varibales
        this.InitGame();
       

    }
	
	// Update is called once per frame
	void Update () {
        if (this.lose) {
            StartCoroutine(GameOver());
        } else {
            this.ShootSpaceShip();
        }

        this.KeyboardEvents();


    }


    void KeyboardEvents() {
        if (Input.GetKeyDown(KeyCode.C)) {
            this.SwitchCamera();
        }

    }

    void SwitchCamera() {
        // disable current camera
        GameObject currCamera = this.cameras[this.currCameraIdx];
        currCamera.GetComponent<AudioListener>().enabled = false;
        currCamera.GetComponent<Camera>().enabled = false;

        // change current selected camera idx
        if (this.currCameraIdx == this.cameras.Length - 1) {
            this.currCameraIdx = 0;
        } else {
            this.currCameraIdx++;
        }

        // activate selected camera
        GameObject SelectedCamera = this.cameras[this.currCameraIdx];
        SelectedCamera.GetComponent<AudioListener>().enabled = true;
        SelectedCamera.GetComponent<Camera>().enabled = true;

    }

    public void InitGame() {
        this.scores = 0;
        this.currLevel = 1;
        this.livesRemaining = GameManager.totalLives;
        this.aliensRemaining = GameManager.rows * GameManager.cols;
        this.firingProb = GameManager.initFiringProb;
        this.straightMissileProb = GameManager.initStraightMissileProb;
        this.wiggyMissileProb = GameManager.initWiggyMissileProb;
        this.aliensSpeed = GameManager.initAliensSpeed;
        this.aliensGroup.transform.position = GameManager.aliensGroupStartPos;
        this.InitAliens();
        this.currCameraIdx = 0;


        this.canvas.GetComponent<Canvas>().UpdateScores(this.scores);
        this.canvas.GetComponent<Canvas>().UpdateLives(this.livesRemaining);
        this.canvas.GetComponent<Canvas>().UpdateLevel (this.currLevel);


    }

    public void InitAliens() {
        // initialize alliens
        Vector3 center = this.aliensGroup.transform.position;
        center += new Vector3(1, 0, 0);
        Quaternion orientation = this.aliensGroup.transform.rotation;
        float offsetX = 1.8f;
        float offsetZ = 1.8f;

        this.aliens = new GameObject[GameManager.rows, GameManager.cols];
        for (int i = 0; i < 2; i++) {
            for (int j = 0; j < cols; j++) {
                GameObject alienObj = Instantiate(smallAllienPrefab, center + new Vector3(offsetX * j, 0, offsetZ * i), orientation);
                this.aliens[i, j] = alienObj;
                alienObj.transform.parent = aliensGroup.transform;

                Alien alien = alienObj.GetComponent<Alien>();
                alien.ConfigAlien("small", 10, new Vector3Int(i, j, 0));
                // aliens in first row are strikers
                if (i == 0) {
                    alien.SetStriker(true);
                }

            }
        }

        for (int i = 2; i < 4; i++) {
            for (int j = 0; j < cols; j++) {
                GameObject alienObj = Instantiate(mediumAllienPrefab, center + new Vector3(offsetX * j, 0, offsetZ * i), orientation);
                this.aliens[i, j] = alienObj;
                alienObj.transform.parent = aliensGroup.transform;

                Alien alien = alienObj.GetComponent<Alien>();
                alien.ConfigAlien("medium", 20, new Vector3Int(i, j, 0));

            }
        }

        for (int i = 4; i < 5; i++) {
            for (int j = 0; j < cols; j++) {
                GameObject alienObj = Instantiate(largeAllienPrefab, center + new Vector3(offsetX * j, 0, offsetZ * i), orientation);
                this.aliens[i, j] = alienObj;
                alienObj.transform.parent = aliensGroup.transform;

                Alien alien = alienObj.GetComponent<Alien>();
                alien.ConfigAlien("large", 30, new Vector3Int(i, j, 0));

            }
        }
    }

    void ShootSpaceShip() {
        if (this.flyingSpaceShip == null) {
            if (Time.time >= this.nextSpaceShipTime) {
                GameObject spaceShipObj = Instantiate(spaceShipPrefab, GameManager.spaceShipStartPos, transform.rotation);
                this.flyingSpaceShip = spaceShipObj;

            }
        }
    }

    public void AddDifficulty() {
        this.aliensSpeed += 5;
        this.firingProb += 0.0005f;
        this.straightMissileProb -= 0.2f;
    }

    public void KillAlien(Vector3Int idx) {
        Alien alien = this.aliens[idx.x, idx.y].GetComponent<Alien>();
        this.AddScore(alien.GetScore());
        this.RestAlienStriker(idx);
        this.aliensRemaining--;
        if (this.aliensRemaining <= 0) {
            this.GoToNextLevel();
        }

        if (this.aliensRemaining == 25 || this.aliensRemaining == 10 || this.aliensRemaining == 5) {
            this.AddDifficulty();
        }
    }

    public void AddScore (int score) {
        this.scores += score;
        this.canvas.GetComponent<Canvas>().UpdateScores(this.scores);
    }

    public void RestAlienStriker(Vector3Int alienIdx) {
        int i = alienIdx.x;
        int j = alienIdx.y;
        if (i == GameManager.rows - 1) {
            return;
        }
        for (int m = i + 1; m < GameManager.rows; m++) {
            if (this.aliens[m, j] != null) {
                Alien nextAlien = this.aliens[m, j].GetComponent<Alien>();
                nextAlien.SetStriker(true);
                break;
            }
        }



    }

    public void DeductLive(int live) {
        this.livesRemaining -= live;
        this.canvas.GetComponent<Canvas>().UpdateLives(this.livesRemaining);
        if (this.livesRemaining == 0) {
            this.Lose();
        }
    }

    public void GoToNextLevel() {
        // reset some variables
        this.firingProb = GameManager.initFiringProb;
        this.straightMissileProb = GameManager.initStraightMissileProb;
        this.wiggyMissileProb = GameManager.initWiggyMissileProb;
        this.aliensSpeed = GameManager.initAliensSpeed;

        this.currLevel++;
        this.aliensGroup.transform.position = GameManager.aliensGroupStartPos;

        if (currLevel > GameManager.totalLevels) {
            currLevel = 1;
        }
        this.aliensGroup.transform.Translate(GameManager.aliensAdventStep * (this.currLevel - 1));
        this.canvas.GetComponent<Canvas>().UpdateLevel(this.currLevel);
        this.InitAliens();
    }

    public void Lose () {
        this.lose = true;
    }

    public IEnumerator GameOver () {
        this.canvas.GetComponent<Canvas>().showLose(true);
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene(0);
    }


    // setters
    public void SetNextSpaceShipTime () {
        float randNum = Random.Range(5, 10);
        this.nextSpaceShipTime = Time.time + randNum;
    }

    // getters

    public float GetFiringProb() {
        return this.firingProb;
    }
    public float GetStraightMissileProb() {
        return this.straightMissileProb;
    }

    public float GetWiggyMissileProb() {
        return this.wiggyMissileProb;
    }

    public int GetAliensSpeed() {
        return this.aliensSpeed;
    }

    public Vector3 GetAdventStep() {
        return GameManager.aliensAdventStep;
    }
}
