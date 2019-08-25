using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

[CreateAssetMenu(fileName = "LayersConfig", menuName = "Config/LayersConfig")]
public class LayersConfig : ScriptableObject
{
    [System.Serializable]
    public struct LayerEntityTypeSetting
    {
        public EntityType _entityType;
        public int layer;
    }

    public LayerEntityTypeSetting[] layerSettings;

    public int LayerForEntity(EntityType entityType)
    {
        for (int i = 0; i < layerSettings.Length; i++)
        {
            if (layerSettings[i]._entityType == entityType)
            {
                return layerSettings[i].layer;
            }
        }
        
        Debug.LogError($"Cant find layer setting for entity type {entityType}");
        return 0;
    }

}
