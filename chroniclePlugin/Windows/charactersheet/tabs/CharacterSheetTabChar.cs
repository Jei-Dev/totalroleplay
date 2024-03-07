using chroniclePlugin.Configuration;
using ImGuiNET;
using System.Linq;
using System.Numerics;

namespace chroniclePlugin.Windows.charactersheet.tabs;
public class CharacterSheetTabChar : CharacterSheetTab
{
	/// <summary>
	/// Whether or not the selected Title is a prefix. Default: False.
	/// </summary>
	public bool isPrefix { get; set; } = false;
	/// <summary>
	/// Edit Mode when inside the Character Sheet. Default: False.
	/// </summary>
	public bool editMode { get; set; } = false;
	private string charNameInput = string.Empty;
	private readonly CharacterConfiguration charConfig;
	private readonly string[] jobList;
	private int charJob;
	private string charPhysBio;
	public CharacterSheetTabChar(CharacterConfiguration _charConfig)
	{
		charConfig = _charConfig;
		jobList = charConfig.characterJobList.Select(job => job.name ?? "(No Jobs)").ToArray();
		charJob = charConfig.currentJob;
		charPhysBio = charConfig.characterPhysicalBiography;
	}

	public override void Draw()
	{
		var charNameString = isPrefix ? $"Name: {charConfig.characterTitle} {charConfig.characterName}" : $"Name: {charConfig.characterName} {charConfig.characterTitle}";
		ImGui.SetCursorPos(new Vector2(0, 470));
		if (ImGui.Button(editMode ? "Save" : "Edit", new Vector2(50, 30)))
		{
			if (editMode)
			{
				Save();
			}
			else
			{
				editMode = true;
			}
		}

		if (!editMode)
		{
			ImGui.SetCursorPos(new Vector2(0, 0));
			ImGui.BeginChild("##DisplayTextChild");
			ImGui.SetCursorPos(new Vector2(0, 0));
			ImGui.Text(charNameString);
			ImGui.Text($"Current Employ: {charConfig.characterJobList[charJob].name}");
			ImGui.TextWrapped(charConfig.characterPhysicalBiography);
			ImGui.NewLine();
			ImGui.EndChild();
		}
		else
		{
			ImGui.SetCursorPos(new Vector2(0, 0));
			ImGui.InputTextWithHint("##charNameEdit", charNameString, ref charNameInput, 150);
			ImGui.ListBox("##charJobSelect", ref charJob, jobList, charConfig.characterJobList.Length);
			ImGui.InputTextMultiline("##PhysicalBiography", ref charPhysBio, 800, new Vector2(300, 170));
			ImGui.NewLine();
		}

		ImGui.SetCursorPos(new Vector2(460, 140));
		ImGui.BeginChildFrame(13, new Vector2(195, 320));

		base.Draw();
	}

	public override void Save()
	{
		if (charNameInput != "") { charConfig.characterName = charNameInput; }
		charConfig.characterPhysicalBiography = charPhysBio;
		CharacterConfiguration.Save();
		editMode = false;
	}
	public override string Title => "Overview";
}
