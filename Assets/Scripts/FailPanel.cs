using UnityEngine.UI;

public class FailPanel : WG_Panel
{

    private Button restartButton;
    protected Button RestartButton { get { return (restartButton == null) ? restartButton = GetComponentInChildren<Button>() : restartButton; } }

    private void OnEnable()
    {
        RestartButton.onClick.AddListener(RestartGame);
        EventManager.OnOpenFailPanel.AddListener(ShowPanel);
    }
    private void OnDisable()
    {
        EventManager.OnOpenFailPanel.RemoveListener(ShowPanel);
    }

    void RestartGame()
    {
        EventManager.OnRestartGame.Invoke();
        HidePanel();

    }
}
