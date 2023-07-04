using Dalamud.Interface.Windowing;
using FFXIVClientStructs.FFXIV.Client.Graphics.Render;
using ImGuiNET;
using System;
using System.IO;
using System.Numerics;
using totalRoleplay.Handlers;
using totalRoleplay.Service;

namespace totalRoleplay.Windows;

public class FakeDialogueWindow : Window, IDisposable
{
	private readonly ImGuiScene.TextureWrap backgroundTexture;
	private readonly FakeDialogueHandler dialogueHandler;
	public FakeDialogueWindow(FakeDialogueHandler dialogueHandler) : base("Fake Dialogue Window")
	{
		this.dialogueHandler = dialogueHandler;
		Flags = ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse | ImGuiWindowFlags.NoTitleBar | ImGuiWindowFlags.NoBackground;

		var imagePath = Path.Combine(IAmGod.pluginInterface.AssemblyLocation.Directory?.FullName!, "FFXIVEmptyDialogueBox.png");
		backgroundTexture = IAmGod.pluginInterface.UiBuilder.LoadImage(imagePath);
		Size = new Vector2(backgroundTexture.Width, backgroundTexture.Height);
	}

	public void Dispose()
	{
		backgroundTexture.Dispose();
	}

	public override void Draw()
	{
		ImGui.Image(backgroundTexture.ImGuiHandle, new Vector2(backgroundTexture.Width, backgroundTexture.Height));
		ImGui.SetCursorPos(new Vector2(55, 30));
		ImGui.Text(dialogueHandler.characterName ?? "John Doe the Doeist");
	}
}
