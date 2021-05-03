using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
