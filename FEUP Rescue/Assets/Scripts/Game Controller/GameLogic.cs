﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameLogic : MonoBehaviour
{
    public static GameLogic instance;
    public GameObject GameOverMenu;
    public GameObject VictoryMenu;
    public bool GameIsPaused;
    public GameObject PauseMenu;
    int points = 0;
    bool speeding = false;
    bool protectedShield = false;
    float protectedTime = 0f;
    public float maxProtectedTime = 2f;
    float speedingTime = 0f;
    public float maxSpeedingTime = 4f;
    public bool inTimeOut = false;
    float deltaTimeOut = 0f;
    public float maxTimeOut = 1f;
    public bool gameOverbool;
    public bool gameWon;
    bool caughtBomb;
    public TextMeshProUGUI score;

    void Awake() {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    void Init()
    {
        gameOverbool = false;
        gameWon = false;
        points = 0;
        speeding = false;
        speedingTime = 0f;
        protectedShield = false;
        protectedTime = 0f;
        inTimeOut = false;
        deltaTimeOut = 0f;
        GameIsPaused = false;
        caughtBomb = false;
        Time.timeScale = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
        if (speeding) {
            speedingTime += Time.deltaTime;
            if(speedingTime >= maxSpeedingTime)
            {
                speeding = false;
                speedingTime = 0f;
            }
        }
        if(protectedShield){
            protectedTime += Time.deltaTime;
            if(protectedTime >= maxProtectedTime){
                PlayerHealth.instance.SetProtected(false);
                protectedShield = false;
                protectedTime = 0f;
            }
        }
        if(inTimeOut) {
            deltaTimeOut += Time.deltaTime;
            if(deltaTimeOut >= maxTimeOut)
            {
                inTimeOut = false;
                deltaTimeOut = 0f;
            }
        }
    }

    public void IncrementPoints() {
        if(HasGameEnded())
            return;
        points++;
        ScoreManager.scoreManager.UpdateScore(points);
    }

    public void SpeedUp() {
        speeding = true;
        speedingTime = 0f;
    }

    public void CaughtBomb() {
        caughtBomb = true;
    }

    public bool isSpeeding() {
        return speeding;
    }

    public bool isProtected(){
        return protectedShield;
    }

    public void ProtectedWithShield(){
        PlayerHealth.instance.SetProtected(true);
        protectedShield = true;
        protectedTime = 0f;
    }

    public void setTimeOut() {
        inTimeOut = true;
    }

    public void GameOver(string reason) {
        gameOverbool = true;
        Timer.instance.StopTimer();
        FindObjectOfType<AudioManager>().Play("GameOver");
        GameOverMenu.SetActive(true);
    }

    public int GetScore() {
        int bomb = 0;
        if(caughtBomb)
            bomb = 10;
        
        return (points + bomb + Timer.instance.GetTimerPoints());
    }

    public void GameWon() {
        gameWon = true;
        Timer.instance.StopTimer();
        ScoreManager.scoreManager.ShowPoints(GetScore());
        FindObjectOfType<AudioManager>().Play("Victory");
        VictoryMenu.SetActive(true);
    }

    public void SetGameOver() {
        gameOverbool = true;
    }

    public bool HasGameEnded() {
        if(gameWon || gameOverbool)
            return true;
        return false;
    }

    void Resume()
    {
        PauseMenu.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    void Pause()
    {
        PauseMenu.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }
}
