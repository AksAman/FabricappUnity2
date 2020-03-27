using DG.Tweening;
using helloVoRld.Core.Singletons;
using UnityEngine;
using UnityEngine.UI;

public class AnimationManager : Singleton<AnimationManager>
{
    public Animator hamburgerAnimator;
    public Animator selectionAnimator;
    public Animator CameraAnimator;

    public bool isSelectionOpen = true;

    [Header("Tweening Camera Parent")]
    public Transform cameraParentTransform;
    public Vector2 cameraParentXPositions;
    public float duration;
    public Ease easeType;

    [Header("Tweening Camera Viewport")]
    public LayoutElement FurniturePanelLayoutElement;
    public LayoutElement SelectorPanelLayoutElement;
    public Camera cameraToTween;
    public bool useXY;
    public float inDuration;
    public float outDuration;

    [Header("Tweening SelectionPanel")]
    public RectTransform selectionMainPanel;
    public RectTransform SelectionPanel;
    private float startValue;
    private float endValue;
    private float initialSelectorFWidth;
    private float currentDuration;
    //private Tweener valueTweener;

    public float cameraX { get; private set; }

    private void Start()
    {
        initialSelectorFWidth = SelectorPanelLayoutElement.flexibleWidth;
    }

    public void AnimateSelectionPanel()
    {
        TweenSelectionPanel();
        TweenCameraViewport();
    }

    private void TweenSelectionPanel()
    {
        float mainWidth = selectionMainPanel.rect.width;
        float wRatio = SelectorPanelLayoutElement.flexibleWidth;
        float xToMove = wRatio * mainWidth;
        float currentDuration = isSelectionOpen ? outDuration : inDuration;
        Debug.Log(xToMove);
        if (isSelectionOpen)
        {
            hamburgerAnimator.Play("HTE Hamburger");
            selectionMainPanel.DOAnchorPosX(xToMove, currentDuration).SetEase(easeType);

            isSelectionOpen = false;
        }

        else
        {
            hamburgerAnimator.Play("HTE Exit");
            selectionMainPanel.DOAnchorPosX(0, currentDuration).SetEase(easeType);
            isSelectionOpen = true;
        }
    }

    public void TweenCameraParent()
    {
        float endValue = isSelectionOpen ? cameraParentXPositions.x : cameraParentXPositions.y;
        Debug.Log(endValue);
        //cameraParentTransform.DOLocalMove(transform.right * endValue, duration, false).SetEase(easeType);
    }

    public void TweenCameraViewport()
    {
        //startValue = isSelectionOpen ? (useXY ? -1*SelectorPanelLayoutElement.flexibleWidth : FurniturePanelLayoutElement.flexibleWidth) : 0;
        //endValue = isSelectionOpen ? 0 : (useXY ? -1*SelectorPanelLayoutElement.flexibleWidth : FurniturePanelLayoutElement.flexibleWidth);
        float startRatio = (SelectorPanelLayoutElement.flexibleWidth * selectionMainPanel.rect.width) / Screen.width;
        startValue = isSelectionOpen ? 0 : -1 * startRatio;
        endValue = isSelectionOpen ? -1 * startRatio : 0;
        currentDuration = isSelectionOpen ? outDuration : inDuration;

        Debug.Log(endValue);
        //cameraToTween.rect = new Rect()
        var valueTweener = DOTween.To(x => cameraX = x, startValue, endValue, currentDuration)
            .SetEase(easeType)
            .SetAutoKill(false);

        valueTweener.OnUpdate(() => cameraToTween.rect = new Rect(cameraX, 0.0f, 1.0f, 1.0f));// worldFillMaterial.SetFloat("_Radius", radius));

    }


    //private IEnumerator SetMaterialParams(float finalValue)
    //{
    //    float initialValue = 0.0f;

    //    while (initialValue < finalValue)
    //    {
    //        initialValue += 1 / easeDuration;
    //        worldFillMaterial.SetFloat("_Radius", initialValue);
    //        //Debug.Log("Setting.." + initialValue.ToString());
    //        yield return null;
    //    }
    //    worldFillMaterial.SetColor("_BackColor", newColor);
    //    currentCoroutine = null;
    //}

    public void TestButton(string teststring)
    {
        Debug.Log(teststring);
    }

    public void GetSFWidth()
    {
        //Debug.Log(SelectorPanelLayoutElement.flexible);
    }

}
