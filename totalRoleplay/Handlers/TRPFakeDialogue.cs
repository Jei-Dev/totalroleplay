using Dalamud.Interface.Windowing;
using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace totalRoleplay.Handlers;

public class TRPFakeDialogue
{
	/// <summary>
	/// Start a Dialogue Fake Prompt
	/// </summary>
	/// <returns>Boolean - Dialogue Started</returns>
	public Boolean StartDialogue()
	{
		if (ImGui.BeginPopup("FakeDialogue", ImGuiWindowFlags.NoBackground | ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoResize))
		{
			return true;
		}
		else { return false; }

	}
}
