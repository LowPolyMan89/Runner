using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyController : MonoBehaviour
{
    private DataProvider dataProvider;
    [SerializeField] private float autosaveTime;
    private void Start()
    {
        StartCoroutine(SlowUpdate());
        dataProvider = DataProvider.Instance;
    }

    private IEnumerator SlowUpdate()
    {
        yield return new WaitForSeconds(autosaveTime);
        dataProvider.SaveLoad(SaveLoadEnum.Save);
        StartCoroutine(SlowUpdate());
    }
}
