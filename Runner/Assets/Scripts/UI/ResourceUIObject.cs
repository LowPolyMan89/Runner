using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceUIObject : MonoBehaviour
{
    [SerializeField] private Text countText;
    [SerializeField] private string rewsourceID;
    private DataProvider dataProvider;
    public Text CountText { get => countText; set => countText = value; }

    private void Start()
    {
        StartCoroutine(SlowUpdate());
        dataProvider = DataProvider.Instance;
    }

    private IEnumerator SlowUpdate()
    {
        yield return new WaitForSeconds(0.5f);
        countText.text = dataProvider.Profile.GetResource(rewsourceID).ToString();
        StartCoroutine(SlowUpdate());
    }
}
