using BepInEx;
using LookingGlass.AutoSortItems;
using LookingGlass.BuffDescriptions;
using LookingGlass.BuffTimers;
using LookingGlass.CommandItemCount;
using LookingGlass.CommandWindowBlur;
using LookingGlass.DPSMeterStuff;
using LookingGlass.EquipTimerFix;
using LookingGlass.EscapeToCloseMenu;
using LookingGlass.HiddenItems;
using LookingGlass.HidePickupNotifs;
using LookingGlass.ItemCounters;
using LookingGlass.ItemStatsNameSpace;
using LookingGlass.PickupNotifsDuration;
using LookingGlass.ResizeCommandWindow;
using LookingGlass.StatsDisplay;
using RiskOfOptions;
using RoR2;
using RoR2.UI;
using System;
using System.Collections;
using System.IO;
using UnityEngine;

namespace LookingGlass
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class BasePlugin : BaseUnityPlugin
    {
        internal static BasePlugin instance;
        internal AutoSortItemsClass autoSortItems;
        internal NoWindowBlur noWindowBlur;
        internal ButtonsToCloseMenu buttonsToCloseMenu;
        internal HidePickupNotifications hidePickupNotifications;
        internal ItemStats itemStats;
        internal CommandItemCountClass commandItemCountClass;
        internal ModifyCommandWindow resizeCommandWindowClass;
        internal StatsDisplayClass statsDisplayClass;
        internal DPSMeter dpsMeter;
        internal ItemCounter itemCounter;
        internal BuffTimersClass buffTimers;
        internal CooldownFixer equipFixer;
        internal UnHiddenItems unHiddenItems;
        internal BuffDescriptionsClass buffDescriptions;
        internal PickupNotifDurationClass pickupNotifDurationClass;
        public static byte[] logo;
        public static Sprite logo2;
        
        public void Awake()
        {
            Log.Init(Logger);
            instance = this;
            try
            {
                string folderName = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(Info.Location), "icons");
                int i = UnityEngine.Random.Range(0, Directory.GetFiles(folderName).Length);
                int i2 = 0;
                foreach (var file in Directory.GetFiles(folderName))
                {
                    if (i2 == i)
                    {
                        logo = File.ReadAllBytes(file);
                    }
                    i2++;
                }

                var tex = new Texture2D(256, 256, TextureFormat.ARGB32, false, false);
                tex.LoadImage(logo);
                tex.filterMode = FilterMode.Point;

                ModSettingsManager.SetModIcon(Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0, 0), 100, 1, SpriteMeshType.Tight, new Vector4(0, 0, 0, 0), true));
            }
            catch (System.Exception)
            {
            }

            autoSortItems = new AutoSortItemsClass();
            noWindowBlur = new NoWindowBlur();
            buttonsToCloseMenu = new ButtonsToCloseMenu();
            hidePickupNotifications = new HidePickupNotifications();
            commandItemCountClass = new CommandItemCountClass();
            resizeCommandWindowClass = new ModifyCommandWindow();
            statsDisplayClass = new StatsDisplayClass();
            buffTimers = new BuffTimersClass();
            dpsMeter = new DPSMeter();
            itemCounter = new ItemCounter();
            equipFixer = new CooldownFixer();
            unHiddenItems = new UnHiddenItems();
            buffDescriptions = new BuffDescriptionsClass();
            pickupNotifDurationClass = new PickupNotifDurationClass();
            StartCoroutine(CheckPlayerStats());
            ItemCatalog.availability.CallWhenAvailable(() =>
            {
                itemStats = new ItemStats();
            });
        }

        private void Update()
        {
            if (ButtonsToCloseMenu.buttonsToClickOnMove.Count != 0 && Input.anyKeyDown && !Input.GetMouseButtonDown(0))
            {
                ButtonsToCloseMenu.CloseMenuAfterFrame();
            }
            dpsMeter.Update();
        }

        internal IEnumerator CheckPlayerStats()
        {
            while (true)
            {
                yield return new WaitForSeconds(StatsDisplayClass.statsDisplayUpdateInterval.Value);

                if (StatsDisplayClass.statsDisplay.Value)
                {
                    yield return statsDisplayClass.CalculateStuff();
                }
            }
        }

        internal void hook_OnEnable(Action<ScoreboardController> orig, ScoreboardController self)
        {
            statsDisplayClass.scoreBoardOpen = true;
            StartCoroutine(statsDisplayClass.CalculateStuff());
            orig(self);
        }

        internal void hook_OnDisable(Action<ScoreboardController> orig, ScoreboardController self)
        {
            statsDisplayClass.scoreBoardOpen = false;
            StartCoroutine(statsDisplayClass.CalculateStuff());
            orig(self);
        }
    }
}
