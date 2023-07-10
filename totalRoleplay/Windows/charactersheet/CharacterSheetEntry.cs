using System;

namespace totalRoleplay.Windows.charactersheet;
public abstract class CharacterSheetEntry
{
	public string? Name { get; protected set; }
	public virtual bool IsValid { get; protected set; } = true;
	public virtual bool IsVisible { get; protected set; } = true;
	protected Guid Id { get; } = Guid.NewGuid();
	public abstract void Load();
	public abstract void Save();
	public abstract void Draw();
	public virtual void OnClose()
	{
		// Nothing.
	}

}
