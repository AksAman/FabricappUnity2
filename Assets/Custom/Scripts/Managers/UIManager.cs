//using DG.Tweening;
//using UnityEngine;
//using System.Collections;
//using UnityEngine.UI;
//using System.Collections.Generic;
//public class UIManager : MonoBehaviour
//{
//    [Header("Material Menu Setup")]
//    public bool debugMode;
//    public int debugMatCount;
//    private int matCount;
//    [Space(5)]
//    public RectTransform matScrollHolder;
//    public GameObject materialButtonPrefab;
//    public AllSmartMaterialsData allMaterials;
//    public GameObject notifier;
//    public Tweener tweener;

//    public GameObject curModel;
//    public Material[] mats;
//    public string[] matNames;

//    List<MaterialButton> spawnedButtons = new List<MaterialButton>();
//    public Color colorTintOnClicked;
    

//    private void Awake()
//    {
//        MaterialButton.OnMatButtonClicked += MatButtonClick;
//    }


//    private void Start()
//    {
//        matCount = debugMode ? debugMatCount : mats.Length;
//        //SetupScrollBarHeight();
//        if (matScrollHolder != null)
//        {
//            AddMatButtons();
//        }
//    }

//    void AddMatButtons()
//    {
//        string curText = "";
//        for (int i = 0; i < matCount; i++)
//        {
//            curText = debugMode ? (i + 1).ToString() : matNames[i];
//            GameObject matButton = Instantiate(materialButtonPrefab, matScrollHolder);
//            int curIndex = debugMode ? 0 : i;
//            MaterialButton curButtonComponent = matButton.GetComponent<MaterialButton>();
//            curButtonComponent.Initialise(curIndex, curText);
//            spawnedButtons.Add(curButtonComponent);
//        }
//    }

//    void MatButtonClick(int buttonIndex)
//    {

//        Debug.Log("Clicked on " + (buttonIndex + 1).ToString());
//        HandleButtonColors(buttonIndex);
//        //ChangeCurModelMaterial(SubMeshTriangle.clickedMatIndex, mats[buttonIndex]);
//        ChangeCurModelMaterial(0, mats[buttonIndex]);
//    }

//    private void HandleButtonColors(int buttonIndex)
//    {
//        for (int i = 0; i < spawnedButtons.Count; i++)
//        {
//            if(i == buttonIndex)
//            {
//                spawnedButtons[i].imageComponent.color = colorTintOnClicked;
//            }
//            else
//            {
//                spawnedButtons[i].imageComponent.color = Color.white;
//            }
//        }
//    }

//    public IEnumerator Wait(float seconds)
//    {
//        yield return new WaitForSeconds(seconds);
//        notifier.GetComponent<CanvasGroup>().DOFade(0, 0.25f);
//    }

//    void SetupScrollBarHeight()
//    {
//        float totalHeight = materialButtonPrefab.GetComponent<RectTransform>().rect.height;
//        totalHeight *= materialButtonPrefab.GetComponent<RectTransform>().localScale.y;
//        totalHeight *= matCount;
//        totalHeight += matScrollHolder.GetComponent<GridLayoutGroup>().padding.top * matCount;

//        matScrollHolder.GetComponent<RectTransform>().sizeDelta = new Vector2(-14.5f, totalHeight);
//    }

//    void ChangeCurModelMaterial(int atIndex, Material newMaterial)
//    {
//        Material[] allMats = curModel.GetComponent<MeshRenderer>().materials;
//        allMats[atIndex] = newMaterial;
//        curModel.GetComponent<MeshRenderer>().materials = allMats;
//    }
//}
