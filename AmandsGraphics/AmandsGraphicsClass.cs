using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityStandardAssets.ImageEffects;
using UnityEngine.SceneManagement;
using System;
using EFT.Weather;
using System.Collections.Generic;
using BSG.CameraEffects;
using HarmonyLib;
using UnityEngine.Rendering;
using EFT;
using EFT.InventoryLogic;
using System.Reflection;
using EFT.UI;

namespace AmandsGraphics
{
    public class AmandsGraphicsClass : MonoBehaviour
    {
        private static LocalPlayer localPlayer;
        private static GameObject FPSCamera;
        private static Camera FPSCameraCamera;
        private static PostProcessVolume FPSCameraPostProcessVolume;
        private static PostProcessLayer FPSCameraPostProcessLayer;
        private static UnityEngine.Rendering.PostProcessing.MotionBlur FPSCameraMotionBlur;
        private static UnityEngine.Rendering.PostProcessing.DepthOfField FPSCameraDepthOfField;
        private static UnityStandardAssets.ImageEffects.DepthOfField FPSCameraWeaponDepthOfField;
        //public static OpticSight opticSight;
        public static SightComponent sightComponent;
        public static Transform backLens;
        private static bool DepthOfFieldEnabled = false;
        private static float DepthOfFieldAnimation = 0f;
        private static float DepthOfFieldFocusDistance = 0f;
        private static bool isLookingEnabled = false;
        private static bool WeaponDepthOfFieldEnabled = false;
        private static EWeaponDepthOfFieldState weaponDepthOfFieldState;
        private static float WeaponDepthOfFieldFocalLength = 0f;
        private static float WeaponDepthOfFieldMaxBlurSize = 0f;

        private static GameObject OpticCamera;
        public static Camera OpticCameraCamera;
        private static PostProcessLayer OpticCameraPostProcessLayer;
        private static UnityEngine.Rendering.PostProcessing.DepthOfField OpticCameraDepthOfField;
        public static ThermalVision OpticCameraThermalVision;
        private static bool OpticDOFEnabled;
        private static float OpticDOFAnimation;
        private static float OpticDOFFocusDistance;
        private static float OpticDOFFocusDistanceAnimation;
        private static RaycastHit hit;
        private static RaycastHit foliageHit;
        private static LayerMask LowLayerMask = LayerMask.GetMask("Terrain", "LowPolyCollider", "HitCollider");
        private static LayerMask HighLayerMask = LayerMask.GetMask("Terrain", "HighPolyCollider", "HitCollider");
        private static LayerMask FoliageLayerMask = LayerMask.GetMask("Terrain", "HighPolyCollider", "HitCollider", "Foliage");
        private static Transform TargetCollider;
        private static Vector3 TargetLocal;
        private static AnimationCurve ApertureAnimationCurve;
        public static bool HoldingBreath = false;

        private static GameObject Weather;
        private static WeatherController weatherController;
        private static CC_Sharpen FPSCameraCC_Sharpen;
        private static Dictionary<BloomAndFlares, float> FPSCameraBloomAndFlares = new Dictionary<BloomAndFlares, float>();
        private static PrismEffects FPSCameraPrismEffects;
        private static CC_Vintage FPSCameraCC_Vintage;
        private static CustomGlobalFog FPSCameraCustomGlobalFog;
        private static Behaviour FPSCameraGlobalFog;
        private static ColorCorrectionCurves FPSCameraColorCorrectionCurves;
        private static NightVision FPSCameraNightVision;
        private static HBAO FPSCameraHBAO;
        public static HBAO_Core.AOSettings FPSCameraHBAOAOSettings;
        public static HBAO_Core.ColorBleedingSettings FPSCameraHBAOColorBleedingSettings;
        private static string scene;
        private static LevelSettings levelSettings;
        private static Vector3 defaulttoneValues;
        private static Vector3 defaultsecondaryToneValues;
        private static bool defaultuseLut;
        private static float defaultrampOffsetR;
        private static float defaultrampOffsetG;
        private static float defaultrampOffsetB;
        private static float defaultZeroLevel;
        private static bool defaultFPSCameraSharpen;
        private static bool defaultFPSCameraWeaponDepthOfField;
        private static float defaultFPSCameraWeaponDepthOfFieldAperture;
        private static float defaultFPSCameraWeaponDepthOfFieldFocalLength;
        private static float defaultFPSCameraWeaponDepthOfFieldFocalSize;
        private static float defaultFPSCameraWeaponDepthOfFieldMaxBlurSize;
        private static UnityStandardAssets.ImageEffects.DepthOfField.BlurSampleCount defaultFPSCameraWeaponDepthOfFieldBlurSampleCount;
        private static bool defaultFPSCameraCC_Vintage;
        private static bool defaultFPSCameraCustomGlobalFog;
        private static bool defaultFPSCameraGlobalFog;
        private static bool defaultFPSCameraColorCorrectionCurves;
        private static Color defaultSkyColor;
        private static Color defaultEquatorColor;
        private static Color defaultGroundColor;
        private static Color defaultNightVisionSkyColor;
        private static Color defaultNightVisionEquatorColor;
        private static Color defaultNightVisionGroundColor;
        public static HBAO_Core.AOSettings defaultFPSCameraHBAOAOSettings;
        public static HBAO_Core.ColorBleedingSettings defaultFPSCameraHBAOColorBleedingSettings;
        private static GradientColorKey[] gradientColorKeys = { };
        private static GradientColorKey[] defaultGradientColorKeys = { };
        private static bool defaultLightsUseLinearIntensity;

        private static Dictionary<string, string> sceneLevelSettings = new Dictionary<string, string>();

        public static bool NVG = false;

        public bool GraphicsMode = false;

