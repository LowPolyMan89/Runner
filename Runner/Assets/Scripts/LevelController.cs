using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    [SerializeField] private string playerPrefabID;
    [SerializeField] private GameObject player;
    [SerializeField] private Transform playerSpawn;
    [SerializeField] private LevelStats levelStats;
    private Player playerScript;
    private int startTimer = 3;

    public int StartTimer { get => startTimer; }
    public LevelStats LevelStats { get => levelStats; }

    private void Awake()
    {
        if(!GameObject.FindObjectOfType<DataProvider>())
        {
            PlayerPrefs.SetString("LoadedScene", SceneManager.GetActiveScene().name);
            PlayerPrefs.Save();
            SceneManager.LoadScene("Init");
        }
        else
        {
            PlayerPrefs.SetString("LoadedScene", "NULL");
            PlayerPrefs.Save();
        }
        levelStats = GameObject.FindObjectOfType<LevelStats>();
    }

    private void Start()
    {
        LoadPalyer();
        playerScript.PlayerPause(true);
        StartCoroutine(LevelStartTimer());
        DataProvider.Instance.EventManager.OnLevelFailAction += FailLevel;
        DataProvider.Instance.EventManager.OnLevelWinAction += WinLevel;
    }

    private GameObject LoadPalyer()
    {
        GameObject _player = Instantiate(Resources.Load("Players/Player_" + playerPrefabID) as GameObject);
        _player.transform.position = playerSpawn.position;
        player = _player;
        playerScript = player.GetComponent<Player>();
        return _player;
    }

    private IEnumerator LevelStartTimer()
    {
        yield return new WaitForSeconds(1f);
        startTimer--;
        if (startTimer > -1)
        {
            DataProvider.Instance.EventManager.UiUpdate();
            StartCoroutine(LevelStartTimer());
        }
        else
        {
            DataProvider.Instance.EventManager.UiUpdate();
            StartLevel();
        }
    }

    private void StartLevel()
    {
        playerScript.PlayerPause(false);
    }

    private string FailLevel(string id)
    {
        playerScript.PlayerPause(true);
        return id;
    }

    public int CalculateCoinToWin()
    {
        int value = 0;
        value = levelStats.CoinCollectCount * 10 * levelStats.LevelStars;
        levelStats.FinalLevelCoins = value;
        DataProvider.Instance.EventManager.AddCoinToProfile(value);
        return value;
    }

    private string WinLevel(string id)
    {
        playerScript.PlayerPause(true);
        CalculateCoinToWin();
        return id;
    }

    private void OnDestroy()
    {

        DataProvider.Instance.EventManager.OnLevelFailAction -= FailLevel;
        DataProvider.Instance.EventManager.OnLevelWinAction -= WinLevel;
    }
}
