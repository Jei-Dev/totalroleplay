using Dalamud.Interface.Windowing;
using ImGuiNET;
using System.Linq;
using System.Numerics;
using totalRoleplay.Configuration;
using totalRoleplay.Windows.charactersheet.tabs;

namespace totalRoleplay.Windows.charactersheet;
public class CharacterSheetWindow : Window
{
	private readonly CharacterConfiguration charConfig;
	private readonly CharacterSheetTab[] tabs;
	public CharacterSheetWindow(CharacterConfiguration _charConfig) : base("Character Sheet###V0.0.1", ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoResize)
	{
		charConfig = _charConfig;
		this.Size = new Vector2(730, 575);
		this.SizeCondition = ImGuiCond.FirstUseEver;

		tabs = new CharacterSheetTab[] {
			new CharacterSheetTabChar(charConfig, this),
			new CharacterSheetTabBio(),
			new CharacterSheetTabCareers(),
			new CharacterSheetTabAbout(),
		};
	}
	public override void Draw()
	{
		if (ImGui.BeginTabBar("###characterTabs"))
		{
			foreach (var charSheetTab in this.tabs.Where(x => x.IsVisible))
			{
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
