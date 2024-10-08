using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// 
/// DEPRECATED
/// 
/// SEE HUDMANAGER AND HUDMODEL INSTEAD
/// 
/// </summary>
public class SetObjectiveText : MonoBehaviour
{
    [SerializeField] LevelProgress levelProgress;

    private void Awake()
    {
        levelProgress.SetObjectiveText(this.GetComponent<TextMeshProUGUI>());
    }
}
