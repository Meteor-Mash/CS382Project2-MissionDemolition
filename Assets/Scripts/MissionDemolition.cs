using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum GameMode {
    idle,
    playing,
    levelEnd
}

public class MissionDemolition : MonoBehaviour
{
    static private MissionDemolition S; 

    [Header("Inscribed")]
    public Text uitLevel; 
    public Text uitShots; 
    public Vector3 castlePos; 
    public GameObject[] castles;
    public GameObject restartButtonPrefab;
    public Transform canvasTransform;

    [Header("Dynamic")]
    public int level; 
    public int levelMax; 
    public int shotsTaken;
    public GameObject castle; 
    public GameMode mode = GameMode.idle;
    public string showing = "Show Slingshot";
    private Slingshot slingshot;

    
    void Start()
    {
        S = this; 

        level = 0;
        shotsTaken = 0;
        levelMax = castles.Length;
        StartLevel();
    }

    void StartLevel() {
        if(castle != null) {
            Destroy(castle);
        }

        Projectile.DESTROY_PROJECTILES();

        castle = Instantiate<GameObject>(castles[level]);
        castle.transform.position = castlePos;

        Goal.goalMet = false;

        UpdateGUI();

        mode = GameMode.playing;

        FollowCam.SWITCH_VIEW(FollowCam.eView.both);
    }

    void UpdateGUI() {
        uitLevel.text = "Level: " + (level + 1) + " of " + levelMax;
        uitShots.text = "Shots Taken: " + shotsTaken;
    }

    void Update()
    {
        UpdateGUI();

        if((mode == GameMode.playing) && Goal.goalMet) {
            mode = GameMode.levelEnd;
            FollowCam.SWITCH_VIEW(FollowCam.eView.both);

            Invoke("NextLevel", 2f);  
        }
    }

    void NextLevel() {
        level++;
        if(level == levelMax) {
            GameOver();
            return;
        }
        StartLevel();
    }

    void GameOver()
    {
        if (castle != null)
        {
            Destroy(castle);
        }
        GameObject slingshotGO = GameObject.Find("Slingshot");
        slingshot = slingshotGO.GetComponent<Slingshot>();
        slingshot.gameOver = true;
        GameObject[] projectiles = GameObject.FindGameObjectsWithTag("Projectile");
        foreach (GameObject tempGO in projectiles)
        {
            Destroy(tempGO);
        }
        GameObject restartButtonGO = Instantiate(restartButtonPrefab, canvasTransform);
    }

    static public void SHOT_FIRED() {
        S.shotsTaken++;
    }

    static public GameObject GET_CASTLE() {
        return S.castle;
    }
}
