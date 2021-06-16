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
using System.Globalization;

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
    public List<Building> buildings = new List<Building>();
    DateTime a;
    DateTime b;
    TimeSpan s;

    public List<CraftComponentConfig> craftComponentConfigs = new List<CraftComponentConfig>();
    public List<CraftComponentConfig> upgradeComponentConfigs = new List<CraftComponentConfig>();
    private void Awake()
    {
        if(Instance != this)
            Instance = this;

        EventManager = GameObject.FindObjectOfType<EventManager>();
        LocalDataManager = new LocalDataManager();
        Profile = new Profile();
        DontDestroyOnLoad(this.gameObject);
    }

    public void AddBuilding(Building building)
    {

        RemoveNullBuldings();

        if(!buildings.Contains(building))
        {
            buildings.Add(building);
        }
    }


    public void RemoveNullBuldings()
    {
        // Find Fist Null Element in O(n)
        var count = buildings.Count;
        for (var i = 0; i < count; i++)
        {
            if (buildings[i] == null)
            {
                // Current Position
                int newCount = i++;
                // Copy non-empty elements to current position in O(n)
                for (; i < count; i++)
                {
                    if (buildings[i] != null)
                    {
                        buildings[newCount++] = buildings[i];
                    }
                }
                // Remove Extra Positions O(n)
                buildings.RemoveRange(newCount, count - newCount);
                break;
            }
        }
    }

    public List<Building> GetBuildings()
    {
        return buildings;
    }

    [ContextMenu("TestTimeSpan")]
    public float GetTimeSpan(float time)
    {
        b = DateTime.UtcNow;
        s = b - a;
        return (float)s.TotalSeconds;
    }
    public string GetTimeSpan(string date)
    {
        b = DateTime.UtcNow;
        s = b - a;
        return s.ToString();
    }

    public DateTime GetTimeSpan(TimeSpan span)
    {
        b = DateTime.UtcNow;
        DateTime s = b + span;
        return s;
    }

    public string GetAllTime(string installDate)
    {


        DateTime a = DateTime.UtcNow; // текущая дата
        DateTime b;

        string inp = installDate;
        string format = "yyyy-MM-dd HH:mm:ssZ";

        if (!DateTime.TryParseExact(inp, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out b))
        {
            Console.WriteLine("Nope!");
        }

        TimeSpan c = b.ToUniversalTime() - a; // прошедшее время сессии
        
        return c.ToString();
    }

    private void CreateNewPlayerProfile()
    {
        LocalDataManager.SavePlayerPrefsString("ProfileID", CreateNewUserID());
        ProfileID = LocalDataManager.LoadPlayerPrefsString("ProfileID");
        Profile.ProfileId = ProfileID;
        Profile.PlayerName = "TestPlayer";
        Profile.LastSaveDate = System.DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ssZ", CultureInfo.InvariantCulture);
        Profile.AddResource("Money", 100);
        Profile.AddResource("Crystals", 100);
        Profile.AddResource("Wood", 0);
        Profile.AddResource("Stone", 0);
        Profile.AddResource("Ore", 0);
        Profile.AddResource("Planks", 0);
        Profile.AddResource("Bricks", 0);
        Profile.AddResource("Iron", 0);
        Profile.OpenedPlayersId.Add("1");

        Profile.BuildingsData data1 = new Profile.BuildingsData();
        data1.buildingID = "Main_Building";
        data1.buildingLevel = 1;
        Profile.buildingsDatas.Add(data1);

        Profile.BuildingsData data2 = new Profile.BuildingsData();
        data2.buildingID = "Wood_Building";
        data2.buildingLevel = 0;
        Profile.buildingsDatas.Add(data2);

        Profile.BuildingsData data3 = new Profile.BuildingsData();
        data3.buildingID = "Stone_Building";
        data3.buildingLevel = 0;
        Profile.buildingsDatas.Add(data3);

        Profile.BuildingsData data4 = new Profile.BuildingsData();
        data4.buildingID = "Iron_Building";
        data4.buildingLevel = 0;
        Profile.buildingsDatas.Add(data4);

        Profile.BuildingsData data5 = new Profile.BuildingsData();
        data5.buildingID = "Portal_Building";
        data5.buildingLevel = 0;
        Profile.buildingsDatas.Add(data5);

        Profile.BuildingsData data6 = new Profile.BuildingsData();
        data6.buildingID = "Bridge_Building";
        data6.buildingLevel = 0;
        Profile.buildingsDatas.Add(data6);
    }

    private void Start()
    {
        a = DateTime.UtcNow;

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
            CreateNewPlayerProfile();

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
        print("Loading local Save data");
        Profile profile = JsonUtility.FromJson<Profile>(File.ReadAllText(Application.persistentDataPath + "/" + LocalDataManager.LoadPlayerPrefsString("ProfileID")));
        Timeline.DropTimeline();
        foreach(var t in profile.timelineEvents)
        {
            Timeline.AddOldTimelineEvent(t.EventID, t.Seconds, t.EventEndDate, (EventAtionType)t.Type);
        }
        Profile = profile;
        UpdateBuildings();
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
        Profile.timelineEvents.Clear();

        foreach(var pbc in Profile.buildingsDatas)
        {
            pbc.buildingTimeline.Clear();
        }

        foreach(var b in buildings)
        {
            foreach(var e in b.timelineEvents)
            {
                Profile.TimelineEventJsonOblect timelineEventJsonOblect = new Profile.TimelineEventJsonOblect();
                timelineEventJsonOblect.EventID = e.EventID;
                timelineEventJsonOblect.EventEndDate = e.EventEndDate.ToString("yyyy-MM-dd HH:mm:ssZ", CultureInfo.InvariantCulture);
                timelineEventJsonOblect.isActive = e.isActive;
                timelineEventJsonOblect.Seconds = e.Seconds;
                timelineEventJsonOblect.Type = (int)e.ActionType;

                foreach(var pb in Profile.buildingsDatas)
                {
                    if(pb.buildingID == b.BuildingId)
                    {
                        pb.buildingTimeline.Add(timelineEventJsonOblect);
                    }
                }
            }
        }

        foreach(var t in Timeline.TimelineEvents)
        {
            Profile.TimelineEventJsonOblect timelineEventJsonOblect = new Profile.TimelineEventJsonOblect();
            timelineEventJsonOblect.EventID = t.EventID;
            timelineEventJsonOblect.EventEndDate = t.EventEndDate.ToString("yyyy-MM-dd HH:mm:ssZ", CultureInfo.InvariantCulture);
            timelineEventJsonOblect.isActive = t.isActive;
            timelineEventJsonOblect.Seconds = t.Seconds;
            timelineEventJsonOblect.Type = (int)t.ActionType;

            Profile.timelineEvents.Add(timelineEventJsonOblect);
        }
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

    private IEnumerator LoadBuildings()
    {
        print("Load Building data set");
        yield return new WaitForSeconds(1f);
        
        foreach (var b in Profile.buildingsDatas)
        {
            foreach (var b2 in buildings)
            {
                if (b2.BuildingId == b.buildingID)
                {
                    print("Find building: " + b2.BuildingId);
                    b2.BuildingLevel = b.buildingLevel;
                    b2.Init();
                }
            }
        }
        print("Load Building data finish");
    }

    internal void UpdateBuildings()
    {
        StartCoroutine(LoadBuildings());
    }

    private void OnDestroy()
    {
        SaveProfile();
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
    public List<TimelineEventJsonOblect> timelineEvents = new List<TimelineEventJsonOblect>();
    public List<BuildingsData> buildingsDatas = new List<BuildingsData>();
    public int LevelWinStrick;
    public int BonusLevelWinStrick;

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
        DataProvider.Instance.SaveLoad(SaveLoadEnum.Save);
        return val;
    }


    public void UpgradeBuilding(string buildingID, int level)
    {
        foreach(var b in buildingsDatas)
        {
            if(b.buildingID == buildingID)
            {
                b.buildingLevel = level;
                DataProvider.Instance.SaveLoad(SaveLoadEnum.Save);
                DataProvider.Instance.UpdateBuildings();
                break;
            }
        }
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
                DataProvider.Instance.SaveLoad(SaveLoadEnum.Save);
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

        DataProvider.Instance.SaveLoad(SaveLoadEnum.Save);
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
    public class BuildingsData
    {
        public string buildingID;
        public int buildingLevel;
        public List<TimelineEventJsonOblect> buildingTimeline = new List<TimelineEventJsonOblect>();
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

    [System.Serializable]
    public class TimelineEventJsonOblect
    {
        public string EventID;
        public string EventEndDate;
        public float Seconds;
        public bool isActive = true;
        public int Type;
    }
}
