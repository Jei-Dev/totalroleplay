using Dalamud.Game.ClientState;
using Dalamud.Game.ClientState.Keys;
using Dalamud.Game.ClientState.Objects;
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
	private readonly float textSpeed = IAmGod.pluginConfiguration.dialogueDrawSpeed;
	public readonly Stopwatch sw = new();
	private bool prevSpace = false;
	private readonly ClientState clientState;
	private readonly TargetManager targetManager;

	public FakeDialogueWindow(FakeDialogueHandler dialogueHandler, KeyState keyState, ClientState clientState, TargetManager targetManager) : base("Fake Dialogue Window")
	{
		this.dialogueHandler = dialogueHandler;
		this.keyState = keyState;
		Flags = ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse | ImGuiWindowFlags.NoTitleBar | ImGuiWindowFlags.NoBackground;

		var imagePath = Path.Combine(IAmGod.pluginInterface.AssemblyLocation.Directory?.FullName!, "FFXIVEmptyDialogueBox.png");
		backgroundTexture = IAmGod.pluginInterface.UiBuilder.LoadImage(imagePath);
		Size = new Vector2(backgroundTexture.Width, backgroundTexture.Height);

		this.clientState = clientState;
		this.targetManager = targetManager;

		dialogueHandler.OnStartDialogue += OnStartDialogue;
		dialogueHandler.OnEndDialogue += OnEndDialogue;
	}

	public void Dispose()
	{
		backgroundTexture.Dispose();
		sw.Reset();
		dialogueHandler.OnStartDialogue -= OnStartDialogue;
		dialogueHandler.OnEndDialogue -= OnEndDialogue;
	}

	public override void Draw()
	{
		var currentLine = dialogueHandler.CurrentDialogueLine;
		ImGui.Image(backgroundTexture.ImGuiHandle, new Vector2(backgroundTexture.Width, backgroundTexture.Height));
		ImGui.SetCursorPos(new Vector2(55, 30));
		ImGui.Text(currentLine?.ActorName
				   .Replace("{YOU}", clientState.LocalPlayer?.Name.ToString())
				   .Replace("{TARGET}", targetManager.Target?.Name.ToString())
				   ?? "No Name");
		ImGui.SetCursorPos(new Vector2(45, 70));
		var displayedCharacters = Math.Clamp(sw.Elapsed.TotalMilliseconds * textSpeed, 0, currentLine?.Content?.Length ?? 0);
		ImGui.TextColored(new Vector4(0, 0, 0, 255),
						  currentLine?.Content?[..(int)displayedCharacters] ?? "");
		ImGui.GetIO().WantTextInput = true;
		HandleInput();
	}
	private void HandleInput()
	{
		var space = ImGui.IsKeyPressed(ImGuiKey.Space);
		if (space && !prevSpace)
		{
			sw.Restart();
			dialogueHandler.proceedToNextPage();
		}
		prevSpace = space;
	}

	private void OnStartDialogue()
	{
		this.IsOpen = true;
		sw.Restart();
	}
	private void OnEndDialogue()
	{
		this.IsOpen = false;
		sw.Reset();
	}
}
