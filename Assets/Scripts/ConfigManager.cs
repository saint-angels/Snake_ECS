using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfigManager : MonoBehaviour
{
    public CameraConfig CameraConfig => cameraConfig;
    public LayersConfig LayersConfig => _layersConfig;

    [SerializeField] private CameraConfig cameraConfig = null;
    [SerializeField] private LayersConfig _layersConfig = null;
}
