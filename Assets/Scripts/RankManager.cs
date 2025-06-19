using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class RankManager : MonoBehaviour
{
    public static RankManager Instance;

    public Transform contentParent;       
    public GameObject RankPrefab; 
    public RankInfor RankMe; 

    private void Awake() => Instance = this;

    [System.Serializable]
    public class RankEntry
    {
        public int rank;           
        public string username;    
        public int score;         
        public bool isMe;
    }

    public void Show(List<RankEntry> list, int myRank)
    {
        foreach (Transform child in contentParent) Destroy(child.gameObject);

        foreach (var entry in list)
        {
            GameObject go = Instantiate(RankPrefab, contentParent);
            var item = go.GetComponent<RankInfor>();
            item.Setup(entry.rank, entry.username, entry.score, entry.isMe);
            if (entry.isMe) RankMe.Setup(entry.rank, entry.username, entry.score, entry.isMe);
        }
    }
}
