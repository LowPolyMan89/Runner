using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class DataProvider : MonoBehaviour
{
    public static DataProvider Instance;
    public LocalDataManager LocalDataManager;
    public string SelectedPlayerId;
    public Profile Profile;
    public string ProfileID;
    public EventManager EventManager;

    private void Awake()
    {
        if(Instance != this)
            Instance = this;

        EventManager = GameObject.FindObjectOfType<EventManager>();
        LocalDataManager = new LocalDataManager();
        Profile = new Profile();
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {

        EventManager.OnSaveLoadAction += SaveLoad;
        EventManager.OnAddCoinAction += AddCoinToProfile;

        if(!LocalDataManager.CheckId("SelectedPlayerId"))
        {
            LocalDataManager.SavePlayerPrefsString("SelectedPlayerId", "1");
            SelectedPlayerId = "1";
            print("Create local staring value SelectedPlayerId: 1 ,  Default player load");
        }
        else
        {
            SelectedPlayerId = LocalDataManager.LoadPlayerPrefsString("SelectedPlayerId");
        }

        if(!LocalDataManager.CheckId("ProfileID"))
        {
            LocalDataManager.SavePlayerPrefsString("ProfileID", CreateNewUserID());
            ProfileID = LocalDataManager.LoadPlayerPrefsString("ProfileID");
            Profile.ProfileId = ProfileID;
            Profile.LevelData levelData = new Profile.LevelData();
            levelData.LevelNumber = 1;
            levelData.StarsCount = 0;
            Profile.PlayerMoney = 100;
            Profile.LevelDatas.Add(levelData);
            Profile.OpenedPlayersId.Add("1");
            Profile.OpenedItemsId.Add("TestItem");

            if (!File.Exists(Application.persistentDataPath + LocalDataManager.LoadPlayerPrefsString("ProfileID")))
            {
                Profile.SaveProfile(ProfileID, Profile);
            }
        }
        else
        {
            ProfileID = LocalDataManager.LoadPlayerPrefsString("ProfileID");
            LoadProfile();
        }

        SceneManager.LoadScene("Lobby");
    }

    public SaveLoadEnum SaveLoad(SaveLoadEnum saveLoadEnum)
    {
        SaveLoadEnum _saveLoadEnum = saveLoadEnum;

        if(_saveLoadEnum == SaveLoadEnum.Save)
        {
            SaveProfile();
        }
        else
        {
            LoadProfile();
        }

        return _saveLoadEnum;
    }

    [ContextMenu("ClearPrefs")]
    public void ClearPrefs()
    {
        PlayerPrefs.DeleteAll();
    }

    public int AddCoinToProfile(int value)
    {
        print("Add profile money: " + value);
        Profile.PlayerMoney += value;
        SaveLoad(SaveLoadEnum.Save);
        return value;
    }

    [ContextMenu("load")]
    public void LoadProfile()
    {
        Profile profile = JsonUtility.FromJson<Profile>(File.ReadAllText(Application.persistentDataPath + "/" + LocalDataManager.LoadPlayerPrefsString("ProfileID")));
        Profile = profile;
    }

    private string CreateNewUserID()
    {
       return Profile.GenerateProfileID();
    }

    [ContextMenu("save")]
    public void SaveProfile()
    {
        Profile.SaveProfile(LocalDataManager.LoadPlayerPrefsString("ProfileID"), Profile);
    }

    [ContextMenu("OpenProfileFolder")]
    public void OpenProfileFolder()
    {
        Profile.OpenProfileFolder();
    }

    [ContextMenu("LoadProfileString")]
    public void LoadProfileString()
    {
         print(Application.persistentDataPath);
         print( Profile.LoadProfile());
    }

    private void OnDestroy()
    {
        EventManager.OnSaveLoadAction -= SaveLoad;
        EventManager.OnAddCoinAction -= AddCoinToProfile;
    }
}

public class LocalDataManager
{
    public bool CheckId(string id)
    {
        return PlayerPrefs.HasKey(id);
    }

    public string LoadPlayerPrefsString(string id)
    {
        return PlayerPrefs.GetString(id);
    }

    public void SavePlayerPrefsString(string id, string value)
    {
        PlayerPrefs.SetString(id, value);
        PlayerPrefs.Save();
    }
}

[System.Serializable]
public class Profile
{
    public string ProfileId;
    public int PlayerMoney;
    public int PlayerStars;
    public List<string> OpenedPlayersId = new List<string>();
    public List<string> OpenedItemsId = new List<string>();
    public List<LevelData> LevelDatas = new List<LevelData>();

    public string GenerateProfileID()
    {
        Guid guid = Guid.NewGuid();
        string str = guid.ToString();
        return str;
    }

    public void SaveProfile(string profileID, object obj)
    {
        string json = JsonUtility.ToJson(obj);
        SaveProfileToDisk(profileID, json);
    }
    public string LoadProfile()
    {
        return File.ReadAllText(Application.persistentDataPath + "/Profile");
    }

    public void OpenProfileFolder()
    {
        File.Open(Application.persistentDataPath, FileMode.Open);
    }

    public void SaveProfileToDisk(string profileID, string data)
    {
        File.WriteAllText(Application.persistentDataPath + "/" + profileID, data);
    }

    [System.Serializable]
    public class LevelData
    {
        public int LevelNumber;
        public int StarsCount;
    }
}
