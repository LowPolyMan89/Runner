using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyUi : MonoBehaviour
{
    public Transform mapPlaceholder;
    public Transform playerPlaceholder;
    public Transform collectionPlaceholder;
    private List<Transform> placeholders = new List<Transform>();

    public Sprite GetResourceSprite(string id)
    {
        DataProvider _dataProvider = DataProvider.Instance;
        Sprite _sprite = null;
        foreach(var s in _dataProvider.iconsItemsDatas)
        {
            if(s.Id == id)
            {
                _sprite = s.sprite;
            }
        }
        return _sprite;
    }

    public string GetResourceText(string id)
    {
        DataProvider _dataProvider = DataProvider.Instance;
        string _text = null;
        foreach (var s in _dataProvider.iconsItemsDatas)
        {
            if (s.Id == id)
            {
                _text = s.Text;
            }
        }
        return _text;
    }

    private void Start()
    {
        placeholders.Add(mapPlaceholder);
        placeholders.Add(playerPlaceholder);
        placeholders.Add(collectionPlaceholder);

        SelectPlacholder(playerPlaceholder);

        if (PlayerPrefs.HasKey("LoadedScene"))
        {
            if (PlayerPrefs.GetString("LoadedScene") != "NULL")
            {
                SceneManager.LoadScene(PlayerPrefs.GetString("LoadedScene"));
            }
        }
    }

    public void SelectPlacholder(Transform placholderToActive)
    {
        foreach(var v in placeholders)
        {
            v.gameObject.SetActive(false);
        }

        placholderToActive.gameObject.SetActive(true);
    }
}
