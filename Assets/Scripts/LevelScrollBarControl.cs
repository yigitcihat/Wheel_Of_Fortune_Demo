using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelScrollBarControl : MonoBehaviour
{
    [SerializeField]
    private int slideCount;
    HorizontalLayoutGroup layoutGroup;
    private void Start()
    {
        layoutGroup = GetComponent<HorizontalLayoutGroup>();
    }
    private void OnEnable()
    {
        EventManager.OnLevelIncrease.AddListener(() => MoveSlider(-slideCount));
        EventManager.OnLevelDecrease.AddListener(() => MoveSlider(slideCount));
    }
    private void OnDisable()
    {
        EventManager.OnLevelIncrease.RemoveListener(() => MoveSlider(-slideCount));
        EventManager.OnLevelDecrease.RemoveListener(() => MoveSlider(slideCount));
    }

    void MoveSlider(int value)
    {
        Debug.Log("Increased");
        int leftPadding = layoutGroup.padding.left;

        leftPadding += value;

        DOTween.To(() => layoutGroup.padding.left, x => layoutGroup.padding.left = x, leftPadding, 0.5f)
            .OnUpdate(() => {
                layoutGroup.SetLayoutHorizontal();
            });

    }
}
