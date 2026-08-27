using HarmonyLib;
using ResoniteHotReloadLib;
using ResoniteModLoader;

namespace WikiLinks;

public partial class WikiLinks : ResoniteMod
{
	public override string Name => ModName;
	public override string Author => ModAuthor;
	public const string HarmonyId = $"com.{ModAuthor}.{ModName}";
	public static ModConfiguration? Config;
	private static Harmony? _harmony;


	// Config options
	[AutoRegisterConfigKey]
	public static ModConfigurationKey<bool> DOCUMENT_STYLE =
		new("document_style", "Spawn as document", () => true);

	[AutoRegisterConfigKey]
	public static ModConfigurationKey<bool> REMAP_TYPES =
		new("remap_types", "Remap the types to try and point to wiki links", () => true);

	public static bool document_style =>
		Config?.GetValue(DOCUMENT_STYLE) ?? true;

	public static bool remap_types =>
		Config?.GetValue(REMAP_TYPES) ?? true;


	// Set the mod
	public static void Init()
	{
		_harmony ??= new Harmony(HarmonyId);
		_harmony.PatchAll();

		Msg($"{ModName} initialized");
	}

	public static void Unload()
	{
		_harmony?.UnpatchAll(HarmonyId);
		Msg($"{ModName} unloaded");
	}

	public override void OnEngineInit()
	{
		Msg($"{ModName} loaded!");

		Config = GetConfiguration();
#if DEBUG
		HotReloader.RegisterForHotReload(this);
#endif
		Init();
	}

#if DEBUG
	public static void BeforeHotReload()
	{
		Unload();
	}

	public static void OnHotReload(ResoniteMod modInstance)
	{
		Config = modInstance.GetConfiguration();
		Init();
	}
#endif
}