using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Networking;
using UnityEditor;
using Unity.VisualScripting;
using UnityEngine.UIElements;
using System.Linq;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.SocialPlatforms.Impl;
using TMPro;
using DG.Tweening.Core.Easing;
using DG.Tweening;

public class LeverManager : MonoBehaviour
{
    const int MAX_LEVEL = 16;
    public static LeverManager main;
    public GameObject PlotList;
    public GameObject Menu;
    public GameObject MapCancas;
    public GameObject PanelPause;
    public GameObject floatingText;
    public float Scale = 1;
    public float worldWidth, worldHeight, menuWidth, TimeWin, TimeLose;
    public int TotalCoin, HealthPoint;
    private bool IsPause, IsWin, IsLose;
    public Sprite SpriteDebugPoint;
    public Transform transformTextCoin;
    public Transform transformTextHealth;
    public List<List<int>> Map = new List<List<int>>();
    public List<Vector2Int> StartList = new List<Vector2Int>();
    public List<Vector2Int> EndList = new List<Vector2Int>();
    public List<List<string>> MapString = new List<List<string>>();
    [SerializeField] private ParticleSystem firework1;
    [SerializeField] private ParticleSystem firework2;
    [SerializeField] private TextMeshProUGUI TextScore;
    [SerializeField] private TextMeshProUGUI TextLevel;
    private int Score =0;
    private int Star = 0;
    public int CountWave;
    private Vector2Int StartPos, EndPos;
    public List<List<Transform>> ListPath = new List<List<Transform>>();

    private void Awake()
    {
        main = this;
        PlotList = GameObject.Find("Plots");

        Resolution currentRes = Screen.currentResolution;
        Screen.SetResolution(currentRes.width, currentRes.height, true);
    }

