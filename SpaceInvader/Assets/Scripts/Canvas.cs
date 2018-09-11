using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Canvas : MonoBehaviour {

    public GameObject scoreText;
    public GameObject livesText;
    public GameObject gameOverText;
    public GameObject SpaceShipScoreText;
    public GameObject levelText;

    bool showSpaceShipScore = false;
    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        if (this.showSpaceShipScore) {
            StartCoroutine(this.ShowSpaceShipScore());
        }
    }

    public void UpdateScores(int score) {
        this.scoreText.GetComponent<Text>().text = "Score: " + score;
    }

    public void UpdateLives(int lives) {
        this.livesText.GetComponent<Text>().text = "Lives: " + lives;
    }

    public void UpdateLevel(int level) {
        this.levelText.GetComponent<Text>().text = "Level: " + level;
    }

    public IEnumerator ShowSpaceShipScore() {
        this.SpaceShipScoreText.GetComponent<Text>().enabled = true;
        yield return new WaitForSeconds(2);
        this.showSpaceShipScore = false;
        this.SpaceShipScoreText.GetComponent<Text>().enabled = false;

    }

    public void SetSpaceShipScore(int score) {
        this.SpaceShipScoreText.GetComponent<Text>().text = "+" + score;
        this.showSpaceShipScore = true;
    }

    public void showLose(bool show) {
        this.gameOverText.gameObject.SetActive(show);

    }
}
