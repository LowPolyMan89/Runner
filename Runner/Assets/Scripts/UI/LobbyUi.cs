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
