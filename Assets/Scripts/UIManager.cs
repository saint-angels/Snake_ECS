using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [System.Serializable]
    public struct EntityCounterSetting
    {
        public EntityType type;
        public TMPro.TextMeshProUGUI textCounter;
    }
    
    [SerializeField] private RectTransform hudContainer = null;
    [SerializeField] private HUDBase hudPrefab;
    [SerializeField] private EntityCounterSetting[] counterSettings;
    
    private List<HUDBase> activeHUDS = new List<HUDBase>();
    

    public void Init()
    {
        ObjectPool.Preload(hudPrefab, 10);
    }

    public void ShowHUDInfos(List<SystemHUDInfo.HUDInfo> hudInfos)
    {
        int delta = activeHUDS.Count - hudInfos.Count;

        bool shouldRemoveExcess = delta > 0;
        bool shouldAquireMore = delta < 0;

        if (shouldRemoveExcess)
        {
            for (int i = activeHUDS.Count - 1; i >= activeHUDS.Count - delta; i--)
            {
                ObjectPool.Despawn(activeHUDS[i]);
            }
            
            activeHUDS.RemoveRange(activeHUDS.Count - delta, delta);
        }
        else if (shouldAquireMore)
        {
            for (int i = 0; i < Mathf.Abs(delta); i++)
            {
                HUDBase newHud = ObjectPool.Spawn(hudPrefab, Vector3.zero, Quaternion.identity, hudContainer);
                activeHUDS.Add(newHud);
            }
        }

        for (int hudInfoIdx = 0; hudInfoIdx < hudInfos.Count; hudInfoIdx++)
        {
            var hudInfo = hudInfos[hudInfoIdx];
            
            Vector2 screenPoint = Root.CameraController.WorldToScreenPoint(hudInfo.position + Vector3.up);
            Vector2 localPoint;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(hudContainer, screenPoint, null, out localPoint))
            {
                activeHUDS[hudInfoIdx].transform.localPosition = localPoint;
                activeHUDS[hudInfoIdx].SetText(hudInfo.infoString);
            }
        }

    }

    public void UpdateEntityCounter(EntityType type, int count)
    {
        TextMeshProUGUI label = GetLabelForEntityType(type);
        if (label != null)
        {
            label.text = $"{count}";
        }
    }
    
    private TMPro.TextMeshProUGUI GetLabelForEntityType(EntityType entityType)
    {
        for (int i = 0; i < counterSettings.Length; i++)
        {
            if (counterSettings[i].type == entityType)
            {
                return counterSettings[i].textCounter;
            }
        }
        
        Debug.LogError($"Can't find counter for type {entityType}");
        return null;
    }
}
