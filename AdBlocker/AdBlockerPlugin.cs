using BepInEx;
using RoR2.UI.MainMenu;
using System.Diagnostics;
using UnityEngine;

namespace AdBlocker
{
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
    [BepInDependency(R2API.R2API.PluginGUID)]
    public class AdBlockerPlugin : BaseUnityPlugin
    {
        public const string PluginGUID = PluginAuthor + "." + PluginName;
        public const string PluginAuthor = "Gorakh";
        public const string PluginName = "AdBlocker";
        public const string PluginVersion = "1.0.0";

        internal static AdBlockerPlugin Instance { get; private set; }

        void Awake()
        {
            Stopwatch stopwatch = Stopwatch.StartNew();

            Log.Init(Logger);

            Instance = SingletonHelper.Assign(Instance, this);

            On.RoR2.UI.MainMenu.MainMenuController.Awake += MainMenuController_Awake;

            stopwatch.Stop();
            Log.Message_NoCallerPrefix($"Initialized in {stopwatch.Elapsed.TotalMilliseconds:F0}ms");
        }

        void OnDestroy()
        {
            On.RoR2.UI.MainMenu.MainMenuController.Awake -= MainMenuController_Awake;

            Instance = SingletonHelper.Unassign(Instance, this);
        }

        static void MainMenuController_Awake(On.RoR2.UI.MainMenu.MainMenuController.orig_Awake orig, MainMenuController self)
        {
            orig(self);

            Transform storeButtonTransform = self.transform.Find("MENU: Title/TitleMenu/SafeZone/PlatformStoreButton");
            if (storeButtonTransform)
            {
                GameObject.Destroy(storeButtonTransform.gameObject);
            }
        }
    }
}