    // Start is called before the first frame update
    void Start()
    {
        // Data
        TotalCoin = 175;
        Score = 0;
        TimeWin = TimeLose = 0;
        HealthPoint = 10;
        IsPause = IsWin = IsLose = false;
        Time.timeScale = 1.0f;
        Map.Clear();
        MapString.Clear();
        if(!MusicManager.Instance.IsPlay()) MusicManager.Instance.PlayMusic();

        // Load Level
        int level = GameManager.Instance.GetLevel();
        if (level == 0) level = 1;
        TextLevel.SetText("Level " + level);
        string levelString = (level / 100 > 0) ? "" + level : ((level / 10 > 0) ? "0" + level : "00" +level);
        string filePath = "level/level_"+ levelString;
        Map = LoadMap(filePath);
        GameObject PlotPrefab = Resources.Load<GameObject>("Prefabs/PlotPrefab");
        GameObject RoadPrefab = Resources.Load<GameObject>("Prefabs/RoadPrefab");

        // Scale
        worldHeight = Camera.main.orthographicSize * 2;
        worldWidth = worldHeight * (Screen.width) / Screen.height;
        float ScaleX = (worldWidth- (200.0f * FindObjectOfType<Canvas>().scaleFactor/ Screen.width)*worldWidth)/ Map[0].Count;
        float ScaleY = worldHeight / Map.Count;
        ScaleY = ScaleX * 2;
        Scale = Mathf.Min(ScaleX, ScaleY);
        float CellSize = PlotPrefab.GetComponent<SpriteRenderer>().bounds.size.x * Scale;
        //float offsetX = -(Map[0].Count * CellSize) / 2 + CellSize / 2;
        RectTransform menuRect = Menu.GetComponent<RectTransform>();
        menuWidth = menuRect.rect.width * menuRect.lossyScale.x;
        float offsetX = -worldWidth/2 + (menuWidth / Screen.width)*worldWidth + CellSize/2;
        float offsetY = -(Map.Count * CellSize) / 2 + CellSize / 2;

        //Debug.Log(worldWidth);
        // Create Plot
        for (int x = 0; x < Map.Count; x++)
        {
            for (int y = 0; y < Map[x].Count; y++)
            {
                if (Map[x][y] == 1)
                {
                    Vector3 Position = new Vector3(offsetX + y * CellSize, offsetY + x * CellSize, 0);
                    GameObject Plot = Instantiate(PlotPrefab, Position, Quaternion.identity);
                    Plot.transform.SetParent(PlotList.transform);
                    Plot.transform.localScale = new Vector3(Scale, Scale , 1);
                }
                else 
                {
                    if (Map[x][y] == 0)
                    {
                        Vector3 Position = new Vector3(offsetX + y * CellSize, offsetY + x * CellSize, 0);
                        GameObject road = Instantiate(RoadPrefab, Position, Quaternion.identity);
                        road.transform.SetParent(PlotList.transform);
                        road.transform.localScale = new Vector3(Scale, Scale, 1);
                        SpriteRenderer srRoad = road.GetComponent<SpriteRenderer>();
                        srRoad.sortingOrder = -10;
                    }
                    else if (Map[x][y] == 2 && MapString[x][y] == "2")
                    {
                        Vector3 Position = new Vector3(offsetX + y * CellSize, offsetY + x * CellSize, 0);
                        GameObject road = Instantiate(RoadPrefab, Position, Quaternion.identity);
                        road.transform.SetParent(PlotList.transform);
                        road.transform.localScale = new Vector3(Scale, Scale, 1);
                        SpriteRenderer srRoad = road.GetComponent<SpriteRenderer>();
                        srRoad.sortingOrder = -10;
                        string spriteName = "Sprites/grass_tile_" + 2;
                        Sprite newSprite = Resources.Load<Sprite>(spriteName);
                        road.GetComponent<SpriteChanger>().ChangeSprite(newSprite);

                    }
                    else if (MapString[x][y][0] == 'l')
                    {
                        Vector3 Position = new Vector3(offsetX + y * CellSize, offsetY + x * CellSize, 0);
                        GameObject road = Instantiate(RoadPrefab, Position, Quaternion.identity);
                        road.transform.SetParent(PlotList.transform);
                        road.transform.localScale = new Vector3(Scale, Scale, 1);
                        SpriteRenderer srRoad = road.GetComponent<SpriteRenderer>();
                        srRoad.sortingOrder = -10;
                        string spriteName;
                        if (MapString[x][y] == "l") spriteName = "Sprites/Environment/lake/l" ;
                        else spriteName = "Sprites/Environment/lake/l_" + int.Parse(MapString[x][y][1].ToString());
                        Sprite newSprite = Resources.Load<Sprite>(spriteName);
                        road.GetComponent<SpriteChanger>().ChangeSprite(newSprite);
                    }
                }    
            }
        }

        // start End
        for (int x = 0; x < MapString.Count; x++)
        {
            for (int y = 0; y < MapString[x].Count; y++)
            {
                if(MapString[x][y] == "s")
                {
                    StartPos = new Vector2Int(y, x);
                    StartList.Add(new Vector2Int(y, x));
                }
                else if (MapString[x][y] == "e")
                {
                    EndPos = new Vector2Int(y, x);
                    EndList.Add(new Vector2Int(y, x));
                }
            }
        }

       AStarPathFinding pathfinder = new AStarPathFinding(Map);

       // List<Vector2Int> path = pathfinder.FindPath(new Vector2Int(4, 0), new Vector2Int(17, 1));
       for(int i=0;i<StartList.Count;i++)
        {
            for(int j=0;j<EndList.Count;j++)
            {
                List<Transform> PathEnermy = new List<Transform>();
                StartPos = StartList[i];
                EndPos = EndList[j];
                List<Vector2Int> path = pathfinder.FindPath(StartPos, EndPos);
                if (path != null)
                {
                    List<Vector2> PathPos = GetCornerPoints(path, CellSize);
                    for (int k = 0; k < PathPos.Count; k++)
                    {
                        GameObject Point = new GameObject("Point");
                        Point.transform.position = PathPos[k];
                        PathEnermy.Add(Point.transform);
                    }
                    ListPath.Add(PathEnermy);
                    Vector3 endPos = PathPos[PathPos.Count - 1];
                    endPos.z = 10f;
                    GameObject HolePrefab = Resources.Load<GameObject>("Prefabs/HolePrefab");
                    GameObject Hole = Instantiate(HolePrefab, endPos, Quaternion.identity);
                    Hole.transform.localScale = new Vector3(Scale, Scale, 1);
                }
            }
        }

        // Sound
        SoundManager.Instance.PlayBegin();
    }

    private void Update()
    {
        if(IsWin && TimeWin >0 && Time.time - TimeWin > 2.0f)
        {
            TimeWin = 0;
            Time.timeScale = 0;
            PopupManager.Instance.ShowWin(Star);
        }
        if (IsLose && TimeLose > 0 && Time.time - TimeLose > 0.5f)
        {
            TimeLose = 0;
            Time.timeScale = 0;

            // Sound
            SoundManager.Instance.PlayLose();
            MusicManager.Instance.StopMusic();

            // PopUp
            PopupManager.Instance.ShowLose();
        }
    }

    public List<List<int>> LoadMap(string resourcePath)
    {
        List<List<int>> mapData = new List<List<int>>();
        MapString.Clear();

        // Load file Resources
        TextAsset levelText = Resources.Load<TextAsset>(resourcePath);
        if (levelText == null)
        {
            Debug.LogError($"❌ Không tìm thấy file Resources/{resourcePath}.txt");
            return mapData;
        }

        string[] lines = levelText.text.Split('\n');
        bool isReading = false;
        bool isNextLineWaveCount = false;

        foreach (string rawLine in lines)
        {
            string line = rawLine.Trim();

            if (line == "[map_begin]")
            {
                isReading = true;
                continue;
            }
            if (line == "[map_end]")
            {
                isReading = false;
            }

            if (isNextLineWaveCount)
            {
                if (int.TryParse(line, out int waveCount))
                {
                    CountWave = waveCount;
                }
                isNextLineWaveCount = false;
                continue;
            }

            if (line == "[waves]")
            {
                isNextLineWaveCount = true;
                continue;
            }

            if (isReading)
            {
                List<int> intRow = new List<int>();
                List<string> strRow = new List<string>();

                foreach (string num in line.Split(' ', StringSplitOptions.RemoveEmptyEntries))
                {
                    strRow.Add(num);

                    if (int.TryParse(num, out int value)) intRow.Add(value);
                    else if (num == "s" || num == "e") intRow.Add(0);
                    else intRow.Add(2);
                }

                mapData.Add(intRow);
                MapString.Add(strRow);
            }
        }

        return mapData;
    }

