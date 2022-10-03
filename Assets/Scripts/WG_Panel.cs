using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class WG_Panel : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    protected CanvasGroup CanvasGroup { get { return (canvasGroup == null) ? canvasGroup = GetComponent<CanvasGroup>() : canvasGroup; } }

    public string PanelID;
    

    public UnityEvent OnPanelShown = new UnityEvent();
    public UnityEvent OnPanelHide = new UnityEvent();



    private List<string> panelList
    {
        get { return WG_PanelList.PanelIDs; }
    }

    protected virtual void Awake()
    {
        WG_PanelList.WG_Panels[PanelID] = this;
    }
    public virtual void ShowPanel()
    {
        CanvasGroup.alpha = 1;
        CanvasGroup.interactable = true;
        CanvasGroup.blocksRaycasts = true;
    }

    public virtual void HidePanel()
    {
        CanvasGroup.alpha = 0;
        CanvasGroup.interactable = false;
        CanvasGroup.blocksRaycasts = false;
    }
}
