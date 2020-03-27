using UnityEngine;
using UnityEngine.EventSystems;

public class SelectedTest : MonoBehaviour
{
    public GameObject currentSelected;

    private void Start()
    {
        EventSystem.current.SetSelectedGameObject(null);
    }

    private void Update()
    {
        currentSelected = EventSystem.current.currentSelectedGameObject;
    }
}
