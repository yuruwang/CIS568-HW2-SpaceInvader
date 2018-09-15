using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Canvas : MonoBehaviour {

    public GameObject scoreText;
    public GameObject livesText;
    public GameObject gameOverText;
    public GameObject ReminderText;
    public GameObject levelText;
    public GameObject warningText;

    bool showReminder = false;
    bool showWarning = false;
    float showWarningDuration = 0;
    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        if (this.showReminder) {
            StartCoroutine(this.ShowReminder());
        }

        if (this.showWarning) {
            StartCoroutine(this.ShowWarning(this.showWarningDuration));
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


    public void SetReminderText(string text) {
        this.ReminderText.GetComponent<Text>().text = text;
        this.showReminder = true;
    }

    public IEnumerator ShowReminder() {
        this.ReminderText.GetComponent<Text>().enabled = true;
        yield return new WaitForSeconds(2);
        this.showReminder = false;
        this.ReminderText.GetComponent<Text>().enabled = false;

    }

    public IEnumerator ShowWarning(float duration) {
        this.warningText.GetComponent<Text>().enabled = true;
        yield return new WaitForSeconds(duration);
        this.warningText.GetComponent<Text>().enabled = false;
        this.showWarning = false;
        this.showWarningDuration = 0;

    }

    public void InvokeWarning(float duration) {
        this.showWarning = true;
        this.showWarningDuration = duration;
    }



    public void showLose(bool show) {
        if (this.showWarning) {
            this.showWarning = false;
        }
        this.gameOverText.gameObject.SetActive(show);
    }


}
