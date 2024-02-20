using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SetObjectiveText : MonoBehaviour
{
    [SerializeField] LevelProgress levelProgress;

    private void Awake()
    {
        levelProgress.SetObjectiveText(this.GetComponent<TextMeshProUGUI>());
    }
}
