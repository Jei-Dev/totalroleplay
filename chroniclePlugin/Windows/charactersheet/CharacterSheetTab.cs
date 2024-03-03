namespace chroniclePlugin.Windows.charactersheet;
public abstract class CharacterSheetTab
{
	public abstract string Title { get; }
	public bool IsOpen { get; set; } = false;
	public virtual bool IsVisible { get; }
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
}
