using System;

namespace totalRoleplay.Windows.charactersheet;
public abstract class CharacterSheetTab : IDisposable
{
	public abstract string Title { get; }
	public bool IsOpen { get; set; } = false;
	public virtual bool IsVisible { get; } = true;
	public virtual void OnOpen()
	{ }
	public virtual void OnClose()
	{ }
	public virtual void Draw()
	{ }
	public virtual void Load()
	{ }
	public virtual void Save()
	{ }
	public virtual void Discard()
	{ }
	public void Dispose()
	{ }
}
