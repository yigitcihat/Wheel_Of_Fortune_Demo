using System.Collections.Generic;
using System.Linq;

public static class WG_PanelList
{
    public static string MainMenuPanel = "MainMenuPanel";
    public static string GamePanel = "GamePanel";

    public static Dictionary<string, WG_Panel> WG_Panels = new Dictionary<string, WG_Panel>();

    private static string[] panelIDs = new string[]
    {
            "None",
            MainMenuPanel,
            GamePanel,
};
    public static List<string> PanelIDs { get { return panelIDs.ToList(); } }
}
