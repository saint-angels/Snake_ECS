using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CameraConfig", menuName = "Config/CameraConfig")]
public class CameraConfig : ScriptableObject
{
    [Header("Screen shake")]
    public float shakeGeneralMagnitude = 1f;
    public float shakeBlockDuration = 1f;

}
