using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDBase : MonoBehaviour
{

    [SerializeField] private TMPro.TextMeshProUGUI textLabel = null;
    
    public void SetText(string text)
    {
        textLabel.text = text;
    }
    
}
