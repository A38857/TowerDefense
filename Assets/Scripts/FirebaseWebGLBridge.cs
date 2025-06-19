using System;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;
using MiniJSON;
using System.Collections.Generic;
using System.Collections;
using System.Text;

public class FirebaseWebGLBridge : MonoBehaviour
{
    public static FirebaseWebGLBridge Instance;

    private string uid;
    private string username;

#if UNITY_WEBGL && !UNITY_EDITOR
    [DllImport("__Internal")]
    private static extern void WriteUserData(string uid, string name, int score, int level, string starsJson);

    [DllImport("__Internal")]
    private static extern void ReadUserData(string uid);

    [DllImport("__Internal")]
    private static extern void UnityReady(); // Gọi từ C# sang JS
#endif

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

    void Start()
    {
        if (Instance == null)
            Instance = this;

#if UNITY_WEBGL && !UNITY_EDITOR
        UnityReady(); // Gọi JS báo Unity đã sẵn sàng
#endif
    }

    // Call JS if login Firebase Success
    public void OnUserLoggedIn(string data)
    {
        //Debug.Log("User logged in with data: " + data);
        string[] split = data.Split('|');
        uid = split[0];
        username = split[1];

        //Debug.Log($"✅ Logged in: {uid} | {username}");
        GameManager.Instance?.SetUserData(uid, username, 0, 1, new int[16]);

#if UNITY_WEBGL && !UNITY_EDITOR
        Application.ExternalCall("ReadUserData", uid);
#endif
    }

    public void OnDataReceived(string json)
    {
        if (string.IsNullOrEmpty(json))
        {
            return;
        }

        try
        {
            var dict = Json.Deserialize(json) as Dictionary<string, object>;

            if (dict == null)
            {
                return;
            }

            PlayerData data = new PlayerData();
            data.userId = dict["userId"].ToString();
            data.username = dict["username"].ToString();
            data.totalScore = Convert.ToInt32(dict["totalScore"]);
            data.levelReached = Convert.ToInt32(dict["levelReached"]);

            if (!dict.ContainsKey("starsPerLevel"))
            {
                data.starsPerLevel = new int[16]; // default
            }
            else
            {
                var starListRaw = dict["starsPerLevel"] as List<object>;
                if (starListRaw != null)
                {
                    data.starsPerLevel = new int[starListRaw.Count];
                    for (int i = 0; i < starListRaw.Count; i++)
                        data.starsPerLevel[i] = Convert.ToInt32(starListRaw[i]);
                }
                else
                {
                    data.starsPerLevel = new int[16];
                }
            }

            GameManager.Instance?.LoadData(data);
        }
        catch (Exception e)
        {
        }
    }

    public void OnJSReady()
    {

#if UNITY_WEBGL && !UNITY_EDITOR
    StartCoroutine(DelayRetrySend()); // ✅ Gọi sau delay
#endif
    }

    IEnumerator DelayRetrySend()
    {
        yield return new WaitForSeconds(0.55f); // đợi 200ms
        Application.ExternalCall("RetrySendUserData");
    }

    // Write Data to Firestore
    public void SaveUserData(int score, int level, int[] starsArray)
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        if (string.IsNullOrEmpty(uid) || string.IsNullOrEmpty(username))
        {
            //Debug.LogWarning("⚠️ Không thể ghi dữ liệu: uid hoặc username trống");
            return;
        }

        string starsJson = JsonUtility.ToJson(new IntArrayWrapper(starsArray));
        //Debug.Log($"📤 Ghi dữ liệu: {uid}, {username}, {score}, {level}, {starsJson}");
        WriteUserData(uid, username, score, level, starsJson);
#endif
    }

    // Read Data from Firestore
    public void ReloadUserData()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        if (!string.IsNullOrEmpty(uid))
        {
            Debug.Log("🔄 Đọc lại dữ liệu từ Firebase...");
            ReadUserData(uid);
        }
        else
        {
            Debug.LogWarning("⚠️ Không thể đọc: uid rỗng");
        }
#endif
    }

    // Check Data (map from Firebase)
    [Serializable]
    public class PlayerData
    {
        public string userId;
        public string username;
        public int totalScore;
        public int levelReached;
        public int[] starsPerLevel;
    }

    [System.Serializable] public class IntArrayWrapper 
    {
        public int[] stars;
        public IntArrayWrapper(int[] array) => stars = array; 
    }

    [System.Serializable]
    public class LeaderboardEntry
    {
        public int rank;
        public string username;
        public int score;
        public bool isMe;
    }

    [System.Serializable]
    public class LeaderboardPayload
    {
        public List<RankManager.RankEntry> list;
        public int myRank;
    }

    public void OnLeaderboardData(string json)
    {
        try
        {
            var root = Json.Deserialize(json) as Dictionary<string, object>;
            var listRaw = root["list"] as List<object>;
            int myRank = Convert.ToInt32(root["myRank"]);

            List<RankManager.RankEntry> entries = new List<RankManager.RankEntry>();
            foreach (var obj in listRaw)
            {
                var dict = obj as Dictionary<string, object>;
                RankManager.RankEntry entry = new RankManager.RankEntry();
                entry.rank = Convert.ToInt32(dict["rank"]);
                entry.username = dict["username"].ToString();
                entry.score = Convert.ToInt32(dict["score"]);
                entry.isMe = Convert.ToBoolean(dict["isMe"]);
                entries.Add(entry);
            }

            RankManager.Instance?.Show(entries, myRank);
        }
        catch (Exception e)
        {
            Debug.LogError("❌ Parse lỗi leaderboard (MiniJSON): " + e);
        }
    }
}
