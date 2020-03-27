using DG.Tweening;
using UnityEngine;
using UnityEngine.UI.Michsky.UI.ModernUIPack;

namespace helloVoRld.Test.UI
{
    public class ColorFill : MonoBehaviour
    {
        //UI References
        public GameObject ColorPicker;

        // Material settings
        public Material[] worldFillMaterials;
        public Color previousColor;
        public Color newColor;
        public float finalValue;
        private readonly float radius;

        // Tweening Settings
        public float easeDuration = 1;
        public Ease easeType;
        public int width;
        public int height;
        public Coroutine currentCoroutine;

        private void Start()
        {
            ColorSwapButton.OnWallButtonClicked += SetWorldColor;

            foreach (Material worldFillMaterial in worldFillMaterials)
            {

                worldFillMaterial.EnableKeyword("_BackColor");
                worldFillMaterial.EnableKeyword("_FrontColor");
                worldFillMaterial.EnableKeyword("_Radius");
                worldFillMaterial.EnableKeyword("_Offset");

                worldFillMaterial.SetColor("_FrontColor", newColor);
                worldFillMaterial.SetColor("_BackColor", previousColor);
                worldFillMaterial.SetFloat("_Radius", 0);
            }
        }
        //public void SetWorldColor(Button clickedButton)
        //{
        //    width = Screen.width;
        //    height = Screen.height;
        //    Vector2 buttonPos = RectTransformUtility.WorldToScreenPoint(null, clickedButton.transform.position);
        //    Vector2 buttonPosNormalized = new Vector2(buttonPos.x / width, buttonPos.y / height);

        //    worldFillMaterial.SetVector("_Offset", new Vector4(-buttonPosNormalized.x, -buttonPosNormalized.y, 0, 1));
        //    if (currentCoroutine == null)
        //    {
        //        this.newColor = clickedButton.GetComponent<Image>().color;

        //        worldFillMaterial.SetColor("_FrontColor", newColor);
        //        finalValue = Screen.orientation == ScreenOrientation.Landscape ? Mathf.Sqrt(Mathf.Pow((float)width / height, 2) + 1)
        //            : Mathf.Sqrt(Mathf.Pow((float)height / width, 2) + 1);
        //        StopCoroutine("SetMaterialParams");
        //        currentCoroutine = StartCoroutine(SetMaterialParams(finalValue));
        //    }

        //    //valueTweener = DOTween.To(x => radius = x, 0, finalValue, easeDuration)
        //    //    .SetEase(easeType)
        //    //    .SetAutoKill(false);

        //    //valueTweener.OnUpdate(() => Debug.Log(radius));// worldFillMaterial.SetFloat("_Radius", radius));

        //}

        public void SetWorldColor(UIGradient gradient)
        {
            width = Screen.width;
            height = Screen.height;
            Vector2 buttonPos = RectTransformUtility.WorldToScreenPoint(null, gradient.transform.position);
            Vector2 buttonPosNormalized = new Vector2(buttonPos.x / width, buttonPos.y / height);
            foreach (Material worldFillMaterial in worldFillMaterials)
            {

                worldFillMaterial.SetVector("_Offset", new Vector4(-buttonPosNormalized.x, -buttonPosNormalized.y, 0, 1));
                worldFillMaterial.SetFloat("_Radius", 0);
                this.newColor = gradient._effectGradient.colorKeys[0].color;

                worldFillMaterial.SetColor("_FrontColor", newColor);
                finalValue = Screen.orientation == ScreenOrientation.Landscape ? Mathf.Sqrt(Mathf.Pow((float)width / height, 2) + 1)
                    : Mathf.Sqrt(Mathf.Pow((float)height / width, 2) + 1);
                worldFillMaterial.DOFloat(finalValue, "_Radius", easeDuration).SetEase(easeType).OnComplete(() => worldFillMaterial.SetColor("_BackColor", newColor));
            }
        }

        //private IEnumerator SetMaterialParams(float finalValue)
        //{
        //    float initialValue = 0.0f;

        //    while(initialValue < finalValue)
        //    {
        //        initialValue += 1/easeDuration;
        //        worldFillMaterial.SetFloat("_Radius", initialValue);
        //        //Debug.Log("Setting.." + initialValue.ToString());
        //        yield return null;
        //    }
        //    worldFillMaterial.SetColor("_BackColor", newColor);
        //    currentCoroutine = null;
        //}

        public void SetColorFromPicker(Color color)
        {
            foreach (Material worldFillMaterial in worldFillMaterials)
            {
                worldFillMaterial.SetColor("_FrontColor", color);
                worldFillMaterial.SetColor("_BackColor", color);
            }
        }

        public void HandleColorPickerState()
        {
            ColorPicker.SetActive(!ColorPicker.activeSelf);
        }

    }

}
