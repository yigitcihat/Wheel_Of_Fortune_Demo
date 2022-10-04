using UnityEngine.UI;

public class FailPanel : WG_Panel
{

    private Button restartButton;
    protected Button RestartButton { get { return (restartButton == null) ? restartButton = GetComponentInChildren<Button>() : restartButton; } }

    private void OnEnable()
    {
        EventManager.OnOpenFailPanel.AddListener(ShowPanel);
    }
    private void OnDisable()
    {
        EventManager.OnOpenFailPanel.RemoveListener(ShowPanel);
    }
    private void OnValidate()
    {
        RestartButton.onClick.AddListener(RestartGame);

    }

    void RestartGame()
    {
        EventManager.OnRestartGame.Invoke();
        HidePanel();

    }
}