        public void Start()
        {
            sceneLevelSettings.Add("City_Scripts", "---City_ levelsettings ---");
            sceneLevelSettings.Add("Laboratory_Scripts", "---Laboratory_levelsettings---");
            sceneLevelSettings.Add("custom_Light", "---Custom_levelsettings---");
            sceneLevelSettings.Add("Factory_Day", "---FactoryDay_levelsettings---");
            sceneLevelSettings.Add("Factory_Night", "---FactoryNight_levelsettings---");
            sceneLevelSettings.Add("Lighthouse_Abadonned_pier", "---Lighthouse_levelsettings---");
            sceneLevelSettings.Add("Shopping_Mall_Terrain", "---Interchange_levelsettings---");
            sceneLevelSettings.Add("woods_combined", "---Woods_levelsettings---");
            sceneLevelSettings.Add("Reserve_Base_DesignStuff", "---Reserve_levelsettings---");
            sceneLevelSettings.Add("shoreline_scripts", "---ShoreLine_levelsettings---");
            sceneLevelSettings.Add("default", "!settings");

            AmandsGraphicsPlugin.MotionBlur.SettingChanged += SettingsUpdated;
            AmandsGraphicsPlugin.MotionBlurSampleCount.SettingChanged += SettingsUpdated;
            AmandsGraphicsPlugin.MotionBlurShutterAngle.SettingChanged += SettingsUpdated;
            AmandsGraphicsPlugin.HBAO.SettingChanged += SettingsUpdated;
            AmandsGraphicsPlugin.HBAOIntensity.SettingChanged += SettingsUpdated;
            AmandsGraphicsPlugin.HBAOSaturation.SettingChanged += SettingsUpdated;
            AmandsGraphicsPlugin.HBAOAlbedoMultiplier.SettingChanged += SettingsUpdated;
            AmandsGraphicsPlugin.SurroundDepthOfField.SettingChanged += SettingsUpdated;
            AmandsGraphicsPlugin.SurroundDOFAperture.SettingChanged += SettingsUpdated;
            AmandsGraphicsPlugin.SurroundDOFKernelSize.SettingChanged += SettingsUpdated;
            AmandsGraphicsPlugin.WeaponDepthOfField.SettingChanged += SettingsUpdated;
            AmandsGraphicsPlugin.WeaponDOFAperture.SettingChanged += SettingsUpdated;
            AmandsGraphicsPlugin.WeaponDOFBlurSampleCount.SettingChanged += SettingsUpdated;
            AmandsGraphicsPlugin.OpticDepthOfField.SettingChanged += SettingsUpdated;
            AmandsGraphicsPlugin.OpticDOFAperture1x.SettingChanged += SettingsUpdated;
            AmandsGraphicsPlugin.OpticDOFAperture2x.SettingChanged += SettingsUpdated;
            AmandsGraphicsPlugin.OpticDOFAperture4x.SettingChanged += SettingsUpdated;
            AmandsGraphicsPlugin.OpticDOFAperture6x.SettingChanged += SettingsUpdated;
            AmandsGraphicsPlugin.OpticDOFKernelSize.SettingChanged += SettingsUpdated;

            AmandsGraphicsPlugin.Brightness.SettingChanged += SettingsUpdated;
            AmandsGraphicsPlugin.Tonemap.SettingChanged += SettingsUpdated;
            AmandsGraphicsPlugin.useLut.SettingChanged += SettingsUpdated;
            AmandsGraphicsPlugin.BloomIntensity.SettingChanged += SettingsUpdated;
            AmandsGraphicsPlugin.CC_Vintage.SettingChanged += SettingsUpdated;
            AmandsGraphicsPlugin.CC_Sharpen.SettingChanged += SettingsUpdated;
            AmandsGraphicsPlugin.CustomGlobalFog.SettingChanged += SettingsUpdated;
            AmandsGraphicsPlugin.CustomGlobalFogIntensity.SettingChanged += SettingsUpdated;
            AmandsGraphicsPlugin.GlobalFog.SettingChanged += SettingsUpdated;
            AmandsGraphicsPlugin.ColorCorrectionCurves.SettingChanged += SettingsUpdated;
            AmandsGraphicsPlugin.LightsUseLinearIntensity.SettingChanged += SettingsUpdated;
            AmandsGraphicsPlugin.SunColor.SettingChanged += SettingsUpdated;
            AmandsGraphicsPlugin.SkyColor.SettingChanged += SettingsUpdated;
            AmandsGraphicsPlugin.NVGColorsFix.SettingChanged += SettingsUpdated;

            AmandsGraphicsPlugin.StreetsFogLevel.SettingChanged += SettingsUpdated;
            AmandsGraphicsPlugin.CustomsFogLevel.SettingChanged += SettingsUpdated;
            AmandsGraphicsPlugin.LighthouseFogLevel.SettingChanged += SettingsUpdated;
            AmandsGraphicsPlugin.InterchangeFogLevel.SettingChanged += SettingsUpdated;
            AmandsGraphicsPlugin.WoodsFogLevel.SettingChanged += SettingsUpdated;
            AmandsGraphicsPlugin.ReserveFogLevel.SettingChanged += SettingsUpdated;
            AmandsGraphicsPlugin.ShorelineFogLevel.SettingChanged += SettingsUpdated;

            AmandsGraphicsPlugin.StreetsTonemap.SettingChanged += SettingsUpdated;
            AmandsGraphicsPlugin.LabsTonemap.SettingChanged += SettingsUpdated;
            AmandsGraphicsPlugin.CustomsTonemap.SettingChanged += SettingsUpdated;
            AmandsGraphicsPlugin.FactoryTonemap.SettingChanged += SettingsUpdated;
            AmandsGraphicsPlugin.FactoryNightTonemap.SettingChanged += SettingsUpdated;
            AmandsGraphicsPlugin.LighthouseTonemap.SettingChanged += SettingsUpdated;
            AmandsGraphicsPlugin.InterchangeTonemap.SettingChanged += SettingsUpdated;
            AmandsGraphicsPlugin.WoodsTonemap.SettingChanged += SettingsUpdated;
            AmandsGraphicsPlugin.ReserveTonemap.SettingChanged += SettingsUpdated;
            AmandsGraphicsPlugin.ShorelineTonemap.SettingChanged += SettingsUpdated;
            AmandsGraphicsPlugin.HideoutTonemap.SettingChanged += SettingsUpdated;

            AmandsGraphicsPlugin.StreetsACES.SettingChanged += SettingsUpdated;
            AmandsGraphicsPlugin.StreetsACESS.SettingChanged += SettingsUpdated;
            AmandsGraphicsPlugin.LabsACES.SettingChanged += SettingsUpdated;
            AmandsGraphicsPlugin.LabsACESS.SettingChanged += SettingsUpdated;
            AmandsGraphicsPlugin.CustomsACES.SettingChanged += SettingsUpdated;
            AmandsGraphicsPlugin.CustomsACESS.SettingChanged += SettingsUpdated;
            AmandsGraphicsPlugin.FactoryACES.SettingChanged += SettingsUpdated;
            AmandsGraphicsPlugin.FactoryACESS.SettingChanged += SettingsUpdated;
            AmandsGraphicsPlugin.FactoryNightACES.SettingChanged += SettingsUpdated;
            AmandsGraphicsPlugin.FactoryNightACESS.SettingChanged += SettingsUpdated;
            AmandsGraphicsPlugin.LighthouseACES.SettingChanged += SettingsUpdated;
            AmandsGraphicsPlugin.LighthouseACESS.SettingChanged += SettingsUpdated;
            AmandsGraphicsPlugin.InterchangeACES.SettingChanged += SettingsUpdated;
            AmandsGraphicsPlugin.InterchangeACESS.SettingChanged += SettingsUpdated;
            AmandsGraphicsPlugin.WoodsACES.SettingChanged += SettingsUpdated;
            AmandsGraphicsPlugin.WoodsACESS.SettingChanged += SettingsUpdated;
            AmandsGraphicsPlugin.ReserveACES.SettingChanged += SettingsUpdated;
            AmandsGraphicsPlugin.ReserveACESS.SettingChanged += SettingsUpdated;
            AmandsGraphicsPlugin.ShorelineACES.SettingChanged += SettingsUpdated;
            AmandsGraphicsPlugin.ShorelineACESS.SettingChanged += SettingsUpdated;
            AmandsGraphicsPlugin.HideoutACES.SettingChanged += SettingsUpdated;
            AmandsGraphicsPlugin.HideoutACESS.SettingChanged += SettingsUpdated;

            AmandsGraphicsPlugin.StreetsFilmic.SettingChanged += SettingsUpdated;
            AmandsGraphicsPlugin.StreetsFilmicS.SettingChanged += SettingsUpdated;
            AmandsGraphicsPlugin.LabsFilmic.SettingChanged += SettingsUpdated;
            AmandsGraphicsPlugin.LabsFilmicS.SettingChanged += SettingsUpdated;
            AmandsGraphicsPlugin.CustomsFilmic.SettingChanged += SettingsUpdated;
            AmandsGraphicsPlugin.CustomsFilmicS.SettingChanged += SettingsUpdated;
            AmandsGraphicsPlugin.FactoryFilmic.SettingChanged += SettingsUpdated;
            AmandsGraphicsPlugin.FactoryFilmicS.SettingChanged += SettingsUpdated;
            AmandsGraphicsPlugin.FactoryNightFilmic.SettingChanged += SettingsUpdated;
            AmandsGraphicsPlugin.FactoryNightFilmicS.SettingChanged += SettingsUpdated;
            AmandsGraphicsPlugin.LighthouseFilmic.SettingChanged += SettingsUpdated;
            AmandsGraphicsPlugin.LighthouseFilmicS.SettingChanged += SettingsUpdated;
            AmandsGraphicsPlugin.InterchangeFilmic.SettingChanged += SettingsUpdated;
            AmandsGraphicsPlugin.InterchangeFilmicS.SettingChanged += SettingsUpdated;
            AmandsGraphicsPlugin.WoodsFilmic.SettingChanged += SettingsUpdated;
            AmandsGraphicsPlugin.WoodsFilmicS.SettingChanged += SettingsUpdated;
            AmandsGraphicsPlugin.ReserveFilmic.SettingChanged += SettingsUpdated;
            AmandsGraphicsPlugin.ReserveFilmicS.SettingChanged += SettingsUpdated;
            AmandsGraphicsPlugin.ShorelineFilmic.SettingChanged += SettingsUpdated;
            AmandsGraphicsPlugin.ShorelineFilmicS.SettingChanged += SettingsUpdated;
            AmandsGraphicsPlugin.HideoutFilmic.SettingChanged += SettingsUpdated;
            AmandsGraphicsPlugin.HideoutFilmicS.SettingChanged += SettingsUpdated;

            AmandsGraphicsPlugin.FactorySkyColor.SettingChanged += SettingsUpdated;
            AmandsGraphicsPlugin.FactoryNVSkyColor.SettingChanged += SettingsUpdated;
            AmandsGraphicsPlugin.FactoryNightSkyColor.SettingChanged += SettingsUpdated;
            AmandsGraphicsPlugin.FactoryNightNVSkyColor.SettingChanged += SettingsUpdated;
            AmandsGraphicsPlugin.HideoutSkyColor.SettingChanged += SettingsUpdated;

            AmandsGraphicsPlugin.LightColorIndex0.SettingChanged += SettingsUpdated;
            AmandsGraphicsPlugin.LightColorIndex1.SettingChanged += SettingsUpdated;
            AmandsGraphicsPlugin.LightColorIndex2.SettingChanged += SettingsUpdated;
            AmandsGraphicsPlugin.LightColorIndex3.SettingChanged += SettingsUpdated;
            AmandsGraphicsPlugin.LightColorIndex4.SettingChanged += SettingsUpdated;
            AmandsGraphicsPlugin.LightColorIndex5.SettingChanged += SettingsUpdated;

            Camera.onPostRender += AmandsOnPostRender;

            ApertureAnimationCurve = new AnimationCurve(
                new Keyframe(1f, AmandsGraphicsPlugin.OpticDOFAperture1x.Value),
                new Keyframe(2f, AmandsGraphicsPlugin.OpticDOFAperture2x.Value),
                new Keyframe(4f, AmandsGraphicsPlugin.OpticDOFAperture4x.Value),
                new Keyframe(6f, AmandsGraphicsPlugin.OpticDOFAperture6x.Value));

            isLookingEnabled = Traverse.CreateWithType("Player").Property("IsLooking").PropertyExists();
        }
        public void Update()
        {
            if (Input.GetKeyDown(AmandsGraphicsPlugin.GraphicsToggle.Value.MainKey) && (!Input.GetKey(KeyCode.LeftShift)) && FPSCamera != null)
            {
                if (GraphicsMode)
                {
                    GraphicsMode = false;
                    ResetGraphics();
                }
                else
                {
                    GraphicsMode = true;
                    UpdateAmandsGraphics();
                }
            }
            if (Input.GetKeyDown(AmandsGraphicsPlugin.GraphicsToggle.Value.MainKey) && Input.GetKey(KeyCode.LeftShift) && FPSCamera != null && GraphicsMode)
            {
                switch (AmandsGraphicsPlugin.DebugMode.Value)
                {
                    case EDebugMode.HBAO:
                        AmandsGraphicsPlugin.HBAO.Value = !AmandsGraphicsPlugin.HBAO.Value;
                        break;
                    case EDebugMode.DefaultToACES:
                        switch (AmandsGraphicsPlugin.Tonemap.Value)
                        {
                            case EGlobalTonemap.Default:
                                AmandsGraphicsPlugin.Tonemap.Value = EGlobalTonemap.ACES;
                                break;
                            case EGlobalTonemap.ACES:
                                AmandsGraphicsPlugin.Tonemap.Value = EGlobalTonemap.Default;
                                break;
                        }
                        break;
                    case EDebugMode.DefaultToFilmic:
                        switch (AmandsGraphicsPlugin.Tonemap.Value)
                        {
                            case EGlobalTonemap.Default:
                                AmandsGraphicsPlugin.Tonemap.Value = EGlobalTonemap.Filmic;
                                break;
                            case EGlobalTonemap.Filmic:
                                AmandsGraphicsPlugin.Tonemap.Value = EGlobalTonemap.Default;
                                break;
                        }
                        break;
                    case EDebugMode.ACESToFilmic:
                        switch (AmandsGraphicsPlugin.Tonemap.Value)
                        {
                            case EGlobalTonemap.ACES:
                                AmandsGraphicsPlugin.Tonemap.Value = EGlobalTonemap.Filmic;
                                break;
                            case EGlobalTonemap.Filmic:
                                AmandsGraphicsPlugin.Tonemap.Value = EGlobalTonemap.ACES;
                                break;
                        }
                        break;
                    case EDebugMode.useLut:
                        AmandsGraphicsPlugin.useLut.Value = !AmandsGraphicsPlugin.useLut.Value;
                        break;
                    case EDebugMode.CC_Vintage:
                        AmandsGraphicsPlugin.CC_Vintage.Value = !AmandsGraphicsPlugin.CC_Vintage.Value;
                        break;
                    case EDebugMode.CC_Sharpen:
                        AmandsGraphicsPlugin.CC_Sharpen.Value = !AmandsGraphicsPlugin.CC_Sharpen.Value;
                        break;
                    case EDebugMode.CustomGlobalFog:
                        AmandsGraphicsPlugin.CustomGlobalFog.Value = !AmandsGraphicsPlugin.CustomGlobalFog.Value;
                        break;
                    case EDebugMode.GlobalFog:
                        AmandsGraphicsPlugin.GlobalFog.Value = !AmandsGraphicsPlugin.GlobalFog.Value;
                        break;
                    case EDebugMode.ColorCorrectionCurves:
                        AmandsGraphicsPlugin.ColorCorrectionCurves.Value = !AmandsGraphicsPlugin.ColorCorrectionCurves.Value;
                        break;
                    case EDebugMode.LightsUseLinearIntensity:
                        AmandsGraphicsPlugin.LightsUseLinearIntensity.Value = !AmandsGraphicsPlugin.LightsUseLinearIntensity.Value;
                        break;
                    case EDebugMode.SunColor:
                        AmandsGraphicsPlugin.SunColor.Value = !AmandsGraphicsPlugin.SunColor.Value;
                        break;
                    case EDebugMode.SkyColor:
                        AmandsGraphicsPlugin.SkyColor.Value = !AmandsGraphicsPlugin.SkyColor.Value;
                        break;
                    case EDebugMode.NVGColorsFix:
                        AmandsGraphicsPlugin.NVGColorsFix.Value = !AmandsGraphicsPlugin.NVGColorsFix.Value;
                        break;
                }
                UpdateAmandsGraphics();
            }
            if ((AmandsGraphicsPlugin.SurroundDepthOfField.Value == EDepthOfField.HoldingBreathOnly || AmandsGraphicsPlugin.OpticDepthOfField.Value == EDepthOfField.HoldingBreathOnly) && localPlayer != null)
            {
                HoldingBreath = Traverse.Create(Traverse.Create(localPlayer).Field("Physical").GetValue<object>()).Property("HoldingBreath").GetValue<bool>();
            }
            if (AmandsGraphicsPlugin.SurroundDepthOfField.Value != EDepthOfField.Off && FPSCameraDepthOfField != null)
            {
                switch (AmandsGraphicsPlugin.SurroundDepthOfField.Value)
                {
                    case EDepthOfField.On:
                        DepthOfFieldEnabled = (localPlayer != null && localPlayer.ProceduralWeaponAnimation != null && localPlayer.ProceduralWeaponAnimation.IsAiming && OpticCamera != null && OpticCamera.activeSelf && localPlayer.ProceduralWeaponAnimation.CurrentAimingMod != null &&
                            localPlayer.ProceduralWeaponAnimation.CurrentAimingMod.GetCurrentOpticZoom() > (AmandsGraphicsPlugin.SurroundDOFOpticZoom.Value - 0.1f) && sightComponent != null &&
                            sightComponent == localPlayer.ProceduralWeaponAnimation.CurrentAimingMod && localPlayer.ProceduralWeaponAnimation.CurrentAimingMod.SelectedScopeIndex == 0);
                        break;
                    case EDepthOfField.HoldingBreathOnly:
                        DepthOfFieldEnabled = (localPlayer != null && localPlayer.ProceduralWeaponAnimation != null && localPlayer.ProceduralWeaponAnimation.IsAiming && OpticCamera != null && OpticCamera.activeSelf && localPlayer.ProceduralWeaponAnimation.CurrentAimingMod != null &&
                            localPlayer.ProceduralWeaponAnimation.CurrentAimingMod.GetCurrentOpticZoom() > (AmandsGraphicsPlugin.SurroundDOFOpticZoom.Value - 0.1f) && sightComponent != null &&
                            sightComponent == localPlayer.ProceduralWeaponAnimation.CurrentAimingMod && localPlayer.ProceduralWeaponAnimation.CurrentAimingMod.SelectedScopeIndex == 0 && HoldingBreath);
                        break;
                }
                if (isLookingEnabled && DepthOfFieldEnabled && Traverse.Create(localPlayer).Property("IsLooking").GetValue<bool>()) DepthOfFieldEnabled = false;

                DepthOfFieldAnimation += ((DepthOfFieldEnabled ? 1f : 0f) - DepthOfFieldAnimation) * Time.deltaTime * AmandsGraphicsPlugin.SurroundDOFSpeed.Value;

                FPSCameraDepthOfField.focusDistance.value = DepthOfFieldFocusDistance;
                FPSCameraDepthOfField.focalLength.value = Mathf.Lerp(AmandsGraphicsPlugin.SurroundDOFFocalLengthOff.Value, AmandsGraphicsPlugin.SurroundDOFFocalLength.Value, DepthOfFieldAnimation);
                FPSCameraDepthOfField.enabled.value = DepthOfFieldAnimation > 0.01f;
            }
            if (AmandsGraphicsPlugin.OpticDepthOfField.Value != EDepthOfField.Off && OpticCameraDepthOfField != null)
            {
                switch (AmandsGraphicsPlugin.OpticDepthOfField.Value)
                {
                    case EDepthOfField.On:
                        OpticDOFEnabled = (localPlayer != null && localPlayer.ProceduralWeaponAnimation != null && localPlayer.ProceduralWeaponAnimation.CurrentAimingMod != null &&
                            localPlayer.ProceduralWeaponAnimation.CurrentAimingMod.GetCurrentOpticZoom() > (AmandsGraphicsPlugin.OpticDOFOpticZoom.Value - 0.1f) && (OpticCameraThermalVision != null ? !OpticCameraThermalVision.enabled : true) && OpticCamera != null);
                        break;
                    case EDepthOfField.HoldingBreathOnly:
                        OpticDOFEnabled = (localPlayer != null && localPlayer.ProceduralWeaponAnimation != null && localPlayer.ProceduralWeaponAnimation.CurrentAimingMod != null &&
                            localPlayer.ProceduralWeaponAnimation.CurrentAimingMod.GetCurrentOpticZoom() > (AmandsGraphicsPlugin.OpticDOFOpticZoom.Value - 0.1f) && (OpticCameraThermalVision != null ? !OpticCameraThermalVision.enabled : true) && OpticCamera != null && HoldingBreath);
                        break;
                }

                if (OpticDOFEnabled)
                {
                    switch (AmandsGraphicsPlugin.OpticDOFRaycastQuality.Value)
                    {
                        case ERaycastQuality.Low:
                            if (Physics.Raycast(OpticCamera.transform.position, OpticCamera.transform.forward, out hit, AmandsGraphicsPlugin.OpticDOFRaycastDistance.Value, LowLayerMask, QueryTriggerInteraction.Ignore))
                            {
                                OpticDOFFocusDistance = hit.distance;
                            }
                            break;
                        case ERaycastQuality.High:
                            if (Physics.Raycast(OpticCamera.transform.position, OpticCamera.transform.forward, out hit, AmandsGraphicsPlugin.OpticDOFRaycastDistance.Value, HighLayerMask, QueryTriggerInteraction.Ignore))
                            {
                                OpticDOFFocusDistance = hit.distance;
                                if (hit.distance > 2f && hit.collider.gameObject.layer == LayerMask.NameToLayer("HitCollider"))
                                {
                                    TargetCollider = hit.collider.transform;
                                    TargetLocal = TargetCollider.InverseTransformPoint(hit.point);
                                }
                            }
                            if (TargetCollider != null)
                            {
                                OpticDOFFocusDistance = Vector3.Distance(TargetCollider.TransformPoint(TargetLocal), OpticCamera.transform.position);
                                if ((Mathf.Abs(Mathf.Tan(Vector3.Angle(TargetCollider.TransformPoint(TargetLocal) - OpticCamera.transform.position, OpticCamera.transform.forward) * Mathf.Deg2Rad) * Vector3.Distance(TargetCollider.TransformPoint(TargetLocal), OpticCamera.transform.position)) > AmandsGraphicsPlugin.OpticDOFTargetDistance.Value / 100f) || Vector3.Distance(TargetCollider.TransformPoint(TargetLocal), OpticCamera.transform.position) < 1f) TargetCollider = null;
                            }
                            break;
                        case ERaycastQuality.Foliage:
                            if (Physics.Raycast(OpticCamera.transform.position, OpticCamera.transform.forward, out hit, AmandsGraphicsPlugin.OpticDOFRaycastDistance.Value, FoliageLayerMask, QueryTriggerInteraction.Collide))
                            {
                                OpticDOFFocusDistance = hit.distance;
                                if (hit.distance > 2f)
                                {
                                    switch (LayerMask.LayerToName(hit.collider.gameObject.layer))
                                    {
                                        case "HitCollider":
                                            TargetCollider = hit.collider.transform;
                                            TargetLocal = TargetCollider.InverseTransformPoint(hit.point);
                                            break;
                                        case "Foliage":
                                            if (Physics.Raycast(hit.point, OpticCamera.transform.forward, out foliageHit, AmandsGraphicsPlugin.OpticDOFRaycastDistance.Value - hit.distance, HighLayerMask, QueryTriggerInteraction.Ignore))
                                            {
                                                if (foliageHit.collider.gameObject.layer == LayerMask.NameToLayer("HitCollider"))
                                                {
                                                    TargetCollider = foliageHit.collider.transform;
                                                    TargetLocal = TargetCollider.InverseTransformPoint(foliageHit.point);
                                                }
                                            }
                                            break;
                                    }
                                }
                            }
                            if (TargetCollider != null)
                            {
                                OpticDOFFocusDistance = Vector3.Distance(TargetCollider.TransformPoint(TargetLocal), OpticCamera.transform.position);
                                if ((Mathf.Abs(Mathf.Tan(Vector3.Angle(TargetCollider.TransformPoint(TargetLocal) - OpticCamera.transform.position, OpticCamera.transform.forward) * Mathf.Deg2Rad) * Vector3.Distance(TargetCollider.TransformPoint(TargetLocal), OpticCamera.transform.position)) > AmandsGraphicsPlugin.OpticDOFTargetDistance.Value / 100f) || Vector3.Distance(TargetCollider.TransformPoint(TargetLocal), OpticCamera.transform.position) < 1f) TargetCollider = null;
                            }
                            break;
                    }
                    OpticCameraDepthOfField.aperture.value = ApertureAnimationCurve.Evaluate(localPlayer.ProceduralWeaponAnimation.CurrentAimingMod.GetCurrentOpticZoom());
                }

                OpticDOFAnimation += ((OpticDOFEnabled ? 1f : 0f) - OpticDOFAnimation) * Time.deltaTime * AmandsGraphicsPlugin.OpticDOFSpeed.Value;
                OpticDOFFocusDistanceAnimation += (OpticDOFFocusDistance - OpticDOFFocusDistanceAnimation) * Time.deltaTime * AmandsGraphicsPlugin.OpticDOFDistanceSpeed.Value;

                OpticCameraDepthOfField.focusDistance.value = OpticDOFFocusDistanceAnimation; //Mathf.Max(OpticCameraDepthOfField.focusDistance.value + ((OpticDepthOfFieldFocusDistance - OpticCameraDepthOfField.focusDistance.value) * Time.deltaTime * AmandsGraphicsPlugin.OpticDepthOfFieldSpeed.Value), 0.01f);
                switch (AmandsGraphicsPlugin.OpticDOFFocalLengthMode.Value)
                {
                    case EOpticDOFFocalLengthMode.Math:
                        OpticCameraDepthOfField.focalLength.value = Mathf.Lerp(0f, Mathf.Sqrt(OpticDOFFocusDistance + 10f) * AmandsGraphicsPlugin.OpticDOFFocalLength.Value, OpticDOFAnimation);
                        break;
                    case EOpticDOFFocalLengthMode.FixedValue:
                        OpticCameraDepthOfField.focalLength.value = AmandsGraphicsPlugin.OpticDOFFocalLength.Value;
                        break;
                }
                OpticCameraDepthOfField.enabled.value = OpticDOFAnimation > 0.01f;
            }
            if (AmandsGraphicsPlugin.WeaponDepthOfField.Value != EWeaponDepthOfField.Off && FPSCameraWeaponDepthOfField != null)
            {
                weaponDepthOfFieldState = EWeaponDepthOfFieldState.Weapon;
                if (localPlayer != null && localPlayer.ProceduralWeaponAnimation != null && localPlayer.ProceduralWeaponAnimation.IsAiming && !DepthOfFieldEnabled)
                {
                    weaponDepthOfFieldState = EWeaponDepthOfFieldState.Aiming;
                    if (OpticCamera != null && OpticCamera.activeSelf && localPlayer.ProceduralWeaponAnimation.CurrentAimingMod != null && sightComponent != null && sightComponent == localPlayer.ProceduralWeaponAnimation.CurrentAimingMod && localPlayer.ProceduralWeaponAnimation.CurrentAimingMod.SelectedScopeIndex == 0)
                    {
                        weaponDepthOfFieldState = EWeaponDepthOfFieldState.Optic;
                    }
                }
                else if (DepthOfFieldEnabled)
                {
                    weaponDepthOfFieldState = EWeaponDepthOfFieldState.Off;
                }
                switch (weaponDepthOfFieldState)
                {
                    case EWeaponDepthOfFieldState.Off:
                        WeaponDepthOfFieldFocalLength += (0.0001f - WeaponDepthOfFieldFocalLength) * Time.deltaTime * AmandsGraphicsPlugin.WeaponDOFSpeed.Value * 2f;
                        WeaponDepthOfFieldMaxBlurSize += (0.001f - WeaponDepthOfFieldMaxBlurSize) * Time.deltaTime * AmandsGraphicsPlugin.WeaponDOFSpeed.Value * 2f;
                        break;
                    case EWeaponDepthOfFieldState.Weapon:
                        WeaponDepthOfFieldFocalLength += ((AmandsGraphicsPlugin.WeaponDOFWeaponFocalLength.Value / 100f) - WeaponDepthOfFieldFocalLength) * Time.deltaTime * AmandsGraphicsPlugin.WeaponDOFSpeed.Value;
                        WeaponDepthOfFieldMaxBlurSize += ((AmandsGraphicsPlugin.WeaponDOFWeaponMaxBlurSize.Value / 10f) - WeaponDepthOfFieldMaxBlurSize) * Time.deltaTime * AmandsGraphicsPlugin.WeaponDOFSpeed.Value;
                        break;
                    case EWeaponDepthOfFieldState.Aiming:
                        WeaponDepthOfFieldFocalLength += ((AmandsGraphicsPlugin.WeaponDOFAimingFocalLength.Value / 100f) - WeaponDepthOfFieldFocalLength) * Time.deltaTime * AmandsGraphicsPlugin.WeaponDOFSpeed.Value;
                        WeaponDepthOfFieldMaxBlurSize += ((AmandsGraphicsPlugin.WeaponDOFAimingMaxBlurSize.Value / 10f) - WeaponDepthOfFieldMaxBlurSize) * Time.deltaTime * AmandsGraphicsPlugin.WeaponDOFSpeed.Value;
                        break;
                    case EWeaponDepthOfFieldState.Optic:
                        WeaponDepthOfFieldFocalLength += ((AmandsGraphicsPlugin.WeaponDOFOpticFocalLength.Value / 100f) - WeaponDepthOfFieldFocalLength) * Time.deltaTime * AmandsGraphicsPlugin.WeaponDOFSpeed.Value;
                        WeaponDepthOfFieldMaxBlurSize += ((AmandsGraphicsPlugin.WeaponDOFOpticMaxBlurSize.Value / 10f) - WeaponDepthOfFieldMaxBlurSize) * Time.deltaTime * AmandsGraphicsPlugin.WeaponDOFSpeed.Value;
                        break;
                }
                FPSCameraWeaponDepthOfField.focalLength = WeaponDepthOfFieldFocalLength * 100f;
                FPSCameraWeaponDepthOfField.maxBlurSize = WeaponDepthOfFieldMaxBlurSize * 10f;
            }
        }
        public void AmandsOnPostRender(Camera cam)
        {
            if (AmandsGraphicsPlugin.SurroundDepthOfField.Value != EDepthOfField.Off && backLens != null)
            {
                DepthOfFieldFocusDistance = Vector3.Distance(Camera.current.transform.position, backLens.position);
            }
        }
        public void ActivateAmandsGraphics(GameObject fpscamera, PrismEffects prismeffects)
        {
            if (FPSCamera == null)
            {
                FPSCamera = fpscamera;
                if (FPSCamera != null)
                {
                    defaultLightsUseLinearIntensity = GraphicsSettings.lightsUseLinearIntensity;
                    if (FPSCameraCamera == null)
                    {
                        FPSCameraCamera = FPSCamera.GetComponent<Camera>();
                    }
                    FPSCameraPostProcessVolume = FPSCamera.GetComponent<PostProcessVolume>();
                    if (FPSCameraPostProcessVolume != null)
                    {
                        FPSCameraPostProcessVolume.profile.TryGetSettings<UnityEngine.Rendering.PostProcessing.MotionBlur>(out FPSCameraMotionBlur);
                        if (FPSCameraMotionBlur == null)
                        {
                            FPSCameraPostProcessVolume.profile.AddSettings<UnityEngine.Rendering.PostProcessing.MotionBlur>();
                            FPSCameraPostProcessVolume.profile.TryGetSettings<UnityEngine.Rendering.PostProcessing.MotionBlur>(out FPSCameraMotionBlur);
                        }
                    }
                    FPSCameraPostProcessLayer = FPSCamera.GetComponent<PostProcessLayer>();
                    if (FPSCameraPostProcessLayer != null)
                    {
                        FPSCameraDepthOfField = Traverse.Create(FPSCameraPostProcessLayer).Field("m_Bundles").GetValue<Dictionary<Type, PostProcessBundle>>()[typeof(UnityEngine.Rendering.PostProcessing.DepthOfField)].settings as UnityEngine.Rendering.PostProcessing.DepthOfField;
                        FPSCameraDepthOfField.enabled.value = false;
                    }
                    FPSCameraWeaponDepthOfField = FPSCamera.GetComponent<UnityStandardAssets.ImageEffects.DepthOfField>();
                    if (FPSCameraWeaponDepthOfField != null)
                    {
                        defaultFPSCameraWeaponDepthOfField = FPSCameraWeaponDepthOfField.enabled;
                        defaultFPSCameraWeaponDepthOfFieldAperture = FPSCameraWeaponDepthOfField.aperture;
                        defaultFPSCameraWeaponDepthOfFieldFocalLength = FPSCameraWeaponDepthOfField.focalLength;
                        defaultFPSCameraWeaponDepthOfFieldFocalSize = FPSCameraWeaponDepthOfField.focalSize;
                        defaultFPSCameraWeaponDepthOfFieldMaxBlurSize = FPSCameraWeaponDepthOfField.maxBlurSize;
                        defaultFPSCameraWeaponDepthOfFieldBlurSampleCount = FPSCameraWeaponDepthOfField.blurSampleCount;
                    }
                    FPSCameraCC_Sharpen = FPSCamera.GetComponent<CC_Sharpen>();
                    if (FPSCameraCC_Sharpen != null)
                    {
                        defaultFPSCameraSharpen = FPSCameraCC_Sharpen.enabled;
                        defaultrampOffsetR = FPSCameraCC_Sharpen.rampOffsetR;
                        defaultrampOffsetG = FPSCameraCC_Sharpen.rampOffsetG;
                        defaultrampOffsetB = FPSCameraCC_Sharpen.rampOffsetB;
                    }
                    if (prismeffects != null)
                    {
                        FPSCameraPrismEffects = prismeffects;
                    }
                    if (FPSCameraPrismEffects != null)
                    {
                        defaulttoneValues = FPSCameraPrismEffects.toneValues;
                        defaultsecondaryToneValues = FPSCameraPrismEffects.secondaryToneValues;
                        defaultuseLut = FPSCameraPrismEffects.useLut;
                    }
                    FPSCameraBloomAndFlares.Clear();
                    foreach (BloomAndFlares bloomAndFlares in FPSCamera.GetComponents<BloomAndFlares>())
                    {
                        FPSCameraBloomAndFlares.Add(bloomAndFlares, bloomAndFlares.bloomIntensity);
                    }
                    scene = SceneManager.GetActiveScene().name;
                    if (!sceneLevelSettings.ContainsKey(scene)) scene = "default";
                    levelSettings = GameObject.Find(sceneLevelSettings[scene]).GetComponent<LevelSettings>();
                    if (levelSettings != null)
                    {
                        defaultZeroLevel = levelSettings.ZeroLevel;
                        defaultSkyColor = levelSettings.SkyColor;
                        defaultEquatorColor = levelSettings.EquatorColor;
                        defaultGroundColor = levelSettings.GroundColor;
                        defaultNightVisionSkyColor = levelSettings.NightVisionSkyColor;
                        defaultNightVisionEquatorColor = levelSettings.NightVisionEquatorColor;
                        defaultNightVisionGroundColor = levelSettings.NightVisionGroundColor;
                    }
                    FPSCameraCC_Vintage = FPSCamera.GetComponent<CC_Vintage>();
                    if (FPSCameraCC_Vintage != null)
                    {
                        defaultFPSCameraCC_Vintage = FPSCameraCC_Vintage.enabled;
                    }
                    FPSCameraCustomGlobalFog = FPSCamera.GetComponent<CustomGlobalFog>();
                    if (FPSCameraCustomGlobalFog != null)
                    {
                        defaultFPSCameraCustomGlobalFog = FPSCameraCustomGlobalFog.enabled;
                    }
                    foreach (Component component in FPSCamera.GetComponents<Component>())
                    {
                        if (component.ToString() == "FPS Camera (UnityStandardAssets.ImageEffects.GlobalFog)")
                        {
                            FPSCameraGlobalFog = component as Behaviour;
                            defaultFPSCameraGlobalFog = FPSCameraGlobalFog.enabled;
                            break;
                        }
                    }
                    FPSCameraColorCorrectionCurves = FPSCamera.GetComponent<ColorCorrectionCurves>();
                    if (FPSCameraColorCorrectionCurves != null)
                    {
                        defaultFPSCameraColorCorrectionCurves = FPSCameraColorCorrectionCurves.enabled;
                    }
                    Weather = GameObject.Find("Weather");
                    if (Weather != null)
                    {
                        weatherController = Weather.GetComponent<WeatherController>();
                        if (weatherController != null && weatherController.TimeOfDayController != null)
                        {
                            defaultGradientColorKeys = weatherController.TimeOfDayController.LightColor.colorKeys;
                        }
                    }
                    FPSCameraNightVision = FPSCamera.GetComponent<NightVision>();
                    if (FPSCameraNightVision != null)
                    {
                        NVG = FPSCameraNightVision.On;
                    }
                    FPSCameraHBAO = FPSCamera.GetComponent<HBAO>();
                    if (FPSCameraHBAO != null)
                    {
                        defaultFPSCameraHBAOAOSettings = FPSCameraHBAO.aoSettings;
                        defaultFPSCameraHBAOColorBleedingSettings = FPSCameraHBAO.colorBleedingSettings;
                        FPSCameraHBAOAOSettings = FPSCameraHBAO.aoSettings;
                        FPSCameraHBAOColorBleedingSettings = FPSCameraHBAO.colorBleedingSettings;
                    }
                    GraphicsMode = true;
                    UpdateAmandsGraphics();
                }
            }
            if (localPlayer == null)
            {
                localPlayer = FindObjectOfType<LocalPlayer>();
            }
        }
        public void ActivateAmandsOpticDepthOfField(GameObject baseopticcamera)
        {
            if (OpticCamera == null)
            {
                OpticCamera = baseopticcamera;
                if (OpticCamera != null)
                {
                    OpticCameraPostProcessLayer = OpticCamera.GetComponent<PostProcessLayer>();
                    if (OpticCameraPostProcessLayer != null)
                    {
                        OpticCameraDepthOfField = Traverse.Create(OpticCameraPostProcessLayer).Field("m_Bundles").GetValue<Dictionary<Type, PostProcessBundle>>()[typeof(UnityEngine.Rendering.PostProcessing.DepthOfField)].settings as UnityEngine.Rendering.PostProcessing.DepthOfField;
                        if (OpticCameraDepthOfField != null)
                        {
                            OpticCameraDepthOfField.enabled.value = AmandsGraphicsPlugin.OpticDepthOfField.Value != EDepthOfField.Off;
                            OpticCameraDepthOfField.kernelSize.value = AmandsGraphicsPlugin.OpticDOFKernelSize.Value;
                        }
                    }
                }
            }
        }
        public void UpdateAmandsGraphics()
        {
            GraphicsSettings.lightsUseLinearIntensity = AmandsGraphicsPlugin.LightsUseLinearIntensity.Value;
            if (FPSCameraMotionBlur != null)
            {
                FPSCameraMotionBlur.enabled.Override(AmandsGraphicsPlugin.MotionBlur.Value);
                FPSCameraMotionBlur.sampleCount.Override(AmandsGraphicsPlugin.MotionBlurSampleCount.Value);
                FPSCameraMotionBlur.shutterAngle.Override(AmandsGraphicsPlugin.MotionBlurShutterAngle.Value);
            }
            HoldingBreath = false;
            if (FPSCameraDepthOfField != null)
            {
                DepthOfFieldEnabled = false;
                FPSCameraDepthOfField.enabled.value = false;
                FPSCameraDepthOfField.aperture.value = AmandsGraphicsPlugin.SurroundDOFAperture.Value;
                FPSCameraDepthOfField.kernelSize.value = AmandsGraphicsPlugin.SurroundDOFKernelSize.Value;
            }
            if (FPSCameraWeaponDepthOfField != null)
            {
                FPSCameraWeaponDepthOfField.enabled = AmandsGraphicsPlugin.WeaponDepthOfField.Value != EWeaponDepthOfField.Off;
                FPSCameraWeaponDepthOfField.aperture = AmandsGraphicsPlugin.WeaponDOFAperture.Value;
                FPSCameraWeaponDepthOfField.focalSize = 100f;
                FPSCameraWeaponDepthOfField.blurSampleCount = AmandsGraphicsPlugin.WeaponDOFBlurSampleCount.Value;
            }
            ApertureAnimationCurve = new AnimationCurve(
                new Keyframe(1f, AmandsGraphicsPlugin.OpticDOFAperture1x.Value),
                new Keyframe(2f, AmandsGraphicsPlugin.OpticDOFAperture2x.Value),
                new Keyframe(4f, AmandsGraphicsPlugin.OpticDOFAperture4x.Value),
                new Keyframe(6f, AmandsGraphicsPlugin.OpticDOFAperture6x.Value));
            if (OpticCameraDepthOfField != null)
            {
                OpticCameraDepthOfField.enabled.value = AmandsGraphicsPlugin.OpticDepthOfField.Value != EDepthOfField.Off;
                OpticCameraDepthOfField.kernelSize.value = AmandsGraphicsPlugin.OpticDOFKernelSize.Value;
            }
            if (NVG)
            {
                ResetGraphics();
                if (FPSCameraCustomGlobalFog != null)
                {
                    if (AmandsGraphicsPlugin.CustomGlobalFog.Value)
                    {
                        FPSCameraCustomGlobalFog.enabled = defaultFPSCameraCustomGlobalFog;
                        FPSCameraCustomGlobalFog.FuncStart = 1f;
                        FPSCameraCustomGlobalFog.BlendMode = CustomGlobalFog.BlendModes.Lighten;
                    }
                    else
                    {
                        FPSCameraCustomGlobalFog.enabled = (scene == "Factory_Day" || scene == "Factory_Night" || scene == "default") ? false : defaultFPSCameraCustomGlobalFog;
                        FPSCameraCustomGlobalFog.FuncStart = AmandsGraphicsPlugin.CustomGlobalFogIntensity.Value;
                        FPSCameraCustomGlobalFog.BlendMode = CustomGlobalFog.BlendModes.Normal;
                    }
                }
                return;
            }
            if (FPSCameraHBAO != null)
            {
                if (AmandsGraphicsPlugin.HBAO.Value)
                {
                    FPSCameraHBAOAOSettings.intensity = AmandsGraphicsPlugin.HBAOIntensity.Value;
                    FPSCameraHBAOColorBleedingSettings.saturation = AmandsGraphicsPlugin.HBAOSaturation.Value;
                    FPSCameraHBAOColorBleedingSettings.albedoMultiplier = AmandsGraphicsPlugin.HBAOAlbedoMultiplier.Value;
                    FPSCameraHBAO.aoSettings = FPSCameraHBAOAOSettings;
                    FPSCameraHBAO.colorBleedingSettings = FPSCameraHBAOColorBleedingSettings;
                }
                else
                {
                    FPSCameraHBAO.aoSettings = defaultFPSCameraHBAOAOSettings;
                    FPSCameraHBAO.colorBleedingSettings = defaultFPSCameraHBAOColorBleedingSettings;
                }
            }
            if (FPSCameraPrismEffects != null)
            {
                switch (AmandsGraphicsPlugin.Tonemap.Value)
                {
                    case EGlobalTonemap.Default:
                        DefaultTonemap();
                        break;
                    case EGlobalTonemap.ACES:
                        ACESTonemap();
                        break;
                    case EGlobalTonemap.Filmic:
                        FilmicTonemap();
                        break;
                    case EGlobalTonemap.PerMap:
                        PerMapTonemap();
                        break;
                }
                FPSCameraPrismEffects.useLut = AmandsGraphicsPlugin.useLut.Value ? defaultuseLut : false;
            }
            foreach (var BloomAndFlares in FPSCameraBloomAndFlares)
            {
                BloomAndFlares.Key.bloomIntensity = BloomAndFlares.Value * AmandsGraphicsPlugin.BloomIntensity.Value;
            }
            if (Weather != null && weatherController != null && weatherController.TimeOfDayController != null)
            {
                if (AmandsGraphicsPlugin.SunColor.Value)
                {
                    gradientColorKeys = weatherController.TimeOfDayController.LightColor.colorKeys;
                    gradientColorKeys[0] = new GradientColorKey(AmandsGraphicsPlugin.LightColorIndex0.Value, 0.0f);
                    gradientColorKeys[1] = new GradientColorKey(AmandsGraphicsPlugin.LightColorIndex1.Value, 0.5115129f);
                    gradientColorKeys[2] = new GradientColorKey(AmandsGraphicsPlugin.LightColorIndex2.Value, 0.5266652f);
                    gradientColorKeys[3] = new GradientColorKey(AmandsGraphicsPlugin.LightColorIndex3.Value, 0.5535668f);
                    gradientColorKeys[4] = new GradientColorKey(AmandsGraphicsPlugin.LightColorIndex4.Value, 0.6971694f);
                    gradientColorKeys[5] = new GradientColorKey(AmandsGraphicsPlugin.LightColorIndex5.Value, 0.9992523f);
                    weatherController.TimeOfDayController.LightColor.colorKeys = gradientColorKeys;
                }
                else
                {
                    weatherController.TimeOfDayController.LightColor.colorKeys = defaultGradientColorKeys;
                }
            }
            if (levelSettings != null)
            {
                if (AmandsGraphicsPlugin.SkyColor.Value)
                {
                    switch (scene)
                    {
                        case "City_Scripts":
                            levelSettings.ZeroLevel = defaultZeroLevel + AmandsGraphicsPlugin.StreetsFogLevel.Value;
                            break;
                        case "Laboratory_Scripts":
                            break;
                        case "custom_Light":
                            levelSettings.ZeroLevel = defaultZeroLevel + AmandsGraphicsPlugin.CustomsFogLevel.Value;
                            break;
                        case "Lighthouse_Abadonned_pier":
                            levelSettings.ZeroLevel = defaultZeroLevel + AmandsGraphicsPlugin.LighthouseFogLevel.Value;
                            break;
                        case "Shopping_Mall_Terrain":
                            levelSettings.ZeroLevel = defaultZeroLevel + AmandsGraphicsPlugin.InterchangeFogLevel.Value;
                            break;
                        case "woods_combined":
                            levelSettings.ZeroLevel = defaultZeroLevel + AmandsGraphicsPlugin.WoodsFogLevel.Value;
                            break;
                        case "Reserve_Base_DesignStuff":
                            levelSettings.ZeroLevel = defaultZeroLevel + AmandsGraphicsPlugin.ReserveFogLevel.Value;
                            break;
                        case "shoreline_scripts":
                            levelSettings.ZeroLevel = defaultZeroLevel + AmandsGraphicsPlugin.ShorelineFogLevel.Value;
                            break;
                        case "Factory_Day":
                            levelSettings.SkyColor = AmandsGraphicsPlugin.FactorySkyColor.Value / 10;
                            levelSettings.EquatorColor = AmandsGraphicsPlugin.FactorySkyColor.Value / 10;
                            levelSettings.GroundColor = AmandsGraphicsPlugin.FactorySkyColor.Value / 10;
                            levelSettings.NightVisionSkyColor = AmandsGraphicsPlugin.FactoryNVSkyColor.Value / 10;
                            levelSettings.NightVisionEquatorColor = AmandsGraphicsPlugin.FactoryNVSkyColor.Value / 10;
                            levelSettings.NightVisionGroundColor = AmandsGraphicsPlugin.FactoryNVSkyColor.Value / 10;
                            break;
                        case "Factory_Night":
                            levelSettings.SkyColor = AmandsGraphicsPlugin.FactoryNightSkyColor.Value / 10;
                            levelSettings.EquatorColor = AmandsGraphicsPlugin.FactoryNightSkyColor.Value / 10;
                            levelSettings.GroundColor = AmandsGraphicsPlugin.FactoryNightSkyColor.Value / 10;
                            levelSettings.NightVisionSkyColor = AmandsGraphicsPlugin.FactoryNightNVSkyColor.Value / 10;
                            levelSettings.NightVisionEquatorColor = AmandsGraphicsPlugin.FactoryNightNVSkyColor.Value / 10;
                            levelSettings.NightVisionGroundColor = AmandsGraphicsPlugin.FactoryNightNVSkyColor.Value / 10;
                            break;
                        default:
                            levelSettings.SkyColor = AmandsGraphicsPlugin.HideoutSkyColor.Value / 10;
                            levelSettings.EquatorColor = AmandsGraphicsPlugin.HideoutSkyColor.Value / 10;
                            levelSettings.GroundColor = AmandsGraphicsPlugin.HideoutSkyColor.Value / 10;
                            levelSettings.NightVisionSkyColor = AmandsGraphicsPlugin.HideoutSkyColor.Value / 10;
                            levelSettings.NightVisionEquatorColor = AmandsGraphicsPlugin.HideoutSkyColor.Value / 10;
                            levelSettings.NightVisionGroundColor = AmandsGraphicsPlugin.HideoutSkyColor.Value / 10;
                            break;
                    }
                }
                else
                {
                    switch (scene)
                    {
                        case "City_Scripts":
                            levelSettings.ZeroLevel = defaultZeroLevel + AmandsGraphicsPlugin.StreetsFogLevel.Value;
                            break;
                        case "Laboratory_Scripts":
                            break;
                        case "custom_Light":
                            levelSettings.ZeroLevel = defaultZeroLevel + AmandsGraphicsPlugin.CustomsFogLevel.Value;
                            break;
                        case "Lighthouse_Abadonned_pier":
                            levelSettings.ZeroLevel = defaultZeroLevel + AmandsGraphicsPlugin.LighthouseFogLevel.Value;
                            break;
                        case "Shopping_Mall_Terrain":
                            levelSettings.ZeroLevel = defaultZeroLevel + AmandsGraphicsPlugin.InterchangeFogLevel.Value;
                            break;
                        case "woods_combined":
                            levelSettings.ZeroLevel = defaultZeroLevel + AmandsGraphicsPlugin.WoodsFogLevel.Value;
                            break;
                        case "Reserve_Base_DesignStuff":
                            levelSettings.ZeroLevel = defaultZeroLevel + AmandsGraphicsPlugin.ReserveFogLevel.Value;
                            break;
                        case "shoreline_scripts":
                            levelSettings.ZeroLevel = defaultZeroLevel + AmandsGraphicsPlugin.ShorelineFogLevel.Value;
                            break;
                    }
                    levelSettings.SkyColor = defaultSkyColor;
                    levelSettings.EquatorColor = defaultEquatorColor;
                    levelSettings.GroundColor = defaultGroundColor;
                    levelSettings.NightVisionSkyColor = defaultNightVisionSkyColor;
                    levelSettings.NightVisionEquatorColor = defaultNightVisionEquatorColor;
                    levelSettings.NightVisionGroundColor = defaultNightVisionGroundColor;
                }
            }
            if (FPSCameraCC_Vintage != null)
            {
                FPSCameraCC_Vintage.enabled = AmandsGraphicsPlugin.CC_Vintage.Value;
            }
            if (FPSCameraCC_Sharpen != null)
            {
                if (AmandsGraphicsPlugin.CC_Sharpen.Value)
                {
                    FPSCameraCC_Sharpen.enabled = defaultFPSCameraSharpen;
                    FPSCameraCC_Sharpen.rampOffsetR = defaultrampOffsetR;
                    FPSCameraCC_Sharpen.rampOffsetG = defaultrampOffsetG;
                    FPSCameraCC_Sharpen.rampOffsetB = defaultrampOffsetB;
                }
                else
                {
                    FPSCameraCC_Sharpen.enabled = true;
                    FPSCameraCC_Sharpen.rampOffsetR = 0f;
                    FPSCameraCC_Sharpen.rampOffsetG = 0f;
                    FPSCameraCC_Sharpen.rampOffsetB = 0f;
                }
            }
            if (FPSCameraCustomGlobalFog != null)
            {
                if (AmandsGraphicsPlugin.CustomGlobalFog.Value)
                {
                    FPSCameraCustomGlobalFog.enabled = defaultFPSCameraCustomGlobalFog;
                    FPSCameraCustomGlobalFog.FuncStart = 1f;
                    FPSCameraCustomGlobalFog.BlendMode = CustomGlobalFog.BlendModes.Lighten;
                }
                else
                {
                    FPSCameraCustomGlobalFog.enabled = (scene == "Factory_Day" || scene == "Factory_Night" || scene == "default") ? false : defaultFPSCameraCustomGlobalFog;
                    FPSCameraCustomGlobalFog.FuncStart = AmandsGraphicsPlugin.CustomGlobalFogIntensity.Value;
                    FPSCameraCustomGlobalFog.BlendMode = CustomGlobalFog.BlendModes.Normal;
                }
            }
            if (FPSCameraGlobalFog != null)
            {
                FPSCameraGlobalFog.enabled = AmandsGraphicsPlugin.GlobalFog.Value;
            }
            if (FPSCameraColorCorrectionCurves != null)
            {
                FPSCameraColorCorrectionCurves.enabled = AmandsGraphicsPlugin.ColorCorrectionCurves.Value;
            }
            // NVG FIX
            //if (AmandsGraphicsPlugin.NVGColorsFix.Value && NVG)
            //{
            //    if (FPSCameraPrismEffects != null)
            //    {
            //        FPSCameraPrismEffects.useLut = defaultuseLut;
            //    }
            //    if (levelSettings != null)
            //    {
            //        levelSettings.ZeroLevel = defaultZeroLevel;
            //    }
            //    if (FPSCameraCC_Vintage != null)
            //    {
            //        FPSCameraCC_Vintage.enabled = defaultFPSCameraCC_Vintage;
            //    }
            //    if (FPSCameraCC_Sharpen != null)
            //    {
            //        FPSCameraCC_Sharpen.enabled = defaultFPSCameraSharpen;
            //        FPSCameraCC_Sharpen.rampOffsetR = defaultrampOffsetR;
            //        FPSCameraCC_Sharpen.rampOffsetG = defaultrampOffsetG;
            //        FPSCameraCC_Sharpen.rampOffsetB = defaultrampOffsetB;
            //    }
            //    if (FPSCameraColorCorrectionCurves != null)
            //    {
            //        FPSCameraColorCorrectionCurves.enabled = defaultFPSCameraColorCorrectionCurves;
            //    }
            //}
        }
        private void ResetGraphics()
        {
            GraphicsSettings.lightsUseLinearIntensity = defaultLightsUseLinearIntensity;
            if (FPSCameraHBAO != null)
            {
                FPSCameraHBAO.aoSettings = defaultFPSCameraHBAOAOSettings;
                FPSCameraHBAO.colorBleedingSettings = defaultFPSCameraHBAOColorBleedingSettings;
            }
            if (FPSCameraWeaponDepthOfField != null)
            {
                FPSCameraWeaponDepthOfField.enabled = WeaponDepthOfFieldEnabled;
                FPSCameraWeaponDepthOfField.aperture = defaultFPSCameraWeaponDepthOfFieldAperture;
                FPSCameraWeaponDepthOfField.focalLength = defaultFPSCameraWeaponDepthOfFieldFocalLength;
                FPSCameraWeaponDepthOfField.focalSize = defaultFPSCameraWeaponDepthOfFieldFocalSize;
                FPSCameraWeaponDepthOfField.maxBlurSize = defaultFPSCameraWeaponDepthOfFieldMaxBlurSize;
                FPSCameraWeaponDepthOfField.blurSampleCount = defaultFPSCameraWeaponDepthOfFieldBlurSampleCount;
            }
            if (FPSCameraPrismEffects != null)
            {
                FPSCameraPrismEffects.tonemapType = Prism.Utils.TonemapType.Filmic;
                FPSCameraPrismEffects.toneValues = defaulttoneValues;
                FPSCameraPrismEffects.secondaryToneValues = defaultsecondaryToneValues;
                FPSCameraPrismEffects.useLut = defaultuseLut;
            }
            if (levelSettings != null)
            {
                levelSettings.ZeroLevel = defaultZeroLevel;
                levelSettings.SkyColor = defaultSkyColor;
                levelSettings.EquatorColor = defaultEquatorColor;
                levelSettings.GroundColor = defaultGroundColor;
                levelSettings.NightVisionSkyColor = defaultNightVisionSkyColor;
                levelSettings.NightVisionEquatorColor = defaultNightVisionEquatorColor;
                levelSettings.NightVisionGroundColor = defaultNightVisionGroundColor;
            }
            foreach (var BloomAndFlares in FPSCameraBloomAndFlares)
            {
                BloomAndFlares.Key.bloomIntensity = BloomAndFlares.Value;
            }
            if (Weather != null && weatherController != null && weatherController.TimeOfDayController != null)
            {
                weatherController.TimeOfDayController.LightColor.colorKeys = defaultGradientColorKeys;
            }
            if (FPSCameraCC_Vintage != null)
            {
                FPSCameraCC_Vintage.enabled = defaultFPSCameraCC_Vintage;
            }
            if (FPSCameraCC_Sharpen != null)
            {
                FPSCameraCC_Sharpen.enabled = defaultFPSCameraSharpen;
                FPSCameraCC_Sharpen.rampOffsetR = defaultrampOffsetR;
                FPSCameraCC_Sharpen.rampOffsetG = defaultrampOffsetG;
                FPSCameraCC_Sharpen.rampOffsetB = defaultrampOffsetB;
            }
            if (FPSCameraCustomGlobalFog != null)
            {
                FPSCameraCustomGlobalFog.enabled = defaultFPSCameraCustomGlobalFog;
                FPSCameraCustomGlobalFog.FuncStart = 1f;
                FPSCameraCustomGlobalFog.BlendMode = CustomGlobalFog.BlendModes.Lighten;
            }
            if (FPSCameraGlobalFog != null)
            {
                FPSCameraGlobalFog.enabled = defaultFPSCameraGlobalFog;
            }
            if (FPSCameraColorCorrectionCurves != null)
            {
                FPSCameraColorCorrectionCurves.enabled = defaultFPSCameraColorCorrectionCurves;
            }
        }
        private void DefaultTonemap()
        {
            if (FPSCameraPrismEffects != null)
            {
                FPSCameraPrismEffects.tonemapType = Prism.Utils.TonemapType.Filmic;
                FPSCameraPrismEffects.toneValues = defaulttoneValues;
                FPSCameraPrismEffects.secondaryToneValues = defaultsecondaryToneValues;
                FPSCameraPrismEffects.toneValues += new Vector3(0f, (AmandsGraphicsPlugin.Brightness.Value - 0.5f), 0f);
            }
        }
        private void ACESTonemap()
        {
            if (FPSCameraPrismEffects != null)
            {
                FPSCameraPrismEffects.tonemapType = Prism.Utils.TonemapType.ACES;
                switch (scene)
                {
                    case "City_Scripts":
                        FPSCameraPrismEffects.toneValues = AmandsGraphicsPlugin.StreetsACES.Value;
                        FPSCameraPrismEffects.secondaryToneValues = AmandsGraphicsPlugin.StreetsACESS.Value;
                        break;
                    case "Laboratory_Scripts":
                        FPSCameraPrismEffects.toneValues = AmandsGraphicsPlugin.LabsACES.Value;
                        FPSCameraPrismEffects.secondaryToneValues = AmandsGraphicsPlugin.LabsACESS.Value;
                        break;
                    case "custom_Light":
                        FPSCameraPrismEffects.toneValues = AmandsGraphicsPlugin.CustomsACES.Value;
                        FPSCameraPrismEffects.secondaryToneValues = AmandsGraphicsPlugin.CustomsACESS.Value;
                        break;
                    case "Factory_Day":
                        FPSCameraPrismEffects.toneValues = AmandsGraphicsPlugin.FactoryACES.Value;
                        FPSCameraPrismEffects.secondaryToneValues = AmandsGraphicsPlugin.FactoryACESS.Value;
                        break;
                    case "Factory_Night":
                        FPSCameraPrismEffects.toneValues = AmandsGraphicsPlugin.FactoryNightACES.Value;
                        FPSCameraPrismEffects.secondaryToneValues = AmandsGraphicsPlugin.FactoryNightACESS.Value;
                        break;
                    case "Lighthouse_Abadonned_pier":
                        FPSCameraPrismEffects.toneValues = AmandsGraphicsPlugin.LighthouseACES.Value;
                        FPSCameraPrismEffects.secondaryToneValues = AmandsGraphicsPlugin.LighthouseACESS.Value;
                        break;
                    case "Shopping_Mall_Terrain":
                        FPSCameraPrismEffects.toneValues = AmandsGraphicsPlugin.InterchangeACES.Value;
                        FPSCameraPrismEffects.secondaryToneValues = AmandsGraphicsPlugin.InterchangeACESS.Value;
                        break;
                    case "woods_combined":
                        FPSCameraPrismEffects.toneValues = AmandsGraphicsPlugin.WoodsACES.Value;
                        FPSCameraPrismEffects.secondaryToneValues = AmandsGraphicsPlugin.WoodsACESS.Value;
                        break;
                    case "Reserve_Base_DesignStuff":
                        FPSCameraPrismEffects.toneValues = AmandsGraphicsPlugin.ReserveACES.Value;
                        FPSCameraPrismEffects.secondaryToneValues = AmandsGraphicsPlugin.ReserveACESS.Value;
                        break;
                    case "shoreline_scripts":
                        FPSCameraPrismEffects.toneValues = AmandsGraphicsPlugin.ShorelineACES.Value;
                        FPSCameraPrismEffects.secondaryToneValues = AmandsGraphicsPlugin.ShorelineACESS.Value;
                        break;
                    default:
                        FPSCameraPrismEffects.toneValues = AmandsGraphicsPlugin.HideoutACES.Value;
                        FPSCameraPrismEffects.secondaryToneValues = AmandsGraphicsPlugin.HideoutACESS.Value;
                        break;
                }
                FPSCameraPrismEffects.toneValues += new Vector3(0f, (AmandsGraphicsPlugin.Brightness.Value - 0.5f) * 4f, 0f);
            }
        }
        private void FilmicTonemap()
        {
            if (FPSCameraPrismEffects != null)
            {
                FPSCameraPrismEffects.tonemapType = Prism.Utils.TonemapType.Filmic;
                switch (scene)
                {
                    case "City_Scripts":
                        FPSCameraPrismEffects.toneValues = AmandsGraphicsPlugin.StreetsFilmic.Value;
                        FPSCameraPrismEffects.secondaryToneValues = AmandsGraphicsPlugin.StreetsFilmicS.Value;
                        break;
                    case "Laboratory_Scripts":
                        FPSCameraPrismEffects.toneValues = AmandsGraphicsPlugin.LabsFilmic.Value;
                        FPSCameraPrismEffects.secondaryToneValues = AmandsGraphicsPlugin.LabsFilmicS.Value;
                        break;
                    case "custom_Light":
                        FPSCameraPrismEffects.toneValues = AmandsGraphicsPlugin.CustomsFilmic.Value;
                        FPSCameraPrismEffects.secondaryToneValues = AmandsGraphicsPlugin.CustomsFilmicS.Value;
                        break;
                    case "Factory_Day":
                        FPSCameraPrismEffects.toneValues = AmandsGraphicsPlugin.FactoryFilmic.Value;
                        FPSCameraPrismEffects.secondaryToneValues = AmandsGraphicsPlugin.FactoryFilmicS.Value;
                        break;
                    case "Factory_Night":
                        FPSCameraPrismEffects.toneValues = AmandsGraphicsPlugin.FactoryNightFilmic.Value;
                        FPSCameraPrismEffects.secondaryToneValues = AmandsGraphicsPlugin.FactoryNightFilmicS.Value;
                        break;
                    case "Lighthouse_Abadonned_pier":
                        FPSCameraPrismEffects.toneValues = AmandsGraphicsPlugin.LighthouseFilmic.Value;
                        FPSCameraPrismEffects.secondaryToneValues = AmandsGraphicsPlugin.LighthouseFilmicS.Value;
                        break;
                    case "Shopping_Mall_Terrain":
                        FPSCameraPrismEffects.toneValues = AmandsGraphicsPlugin.InterchangeFilmic.Value;
                        FPSCameraPrismEffects.secondaryToneValues = AmandsGraphicsPlugin.InterchangeFilmicS.Value;
                        break;
                    case "woods_combined":
                        FPSCameraPrismEffects.toneValues = AmandsGraphicsPlugin.WoodsFilmic.Value;
                        FPSCameraPrismEffects.secondaryToneValues = AmandsGraphicsPlugin.WoodsFilmicS.Value;
                        break;
                    case "Reserve_Base_DesignStuff":
                        FPSCameraPrismEffects.toneValues = AmandsGraphicsPlugin.ReserveFilmic.Value;
                        FPSCameraPrismEffects.secondaryToneValues = AmandsGraphicsPlugin.ReserveFilmicS.Value;
                        break;
                    case "shoreline_scripts":
                        FPSCameraPrismEffects.toneValues = AmandsGraphicsPlugin.ShorelineFilmic.Value;
                        FPSCameraPrismEffects.secondaryToneValues = AmandsGraphicsPlugin.ShorelineFilmicS.Value;
                        break;
                    default:
                        FPSCameraPrismEffects.toneValues = AmandsGraphicsPlugin.HideoutFilmic.Value;
                        FPSCameraPrismEffects.secondaryToneValues = AmandsGraphicsPlugin.HideoutFilmicS.Value;
                        break;
                }
                FPSCameraPrismEffects.toneValues += new Vector3(0f, (AmandsGraphicsPlugin.Brightness.Value - 0.5f), 0f);
            }
        }
        private void PerMapTonemap()
        {
            if (FPSCameraPrismEffects != null)
            {
                ETonemap tonemap = ETonemap.ACES;
                switch (scene)
                {
                    case "City_Scripts":
                        tonemap = AmandsGraphicsPlugin.StreetsTonemap.Value;
                        break;
                    case "Laboratory_Scripts":
                        tonemap = AmandsGraphicsPlugin.LabsTonemap.Value;
                        break;
                    case "custom_Light":
                        tonemap = AmandsGraphicsPlugin.CustomsTonemap.Value;
                        break;
                    case "Factory_Day":
                        tonemap = AmandsGraphicsPlugin.FactoryTonemap.Value;
                        break;
                    case "Factory_Night":
                        tonemap = AmandsGraphicsPlugin.FactoryNightTonemap.Value;
                        break;
                    case "Lighthouse_Abadonned_pier":
                        tonemap = AmandsGraphicsPlugin.LighthouseTonemap.Value;
                        break;
                    case "Shopping_Mall_Terrain":
                        tonemap = AmandsGraphicsPlugin.InterchangeTonemap.Value;
                        break;
                    case "woods_combined":
                        tonemap = AmandsGraphicsPlugin.WoodsTonemap.Value;
                        break;
                    case "Reserve_Base_DesignStuff":
                        tonemap = AmandsGraphicsPlugin.ReserveTonemap.Value;
                        break;
                    case "shoreline_scripts":
                        tonemap = AmandsGraphicsPlugin.ShorelineTonemap.Value;
                        break;
                    default:
                        tonemap = AmandsGraphicsPlugin.HideoutTonemap.Value;
                        break;
                }
                switch (tonemap)
                {
                    case ETonemap.Default:
                        DefaultTonemap();
                        break;
                    case ETonemap.ACES:
                        ACESTonemap();
                        break;
                    case ETonemap.Filmic:
                        FilmicTonemap();
                        break;
                }
            }
        }
        private void SettingsUpdated(object sender, EventArgs e)
        {
            if (GraphicsMode)
            {
                UpdateAmandsGraphics();
            }
        }
    }
    public enum EDepthOfField
    {
        Off,
        On,
        HoldingBreathOnly
    }
    public enum EWeaponDepthOfField
    {
        Off,
        On
    }
    public enum EWeaponDepthOfFieldState
    {
        Off,
        Weapon,
        Aiming,
        Optic
    }
    public enum EOpticDOFFocalLengthMode
    {
        Math,
        FixedValue
    }
    public enum ERaycastQuality
    {
        Low,
        High,
        Foliage
    }
    public enum ETonemap
    {
        Default,
        ACES,
        Filmic
    }
    public enum EGlobalTonemap
    {
        Default,
        ACES,
        Filmic,
        PerMap
    }
    public enum EDebugMode
    {
        HBAO,
        DefaultToACES,
        DefaultToFilmic,
        ACESToFilmic,
        useLut,
        CC_Vintage,
        CC_Sharpen,
        CustomGlobalFog,
        GlobalFog,
        ColorCorrectionCurves,
        LightsUseLinearIntensity,
        SunColor,
        SkyColor,
        NVGColorsFix
    }
}

