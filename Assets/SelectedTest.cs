using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectedTest : MonoBehaviour
{
    public GameObject currentSelected;

    private void Start()
    {
        EventSystem.current.SetSelectedGameObject(null);
    }
    void Update()
    {
        currentSelected = EventSystem.current.currentSelectedGameObject;
    }
}
