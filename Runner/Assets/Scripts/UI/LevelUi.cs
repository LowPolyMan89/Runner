using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelUi : MonoBehaviour
{
    [SerializeField] private Text timerText;
    [SerializeField] private LevelController levelController;
    [SerializeField] private Image hitPointImage;
    [SerializeField] private LevelStats levelStats;
    [SerializeField] private Image progressBar;
    [SerializeField] private Text coinsCountText;
    [SerializeField] private EventManager eventManager;
    [SerializeField] private GameObject failLevelPanel;
    [SerializeField] private GameObject winLevelPanel;
    [SerializeField] private Text finalCoinText;

    private void Awake()
    {
        levelController = GameObject.FindObjectOfType<LevelController>();
        eventManager = DataProvider.Instance.EventManager;
        eventManager.OnUiUpdateAction += UpdateUi;
        eventManager.OnLevelFailAction += LevelFail;
        eventManager.OnLevelWinAction += LevelWin;
        failLevelPanel.SetActive(false);
        winLevelPanel.SetActive(false);
        coinsCountText.text = "0";
    }

    private string LevelFail(string id)
    {
        failLevelPanel.SetActive(true);
        return id;
    }

    public void RestartLevelButton()
    {
        SceneManager.LoadScene("Level_" + levelStats.LevelId);
    }

    public void GoToLobbyButton()
    {
        SceneManager.LoadScene("Lobby");
    }

    private string LevelWin(string id)
    {
        winLevelPanel.SetActive(true);
        return id;
    }

    private void UpdateUi()
    {
        if (!levelStats)
            levelStats = GameObject.FindObjectOfType<LevelStats>();

        if (levelController.StartTimer > -1)
            timerText.text = levelController.StartTimer.ToString("0");
        else
            timerText.enabled = false;

        if (levelStats)
        {

            hitPointImage.fillAmount = (float)levelStats.PlayerHitPoint / 3;

        }
        coinsCountText.text = levelStats.CoinCollectCount.ToString();
    }

    private void OnDestroy()
    {
        eventManager.OnUiUpdateAction -= UpdateUi;
        eventManager.OnLevelFailAction -= LevelFail;
        eventManager.OnLevelWinAction -= LevelWin;
    }

}