    public List<Vector2> GetCornerPoints(List<Vector2Int> path, float CellSize)
    {
        List<Vector2> corners = new List<Vector2>();

        if (path == null || path.Count < 2) return corners;
        if (path[0].x == path[1].x) corners.Add(Utils.GridToWorld(new Vector2Int(path[0].x, path[0].y - 1), CellSize, Map[0].Count,Map.Count));
        if (path[0].y == path[1].y) corners.Add(Utils.GridToWorld(new Vector2Int(path[0].x-1, path[0].y), CellSize, Map[0].Count, Map.Count));
        for (int i = 1; i < path.Count - 2; i++)
        {
            Vector2Int prev = path[i - 1];
            Vector2Int curr = path[i];
            Vector2Int next = path[i + 1];
            Vector2Int dir1 = curr - prev;
            Vector2Int dir2 = next - curr;
            if (dir1 != dir2) corners.Add(Utils.GridToWorld(curr, CellSize, Map[0].Count, Map.Count));
        }
        if (path[path.Count - 1].x == path[path.Count - 2].x) corners.Add(Utils.GridToWorld(new Vector2Int(path[path.Count - 1].x , path[path.Count - 1].y+1), CellSize, Map[0].Count, Map.Count));
        if (path[path.Count - 1].y == path[path.Count - 2].y) corners.Add(Utils.GridToWorld(new Vector2Int(path[path.Count - 1].x+1, path[path.Count - 1].y ), CellSize, Map[0].Count, Map.Count));

        return corners;
    }

    public void SetPause()
    {
        IsPause = true;
        Time.timeScale = 0f;
        PopupManager.Instance.ShowPausePanel();
    }

    public void SetResume()
    {
        IsPause = false;
        Time.timeScale = 1.0f;
        PopupManager.Instance.HidePausePanel();
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void NextLevel()
    {
        if (GameManager.Instance.GetLevel() > MAX_LEVEL) SceneManager.LoadScene("MainMenu");
        else SceneManager.LoadScene("PlayScene");
    }

    public void GoHome()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void IncreseCoin(int AmountCoin)
    {
        TotalCoin += AmountCoin;
        GameObject floatTextInstance = Instantiate(floatingText, transformTextCoin.transform.position - new Vector3(6, -10, -100), Quaternion.identity);
        floatTextInstance.transform.SetParent(MapCancas.transform);
        floatTextInstance.GetComponent<FloatText>().Show("+"+ AmountCoin, Color.yellow);
    }

    public void SetWin(bool IsWin)
    {
        // Data
        this.IsWin = IsWin;
        TimeWin = Time.time;
        TextScore.SetText("Score: " + Score);
        if (HealthPoint >= 9) Star = 3;
        else if (HealthPoint >= 5) Star = 2;
        else if (HealthPoint >= 1) Star = 1;
        else Star = 0;
        GameManager.Instance.SetLevel(GameManager.Instance.GetLevel()+1);
        GameManager.Instance.UpdateProgress(GameManager.Instance.GetLevel()-2, GameManager.Instance.GetLevel() - 1, Star, Score);
        GameManager.Instance.SaveToFirebase();

        // Effect
        firework1.Play();
        firework2.Play();

        // Sound
        SoundManager.Instance.PlayWin();
        MusicManager.Instance.StopMusic();
    }

    public bool GetWin()
    {
        return IsWin;
    }

    public bool GetLose()
    {
        return IsLose;
    }

    public void AddScore()
    {
        Score += 10;
    }

    public void ChangeHealth(int Hp = 1)
    {
        // Change
        if (HealthPoint > 0)
        {
            HealthPoint -= Hp;
            GameObject floatTextInstance = Instantiate(floatingText, transformTextHealth.transform.position - new Vector3(6, -10, -100), Quaternion.identity);
            floatTextInstance.transform.SetParent(MapCancas.transform);
            floatTextInstance.GetComponent<FloatText>().Show("-" + Hp, Color.red);
            if (HealthPoint <= 0) HealthPoint = 0;
        }

        // Lose
        if(HealthPoint == 0 && EnermySpawner.main.GetEnermyALive() >= 0)
        {
            IsLose = true;
            Score = 0;
            TimeLose = Time.time;
        }
    }

    public bool SpendCoin(int AmountCoin)
    {
        if(AmountCoin <= TotalCoin)
        {
            TotalCoin -= AmountCoin;
            return true;
        }
        else
        {
            return false;
        }
    }
}
