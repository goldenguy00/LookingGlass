using System.Text.RegularExpressions;
using LookingGlass.DPSMeterStuff;
using RoR2;
using UnityEngine;

namespace LookingGlass.StatsDisplay
{
    internal static class StatsDisplayDefinitions
    {
        internal static string floatPrecision;
        internal const string NA = "N/A";
        internal static string utilityString = StatsDisplayClass.builtInColors.Value ? "<style=\"cIsUtility>" : string.Empty;
        internal static string damageString = StatsDisplayClass.builtInColors.Value ? "<style=\"cIsDamage>" : string.Empty;
        internal static string healingString = StatsDisplayClass.builtInColors.Value ? "<style=\"cIsHealing>" : string.Empty;
        internal static string healthString = StatsDisplayClass.builtInColors.Value ? "<style=\"cIsHealth>" : string.Empty;
        internal static string voidString = StatsDisplayClass.builtInColors.Value ? "<style=\"cIsVoid>" : string.Empty;
        internal static string styleString = StatsDisplayClass.builtInColors.Value ? "</style>" : string.Empty;
        /*internal static void SetupDefs()
        {
            string utilityString = StatsDisplayClass.builtInColors.Value ? "<style=\"cIsUtility>" : string.Empty;
            string damageString = StatsDisplayClass.builtInColors.Value ? "<style=\"cIsDamage>" : string.Empty;
            string healingString = StatsDisplayClass.builtInColors.Value ? "<style=\"cIsHealing>" : string.Empty;
            string healthString = StatsDisplayClass.builtInColors.Value ? "<style=\"cIsHealth>" : string.Empty;
            string voidString = StatsDisplayClass.builtInColors.Value ? "<style=\"cIsVoid>" : string.Empty;
            string styleString = StatsDisplayClass.builtInColors.Value ? "</style>" : string.Empty;
            //NumberFormatInfo floatPrecision = new NumberFormatInfo();
            //floatPrecision.NumberDecimalDigits = StatsDisplayClass.floatPrecision.Value;
            floatPrecision = "0." + new string('#', StatsDisplayClass.floatPrecision.Value);

            StatsDisplayClass.statDictionary.Clear();

            StatsDisplayClass.statDictionary["luck", cachedUserBody => $"{utilityString}{Utils.GetLuckFromCachedUserBody(cachedUserBody).ToString(floatPrecision)}{styleString}"));
            StatsDisplayClass.statDictionary["baseDamage", cachedUserBody => $"{damageString}{cachedUserBody.baseDamage.ToString(floatPrecision)}{styleString}"));
            StatsDisplayClass.statDictionary["crit", cachedUserBody => $"{damageString}{cachedUserBody.crit.ToString(floatPrecision)}%{styleString}"));
            StatsDisplayClass.statDictionary["attackSpeed", cachedUserBody => $"{damageString}{cachedUserBody.attackSpeed.ToString(floatPrecision)}{styleString}"));
            StatsDisplayClass.statDictionary["armor", cachedUserBody => $"{healingString}{cachedUserBody.armor.ToString(floatPrecision)}{styleString}"));
            StatsDisplayClass.statDictionary["armorDamageReduction", cachedUserBody => $"{healingString}{Util.ConvertAmplificationPercentageIntoReductionPercentage(cachedUserBody.armor).ToString(floatPrecision)}%{styleString}"));
            StatsDisplayClass.statDictionary["regen", cachedUserBody => $"{healingString}{cachedUserBody.regen.ToString(floatPrecision)}{styleString}"));
            StatsDisplayClass.statDictionary["speed", cachedUserBody => $"{utilityString}{cachedUserBody.moveSpeed.ToString(floatPrecision)}{styleString}"));

            StatsDisplayClass.statDictionary["availableJumps", cachedUserBody =>
            {
                return !cachedUserBody.characterMotor
                    ? $"{utilityString}{NA}{styleString}"
                    : $"{utilityString}{cachedUserBody.maxJumpCount - cachedUserBody.characterMotor.jumpCount}{styleString}"));
            }));

            StatsDisplayClass.statDictionary["maxJumps", cachedUserBody => $"{utilityString}{cachedUserBody.maxJumpCount}{styleString}"));
            StatsDisplayClass.statDictionary["killCount", cachedUserBody => $"{healthString}{cachedUserBody.killCountServer}{styleString}"));
            StatsDisplayClass.statDictionary["mountainShrines", cachedUserBody => $"{utilityString}{(TeleporterInteraction.instance ? TeleporterInteraction.instance.shrineBonusStacks : NA)}{styleString}"));
            StatsDisplayClass.statDictionary["experience", cachedUserBody => $"{utilityString}{cachedUserBody.experience.ToString(floatPrecision)}{styleString}"));
            StatsDisplayClass.statDictionary["level", cachedUserBody => $"{utilityString}{cachedUserBody.level}{styleString}"));
            StatsDisplayClass.statDictionary["maxHealth", cachedUserBody => $"{healthString}{cachedUserBody.maxHealth}{styleString}"));
            StatsDisplayClass.statDictionary["maxBarrier", cachedUserBody => $"{utilityString}{cachedUserBody.maxBarrier}{styleString}"));
            StatsDisplayClass.statDictionary["barrierDecayRate", cachedUserBody => $"{utilityString}{cachedUserBody.barrierDecayRate.ToString(floatPrecision)}{styleString}"));
            StatsDisplayClass.statDictionary["maxShield", cachedUserBody => $"{utilityString}{cachedUserBody.maxShield}{styleString}"));
            StatsDisplayClass.statDictionary["acceleration", cachedUserBody => $"{utilityString}{cachedUserBody.acceleration.ToString(floatPrecision)}{styleString}"));
            StatsDisplayClass.statDictionary["jumpPower", cachedUserBody => $"{utilityString}{cachedUserBody.jumpPower.ToString(floatPrecision)}{styleString}"));
            StatsDisplayClass.statDictionary["maxJumpHeight", cachedUserBody => $"{utilityString}{cachedUserBody.maxJumpHeight.ToString(floatPrecision)}{styleString}"));
            StatsDisplayClass.statDictionary["damage", cachedUserBody => $"{damageString}{cachedUserBody.damage.ToString(floatPrecision)}{styleString}"));
            StatsDisplayClass.statDictionary["critMultiplier", cachedUserBody => $"{damageString}{cachedUserBody.critMultiplier.ToString(floatPrecision)}{styleString}"));
            StatsDisplayClass.statDictionary["bleedChance", cachedUserBody => $"{damageString}{cachedUserBody.bleedChance.ToString(floatPrecision)}%{styleString}"));
            StatsDisplayClass.statDictionary["visionDistance", cachedUserBody => $"{utilityString}{cachedUserBody.visionDistance.ToString(floatPrecision)}{styleString}"));
            StatsDisplayClass.statDictionary["critHeal", cachedUserBody => $"{healingString}{cachedUserBody.critHeal.ToString(floatPrecision)}{styleString}"));
            StatsDisplayClass.statDictionary["cursePenalty", cachedUserBody => $"{utilityString}{cachedUserBody.cursePenalty.ToString(floatPrecision)}{styleString}"));

            StatsDisplayClass.statDictionary["hasOneShotProtection", cachedUserBody =>
            {
                return !cachedUserBody.healthComponent
                    ? $"{utilityString}{NA}{styleString}"
                    : $"{utilityString}{(cachedUserBody.oneShotProtectionFraction * cachedUserBody.healthComponent.fullCombinedHealth - cachedUserBody.healthComponent.missingCombinedHealth) >= 0}{styleString}"));
            }));

            StatsDisplayClass.statDictionary["isGlass", cachedUserBody => $"{utilityString}{cachedUserBody.isGlass}{styleString}"));
            StatsDisplayClass.statDictionary["canPerformBackstab", cachedUserBody => $"{damageString}{cachedUserBody.canPerformBackstab}{styleString}"));
            StatsDisplayClass.statDictionary["canReceiveBackstab", cachedUserBody => $"{damageString}{cachedUserBody.canReceiveBackstab}{styleString}"));

            StatsDisplayClass.statDictionary["healthPercentage", cachedUserBody => 
            {
                return !cachedUserBody.healthComponent
                    ? $"{healthString}{NA}{styleString}"
                    : $"{healthString}{(cachedUserBody.healthComponent.combinedHealthFraction * 100f).ToString(floatPrecision)}{styleString}"));
            }));

            StatsDisplayClass.statDictionary["goldPortal", cachedUserBody => $"{damageString}{(TeleporterInteraction.instance ? TeleporterInteraction.instance.shouldAttemptToSpawnGoldshoresPortal.ToString() : NA)}{styleString}"));
            StatsDisplayClass.statDictionary["msPortal", cachedUserBody => $"{utilityString}{(TeleporterInteraction.instance ? TeleporterInteraction.instance.shouldAttemptToSpawnMSPortal.ToString() : NA)}{styleString}"));
            StatsDisplayClass.statDictionary["shopPortal", cachedUserBody => $"{utilityString}{(TeleporterInteraction.instance ? TeleporterInteraction.instance.shouldAttemptToSpawnShopPortal.ToString() : NA)}{styleString}"));

            StatsDisplayClass.statDictionary["voidPortal", cachedUserBody =>
            {
                if (TeleporterInteraction.instance == null)
                {
                    return $"{voidString}{NA}{styleString}"));
                }
                foreach (var item in TeleporterInteraction.instance.portalSpawners)
                {
                    if (item.portalSpawnCard.name == "iscVoidPortal" && item.NetworkwillSpawn)
                    {
                        return $"{voidString}True{styleString}"));
                    }
                }
                return $"{voidString}False{styleString}"));
            }));

            StatsDisplayClass.statDictionary["dps", cachedUserBody => $"{damageString}{BasePlugin.instance.dpsMeter.damageDealtSincePeriod / DPSMeter.DPS_MAX_TIME}{styleString}"));
            StatsDisplayClass.statDictionary["currentCombatDamage", cachedUserBody => $"{damageString}{BasePlugin.instance.dpsMeter.currentCombatDamage}{styleString}"));
            StatsDisplayClass.statDictionary["remainingComboDuration", cachedUserBody => $"{utilityString}{(int)BasePlugin.instance.dpsMeter.timer + 1}{styleString}"));
            StatsDisplayClass.statDictionary["maxCombo", cachedUserBody => $"{damageString}{BasePlugin.instance.dpsMeter.maxCombo}{styleString}"));
            StatsDisplayClass.statDictionary["maxComboThisRun", cachedUserBody => $"{damageString}{BasePlugin.instance.dpsMeter.maxRunCombo}{styleString}"));

            StatsDisplayClass.statDictionary["currentCombatKills", cachedUserBody => $"{damageString}{BasePlugin.instance.dpsMeter.currentComboKills}{styleString}"));
            StatsDisplayClass.statDictionary["maxKillCombo", cachedUserBody => $"{damageString}{BasePlugin.instance.dpsMeter.maxCombo}{styleString}"));
            StatsDisplayClass.statDictionary["maxKillComboThisRun", cachedUserBody => $"{damageString}{BasePlugin.instance.dpsMeter.maxRunCombo}{styleString}"));

            StatsDisplayClass.statDictionary["critWithLuck", cachedUserBody => $"{damageString}{(Utils.CalculateChanceWithLuck(cachedUserBody.crit / 100f, Utils.GetLuckFromCachedUserBody(cachedUserBody)) * 100f).ToString(floatPrecision)}%{styleString}"));
            StatsDisplayClass.statDictionary["bleedChanceWithLuck", cachedUserBody => $"{damageString}{(Utils.CalculateChanceWithLuck(cachedUserBody.bleedChance / 100f, Utils.GetLuckFromCachedUserBody(cachedUserBody)) * 100f).ToString(floatPrecision)}%{styleString}"));

            StatsDisplayClass.statDictionary["velocity", cachedUserBody =>
            {
                return cachedUserBody.TryGetComponent<Rigidbody>(out var rigidBody)
                    ? $"{utilityString}{rigidBody.velocity.magnitude.ToString(floatPrecision)}{styleString}"
                    : $"{utilityString}{NA}{styleString}"));
            }));

            StatsDisplayClass.statDictionary["teddyBearBlockChance", cachedUserBody =>
            {
                int stackCount = cachedUserBody.inventory.GetItemCount(RoR2Content.Items.Bear);
                return $"{utilityString}{(0.15f * stackCount / ((0.15f * stackCount) + 1) * 100f).ToString(floatPrecision)}%{styleString}"));
            }));

            StatsDisplayClass.statDictionary["saferSpacesCD", cachedUserBody =>
            {
                int stackCount = cachedUserBody.inventory.GetItemCount(DLC1Content.Items.BearVoid);
                return stackCount == 0
                    ? $"{utilityString}{NA}{styleString}"
                    : $"{utilityString}{(15 * Mathf.Pow(.9f, stackCount)).ToString(floatPrecision)}{styleString}"));
            }));

            StatsDisplayClass.statDictionary["instaKillChance", cachedUserBody =>
            {
                int stackCount = cachedUserBody.inventory.GetItemCount(DLC1Content.Items.CritGlassesVoid);
                float instakillChance = Utils.CalculateChanceWithLuck(.005f * stackCount, Utils.GetLuckFromCachedUserBody(cachedUserBody)) * 100f;
                return $"{damageString}{instakillChance.ToString(floatPrecision)}%{styleString}"));
            }));

            StatsDisplayClass.statDictionary["difficultyCoefficient", cachedUserBody => $"{utilityString}{Run.instance.difficultyCoefficient}{styleString}"));
            StatsDisplayClass.statDictionary["stage", cachedUserBody => $"{utilityString}{Language.GetString(RoR2.Stage.instance.sceneDef.nameToken)}{styleString}"));
        }*/
        internal static void SetupStyles()
        {

            utilityString = StatsDisplayClass.builtInColors.Value ? "<style=\"cIsUtility>" : string.Empty;
            damageString = StatsDisplayClass.builtInColors.Value ? "<style=\"cIsDamage>" : string.Empty;
            healingString = StatsDisplayClass.builtInColors.Value ? "<style=\"cIsHealing>" : string.Empty;
            healthString = StatsDisplayClass.builtInColors.Value ? "<style=\"cIsHealth>" : string.Empty;
            voidString = StatsDisplayClass.builtInColors.Value ? "<style=\"cIsVoid>" : string.Empty;
            styleString = StatsDisplayClass.builtInColors.Value ? "</style>" : string.Empty;
            //NumberFormatInfo floatPrecision = new NumberFormatInfo();
            //floatPrecision.NumberDecimalDigits = StatsDisplayClass.floatPrecision.Value;
            floatPrecision = "0." + new string('#', StatsDisplayClass.floatPrecision.Value);

        }
        internal static void SetupDefsCompiled()
        {
            SetupStyles();
            StatsDisplayClass.statDictionaryCompiled.Clear();

            StatsDisplayClass.statDictionaryCompiled.Add((new Regex($@"(?<!\\)\[luck\]", RegexOptions.Compiled), cachedUserBody => $"{utilityString}{Utils.GetLuckFromCachedUserBody(cachedUserBody).ToString(floatPrecision)}{styleString}"));
            StatsDisplayClass.statDictionaryCompiled.Add((new Regex($@"(?<!\\)\[baseDamage\]", RegexOptions.Compiled), cachedUserBody => $"{damageString}{cachedUserBody.baseDamage.ToString(floatPrecision)}{styleString}"));
            StatsDisplayClass.statDictionaryCompiled.Add((new Regex($@"(?<!\\)\[crit\]", RegexOptions.Compiled), cachedUserBody => $"{damageString}{cachedUserBody.crit.ToString(floatPrecision)}%{styleString}"));
            StatsDisplayClass.statDictionaryCompiled.Add((new Regex($@"(?<!\\)\[attackSpeed\]", RegexOptions.Compiled), cachedUserBody => $"{damageString}{cachedUserBody.attackSpeed.ToString(floatPrecision)}{styleString}"));
            StatsDisplayClass.statDictionaryCompiled.Add((new Regex($@"(?<!\\)\[armor\]", RegexOptions.Compiled), cachedUserBody => $"{healingString}{cachedUserBody.armor.ToString(floatPrecision)}{styleString}"));
            StatsDisplayClass.statDictionaryCompiled.Add((new Regex($@"(?<!\\)\[armorDamageReduction\]", RegexOptions.Compiled), cachedUserBody => $"{healingString}{Util.ConvertAmplificationPercentageIntoReductionPercentage(cachedUserBody.armor).ToString(floatPrecision)}%{styleString}"));
            StatsDisplayClass.statDictionaryCompiled.Add((new Regex($@"(?<!\\)\[regen\]", RegexOptions.Compiled), cachedUserBody => $"{healingString}{cachedUserBody.regen.ToString(floatPrecision)}{styleString}"));
            StatsDisplayClass.statDictionaryCompiled.Add((new Regex($@"(?<!\\)\[speed\]", RegexOptions.Compiled), cachedUserBody => $"{utilityString}{cachedUserBody.moveSpeed.ToString(floatPrecision)}{styleString}"));

            StatsDisplayClass.statDictionaryCompiled.Add((new Regex($@"(?<!\\)\[availableJumps\]", RegexOptions.Compiled), cachedUserBody =>
            {
                return !cachedUserBody.characterMotor
                    ? $"{utilityString}{NA}{styleString}"
                    : $"{utilityString}{cachedUserBody.maxJumpCount - cachedUserBody.characterMotor.jumpCount}{styleString}";
            }));

            StatsDisplayClass.statDictionaryCompiled.Add((new Regex($@"(?<!\\)\[maxJumps\]", RegexOptions.Compiled), cachedUserBody => $"{utilityString}{cachedUserBody.maxJumpCount}{styleString}"));
            StatsDisplayClass.statDictionaryCompiled.Add((new Regex($@"(?<!\\)\[killCount\]", RegexOptions.Compiled), cachedUserBody => $"{healthString}{cachedUserBody.killCountServer}{styleString}"));
            StatsDisplayClass.statDictionaryCompiled.Add((new Regex($@"(?<!\\)\[mountainShrines\]", RegexOptions.Compiled), cachedUserBody => $"{utilityString}{(TeleporterInteraction.instance ? TeleporterInteraction.instance.shrineBonusStacks : NA)}{styleString}"));
            StatsDisplayClass.statDictionaryCompiled.Add((new Regex($@"(?<!\\)\[experience\]", RegexOptions.Compiled), cachedUserBody => $"{utilityString}{cachedUserBody.experience.ToString(floatPrecision)}{styleString}"));
            StatsDisplayClass.statDictionaryCompiled.Add((new Regex($@"(?<!\\)\[level\]", RegexOptions.Compiled), cachedUserBody => $"{utilityString}{cachedUserBody.level}{styleString}"));
            StatsDisplayClass.statDictionaryCompiled.Add((new Regex($@"(?<!\\)\[maxHealth\]", RegexOptions.Compiled), cachedUserBody => $"{healthString}{cachedUserBody.maxHealth}{styleString}"));
            StatsDisplayClass.statDictionaryCompiled.Add((new Regex($@"(?<!\\)\[maxBarrier\]", RegexOptions.Compiled), cachedUserBody => $"{utilityString}{cachedUserBody.maxBarrier}{styleString}"));
            StatsDisplayClass.statDictionaryCompiled.Add((new Regex($@"(?<!\\)\[barrierDecayRate\]", RegexOptions.Compiled), cachedUserBody => $"{utilityString}{cachedUserBody.barrierDecayRate.ToString(floatPrecision)}{styleString}"));
            StatsDisplayClass.statDictionaryCompiled.Add((new Regex($@"(?<!\\)\[maxShield\]", RegexOptions.Compiled), cachedUserBody => $"{utilityString}{cachedUserBody.maxShield}{styleString}"));
            StatsDisplayClass.statDictionaryCompiled.Add((new Regex($@"(?<!\\)\[acceleration\]", RegexOptions.Compiled), cachedUserBody => $"{utilityString}{cachedUserBody.acceleration.ToString(floatPrecision)}{styleString}"));
            StatsDisplayClass.statDictionaryCompiled.Add((new Regex($@"(?<!\\)\[jumpPower\]", RegexOptions.Compiled), cachedUserBody => $"{utilityString}{cachedUserBody.jumpPower.ToString(floatPrecision)}{styleString}"));
            StatsDisplayClass.statDictionaryCompiled.Add((new Regex($@"(?<!\\)\[maxJumpHeight\]", RegexOptions.Compiled), cachedUserBody => $"{utilityString}{cachedUserBody.maxJumpHeight.ToString(floatPrecision)}{styleString}"));
            StatsDisplayClass.statDictionaryCompiled.Add((new Regex($@"(?<!\\)\[damage\]", RegexOptions.Compiled), cachedUserBody => $"{damageString}{cachedUserBody.damage.ToString(floatPrecision)}{styleString}"));
            StatsDisplayClass.statDictionaryCompiled.Add((new Regex($@"(?<!\\)\[critMultiplier\]", RegexOptions.Compiled), cachedUserBody => $"{damageString}{cachedUserBody.critMultiplier.ToString(floatPrecision)}{styleString}"));
            StatsDisplayClass.statDictionaryCompiled.Add((new Regex($@"(?<!\\)\[bleedChance\]", RegexOptions.Compiled), cachedUserBody => $"{damageString}{cachedUserBody.bleedChance.ToString(floatPrecision)}%{styleString}"));
            StatsDisplayClass.statDictionaryCompiled.Add((new Regex($@"(?<!\\)\[visionDistance\]", RegexOptions.Compiled), cachedUserBody => $"{utilityString}{cachedUserBody.visionDistance.ToString(floatPrecision)}{styleString}"));
            StatsDisplayClass.statDictionaryCompiled.Add((new Regex($@"(?<!\\)\[critHeal\]", RegexOptions.Compiled), cachedUserBody => $"{healingString}{cachedUserBody.critHeal.ToString(floatPrecision)}{styleString}"));
            StatsDisplayClass.statDictionaryCompiled.Add((new Regex($@"(?<!\\)\[cursePenalty\]", RegexOptions.Compiled), cachedUserBody => $"{utilityString}{cachedUserBody.cursePenalty.ToString(floatPrecision)}{styleString}"));

            StatsDisplayClass.statDictionaryCompiled.Add((new Regex($@"(?<!\\)\[hasOneShotProtection\]", RegexOptions.Compiled), cachedUserBody =>
            {
                return !cachedUserBody.healthComponent
                    ? $"{utilityString}{NA}{styleString}"
                    : $"{utilityString}{(cachedUserBody.oneShotProtectionFraction * cachedUserBody.healthComponent.fullCombinedHealth - cachedUserBody.healthComponent.missingCombinedHealth) >= 0}{styleString}";
            }));

            StatsDisplayClass.statDictionaryCompiled.Add((new Regex($@"(?<!\\)\[isGlass\]", RegexOptions.Compiled), cachedUserBody => $"{utilityString}{cachedUserBody.isGlass}{styleString}"));
            StatsDisplayClass.statDictionaryCompiled.Add((new Regex($@"(?<!\\)\[canPerformBackstab\]", RegexOptions.Compiled), cachedUserBody => $"{damageString}{cachedUserBody.canPerformBackstab}{styleString}"));
            StatsDisplayClass.statDictionaryCompiled.Add((new Regex($@"(?<!\\)\[canReceiveBackstab\]", RegexOptions.Compiled), cachedUserBody => $"{damageString}{cachedUserBody.canReceiveBackstab}{styleString}"));

            StatsDisplayClass.statDictionaryCompiled.Add((new Regex($@"(?<!\\)\[healthPercentage\]", RegexOptions.Compiled), cachedUserBody => 
            {
                return !cachedUserBody.healthComponent
                    ? $"{healthString}{NA}{styleString}"
                    : $"{healthString}{(cachedUserBody.healthComponent.combinedHealthFraction * 100f).ToString(floatPrecision)}{styleString}";
            }));

            StatsDisplayClass.statDictionaryCompiled.Add((new Regex($@"(?<!\\)\[goldPortal\]", RegexOptions.Compiled), cachedUserBody => 
                $"{damageString}{(TeleporterInteraction.instance ? TeleporterInteraction.instance.shouldAttemptToSpawnGoldshoresPortal.ToString() : NA)}{styleString}"));
            StatsDisplayClass.statDictionaryCompiled.Add((new Regex($@"(?<!\\)\[msPortal\]", RegexOptions.Compiled), cachedUserBody => 
                $"{utilityString}{(TeleporterInteraction.instance ? TeleporterInteraction.instance.shouldAttemptToSpawnMSPortal.ToString() : NA)}{styleString}"));
            StatsDisplayClass.statDictionaryCompiled.Add((new Regex($@"(?<!\\)\[shopPortal\]", RegexOptions.Compiled), cachedUserBody => 
                $"{utilityString}{(TeleporterInteraction.instance ? TeleporterInteraction.instance.shouldAttemptToSpawnShopPortal.ToString() : NA)}{styleString}"));

            StatsDisplayClass.statDictionaryCompiled.Add((new Regex($@"(?<!\\)\[voidPortal\]", RegexOptions.Compiled), cachedUserBody =>
            {
                if (TeleporterInteraction.instance == null)
                {
                    return $"{voidString}{NA}{styleString}";
                }
                foreach (var item in TeleporterInteraction.instance.portalSpawners)
                {
                    if (item.portalSpawnCard.name == "iscVoidPortal" && item.NetworkwillSpawn)
                    {
                        return $"{voidString}True{styleString}";
                    }
                }
                return $"{voidString}False{styleString}";
            }));

            StatsDisplayClass.statDictionaryCompiled.Add((new Regex($@"(?<!\\)\[dps\]", RegexOptions.Compiled), cachedUserBody => $"{damageString}{BasePlugin.instance.dpsMeter.damageDealtSincePeriod / DPSMeter.DPS_MAX_TIME}{styleString}"));
            StatsDisplayClass.statDictionaryCompiled.Add((new Regex($@"(?<!\\)\[currentCombatDamage\]", RegexOptions.Compiled), cachedUserBody => $"{damageString}{BasePlugin.instance.dpsMeter.currentCombatDamage}{styleString}"));
            StatsDisplayClass.statDictionaryCompiled.Add((new Regex($@"(?<!\\)\[remainingComboDuration\]", RegexOptions.Compiled), cachedUserBody => $"{utilityString}{(int)BasePlugin.instance.dpsMeter.timer + 1}{styleString}"));
            StatsDisplayClass.statDictionaryCompiled.Add((new Regex($@"(?<!\\)\[maxCombo\]", RegexOptions.Compiled), cachedUserBody => $"{damageString}{BasePlugin.instance.dpsMeter.maxCombo}{styleString}"));
            StatsDisplayClass.statDictionaryCompiled.Add((new Regex($@"(?<!\\)\[maxComboThisRun\]", RegexOptions.Compiled), cachedUserBody => $"{damageString}{BasePlugin.instance.dpsMeter.maxRunCombo}{styleString}"));

            StatsDisplayClass.statDictionaryCompiled.Add((new Regex($@"(?<!\\)\[currentCombatKills\]", RegexOptions.Compiled), cachedUserBody => $"{damageString}{BasePlugin.instance.dpsMeter.currentComboKills}{styleString}"));
            StatsDisplayClass.statDictionaryCompiled.Add((new Regex($@"(?<!\\)\[maxKillCombo\]", RegexOptions.Compiled), cachedUserBody => $"{damageString}{BasePlugin.instance.dpsMeter.maxCombo}{styleString}"));
            StatsDisplayClass.statDictionaryCompiled.Add((new Regex($@"(?<!\\)\[maxKillComboThisRun\]", RegexOptions.Compiled), cachedUserBody => $"{damageString}{BasePlugin.instance.dpsMeter.maxRunCombo}{styleString}"));

            StatsDisplayClass.statDictionaryCompiled.Add((new Regex($@"(?<!\\)\[critWithLuck\]", RegexOptions.Compiled), cachedUserBody =>
                $"{damageString}{(Utils.CalculateChanceWithLuck(cachedUserBody.crit / 100f, Utils.GetLuckFromCachedUserBody(cachedUserBody)) * 100f).ToString(floatPrecision)}%{styleString}"));
            StatsDisplayClass.statDictionaryCompiled.Add((new Regex($@"(?<!\\)\[bleedChanceWithLuck\]", RegexOptions.Compiled), cachedUserBody =>
                $"{damageString}{(Utils.CalculateChanceWithLuck(cachedUserBody.bleedChance / 100f, Utils.GetLuckFromCachedUserBody(cachedUserBody)) * 100f).ToString(floatPrecision)}%{styleString}"));

            StatsDisplayClass.statDictionaryCompiled.Add((new Regex($@"(?<!\\)\[velocity\]", RegexOptions.Compiled), cachedUserBody =>
            {
                return cachedUserBody.TryGetComponent<Rigidbody>(out var rigidBody)
                    ? $"{utilityString}{rigidBody.velocity.magnitude.ToString(floatPrecision)}{styleString}"
                    : $"{utilityString}{NA}{styleString}";
            }));

            StatsDisplayClass.statDictionaryCompiled.Add((new Regex($@"(?<!\\)\[teddyBearBlockChance\]", RegexOptions.Compiled), cachedUserBody =>
            {
                int stackCount = cachedUserBody.inventory.GetItemCount(RoR2Content.Items.Bear);
                return $"{utilityString}{(0.15f * stackCount / ((0.15f * stackCount) + 1) * 100f).ToString(floatPrecision)}%{styleString}";
            }));

            StatsDisplayClass.statDictionaryCompiled.Add((new Regex($@"(?<!\\)\[saferSpacesCD\]", RegexOptions.Compiled), cachedUserBody =>
            {
                int stackCount = cachedUserBody.inventory.GetItemCount(DLC1Content.Items.BearVoid);
                return stackCount == 0
                    ? $"{utilityString}{NA}{styleString}"
                    : $"{utilityString}{(15 * Mathf.Pow(.9f, stackCount)).ToString(floatPrecision)}{styleString}";
            }));

            StatsDisplayClass.statDictionaryCompiled.Add((new Regex($@"(?<!\\)\[instaKillChance\]", RegexOptions.Compiled), cachedUserBody =>
            {
                int stackCount = cachedUserBody.inventory.GetItemCount(DLC1Content.Items.CritGlassesVoid);
                float instakillChance = Utils.CalculateChanceWithLuck(.005f * stackCount, Utils.GetLuckFromCachedUserBody(cachedUserBody)) * 100f;
                return $"{damageString}{instakillChance.ToString(floatPrecision)}%{styleString}";
            }));

            StatsDisplayClass.statDictionaryCompiled.Add((new Regex($@"(?<!\\)\[difficultyCoefficient\]", RegexOptions.Compiled), cachedUserBody => $"{utilityString}{Run.instance.difficultyCoefficient}{styleString}"));
            StatsDisplayClass.statDictionaryCompiled.Add((new Regex($@"(?<!\\)\[stage\]", RegexOptions.Compiled), cachedUserBody => $"{utilityString}{Language.GetString(RoR2.Stage.instance.sceneDef.nameToken)}{styleString}"));
        }
    }
}
