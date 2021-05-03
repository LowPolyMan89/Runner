using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyPlayerSpawn : MonoBehaviour
{
    [SerializeField] private Transform lobbyPlayerSpawnPoint;
    [SerializeField] private string playerPrefabID;
    public GameObject LobbyPlayer;


    private void Start()
    {
        playerPrefabID = DataProvider.Instance.SelectedPlayerId;
        SpawnLobbyPlayer(playerPrefabID);
    }



    public GameObject SpawnLobbyPlayer(string id)
    {
        GameObject _player = Instantiate(Resources.Load("Players/Player_" + playerPrefabID + "_Lobby") as GameObject);
        _player.transform.position = lobbyPlayerSpawnPoint.position;
        _player.transform.rotation = lobbyPlayerSpawnPoint.rotation;
        
        if(LobbyPlayer)
        {
            Destroy(LobbyPlayer);
        }

        LobbyPlayer = _player;
        return _player;
    }
}
