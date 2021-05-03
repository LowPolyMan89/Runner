using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    public LobbyUi LobbyUi;
    public List<Map> maps = new List<Map>();

    private void Start()
    {
        LobbyUi = GameObject.FindObjectOfType<LobbyUi>();
        LoadMap("1");
    }

    public void LoadMap(string id)
    {
        GameObject ma = Instantiate(Resources.Load("Maps/Map_" + id) as GameObject);
        ma.transform.SetParent(LobbyUi.mapPlaceholder);
        RectTransform rectTransform = ma.GetComponent<RectTransform>();
        rectTransform.offsetMin = Vector3.zero;
        rectTransform.offsetMax = Vector3.zero;
        maps.Add(ma.GetComponent<Map>());

    }
}
