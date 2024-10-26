﻿using BepInEx.Configuration;
using LookingGlass.Base;
using RiskOfOptions.OptionConfigs;
using RiskOfOptions.Options;
using RiskOfOptions;
using System;
using System.Collections.Generic;
using RoR2;
using System.Text.RegularExpressions;
using RoR2.UI;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using MonoMod.RuntimeDetour;
using System.Collections;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace LookingGlass.StatsDisplay
{
    internal class StatsDisplayClass : BaseThing
    {
        public static ConfigEntry<bool> statsDisplay;
        public static ConfigEntry<bool> useSecondaryStatsDisplay;
        public static ConfigEntry<string> secondaryStatsDisplayString;
        public static ConfigEntry<string> statsDisplayString;
        public static ConfigEntry<float> statsDisplaySize;
        public static ConfigEntry<float> statsDisplayUpdateInterval;
        public static ConfigEntry<bool> builtInColors;
        public static ConfigEntry<bool> statsDisplayOverrideHeight;
        public static ConfigEntry<int> statsDisplayOverrideHeightValue;
        public static ConfigEntry<int> floatPrecision;
        public static Dictionary<string, Func<CharacterBody, string>> statDictionary = [];
        public static List<(Regex, Func<CharacterBody, string>)> statDictionaryCompiled = [];
        Transform statTracker = null;
        TextMeshProUGUI textComponent;
        GameObject textComponentGameObject;
        LayoutElement layoutElement;
        Image cachedImage;
        private static Hook overrideHook;
        private static Hook overrideHook2;
        internal bool scoreBoardOpen = false;

        public StatsDisplayClass()
        {
            Setup();
            SetupRiskOfOptions();
        }
        const string syntaxList = " \n luck \n baseDamage \n crit \n attackSpeed \n armor \n armorDamageReduction \n regen \n speed \n availableJumps \n maxJumps \n killCount \n mountainShrines \n experience \n level \n maxHealth \n maxBarrier \n barrierDecayRate \n maxShield \n acceleration \n jumpPower \n maxJumpHeight \n damage \n critMultiplier \n bleedChance \n visionDistance \n critHeal \n cursePenalty \n hasOneShotProtection \n isGlass \n canPerformBackstab \n canReceiveBackstab \n healthPercentage \n goldPortal \n msPortal \n shopPortal \n dps \n currentCombatDamage \n remainingComboDuration \n maxCombo \n maxComboThisRun \n currentCombatKills \n maxKillCombo \n maxKillComboThisRun \n critWithLuck \n bleedChanceWithLuck \n velocity \n teddyBearBlockChance \n saferSpacesCD \n instaKillChance \n voidPortal \n difficultyCoefficient \n stage";
        public void Setup()
        {
            statsDisplay = BasePlugin.instance.Config.Bind<bool>("Stats Display", "StatsDisplay", true, "Enables Stats Display");
            statsDisplayString = BasePlugin.instance.Config.Bind<string>("Stats Display", "Stats Display String",
                "<size=120%>Stats</size>\n" +
                "Luck: [luck]\n" +
                "Damage: [damage]\n" +
                "Crit Chance: [critWithLuck]\n" +
                "Attack Speed: [attackSpeed]\n" +
                "Armor: [armor] | [armorDamageReduction]\n" +
                "Regen: [regen]\n" +
                "Speed: [speed]\n" +
                "Jumps: [availableJumps]/[maxJumps]\n" +
                "Kills: [killCount]\n" +
                "Mountain Shrines: [mountainShrines]\n" +
                "DPS: [dps]\n" +
                "Combo: [currentCombatDamage]\n" +
                "Combo Timer: [remainingComboDuration]\n" +
                "Max Combo: [maxCombo]"
                , $"String for the stats display. You can customize this with Unity Rich Text if you want, see \n https://docs.unity3d.com/Packages/com.unity.textmeshpro@4.0/manual/RichText.html for more info. \nAvailable syntax for the [] stuff is:{syntaxList}");
            statsDisplaySize = BasePlugin.instance.Config.Bind<float>("Stats Display", "StatsDisplay font size", -1, "General font size of the stats display menu. If set to -1, will copy the font size from the objective panel");
            statsDisplayUpdateInterval = BasePlugin.instance.Config.Bind<float>("Stats Display", "StatsDisplay update interval", 0.33f, "The interval at which stats display updates, in seconds. Lower values will increase responsiveness, but may potentially affect performance");
            builtInColors = BasePlugin.instance.Config.Bind<bool>("Stats Display", "Use default colors", true, "Uses the default styling for stats display syntax items.");
            builtInColors.SettingChanged += BuiltInColors_SettingChanged;
            statsDisplayOverrideHeight = BasePlugin.instance.Config.Bind<bool>("Stats Display", "Override Stats Display Height", false, "Sets a user-specified height for Stats Display (may be necessary if you get particularly creative with formatting)");
            statsDisplayOverrideHeightValue = BasePlugin.instance.Config.Bind<int>("Stats Display", "Stats Display Height Value", 7, "Height, in lines of full-size text, for the Stats Display panel");
            floatPrecision = BasePlugin.instance.Config.Bind<int>("Stats Display", "StatsDisplay Float Precision", 2, "How many decimal points will be used in floating point values");
            floatPrecision.SettingChanged += BuiltInColors_SettingChanged;
            useSecondaryStatsDisplay = BasePlugin.instance.Config.Bind<bool>("Stats Display", "Use Secondary StatsDisplay", false, "Will enable the use of the secondary stats display string. This will overwrite the stats display string whenever the scoreboard is held open.");
            secondaryStatsDisplayString = BasePlugin.instance.Config.Bind<string>("Stats Display", "Secondary Stats Display String",
                "<size=120%>Stats</size>\n" +
                "Luck: [luck]\n" +
                "Damage: [damage]\n" +
                "Crit Chance: [critWithLuck]\n" +
                "Bleed Chance: [bleedChanceWithLuck]\n" +
                "Attack Speed: [attackSpeed]\n" +
                "Armor: [armor] | [armorDamageReduction]\n" +
                "Regen: [regen]\n" +
                "Speed: [speed]\n" +
                "Jumps: [availableJumps]/[maxJumps]\n" +
                "Kills: [killCount]\n" +
                "Mountain Shrines: [mountainShrines]\n" +
                "Max Combo: [maxCombo]\n" +
                "<size=120%>Portals:</size> \n" +
                "<size=50%>Gold:[goldPortal] Shop:[shopPortal] Celestial:[msPortal] Void:[voidPortal]</size>"
                , $"Secondary string for the stats display. You can customize this with Unity Rich Text if you want, see \n https://docs.unity3d.com/Packages/com.unity.textmeshpro@4.0/manual/RichText.html for more info. \nAvailable syntax for the [] stuff is: {syntaxList}");
            //StatsDisplayDefinitions.SetupDefs();
            StatsDisplayDefinitions.SetupDefsCompiled();

            var targetMethod = typeof(ScoreboardController).GetMethod(nameof(ScoreboardController.OnEnable), System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var destMethod = typeof(BasePlugin).GetMethod(nameof(BasePlugin.hook_OnEnable), System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            overrideHook = new Hook(targetMethod, destMethod, this);
            targetMethod = typeof(ScoreboardController).GetMethod(nameof(ScoreboardController.OnDisable), System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            destMethod = typeof(BasePlugin).GetMethod(nameof(BasePlugin.hook_OnDisable), System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            overrideHook2 = new Hook(targetMethod, destMethod, this);
        }

        private void BuiltInColors_SettingChanged(object sender, EventArgs e)
        {
            StatsDisplayDefinitions.SetupDefsCompiled();
        }

        public void SetupRiskOfOptions()
        {
            ModSettingsManager.AddOption(new CheckBoxOption(statsDisplay, new CheckBoxConfig() { restartRequired = false }));
            ModSettingsManager.AddOption(new StringInputFieldOption(statsDisplayString, new InputFieldConfig() { restartRequired = false, lineType = TMP_InputField.LineType.MultiLineNewline, submitOn = InputFieldConfig.SubmitEnum.OnExit, richText = false }));
            ModSettingsManager.AddOption(new SliderOption(statsDisplaySize, new SliderConfig() { restartRequired = false, min = -1, max = 100 }));
            ModSettingsManager.AddOption(new CheckBoxOption(builtInColors, new CheckBoxConfig() { restartRequired = false }));
            ModSettingsManager.AddOption(new SliderOption(statsDisplayUpdateInterval, new SliderConfig() { restartRequired = false, min = 0.01f, max = 1f, formatString = "{0:F2}s" }));
            ModSettingsManager.AddOption(new CheckBoxOption(statsDisplayOverrideHeight, new CheckBoxConfig() { restartRequired = false }));
            ModSettingsManager.AddOption(new IntSliderOption(statsDisplayOverrideHeightValue, new IntSliderConfig() { restartRequired = false, min = 0, max = 100 }));
            ModSettingsManager.AddOption(new IntSliderOption(floatPrecision, new IntSliderConfig() { restartRequired = false, min = 0, max = 5 }));

            ModSettingsManager.AddOption(new CheckBoxOption(useSecondaryStatsDisplay, new CheckBoxConfig() { restartRequired = false }));
            ModSettingsManager.AddOption(new StringInputFieldOption(secondaryStatsDisplayString, new InputFieldConfig() { restartRequired = false, lineType = TMP_InputField.LineType.MultiLineNewline, submitOn = InputFieldConfig.SubmitEnum.OnExit, richText = false }));
        }
        bool isRiskUI = false;
        float originalFontSize = -1;
        VerticalLayoutGroup layoutGroup;

        public IEnumerator CalculateStuff()
        {
            if (!statsDisplay.Value || !Run.instance)
                yield break;

            if (!statTracker)
            {
                var hud = HUD.readOnlyInstanceList?.FirstOrDefault();
                if (!hud || !hud.gameModeUiInstance)
                    yield break;

                var childLoc = hud.gameModeUiInstance.GetComponent<ChildLocator>();
                if (childLoc)
                {
                    yield return CreateStatTracker(childLoc.FindChild("RightInfoBar"));
                }
            }

            var cachedUserBody = LocalUserManager.GetFirstLocalUser()?.cachedBody;

            if (!cachedUserBody)
                yield break;

            yield return UpdateStatDisplay(cachedUserBody);
        }

        IEnumerator UpdateStatDisplay(CharacterBody cachedUserBody)
        {
            if (textComponentGameObject)
                textComponentGameObject.SetActive(true);

            if (textComponent && layoutElement)
            {
                var stats = (useSecondaryStatsDisplay.Value && scoreBoardOpen) ? secondaryStatsDisplayString.Value : statsDisplayString.Value;
                foreach ((var regex, var func) in statDictionaryCompiled)
                {
                    stats = regex.Replace(stats, func(cachedUserBody));
                    yield return null;
                }

                textComponent.text = stats;

                int nlines = statsDisplayOverrideHeight.Value
                    ? statsDisplayOverrideHeightValue.Value
                    : stats.Split('\n').Length;

                if (originalFontSize == -1)
                    originalFontSize = textComponent.fontSize;

                textComponent.fontSize = statsDisplaySize.Value == -1 ? originalFontSize : statsDisplaySize.Value;

                Run.instance.StartCoroutine(FixScaleAfterFrame(nlines));

                if (!cachedImage)
                    cachedImage = layoutElement.transform.parent.GetComponent<Image>();

                if (cachedImage)
                    cachedImage.enabled = nlines != 0;

                if (isRiskUI && layoutGroup)
                    layoutGroup.padding.bottom = (int)((nlines / 16f) * 50);
            }
            else
            {
                layoutGroup = null;
                textComponent = null;
                layoutElement = null;
                if (statTracker)
                {
                    Log.Debug($"Somehow statTracker [{statTracker.gameObject.name}] got set but other items didn't attempting to delete it and try again.");
                    GameObject.DestroyImmediate(statTracker.gameObject);
                }
            }

            yield return null;
        }

        IEnumerator CreateStatTracker(Transform rightInfoBar)
        {
            if (!rightInfoBar)
                yield break;

            var objectivePanel = rightInfoBar.Find("ObjectivePanel");
            if (!objectivePanel)
                yield break;

            var stats = GameObject.Instantiate(objectivePanel.gameObject, rightInfoBar);
            stats.name = "PlayerStats";

            if (stats.transform.Find("Label"))
                stats.transform.Find("Label").gameObject.SetActive(true);

            if (stats.transform.Find("StripContainer"))
                GameObject.Destroy(stats.transform.Find("StripContainer").gameObject);

            if (stats.transform.Find("Minimap"))
                GameObject.DestroyImmediate(stats.transform.Find("Minimap").gameObject);

            if (stats.TryGetComponent<HudObjectiveTargetSetter>(out var targetComp))
                UnityEngine.Object.Destroy(targetComp);

            if (stats.TryGetComponent<ObjectivePanelController>(out var objComp))
                UnityEngine.Object.Destroy(objComp);

            var rect = stats.GetComponent<RectTransform>();
            layoutGroup = stats.GetComponent<VerticalLayoutGroup>();
            textComponent = stats.GetComponentInChildren<TextMeshProUGUI>();
            layoutElement = stats.GetComponentInChildren<LayoutElement>();

            if (!rect || !layoutGroup || !textComponent || !layoutElement)
            {
                layoutGroup = null;
                textComponent = null;
                layoutElement = null;
                GameObject.DestroyImmediate(stats);
                yield break;
            }

            rect.localPosition = Vector3.zero;
            rect.localEulerAngles = Vector3.zero;
            rect.localScale = Vector3.one;

            layoutGroup.enabled = false;
            layoutGroup.enabled = true;

            textComponent.alignment = TMPro.TextAlignmentOptions.TopLeft;
            textComponent.color = Color.white;
            textComponentGameObject = textComponent.gameObject;

            if (stats.transform.Find("Seperator"))
            {
                isRiskUI = true;
                var layout = stats.transform.Find("Seperator").GetComponent<VerticalLayoutGroup>();
                layout.padding.top = 0;
                layout.childAlignment = TextAnchor.UpperLeft;
            }

            statTracker = stats.transform;
        }

        IEnumerator FixScaleAfterFrame(int nlines)//needed to make the tab stuff work nicely
        {
            yield return new WaitForEndOfFrame();

            float intendedHeight = statsDisplayOverrideHeight.Value
                ? textComponent.fontSize * (nlines + 1)
                : textComponent.renderedHeight;

            layoutElement.preferredHeight = intendedHeight;
        }
    }
}
