using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelStats : MonoBehaviour
{
    public static LevelStats Instance;

    [SerializeField] private string levelId;
    [SerializeField] private int coinCollectCount = 0;
    [SerializeField] private int playerHitPoint = 3;
    [SerializeField] private int levelStars = 3;
    [SerializeField] private int finalLevelCoins = 0;
    [SerializeField] private int coinCount = 0;
    [SerializeField] private Vector3 starsCoinRange;
    [SerializeField] private int finalStars = 0;

    public int CoinCollectCount { get => coinCollectCount; }
    public int PlayerHitPoint { get => playerHitPoint; }
    public string LevelId { get => levelId; }
    public int LevelStars { get => levelStars; }
    public int FinalLevelCoins { get => finalLevelCoins; set => finalLevelCoins = value; }
    public Vector3 StarsCoinRange { get => starsCoinRange; }
    public int FinalStars { get => finalStars; set => finalStars = value; }

    private void Awake()
    {
        if (!GameObject.FindObjectOfType<DataProvider>())
        {
            return;
        }


        if (Instance == null)
        {
            Instance = this;
        }

        levelStars = 1;
    }

    private void Start()
    {
        DataProvider.Instance.EventManager.OnTakeDamageAction += AddHitPoint;
        List<Coin> coins = new List<Coin>();
        coins.AddRange(GameObject.FindObjectsOfType<Coin>());
        coinCount = coins.Count;
    }

    public void AddCoin(int value)
    {
        coinCollectCount += value;
        DataProvider.Instance.EventManager.UiUpdate();
    }

    public int AddHitPoint(int value)
    {
        playerHitPoint += value;
        if(playerHitPoint <= 0)
        {
            DataProvider.Instance.EventManager.LevelFailAction(levelId);
            DataProvider.Instance.EventManager.PlayerDeadAction();
        }
        DataProvider.Instance.EventManager.UiUpdate();
        return value;
    }

    private void OnDestroy()
    {
        if (!GameObject.FindObjectOfType<DataProvider>())
        {
            return;
        }

        DataProvider.Instance.EventManager.OnTakeDamageAction -= AddHitPoint;
    }
}
