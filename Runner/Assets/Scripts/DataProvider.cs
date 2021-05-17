using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using PlayFab;
using PlayFab.ClientModels;
using PlayFab.PfEditor.EditorModels;

public class DataProvider : MonoBehaviour
{
    public static DataProvider Instance;
    public Timeline Timeline;
    public LocalDataManager LocalDataManager;
    public string SelectedPlayerId;
    public Profile Profile;
    public string ProfileID;
    public EventManager EventManager;
    public float LastSessionTime;
    DateTime a;
    DateTime b;
    TimeSpan s;

    private void Awake()
    {
        if(Instance != this)
            Instance = this;

        EventManager = GameObject.FindObjectOfType<EventManager>();
        LocalDataManager = new LocalDataManager();
        Profile = new Profile();
        DontDestroyOnLoad(this.gameObject);
    }

    [ContextMenu("TestTimeSpan")]
    public float GetTimeSpan(float time)
    {
        b = DateTime.Now;
        s = b - a;
        return (float)s.TotalSeconds;
    }
    public string GetTimeSpan(string date)
    {
        b = DateTime.Now;
        s = b - a;
        return s.ToString();
    }

    public DateTime GetTimeSpan(TimeSpan span)
    {
        b = DateTime.Now;
        DateTime s = b + span;
        return s;
    }

    public string GetAllTime(string installDate)
    {
        DateTime a = DateTime.Now; // текущая дата
        DateTime b = DateTime.Parse(installDate); // дата начала всего
        TimeSpan c = b - a; // прошедшее время сессии
        return c.ToString();
    }

    private void Start()
    {
        a = DateTime.Now;

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
            Profile.PlayerName = "TestPlayer";
            Profile.LevelData levelData = new Profile.LevelData();
            Profile.LastSaveDate = System.DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss");
            Profile.InstallDate = DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss");
            levelData.LevelNumber = 1;
            levelData.StarsCount = 0;
            Profile.AddResource("Money", 100);
            Profile.AddResource("Crystals", 100);
            Profile.AddResource("Wood", 0);
            Profile.AddResource("Stone", 0);
            Profile.AddResource("Ore", 0);
            Profile.AddResource("Planks", 0);
            Profile.AddResource("Bricks", 0);
            Profile.AddResource("Iron", 0);
            Profile.LevelDatas.Add(levelData);
            Profile.OpenedPlayersId.Add("1");
            Profile.Collection.Add("TestItem");

            if (!File.Exists(Application.persistentDataPath + LocalDataManager.LoadPlayerPrefsString("ProfileID")))
            {
                Profile.SaveProfile(ProfileID, Profile);
            }
        }
        else
        {
            ProfileID = LocalDataManager.LoadPlayerPrefsString("ProfileID");
            LoadProfileFromLocal();
        }
        
