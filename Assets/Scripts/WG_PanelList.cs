using System.Collections.Generic;
using System.Linq;

public static class WG_PanelList
{
    public static string MainMenuPanel = "MainMenuPanel";
    public static string GamePanel = "GamePanel";
    public static string FailPanel = "FailPanel";

    public static Dictionary<string, WG_Panel> WG_Panels = new Dictionary<string, WG_Panel>();

    private static string[] panelIDs = new string[]
    {
            "None",
            MainMenuPanel,
            GamePanel,
            FailPanel
};
    public static List<string> PanelIDs { get { return panelIDs.ToList(); } }
}
