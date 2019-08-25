using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class Root : MonoBehaviour
{
    public static float SimulationTick => Instance.simulationTick;

    public static ConfigManager ConfigManager => Instance.configManager;
    public static CameraController CameraController => Instance.cameraController;
    public static UIManager UIManager => Instance.uiManager;
    
    public static PlayerInput PlayerInput => Instance.playerInput;

   
    [SerializeField] private ConfigManager configManager = null;
    [SerializeField] private CameraController cameraController = null;
    [SerializeField] private UIManager uiManager = null;
    [SerializeField] private PlayerInput playerInput = null;
    [Range(0f, 1f)]
    [SerializeField] private float simulationTick = .5f;


    public static Root Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<Root>();
            }

            return _instance;
        }
    }

    private static Root _instance;

    private void Awake()
    {
//        _instance = this;
    }

    private void Start()
    {
        cameraController.Init();
        uiManager.Init();
    }

    
}
