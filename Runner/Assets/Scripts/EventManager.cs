using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{

    public event Action OnUiUpdateAction;
    public event Action OnCoinCollectAction;
    public event Action OnPlayerDeadAction;
    public event Func<int,int> OnTakeDamageAction;
    public event Func<string, string> OnLevelWinAction;
    public event Func<string, string> OnLevelFailAction;
    public event Func<SaveLoadEnum, SaveLoadEnum> OnSaveLoadAction;

    public delegate int AddCoinAction(int value);
    public event AddCoinAction OnAddCoinAction;

    private void Start()
    {
        OnUiUpdateAction += CoinCollectAction;
        OnUiUpdateAction += PlayerDeadAction;
    }

    public int AddCoinToProfile(int value)
    {
        if(OnAddCoinAction != null)
        {
            OnAddCoinAction(value);
        }
        return value;
    }

    public void UiUpdate()
    {
        if (OnUiUpdateAction != null)
        {
            OnUiUpdateAction();
        }
    }

    public void PlayerDeadAction()
    {
        if (OnPlayerDeadAction != null)
        {
            OnPlayerDeadAction();
        }
    }

    public int TakeDamageAction(int value)
    {
        if (OnTakeDamageAction != null)
        {
            OnTakeDamageAction(value);
        }
        return value;
    }

    public string LevelWinAction(string value)
    {
        if (OnLevelWinAction != null)
        {
            print("Vevel win action: level id - " + value);
            OnLevelWinAction(value);
        }
        return value;
    }

    public string LevelFailAction(string value)
    {
        if (OnLevelFailAction != null)
        {
            print("Vevel fail action: level id - " + value);
            OnLevelFailAction(value);
        }
        return value;
    }

    public SaveLoadEnum SaveLoadAction(SaveLoadEnum value)
    {
        if (OnSaveLoadAction != null)
        {
            OnSaveLoadAction(value);
        }
        return value;
    }

    public void CoinCollectAction()
    {
        if (OnCoinCollectAction != null)
        {
            OnCoinCollectAction();
        }
    }

    private void OnDestroy()
    {
        OnUiUpdateAction -= CoinCollectAction;
        OnUiUpdateAction -= PlayerDeadAction;
    }
}

public enum SaveLoadEnum
{
    Save,Load
}
