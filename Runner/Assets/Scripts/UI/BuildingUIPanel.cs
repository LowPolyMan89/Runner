using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildingUIPanel : MonoBehaviour
{
    public Building building;
    public ResourceToUpgradeUI ResourceToUpgradeUI;
    public LobbyUi UI;

    private void Start()
    {
        UI = GameObject.FindObjectOfType<LobbyUi>();
    }

    [ContextMenu("Open")]
    public void OpenPanel()
    {
        foreach(var p in ResourceToUpgradeUI.resourcePanels)
        {
            Destroy(p, 0.2f);
        }

        if(building.BuildingLevel + 1 > building.CrafterComponent.UpgradeComponentConfigs.Count)
        {
            ResourceToUpgradeUI.upgradeButton.interactable = false;
            ResourceToUpgradeUI.upgradeButtonText.text = "MAX";
        }
        else
        {
            ResourceToUpgradeUI.upgradeButton.interactable = true;
            ResourceToUpgradeUI.upgradeButtonText.text = "UPGRADE";

            CreateResorceToUpgradeUIElement(UI.GetResourceSprite("Money"), building.CrafterComponent.UpgradeComponentConfigs[building.BuildingLevel].Money);

            foreach (var r in building.CrafterComponent.UpgradeComponentConfigs[building.BuildingLevel].needcraftComponents)
            {
                CreateResorceToUpgradeUIElement(UI.GetResourceSprite(r.ComponentID), r.Value);
            }
        }
    }

    private void CreateResorceToUpgradeUIElement(Sprite icon, int count)
    {
        GameObject newpanel = Instantiate(ResourceToUpgradeUI.resorceInfoPrefab);
        newpanel.transform.SetParent(ResourceToUpgradeUI.parentObject);
        newpanel.transform.localScale = Vector3.one;
        ResourceUIPanel resourceUIPanel = newpanel.GetComponent<ResourceUIPanel>();
        resourceUIPanel.Image.sprite = icon;
        resourceUIPanel.count.text = count.ToString();
        ResourceToUpgradeUI.resourcePanels.Add(newpanel);
    }
}

[System.Serializable]
public class ResourceToUpgradeUI
{
    public Transform parentObject;
    public GameObject resorceInfoPrefab;
    public Button upgradeButton;
    public TextMeshProUGUI upgradeButtonText;
    public List<GameObject> resourcePanels = new List<GameObject>();
}
