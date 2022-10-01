using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelScrollBarControl : MonoBehaviour
{
    HorizontalLayoutGroup layoutGroup;
    private void Start()
    {
        layoutGroup = GetComponent<HorizontalLayoutGroup>();
    }
    private void OnEnable()
    {
        EventManager.OnLevelIncrease.AddListener(() => MoveSlider(-150));
        EventManager.OnLevelDecrease.AddListener(() => MoveSlider(150));
    }
    private void OnDisable()
    {
        EventManager.OnLevelIncrease.RemoveListener(() => MoveSlider(-150));
        EventManager.OnLevelDecrease.RemoveListener(() => MoveSlider(150));
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
