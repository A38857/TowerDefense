using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    const int MAX_LEVEL = 16;
    public static GameManager Instance;
    private string userId;
    private string username;
    private int currentLevel;
    private int[] starList;
    private int totalScore;
    private int levelReached;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        currentLevel = levelReached;
#if UNITY_EDITOR
        levelReached = 16;
        currentLevel = 4;
        int[] aList = { 3,2,1,0,0,0,0,0,0,0,0,0,0,0,0,0 };
        starList = aList;
#endif
    }

    public void SetLevel(int newlevel)
    {
        currentLevel = newlevel;
    }

    public int GetLevel()
    {
        return currentLevel;
    }

    public void SetUserData(string uid,string uname, int score, int level, int[] stars)
    {
        userId = uid;
        username = uname;
        totalScore = score;
        levelReached = level;
        starList = stars;
    }

    public string GetUserId() => userId;
    public int[] GetStarList() => starList;
    public string GetUsername() => username;
    public int GetScore() => totalScore;
    public int GetLevelReached() => levelReached;

    public void UpdateProgress(int levelIndex, int level, int starsEarned, int scoreEarned)
    {
        if (levelIndex >= 0 && levelIndex < MAX_LEVEL)
        {
            if (starsEarned > starList[levelIndex]) starList[levelIndex] = starsEarned;
            totalScore += scoreEarned;
            if (level + 1 > levelReached) levelReached = level + 1;
        }
    }

    public void SaveToFirebase()
    {
        if (!Application.isEditor)
        {
            var wrapper = new FirebaseWebGLBridge.IntArrayWrapper(starList);
            string starsJson = JsonUtility.ToJson(wrapper);
            Application.ExternalCall("WriteUserData", userId, username, totalScore, levelReached, starsJson);
        }
    }

    public void LoadData(FirebaseWebGLBridge.PlayerData data)
    {
        this.userId = string.IsNullOrEmpty(data.userId) ? userId : data.userId;
        this.username = data.username; 
        this.totalScore = data.totalScore;
        this.levelReached = data.levelReached;
        this.starList = data.starsPerLevel;
        currentLevel = levelReached;
        MenuUIManager.Instance.SetLevelButton();

        Debug.Log($"✅ LoadData OK: {userId} | {username}");
    }

    public void Rank()
    {
        Application.ExternalCall("ShowLeaderboard", GameManager.Instance.userId);
    }    
}