        SceneManager.LoadScene("Lobby");
    }

    [ContextMenu("LoadDataFromServer")]
    public void LoadDataFromServer()
    {
        if (PlayFabClientAPI.IsClientLoggedIn())
        {
            PlayFabClientAPI.GetUserData(new GetUserDataRequest { }, OnDataGet, OnDataError);
        }
        else
        {
            print("Play Fab not loggined, data doesnt send!");
        }
    }

    private void OnDataGet(GetUserDataResult result)
    {
        if (result.Data != null && result.Data.ContainsKey("Profile"))
        {
            LoadProfileFromServer(result.Data["Profile"].Value);
        }
    }

    [ContextMenu("SaveDataToServer")]
    public void SaveDataToServer()
    {
        StartCoroutine(IsPlayFabLoggin());
    }

    private void OnDataSend(UpdateUserDataResult result)
    {
        print("Send profile to server");
    }

    private void OnDataError(PlayFab.PlayFabError error)
    {
        print(error.ErrorMessage);
    }

    private IEnumerator IsPlayFabLoggin()
    {
        yield return new WaitForSeconds(2f);
        if (PlayFabClientAPI.IsClientLoggedIn())
        {
            var request = new UpdateUserDataRequest { Data = new Dictionary<string, string> { { "Profile", Profile.SaveProfile(ProfileID, Profile) } } };
            PlayFabClientAPI.UpdateUserData(request, OnDataSend, OnDataError);
        }
        else
        {
            print("Play Fab not loggined, data doesnt send!");
        }
    }

    public SaveLoadEnum SaveLoad(SaveLoadEnum saveLoadEnum)
    {
        SaveLoadEnum _saveLoadEnum = saveLoadEnum;

        if(_saveLoadEnum == SaveLoadEnum.Save)
        {
            Profile.LastSaveDate = System.DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss");
            Profile.LastSaveTime += GetTimeSpan(1);
            LastSessionTime = GetTimeSpan(1);
            Profile.AllTimeAfterInstall = GetAllTime(Profile.InstallDate);
            SaveProfile();
        }
        else
        {
            LoadProfileFromLocal();
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
        Profile.AddResource("Money", value);
        SaveLoad(SaveLoadEnum.Save);
        return value;
    }

    [ContextMenu("LoadProfileFromLocal")]
    public void LoadProfileFromLocal()
    {
        Profile profile = JsonUtility.FromJson<Profile>(File.ReadAllText(Application.persistentDataPath + "/" + LocalDataManager.LoadPlayerPrefsString("ProfileID")));
        Profile = profile;
    }

    public void LoadProfileFromServer(string data)
    {
        Profile profile = JsonUtility.FromJson<Profile>(data);
        Profile = profile;
    }

    private string CreateNewUserID()
    {
       return Profile.GenerateProfileID();
    }

    [ContextMenu("save")]
    public void SaveProfile()
    {
        Profile.LastSaveDate = System.DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss");
        Profile.LastSaveTime += GetTimeSpan(1);
        LastSessionTime = GetTimeSpan(1);
        Profile.AllTimeAfterInstall = GetAllTime(Profile.InstallDate);
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
    public string PlayerName;
    public string LastSaveDate;
    public List<Resource> Resources = new List<Resource>();
    public List<string> OpenedPlayersId = new List<string>();
    public List<string> Collection = new List<string>();
    public List<LevelData> LevelDatas = new List<LevelData>();
    public List<LevelData> BonusLevelDatas = new List<LevelData>();
    public List<ItemData> Items = new List<ItemData>();
    public List<SpecialOffer> Offers = new List<SpecialOffer>();
    public List<PlayerEvent> Events = new List<PlayerEvent>();
    public float LastSaveTime;
    public int LevelWinStrick;
    public int BonusLevelWinStrick;
    public string InstallDate;
    public string AllTimeAfterInstall;

    public int GetResource(string id)
    {
        int val = 0;
        foreach(var r in Resources)
        {
            if(r.ID.Contains(id))
            {
                val = r.Value;
            }
        }

        return val;
    }

    public void AddResource(string id, int value)
    {
        bool isHave = false;

        foreach (var r in Resources)
        {
            if (r.ID.Contains(id))
            {
                isHave = true;
                r.Value += value;
                return;
            }
        }

        if(!isHave)
        {
            Resource resource = new Resource();
            resource.ID = id;
            resource.Value = value;
            Resources.Add(resource);
            return;
        }
    }

    public bool SubstractResource(string id, int value)
    {
        bool isCan = false;

        foreach (var r in Resources)
        {
            if (r.ID.Contains(id))
            {
                if((r.Value - value) >= 0)
                {
                    r.Value -= value;
                    isCan = true;
                }           
            }
        }

        return isCan;
    }



    public string GenerateProfileID()
    {
        Guid guid = Guid.NewGuid();
        string str = guid.ToString();
        return str;
    }

    public string SaveProfile(string profileID, object obj)
    {
        string json = JsonUtility.ToJson(obj);

        SaveProfileToDisk(profileID, json);

        return json;
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

    [System.Serializable]
    public class Resource
    {
        public string ID;
        public int Value;
    }

    [System.Serializable]
    public class ItemData
    {
        public string ItemId;
        public int Count;
    }

    [System.Serializable]
    public class SpecialOffer
    {
        public string OfferId;
        public string StartDate;
        public string EndDate;
        public float StartTime;
        public float Duration;
        public bool isActive;
    }

    [System.Serializable]
    public class PlayerEvent
    {
        public string OfferId;
        public string StartDate;
        public string EndDate;
        public float StartTime;
        public float Duration;
        public bool isActive;
    }
}
