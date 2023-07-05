using Dalamud.Game.ClientState.Keys;
using Dalamud.Interface.Windowing;
using Dalamud.Logging;
using FFXIVClientStructs.FFXIV.Client.Graphics.Render;
using ImGuiNET;
using System;
using System.Diagnostics;
using System.IO;
using System.Numerics;
using System.Timers;
using totalRoleplay.Handlers;
using totalRoleplay.Service;

namespace totalRoleplay.Windows;

public class FakeDialogueWindow : Window, IDisposable
{
	private readonly ImGuiScene.TextureWrap backgroundTexture;
	private readonly FakeDialogueHandler dialogueHandler;
	private readonly KeyState keyState;
	private readonly double textSpeed = 0.05;
	public readonly Stopwatch sw = new();
	private bool prevSpace = false;
	public FakeDialogueWindow(FakeDialogueHandler dialogueHandler, KeyState keyState) : base("Fake Dialogue Window")
	{
		this.dialogueHandler = dialogueHandler;
		this.keyState = keyState;
		Flags = ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse | ImGuiWindowFlags.NoTitleBar | ImGuiWindowFlags.NoBackground;

		var imagePath = Path.Combine(IAmGod.pluginInterface.AssemblyLocation.Directory?.FullName!, "FFXIVEmptyDialogueBox.png");
		backgroundTexture = IAmGod.pluginInterface.UiBuilder.LoadImage(imagePath);
		Size = new Vector2(backgroundTexture.Width, backgroundTexture.Height);

		dialogueHandler.onEndDialogue += onEndDialogue;
	}

	public void Dispose()
	{
		backgroundTexture.Dispose();
		sw.Reset();
		dialogueHandler.onEndDialogue -= onEndDialogue;
	}

	public override void Draw()
	{
		ImGui.Image(backgroundTexture.ImGuiHandle, new Vector2(backgroundTexture.Width, backgroundTexture.Height));
		ImGui.SetCursorPos(new Vector2(55, 30));
		ImGui.Text(dialogueHandler.dialogueText[Math.Clamp(dialogueHandler.currentPage, 0, dialogueHandler.currentPage)].characterName ?? "No Name");
		ImGui.SetCursorPos(new Vector2(45, 70));
		var displayedCharacters = Math.Clamp(sw.Elapsed.TotalMilliseconds * textSpeed, 0, dialogueHandler.dialogueText[dialogueHandler.currentPage].pageText.Length);
		ImGui.TextColored(new Vector4(0, 0, 0, 255),
			dialogueHandler.dialogueText[dialogueHandler.currentPage]
			.pageText[..(int)displayedCharacters]);
		ImGui.SetNextFrameWantCaptureKeyboard(true);
		HandleInput();
	}
	private void HandleInput()
	{
		var space = keyState[VirtualKey.SPACE];
		if (space && !prevSpace)
		{
			sw.Restart();
			dialogueHandler.proceedToNextPage();
		}
		prevSpace = space;
	}

	private void onEndDialogue()
	{
		this.IsOpen = false;
		sw.Reset();
	}
}
