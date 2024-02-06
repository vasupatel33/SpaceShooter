
using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections.Generic;


[Serializable]
public enum AnimationAction2
{
    slideToRight,
    slideToLeft,
    slideUp,
    slideDown,
    noAction
}

[RequireComponent(typeof(CanvasGroup))]
public class DotweenAutoAnimScript : MonoBehaviour
{
    //Enum option for opening and closing panel
    public AnimationAction2 OpenAnimation, CloseAnimation;

    //Animation Effect adding to the animation of the panel. 
    public Ease OpeningEase, ClosingEase;

    //CanvasGroup attached to the gameObject
    public CanvasGroup thisPanelCanvasGroup;

    //total time for the animation duration.
    public float animationTime = 0.25f;

    [Tooltip("Make this true if the panel has to be destroyed after it has been closed.")]
    public bool NeedToBeDestroyed;

    [Tooltip("Provide close button reference if the panel has to be closed by a button click.")]
    public Button _closeButton;

    ///<summary>
    /// Positions of all four side of the screen. It is calculated using the screen camera .
    ///</summary>
    private Vector3
        leftScreenPosition,
        rightScreenPosition,
        topScreenPosition,
        bottomScreenPosition;


    /// <summary>
    /// offset positions of all side of the screen calculated with panel size in mind.
    /// </summary>
    private Vector3
        leftOffSetScreenPosition,
        rightOffSetScreenPosition,
        topOffSetScreenPosition,
        bottomOffSetScreenPosition;

    private bool positionSet;
    private Camera _mainCamera; //Main camera of the scene.
    List<GameObject> AllList;

    private void Awake()
    {
        if (thisPanelCanvasGroup == null) thisPanelCanvasGroup = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        if (_closeButton != null)
        {
            _closeButton.onClick.AddListener(() => PlayAnimation(false));
        }
    }

    private void OnEnable()
    {
        if (!positionSet)
        {
            //Set the positions of all sides of the screen
            UpdateAllSideScreenPositions();

            //update the offset position of the scree as per the size of the panel
            SetOffScreenPositionForPanel();
        }

        //set the alpha value of the canvas group to 0 for its initial value
        if (thisPanelCanvasGroup != null)
            thisPanelCanvasGroup.alpha = 0;
        PlayAnimation(true);
    }

    //Set the all position of all the screen side positions.
    private void UpdateAllSideScreenPositions()
    {
        if (_mainCamera == null)
            _mainCamera = Camera.main;
        Vector3 screenDimen = _mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0f)) * 2;
        if (screenDimen.x < screenDimen.y)
        {
            screenDimen = new Vector3(screenDimen.y, screenDimen.x, screenDimen.z);
        }

        topScreenPosition = new Vector3(0, screenDimen.y / 2, 0);
        bottomScreenPosition = new Vector3(0, -screenDimen.y / 2, 0);
        leftScreenPosition = new Vector3(-screenDimen.x / 2, 0, 0);
        rightScreenPosition = new Vector3(screenDimen.x / 2, 0, 0);
    }

    /// <summary>
    /// Set the positions outside of the screen so the panel could outside of the view/screen if necessary.
    /// </summary>
    private void SetOffScreenPositionForPanel()
    {
        RectTransform myRect = transform.GetComponent<RectTransform>();
        float sizeToAdd = myRect.rect.width / 100;
        leftOffSetScreenPosition = leftScreenPosition - new Vector3(sizeToAdd, 0, 0);
        rightOffSetScreenPosition = rightScreenPosition + new Vector3(sizeToAdd, 0, 0);
        sizeToAdd = myRect.rect.height / 100;
        topOffSetScreenPosition = topScreenPosition + new Vector3(0, sizeToAdd, 0);
        bottomOffSetScreenPosition = bottomScreenPosition - new Vector3(0, sizeToAdd, 0);
        positionSet = true;
    }


    /// <summary>
    /// Start playing the selected animation for the panel
    /// </summary>
    /// <param name="playOpenAnimation"></param>
    private void PlayAnimation(bool playOpenAnimation)
    {
        try
        {
            Vector3 posToGive = Vector3.zero;
            Vector3 initPos = Vector3.zero;
            float scale = 1;
            AnimationAction2 playAnimationType = playOpenAnimation ? OpenAnimation : CloseAnimation;
            switch (playAnimationType)
            {
                case AnimationAction2.slideToLeft:
                    initPos = playOpenAnimation ? rightOffSetScreenPosition : transform.position;
                    transform.position = initPos;
                    posToGive = playOpenAnimation ? new Vector3(0, transform.position.y, 0) : leftOffSetScreenPosition;
                    StartSlideAnimation(playOpenAnimation, posToGive);
                    break;
                case AnimationAction2.slideToRight:
                    initPos = playOpenAnimation ? leftOffSetScreenPosition : transform.position;
                    transform.position = initPos;
                    posToGive = playOpenAnimation
                        ? new Vector3(0, transform.position.y, 0)
                        : rightOffSetScreenPosition;
                    StartSlideAnimation(playOpenAnimation, posToGive);
                    break;
                case AnimationAction2.slideUp:
                    initPos = playOpenAnimation ? bottomOffSetScreenPosition : transform.position;
                    transform.position = initPos;
                    posToGive = playOpenAnimation ? new Vector3(transform.position.x, 0, 0) : topOffSetScreenPosition;
                    StartSlideAnimation(playOpenAnimation, posToGive);
                    break;
                case AnimationAction2.slideDown:
                    initPos = playOpenAnimation ? topOffSetScreenPosition : transform.position;
                    transform.position = initPos;
                    posToGive = playOpenAnimation
                        ? new Vector3(transform.position.x, 0, 0)
                        : bottomOffSetScreenPosition;
                    StartSlideAnimation(playOpenAnimation, posToGive);
                    break;
                default:
                    HideOrDestroy();
                    break;
            }
        }
        catch (Exception e)
        {
            HideOrDestroy();
            Debug.LogWarning($"Exception while running dotWeen animation " + e);
        }
    }

    /// <summary>
    /// Start playing the slide animation as per the opening or closing animation selected from the unityEditor
    /// </summary>
    /// <param name="openingAnimation">Open status of the panel. True for opening panel: False for closing panel</param>
    /// <param name="position">Position to which the panel should slide.</param>
    private void StartSlideAnimation(bool openingAnimation, Vector3 position)
    {
        try
        {
            if (openingAnimation)
            {
                transform.DOMove(position, animationTime).SetEase(OpeningEase)
                    .OnComplete(() => transform.localPosition = position);
                if (thisPanelCanvasGroup != null)
                    thisPanelCanvasGroup.DOFade(1, animationTime);
            }
            else
            {
                if (thisPanelCanvasGroup != null)
                    thisPanelCanvasGroup.DOFade(0, animationTime);
                transform.DOMove(position, animationTime).SetEase(ClosingEase).OnComplete(HideOrDestroy);
            }
        }
        catch (Exception e)
        {
            Debug.Log("Exception throw on panel sliding animation. " + e);
        }
    }

    /// <summary>
    /// Hide or destroy the gameObject when closing the panel
    /// </summary>
    private void HideOrDestroy()
    {
        try
        {
            if (NeedToBeDestroyed)
            {
                Destroy(gameObject);
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
        catch (Exception e)
        {
            Debug.Log("NullReference exception: " + e);
        }
    }
}
