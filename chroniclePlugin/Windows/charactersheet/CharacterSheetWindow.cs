using chroniclePlugin.Configuration;
using chroniclePlugin.Windows.charactersheet.tabs;
using Dalamud.Interface.Windowing;
using Dalamud.Plugin.Services;
using ImGuiNET;
using System.Linq;
using System.Numerics;

namespace chroniclePlugin.Windows.charactersheet;
public class CharacterSheetWindow : Window
{
	private readonly CharacterSheetTab[] tabs;
	private readonly IPluginLog log;
	public CharacterSheetWindow(CharacterConfiguration _charConfig, CharacterConfiguration charConfig, IPluginLog _log) : base("Character Sheet###V0.0.1", ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoResize)
	{
		charConfig = _charConfig;
		log = _log;
		Size = new Vector2(730, 585);
		SizeCondition = ImGuiCond.FirstUseEver;

		tabs = [
			new CharacterSheetTabChar(charConfig),
			new CharacterSheetTabBio(),
			new CharacterSheetTabCareers(),
			new CharacterSheetTabAbout(),
		];
	}
	public override void Draw()
	{
		if (ImGui.BeginTabBar("###characterTabs"))
		{
			//log.Verbose("Rendering Tabs");
			foreach (var charSheetTab in tabs.Where(x => x.IsVisible))
			{
				//log.Verbose("Loaded Tab: " + charSheetTab.Title);
				if (ImGui.BeginTabItem(charSheetTab.Title))
				{
					if (!charSheetTab.IsOpen)
					{
						charSheetTab.IsOpen = true;
						charSheetTab.OnOpen();
					}
					if (ImGui.BeginChild($"###sheet_scrolling_{charSheetTab.Title}", new Vector2(-1, -1), false))
					{
						charSheetTab.Draw();
					}

					ImGui.EndChild();
					ImGui.EndTabItem();
				}
				else if (charSheetTab.IsOpen)
				{
					charSheetTab.IsOpen = false;
					charSheetTab.OnClose();
				}
			}
		}
	}
}
