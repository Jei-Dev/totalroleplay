using Dalamud.Interface.Windowing;
using ImGuiNET;
using System;
using System.Numerics;

namespace chroniclePlugin.Windows;

public class CurrencyWindow : Window, IDisposable
{
	public record Currency
	{
		public required string currencyID { get; init; }
		public required string currencyName { get; init; }
		public required int currencyAmount { get; set; }
	};

	public CurrencyWindow() : base("TRP - Currency", 0)
	{
		Size = new Vector2(200, 200);
		SizeCondition = ImGuiCond.Once;
	}

	private readonly Currency[] currency = [
			new() { currencyID = "GIL0001", currencyName = "Gil", currencyAmount = 100 },
	];

	public void Dispose() { }

	public override void Draw()
	{
		ImGui.Text("Currency");
		if (ImGui.BeginListBox("##currency", new Vector2(float.Epsilon, (ImGui.GetTextLineHeightWithSpacing() * currency.Length) + ImGui.GetStyle().FramePadding.Y)))
		{
			for (var i = 0; i < currency.Length; i++)
			{
#pragma warning disable S108 // Nested blocks of code should not be left empty
				if (ImGui.Selectable(currency[i].currencyName + "  " + currency[i].currencyAmount, false)) { }
#pragma warning restore S108 // Nested blocks of code should not be left empty
			}
		}
	}
}

