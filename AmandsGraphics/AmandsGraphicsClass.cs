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
using Comfort.Common;
using UnityEngine.UI;
using TMPro;

namespace AmandsGraphics
{
    public class AmandsGraphicsClass : MonoBehaviour
    {
        public static LocalPlayer localPlayer;
        public static EAimingMode aimingMode;
        public static List<TacticalComboVisualController> tacticalComboVisualControllers = new List<TacticalComboVisualController>();
        public static Dictionary<Light, float> registeredLights = new Dictionary<Light, float>();
        public static Dictionary<VolumetricLight, float> registeredVolumetricLights = new Dictionary<VolumetricLight, float>();
        private static GameObject FPSCamera;
        private static Camera FPSCameraCamera;
        private static PostProcessVolume FPSCameraPostProcessVolume;
        private static PostProcessLayer FPSCameraPostProcessLayer;
        private static UnityEngine.Rendering.PostProcessing.MotionBlur FPSCameraMotionBlur;
        private static UnityEngine.Rendering.PostProcessing.DepthOfField FPSCameraDepthOfField;
        private static UnityStandardAssets.ImageEffects.DepthOfField FPSCameraWeaponDepthOfField;
        public static SightComponent sightComponent;
        public static Transform backLens;

        public static GameObject ActiveUIScreen;
        private static GameObject AmandsToggleTextUIGameObject;
        private static RectTransform AmandsToggleTextUITransform;
        private static VerticalLayoutGroup AmandsToggleTextUIVerticalLayoutGroup;
        private static GameObject AmandsToggleTextGameObject;
        private static AmandsToggleText amandsToggleText;

        private static bool SurroundDepthOfField = false;
        private static bool UIDepthOfField = false;

        private static bool SurroundDepthOfFieldEnabled = false;
        private static float SurroundDepthOfFieldAnimation = 0f;
        private static float SurroundDepthOfFieldFocusDistance = 0f;
        private static bool IsLooking = false;
        private static bool isLookingEnabled = false;

        public static bool CameraClassBlur = false;
        private static float UIDepthOfFieldAnimation = 0f;

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
        private static LayerMask LowLayerMask;
        private static LayerMask HighLayerMask;
        private static LayerMask FoliageLayerMask;
        private static Transform TargetCollider;
        private static Vector3 TargetLocal;
        private static AnimationCurve ApertureAnimationCurve;
        public static bool HoldingBreath = false;

        public static AmandsHitEffectClass amandsHitEffectClass;
        public static FastBlur fastBlur;
        public static UnityEngine.Rendering.PostProcessing.ChromaticAberration FPSCameraChromaticAberration;
        public static float ChromaticAberrationAnimation = 0.0f;
        public static float ChromaticAberrationIntensity = 0.0f;

        private static WeatherController weatherController;
        private static ToDController toDController;
        private static TOD_Sky tOD_Sky;
        private static object mBOIT_Scattering;
        private static CC_Sharpen FPSCameraCC_Sharpen;
        private static Dictionary<BloomAndFlares, float> FPSCameraBloomAndFlares = new Dictionary<BloomAndFlares, float>();
        private static PrismEffects FPSCameraPrismEffects;
        private static CC_Vintage FPSCameraCC_Vintage;
        private static CustomGlobalFog FPSCameraCustomGlobalFog;
        private static Behaviour FPSCameraGlobalFog;
        private static ColorCorrectionCurves FPSCameraColorCorrectionCurves;
        public static NightVision FPSCameraNightVision;
        public static float defaultNightVisionNoiseIntensity;
        private static HBAO FPSCameraHBAO;
        public static HBAO_Core.AOSettings FPSCameraHBAOAOSettings;
        public static HBAO_Core.ColorBleedingSettings FPSCameraHBAOColorBleedingSettings;
        public static string scene;
        private static LevelSettings levelSettings;
        private static Vector3 defaulttoneValues;
        private static Vector3 defaultsecondaryToneValues;
        private static bool defaultuseLut;
        private static float defaultrampOffsetR;
        private static float defaultrampOffsetG;
        private static float defaultrampOffsetB;
        private static float defaultZeroLevel;
        private static float defaultMBOITZeroLevel;
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
        private static AnimationCurve defaultAmbientContrast;
        private static AnimationCurve NightAmbientContrast;
        private static AnimationCurve NVGAmbientContrast;
        private static AnimationCurve defaultAmbientBrightness;
        private static float defaultLightIntensity;
        private static float defaultSunMeshBrightness;
        private static float defaultScatteringBrightnessMultiplier;

        private static Dictionary<string, string> sceneLevelSettings = new Dictionary<string, string>();

        public static bool NVG = false;

        public bool GraphicsMode = false;

        private void Awake()
        {
            LowLayerMask = LayerMask.GetMask("Terrain", "LowPolyCollider", "HitCollider");
            HighLayerMask = LayerMask.GetMask("Terrain", "HighPolyCollider", "HitCollider");
            FoliageLayerMask = LayerMask.GetMask("Terrain", "HighPolyCollider", "HitCollider", "Foliage");
        }
        
        public void Start()
        {
            sceneLevelSettings.Add("Sandbox_Scripts", "---Sandbox_ levelsettings ---");
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
            AmandsGraphicsPlugin.LabsHBAOIntensity.SettingChanged += SettingsUpdated;
            AmandsGraphicsPlugin.LabsHBAOSaturation.SettingChanged += SettingsUpdated;
            AmandsGraphicsPlugin.LabsHBAOAlbedoMultiplier.SettingChanged += SettingsUpdated;
            AmandsGraphicsPlugin.SurroundDepthOfField.SettingChanged += SettingsUpdated;
            AmandsGraphicsPlugin.DOFKernelSize.SettingChanged += SettingsUpdated;
            AmandsGraphicsPlugin.UIDepthOfField.SettingChanged += SettingsUpdated;
            AmandsGraphicsPlugin.WeaponDepthOfField.SettingChanged += SettingsUpdated;
            AmandsGraphicsPlugin.WeaponDOFAperture.SettingChanged += SettingsUpdated;
            AmandsGraphicsPlugin.WeaponDOFBlurSampleCount.SettingChanged += SettingsUpdated;
            AmandsGraphicsPlugin.OpticDepthOfField.SettingChanged += SettingsUpdated;
            AmandsGraphicsPlugin.OpticDOFAperture1x.SettingChanged += SettingsUpdated;
            AmandsGraphicsPlugin.OpticDOFAperture2x.SettingChanged += SettingsUpdated;
            AmandsGraphicsPlugin.OpticDOFAperture4x.SettingChanged += SettingsUpdated;
            AmandsGraphicsPlugin.OpticDOFAperture6x.SettingChanged += SettingsUpdated;
            AmandsGraphicsPlugin.OpticDOFKernelSize.SettingChanged += SettingsUpdated;

            AmandsGraphicsPlugin.Flashlight.SettingChanged += SettingsUpdated;

            AmandsGraphicsPlugin.NVG.SettingChanged += SettingsUpdated;
            AmandsGraphicsPlugin.NVGTonemap.SettingChanged += SettingsUpdated;
            AmandsGraphicsPlugin.NVGAmbientContrast.SettingChanged += SettingsUpdated;
            AmandsGraphicsPlugin.InterchangeNVGAmbientContrast.SettingChanged += SettingsUpdated;
            AmandsGraphicsPlugin.NVGNoiseIntensity.SettingChanged += SettingsUpdated;
            AmandsGraphicsPlugin.InterchangeNVGNoiseIntensity.SettingChanged += SettingsUpdated;
            AmandsGraphicsPlugin.NVGOriginalSkyColor.SettingChanged += SettingsUpdated;
            AmandsGraphicsPlugin.InterchangeNVGOriginalSkyColor.SettingChanged += SettingsUpdated;
            AmandsGraphicsPlugin.NVGOriginalColor.SettingChanged += SettingsUpdated;
            AmandsGraphicsPlugin.NVGCustomGlobalFogIntensity.SettingChanged += SettingsUpdated;
            AmandsGraphicsPlugin.NVGMoonLightIntensity.SettingChanged += SettingsUpdated;

            AmandsGraphicsPlugin.NightAmbientLight.SettingChanged += SettingsUpdated;
            AmandsGraphicsPlugin.NightAmbientContrast.SettingChanged += SettingsUpdated;
            AmandsGraphicsPlugin.InterchangeNightAmbientContrast.SettingChanged += SettingsUpdated;

            AmandsGraphicsPlugin.MysticalGlow.SettingChanged += SettingsUpdated;
            AmandsGraphicsPlugin.MysticalGlowIntensity.SettingChanged += SettingsUpdated;
            AmandsGraphicsPlugin.GroundZeroMysticalGlowIntensity.SettingChanged += SettingsUpdated;
            AmandsGraphicsPlugin.StreetsMysticalGlowIntensity.SettingChanged += SettingsUpdated;
            AmandsGraphicsPlugin.CustomsMysticalGlowIntensity.SettingChanged += SettingsUpdated;
            AmandsGraphicsPlugin.LighthouseMysticalGlowIntensity.SettingChanged += SettingsUpdated;
            AmandsGraphicsPlugin.InterchangeMysticalGlowIntensity.SettingChanged += SettingsUpdated;
            AmandsGraphicsPlugin.WoodsMysticalGlowIntensity.SettingChanged += SettingsUpdated;
            AmandsGraphicsPlugin.ReserveMysticalGlowIntensity.SettingChanged += SettingsUpdated;
            AmandsGraphicsPlugin.ShorelineMysticalGlowIntensity.SettingChanged += SettingsUpdated;

            AmandsGraphicsPlugin.HealthEffectHit.SettingChanged += SettingsUpdated;

            AmandsGraphicsPlugin.SunMeshBrightness.SettingChanged += SettingsUpdated;
            AmandsGraphicsPlugin.SkyBrightness.SettingChanged += SettingsUpdated;

            AmandsGraphicsPlugin.Brightness.SettingChanged += SettingsUpdated;
            AmandsGraphicsPlugin.Tonemap.SettingChanged += SettingsUpdated;
            AmandsGraphicsPlugin.UseBSGLUT.SettingChanged += SettingsUpdated;
            AmandsGraphicsPlugin.BloomIntensity.SettingChanged += SettingsUpdated;
            AmandsGraphicsPlugin.UseBSGCC_Vintage.SettingChanged += SettingsUpdated;
            AmandsGraphicsPlugin.UseBSGCC_Sharpen.SettingChanged += SettingsUpdated;
            AmandsGraphicsPlugin.UseBSGCustomGlobalFog.SettingChanged += SettingsUpdated;
            AmandsGraphicsPlugin.CustomGlobalFogIntensity.SettingChanged += SettingsUpdated;
            AmandsGraphicsPlugin.UseBSGGlobalFog.SettingChanged += SettingsUpdated;
            AmandsGraphicsPlugin.UseBSGColorCorrectionCurves.SettingChanged += SettingsUpdated;
            AmandsGraphicsPlugin.LightsUseLinearIntensity.SettingChanged += SettingsUpdated;
            AmandsGraphicsPlugin.SunColor.SettingChanged += SettingsUpdated;
            AmandsGraphicsPlugin.SkyColor.SettingChanged += SettingsUpdated;

            AmandsGraphicsPlugin.GroundZeroFogLevel.SettingChanged += SettingsUpdated;
            AmandsGraphicsPlugin.StreetsFogLevel.SettingChanged += SettingsUpdated;
            AmandsGraphicsPlugin.CustomsFogLevel.SettingChanged += SettingsUpdated;
            AmandsGraphicsPlugin.LighthouseFogLevel.SettingChanged += SettingsUpdated;
            AmandsGraphicsPlugin.InterchangeFogLevel.SettingChanged += SettingsUpdated;
            AmandsGraphicsPlugin.WoodsFogLevel.SettingChanged += SettingsUpdated;
            AmandsGraphicsPlugin.ReserveFogLevel.SettingChanged += SettingsUpdated;
            AmandsGraphicsPlugin.ShorelineFogLevel.SettingChanged += SettingsUpdated;

            AmandsGraphicsPlugin.GroundZeroTonemap.SettingChanged += SettingsUpdated;
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

            AmandsGraphicsPlugin.GroundZeroACES.SettingChanged += SettingsUpdated;
            AmandsGraphicsPlugin.GroundZeroACESS.SettingChanged += SettingsUpdated;
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

            AmandsGraphicsPlugin.GroundZeroFilmic.SettingChanged += SettingsUpdated;
            AmandsGraphicsPlugin.GroundZeroFilmicS.SettingChanged += SettingsUpdated;
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

            NightAmbientContrast = new AnimationCurve(new Keyframe(-0.2522f, AmandsGraphicsPlugin.NightAmbientContrast.Value), new Keyframe(-0.1261f, 1.15f));
            NVGAmbientContrast = new AnimationCurve(new Keyframe(0f, AmandsGraphicsPlugin.NVGAmbientContrast.Value));
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
                AmandsToggleText(GraphicsMode);
            }
            if (Input.GetKeyDown(AmandsGraphicsPlugin.GraphicsToggle.Value.MainKey) && Input.GetKey(KeyCode.LeftShift) && FPSCamera != null && GraphicsMode)
            {
                switch (AmandsGraphicsPlugin.DebugMode.Value)
                {
                    case EDebugMode.SquidInkFix:
                        AmandsGraphicsPlugin.SquidInkFix.Value = AmandsGraphicsPlugin.SquidInkFix.Value == EEnabledFeature.On ? EEnabledFeature.Off : EEnabledFeature.On;
                        UpdateAmandsGraphics();
                        break;
                    case EDebugMode.Flashlight:
                        AmandsGraphicsPlugin.Flashlight.Value = AmandsGraphicsPlugin.Flashlight.Value == EEnabledFeature.On ? EEnabledFeature.Off : EEnabledFeature.On;
                        break;
                    case EDebugMode.NVG:
                        AmandsGraphicsPlugin.NVG.Value = AmandsGraphicsPlugin.NVG.Value == EEnabledFeature.On ? EEnabledFeature.Off : EEnabledFeature.On;
                        break;
                    case EDebugMode.NVGOriginalColor:
                        AmandsGraphicsPlugin.NVGOriginalColor.Value = !AmandsGraphicsPlugin.NVGOriginalColor.Value;
                        break;
                    case EDebugMode.NightAmbientLight:
                        AmandsGraphicsPlugin.NightAmbientLight.Value = AmandsGraphicsPlugin.NightAmbientLight.Value == EEnabledFeature.On ? EEnabledFeature.Off : EEnabledFeature.On;
                        break;
                    case EDebugMode.HBAO:
                        AmandsGraphicsPlugin.HBAO.Value = AmandsGraphicsPlugin.HBAO.Value == EEnabledFeature.On ? EEnabledFeature.Off : EEnabledFeature.On;
                        break;
                    case EDebugMode.MysticalGlow:
                        AmandsGraphicsPlugin.MysticalGlow.Value = AmandsGraphicsPlugin.MysticalGlow.Value == EEnabledFeature.On ? EEnabledFeature.Off : EEnabledFeature.On;
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
                        UpdateAmandsGraphics();
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
                        UpdateAmandsGraphics();
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
                        UpdateAmandsGraphics();
                        break;
                    case EDebugMode.useLut:
                        AmandsGraphicsPlugin.UseBSGLUT.Value = !AmandsGraphicsPlugin.UseBSGLUT.Value;
                        break;
                    case EDebugMode.CC_Vintage:
                        AmandsGraphicsPlugin.UseBSGCC_Vintage.Value = !AmandsGraphicsPlugin.UseBSGCC_Vintage.Value;
                        break;
                    case EDebugMode.CC_Sharpen:
                        AmandsGraphicsPlugin.UseBSGCC_Sharpen.Value = !AmandsGraphicsPlugin.UseBSGCC_Sharpen.Value;
                        break;
                    case EDebugMode.CustomGlobalFog:
                        AmandsGraphicsPlugin.UseBSGCustomGlobalFog.Value = !AmandsGraphicsPlugin.UseBSGCustomGlobalFog.Value;
                        break;
                    case EDebugMode.GlobalFog:
                        AmandsGraphicsPlugin.UseBSGGlobalFog.Value = !AmandsGraphicsPlugin.UseBSGGlobalFog.Value;
                        break;
                    case EDebugMode.ColorCorrectionCurves:
                        AmandsGraphicsPlugin.UseBSGColorCorrectionCurves.Value = !AmandsGraphicsPlugin.UseBSGColorCorrectionCurves.Value;
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
                }
                //UpdateAmandsGraphics();
            }
            if ((AmandsGraphicsPlugin.SurroundDepthOfField.Value == EDepthOfField.HoldingBreathOnly || AmandsGraphicsPlugin.OpticDepthOfField.Value == EDepthOfField.HoldingBreathOnly) && localPlayer != null)
            {
                HoldingBreath = Traverse.Create(Traverse.Create(localPlayer).Field("Physical").GetValue<object>()).Property("HoldingBreath").GetValue<bool>();
            }
            if ((SurroundDepthOfField || UIDepthOfField) && FPSCameraDepthOfField != null)
            {
                SurroundDepthOfFieldEnabled = SurroundDepthOfField && (localPlayer != null && localPlayer.ProceduralWeaponAnimation != null && localPlayer.ProceduralWeaponAnimation.IsAiming && OpticCamera != null && OpticCamera.activeSelf && localPlayer.ProceduralWeaponAnimation.CurrentAimingMod != null &&
                    localPlayer.ProceduralWeaponAnimation.CurrentAimingMod.GetCurrentOpticZoom() > (AmandsGraphicsPlugin.SurroundDOFOpticZoom.Value - 0.1f) && sightComponent != null &&
                    sightComponent == localPlayer.ProceduralWeaponAnimation.CurrentAimingMod && localPlayer.ProceduralWeaponAnimation.CurrentAimingMod.SelectedScopeIndex == 0 && (AmandsGraphicsPlugin.SurroundDepthOfField.Value == EDepthOfField.HoldingBreathOnly ? HoldingBreath : true));
                IsLooking = isLookingEnabled && Traverse.Create(localPlayer).Property("IsLooking").GetValue<bool>();
                if (IsLooking)
                {
                    SurroundDepthOfFieldEnabled = false;
                }
                UIDepthOfFieldAnimation += (((UIDepthOfField && CameraClassBlur) ? 1f : 0f) - UIDepthOfFieldAnimation) * Time.deltaTime * AmandsGraphicsPlugin.UIDOFSpeed.Value;
                SurroundDepthOfFieldAnimation += ((SurroundDepthOfFieldEnabled ? 1f : 0f) - SurroundDepthOfFieldAnimation) * Time.deltaTime * AmandsGraphicsPlugin.SurroundDOFSpeed.Value;

                FPSCameraDepthOfField.aperture.value = Mathf.Lerp(Mathf.Lerp(AmandsGraphicsPlugin.SurroundDOFAperture.Value, AmandsGraphicsPlugin.UIDOFAperture.Value, UIDepthOfFieldAnimation), AmandsGraphicsPlugin.SurroundDOFAperture.Value, SurroundDepthOfFieldAnimation);
                FPSCameraDepthOfField.focusDistance.value = Mathf.Lerp(Mathf.Lerp(SurroundDepthOfFieldFocusDistance, AmandsGraphicsPlugin.UIDOFDistance.Value, UIDepthOfFieldAnimation), SurroundDepthOfFieldFocusDistance, SurroundDepthOfFieldAnimation);
                FPSCameraDepthOfField.focalLength.value = Mathf.Lerp(Mathf.Lerp(AmandsGraphicsPlugin.SurroundDOFFocalLengthOff.Value, AmandsGraphicsPlugin.UIDOFFocalLength.Value, UIDepthOfFieldAnimation), AmandsGraphicsPlugin.SurroundDOFFocalLength.Value, SurroundDepthOfFieldAnimation);
                FPSCameraDepthOfField.enabled.value = SurroundDepthOfFieldAnimation > 0.01f || UIDepthOfFieldAnimation > 0.01f;
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
            if (AmandsGraphicsPlugin.WeaponDepthOfField.Value != EWeaponDepthOfField.Off && FPSCameraWeaponDepthOfField != null && GraphicsMode)
            {
                if (NVG)
                {
                    if (SurroundDepthOfFieldEnabled || (UIDepthOfField && CameraClassBlur))
                    {
                        weaponDepthOfFieldState = EWeaponDepthOfFieldState.Off;
                    }
                    else
                    {
                        weaponDepthOfFieldState = EWeaponDepthOfFieldState.NVG;
                    }
                }
                else
                {
                    weaponDepthOfFieldState = EWeaponDepthOfFieldState.Weapon;
                    if (localPlayer != null && localPlayer.ProceduralWeaponAnimation != null && localPlayer.ProceduralWeaponAnimation.IsAiming && !(SurroundDepthOfFieldEnabled || (UIDepthOfField && CameraClassBlur)))
                    {
                        if (aimingMode == EAimingMode.IronSight)
                        {
                            weaponDepthOfFieldState = EWeaponDepthOfFieldState.IronSight;
                        }
                        else
                        {
                            weaponDepthOfFieldState = EWeaponDepthOfFieldState.Sight;
                        }
                    }
                    else if (SurroundDepthOfFieldEnabled || (UIDepthOfField && CameraClassBlur))
                    {
                        weaponDepthOfFieldState = EWeaponDepthOfFieldState.Off;
                    }
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
                    case EWeaponDepthOfFieldState.IronSight:
                        WeaponDepthOfFieldFocalLength += ((AmandsGraphicsPlugin.WeaponDOFIronSightFocalLength.Value / 100f) - WeaponDepthOfFieldFocalLength) * Time.deltaTime * AmandsGraphicsPlugin.WeaponDOFSpeed.Value;
                        WeaponDepthOfFieldMaxBlurSize += ((AmandsGraphicsPlugin.WeaponDOFIronSightMaxBlurSize.Value / 10f) - WeaponDepthOfFieldMaxBlurSize) * Time.deltaTime * AmandsGraphicsPlugin.WeaponDOFSpeed.Value;
                        break;
                    case EWeaponDepthOfFieldState.Sight:
                        WeaponDepthOfFieldFocalLength += ((AmandsGraphicsPlugin.WeaponDOFSightFocalLength.Value / 100f) - WeaponDepthOfFieldFocalLength) * Time.deltaTime * AmandsGraphicsPlugin.WeaponDOFSpeed.Value;
                        WeaponDepthOfFieldMaxBlurSize += ((AmandsGraphicsPlugin.WeaponDOFSightMaxBlurSize.Value / 10f) - WeaponDepthOfFieldMaxBlurSize) * Time.deltaTime * AmandsGraphicsPlugin.WeaponDOFSpeed.Value;
                        break;
                    case EWeaponDepthOfFieldState.NVG:
                        WeaponDepthOfFieldFocalLength += ((AmandsGraphicsPlugin.WeaponDOFNVGFocalLength.Value / 100f) - WeaponDepthOfFieldFocalLength) * Time.deltaTime * AmandsGraphicsPlugin.WeaponDOFSpeed.Value;
                        WeaponDepthOfFieldMaxBlurSize += ((AmandsGraphicsPlugin.WeaponDOFNVGMaxBlurSize.Value / 10f) - WeaponDepthOfFieldMaxBlurSize) * Time.deltaTime * AmandsGraphicsPlugin.WeaponDOFSpeed.Value;
                        break;
                }
                FPSCameraWeaponDepthOfField.focalLength = WeaponDepthOfFieldFocalLength * 100f;
                FPSCameraWeaponDepthOfField.maxBlurSize = WeaponDepthOfFieldMaxBlurSize * 10f;
            }
        }
        public void AmandsOnPostRender(Camera cam)
        {
            if (SurroundDepthOfField && backLens != null)
            {
                SurroundDepthOfFieldFocusDistance = Mathf.Clamp(Vector3.Distance(Camera.current.transform.position, backLens.position),0.001f,1f);
            }
            else
            {
                SurroundDepthOfFieldFocusDistance = 0.1f;
            }
        }
        public void ActivateAmandsGraphics(GameObject fpscamera, PrismEffects prismeffects)
        {
            if (FPSCamera == null)
            {
                FPSCamera = fpscamera;
                if (FPSCamera != null)
                {
                    registeredLights.Clear();
                    registeredVolumetricLights.Clear();
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
                        if (component.ToString() == "FPS Camera (MBOIT_Scattering)")
                        {
                            mBOIT_Scattering = component;
                            if (mBOIT_Scattering != null)
                            {
                                defaultMBOITZeroLevel = Traverse.Create(mBOIT_Scattering).Field("ZeroLevel").GetValue<float>();
                            }
                        }
                    }
                    FPSCameraColorCorrectionCurves = FPSCamera.GetComponent<ColorCorrectionCurves>();
                    if (FPSCameraColorCorrectionCurves != null)
                    {
                        defaultFPSCameraColorCorrectionCurves = FPSCameraColorCorrectionCurves.enabled;
                    }
                    weatherController = WeatherController.Instance;
                    if (weatherController != null)
                    {
                        if (weatherController.TimeOfDayController != null) defaultGradientColorKeys = weatherController.TimeOfDayController.LightColor.colorKeys;
                        toDController = weatherController.TimeOfDayController;
                        if (toDController != null)
                        {
                            defaultAmbientBrightness = toDController.AmbientBrightness;
                            defaultAmbientContrast = toDController.AmbientContrast;
                            defaultScatteringBrightnessMultiplier = toDController.ScatteringBrightnessMultiplier;
                        }
                        tOD_Sky = TOD_Sky.Instance;
                        if (tOD_Sky != null)
                        {
                            defaultSunMeshBrightness = tOD_Sky.Sun.MeshBrightness;
                            defaultLightIntensity = tOD_Sky.Night.LightIntensity;
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
                    if ((AmandsGraphicsPlugin.SurroundDepthOfField.Value != EDepthOfField.Off || AmandsGraphicsPlugin.UIDepthOfField.Value != EUIDepthOfField.Off || AmandsGraphicsPlugin.OpticDepthOfField.Value != EDepthOfField.Off) && Graphics.activeTier == GraphicsTier.Tier2)
                    {
                        PreloaderUI.Instance.CloseErrorScreen();
                        PreloaderUI.Instance.ShowErrorScreen("High-Quality Color is Off", "Enable High-Quality Color on Graphics Settings for SurroundDOF, UIDOF and OpticDOF to work as intended");
                    }
                    // Needs bug detection
                    /*if (false)
                    {
                        NotificationManagerClass.DisplayMessageNotification("Motion Blur needs anti-aliasing set to TAA", EFT.Communications.ENotificationDurationType.Long, EFT.Communications.ENotificationIconType.Alert, Color.red);
                    }*/
                }
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
        public void AmandsGraphicsHitEffect(float power)
        {
            if (FPSCameraChromaticAberration != null)
            {
                ChromaticAberrationAnimation = 1f;
                ChromaticAberrationIntensity = (power / AmandsGraphicsPlugin.HitCAPower.Value) * AmandsGraphicsPlugin.HitCAIntensity.Value;
                FPSCameraChromaticAberration.intensity.value = ChromaticAberrationIntensity;
                FPSCameraChromaticAberration.enabled.value = true;
            }
        }
        public void UpdateAmandsGraphics()
        {
            if (AmandsGraphicsPlugin.HealthEffectHit.Value == EEnabledFeature.On)
            {
                if (amandsHitEffectClass == null)
                {
                    amandsHitEffectClass = AmandsGraphicsPlugin.Hook.AddComponent<AmandsHitEffectClass>();
                }
            }
            else
            {
                if (amandsHitEffectClass != null)
                {
                    Destroy(amandsHitEffectClass);
                    ChromaticAberrationAnimation = 0f;
                    if (FPSCameraChromaticAberration != null)
                    {
                        FPSCameraChromaticAberration.intensity.value = 0f;
                        FPSCameraChromaticAberration.enabled.value = false;
                    }
                }
            }
            if (mBOIT_Scattering != null)
            {
                switch (scene)
                {
                    case "Sandbox_Scripts":
                        Traverse.Create(mBOIT_Scattering).Field("ZeroLevel").SetValue(defaultMBOITZeroLevel + AmandsGraphicsPlugin.GroundZeroFogLevel.Value);
                        break;
                    case "City_Scripts":
                        Traverse.Create(mBOIT_Scattering).Field("ZeroLevel").SetValue(defaultMBOITZeroLevel + AmandsGraphicsPlugin.StreetsFogLevel.Value);
                        break;
                    case "Laboratory_Scripts":
                        break;
                    case "custom_Light":
                        Traverse.Create(mBOIT_Scattering).Field("ZeroLevel").SetValue(defaultMBOITZeroLevel + AmandsGraphicsPlugin.CustomsFogLevel.Value);
                        break;
                    case "Lighthouse_Abadonned_pier":
                        Traverse.Create(mBOIT_Scattering).Field("ZeroLevel").SetValue(defaultMBOITZeroLevel + AmandsGraphicsPlugin.LighthouseFogLevel.Value);
                        break;
                    case "Shopping_Mall_Terrain":
                        Traverse.Create(mBOIT_Scattering).Field("ZeroLevel").SetValue(defaultMBOITZeroLevel + AmandsGraphicsPlugin.InterchangeFogLevel.Value);
                        break;
                    case "woods_combined":
                        Traverse.Create(mBOIT_Scattering).Field("ZeroLevel").SetValue(defaultMBOITZeroLevel + AmandsGraphicsPlugin.WoodsFogLevel.Value);
                        break;
                    case "Reserve_Base_DesignStuff":
                        Traverse.Create(mBOIT_Scattering).Field("ZeroLevel").SetValue(defaultMBOITZeroLevel + AmandsGraphicsPlugin.ReserveFogLevel.Value);
                        break;
                    case "shoreline_scripts":
                        Traverse.Create(mBOIT_Scattering).Field("ZeroLevel").SetValue(defaultMBOITZeroLevel + AmandsGraphicsPlugin.ShorelineFogLevel.Value);
                        break;
                }
            }
            if (AmandsGraphicsPlugin.Flashlight.Value == EEnabledFeature.On)
            {
                foreach (KeyValuePair<Light, float> registeredLight in registeredLights)
                {
                    registeredLight.Key.range = registeredLight.Value * AmandsGraphicsPlugin.FlashlightRange.Value;
                }
                foreach (KeyValuePair<VolumetricLight, float> registeredVolumetricLight in registeredVolumetricLights)
                {
                    registeredVolumetricLight.Key.ExtinctionCoef = AmandsGraphicsPlugin.FlashlightExtinctionCoef.Value;
                    if (registeredVolumetricLight.Key.VolumetricMaterial != null)
                    {
                        registeredVolumetricLight.Key.VolumetricMaterial.SetVector("_VolumetricLight", new Vector4(registeredVolumetricLight.Key.ScatteringCoef, registeredVolumetricLight.Key.ExtinctionCoef, AmandsGraphicsPlugin.FlashlightRange.Value, 1f - registeredVolumetricLight.Key.SkyboxExtinctionCoef));
                    }
                }
            }
            else
            {
                foreach (KeyValuePair<Light, float> changedLight in registeredLights)
                {
                    changedLight.Key.range = changedLight.Value;
                }
                foreach (KeyValuePair<VolumetricLight, float> changedVolumetricLight in registeredVolumetricLights)
                {
                    changedVolumetricLight.Key.ExtinctionCoef = changedVolumetricLight.Value;
                    if (changedVolumetricLight.Key.VolumetricMaterial != null)
                    {
                        changedVolumetricLight.Key.VolumetricMaterial.SetVector("_VolumetricLight", new Vector4(changedVolumetricLight.Key.ScatteringCoef, changedVolumetricLight.Key.ExtinctionCoef, AmandsGraphicsPlugin.FlashlightRange.Value, 1f - changedVolumetricLight.Key.SkyboxExtinctionCoef));
                    }
                }
            }
            GraphicsSettings.lightsUseLinearIntensity = AmandsGraphicsPlugin.LightsUseLinearIntensity.Value;
            if (FPSCameraMotionBlur != null)
            {
                FPSCameraMotionBlur.enabled.Override(AmandsGraphicsPlugin.MotionBlur.Value == EEnabledFeature.On);
                FPSCameraMotionBlur.sampleCount.Override(AmandsGraphicsPlugin.MotionBlurSampleCount.Value);
                FPSCameraMotionBlur.shutterAngle.Override(AmandsGraphicsPlugin.MotionBlurShutterAngle.Value);
            }
            HoldingBreath = false;
            if (FPSCameraDepthOfField != null)
            {
                SurroundDepthOfField = AmandsGraphicsPlugin.SurroundDepthOfField.Value != EDepthOfField.Off;
                UIDepthOfField = AmandsGraphicsPlugin.UIDepthOfField.Value != EUIDepthOfField.Off;
                FPSCameraDepthOfField.enabled.value = false;
                FPSCameraDepthOfField.kernelSize.value = AmandsGraphicsPlugin.DOFKernelSize.Value;
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
            if (AmandsGraphicsPlugin.NightAmbientLight.Value == EEnabledFeature.On)
            {
                if (toDController != null)
                {
                    if (scene == "Shopping_Mall_Terrain")
                    {
                        NightAmbientContrast = new AnimationCurve(new Keyframe(-0.2522f, AmandsGraphicsPlugin.InterchangeNightAmbientContrast.Value), new Keyframe(-0.1261f, 1.15f));
                    }
                    else
                    {
                        NightAmbientContrast = new AnimationCurve(new Keyframe(-0.2522f, AmandsGraphicsPlugin.NightAmbientContrast.Value), new Keyframe(-0.1261f, 1.15f));
                    }
                    toDController.AmbientContrast = NightAmbientContrast;
                    toDController.ScatteringBrightnessMultiplier = defaultScatteringBrightnessMultiplier * AmandsGraphicsPlugin.SkyBrightness.Value;
                }
            }
            else
            {
                if (toDController != null)
                {
                    toDController.AmbientContrast = defaultAmbientContrast;
                    toDController.ScatteringBrightnessMultiplier = defaultScatteringBrightnessMultiplier * AmandsGraphicsPlugin.SkyBrightness.Value;
                }
            }
            if (tOD_Sky != null)
            {
                tOD_Sky.Sun.MeshBrightness = AmandsGraphicsPlugin.SunMeshBrightness.Value;
                tOD_Sky.Moon.MeshBrightness = 3f;
                tOD_Sky.Moon.MeshContrast = 0.5f;
                tOD_Sky.Night.LightIntensity = defaultLightIntensity;
            }
            if (AmandsGraphicsPlugin.NVG.Value == EEnabledFeature.On && NVG)
            {
                if (toDController != null)
                {
                    if (scene != "Laboratory_Scripts") NVGAmbientContrast.RemoveKey(0);
                    switch (scene)
                    {
                        case "Shopping_Mall_Terrain":
                            NVGAmbientContrast.AddKey(0f, AmandsGraphicsPlugin.InterchangeNVGAmbientContrast.Value);
                            break;
                        case "Laboratory_Scripts":
                            toDController.AmbientContrast = defaultAmbientContrast;
                            break;
                        default:
                            NVGAmbientContrast.AddKey(0f, AmandsGraphicsPlugin.NVGAmbientContrast.Value);
                            break;
                    }
                    if (scene != "Laboratory_Scripts") toDController.AmbientContrast = NVGAmbientContrast;
                }
                if (levelSettings != null)
                {
                    switch (scene)
                    {
                        case "Shopping_Mall_Terrain":
                            levelSettings.NightVisionSkyColor = Color.Lerp(Color.black, defaultNightVisionSkyColor, AmandsGraphicsPlugin.InterchangeNVGOriginalSkyColor.Value);
                            levelSettings.NightVisionEquatorColor = Color.Lerp(Color.black, defaultNightVisionEquatorColor, AmandsGraphicsPlugin.InterchangeNVGOriginalSkyColor.Value);
                            levelSettings.NightVisionGroundColor = Color.Lerp(Color.black, defaultNightVisionGroundColor, AmandsGraphicsPlugin.InterchangeNVGOriginalSkyColor.Value);
                            break;
                        default:
                            levelSettings.NightVisionSkyColor = Color.Lerp(Color.black, defaultNightVisionSkyColor, AmandsGraphicsPlugin.NVGOriginalSkyColor.Value);
                            levelSettings.NightVisionEquatorColor = Color.Lerp(Color.black, defaultNightVisionEquatorColor, AmandsGraphicsPlugin.NVGOriginalSkyColor.Value);
                            levelSettings.NightVisionGroundColor = Color.Lerp(Color.black, defaultNightVisionGroundColor, AmandsGraphicsPlugin.NVGOriginalSkyColor.Value);
                            break;
                    }
                }
                if (tOD_Sky != null && tOD_Sky.Night != null)
                {
                    tOD_Sky.Night.LightIntensity = defaultLightIntensity * AmandsGraphicsPlugin.NVGMoonLightIntensity.Value;
                }
                if (FPSCameraNightVision != null)
                {
                    switch (scene)
                    {
                        case "Shopping_Mall_Terrain":
                            FPSCameraNightVision.NoiseIntensity = defaultNightVisionNoiseIntensity * AmandsGraphicsPlugin.InterchangeNVGNoiseIntensity.Value;
                            break;
                        default:
                            FPSCameraNightVision.NoiseIntensity = defaultNightVisionNoiseIntensity * AmandsGraphicsPlugin.NVGNoiseIntensity.Value;
                            break;
                    }
                    FPSCameraNightVision.ApplySettings();
                }
            }
            else if (NVG)
            {
                ResetGraphics();
                if (FPSCameraWeaponDepthOfField != null)
                {
                    FPSCameraWeaponDepthOfField.enabled = AmandsGraphicsPlugin.WeaponDepthOfField.Value != EWeaponDepthOfField.Off;
                    FPSCameraWeaponDepthOfField.aperture = AmandsGraphicsPlugin.WeaponDOFAperture.Value;
                    FPSCameraWeaponDepthOfField.focalSize = 100f;
                    FPSCameraWeaponDepthOfField.blurSampleCount = AmandsGraphicsPlugin.WeaponDOFBlurSampleCount.Value;
                }
                return;
            }

            if (FPSCameraHBAO != null)
            {
                if (AmandsGraphicsPlugin.HBAO.Value == EEnabledFeature.On)
                {
                    switch (scene)
                    {
                        case "Laboratory_Scripts":
                            FPSCameraHBAOAOSettings.intensity = AmandsGraphicsPlugin.LabsHBAOIntensity.Value;
                            FPSCameraHBAOColorBleedingSettings.saturation = AmandsGraphicsPlugin.LabsHBAOSaturation.Value;
                            FPSCameraHBAOColorBleedingSettings.albedoMultiplier = AmandsGraphicsPlugin.LabsHBAOAlbedoMultiplier.Value;
                            break;
                        default:
                            FPSCameraHBAOAOSettings.intensity = AmandsGraphicsPlugin.HBAOIntensity.Value;
                            FPSCameraHBAOColorBleedingSettings.saturation = AmandsGraphicsPlugin.HBAOSaturation.Value;
                            FPSCameraHBAOColorBleedingSettings.albedoMultiplier = AmandsGraphicsPlugin.HBAOAlbedoMultiplier.Value;
                            break;
                    }
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
                if (NVG)
                {
                    switch (AmandsGraphicsPlugin.NVGTonemap.Value)
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
                else
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
                }
                FPSCameraPrismEffects.useLut = AmandsGraphicsPlugin.UseBSGLUT.Value ? defaultuseLut : false;
            }
            foreach (var BloomAndFlares in FPSCameraBloomAndFlares)
            {
                BloomAndFlares.Key.bloomIntensity = BloomAndFlares.Value * AmandsGraphicsPlugin.BloomIntensity.Value;
            }
            if (weatherController != null && weatherController.TimeOfDayController != null)
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
                        case "Sandbox_Scripts":
                            if (AmandsGraphicsPlugin.MysticalGlow.Value == EEnabledFeature.On)
                            {
                                levelSettings.SkyColor = Color.white * AmandsGraphicsPlugin.MysticalGlowIntensity.Value * AmandsGraphicsPlugin.GroundZeroMysticalGlowIntensity.Value;
                            }
                            else
                            {
                                levelSettings.SkyColor = defaultSkyColor;
                            }
                            levelSettings.ZeroLevel = defaultZeroLevel + AmandsGraphicsPlugin.GroundZeroFogLevel.Value;
                            break;
                        case "City_Scripts":
                            if (AmandsGraphicsPlugin.MysticalGlow.Value == EEnabledFeature.On)
                            {
                                levelSettings.SkyColor = Color.white * AmandsGraphicsPlugin.MysticalGlowIntensity.Value * AmandsGraphicsPlugin.StreetsMysticalGlowIntensity.Value;
                            }
                            else
                            {
                                levelSettings.SkyColor = defaultSkyColor;
                            }
                            levelSettings.ZeroLevel = defaultZeroLevel + AmandsGraphicsPlugin.StreetsFogLevel.Value;
                            break;
                        case "Laboratory_Scripts":
                            break;
                        case "custom_Light":
                            if (AmandsGraphicsPlugin.MysticalGlow.Value == EEnabledFeature.On)
                            {
                                levelSettings.SkyColor = Color.white * AmandsGraphicsPlugin.MysticalGlowIntensity.Value * AmandsGraphicsPlugin.CustomsMysticalGlowIntensity.Value;
                            }
                            else
                            {
                                levelSettings.SkyColor = defaultSkyColor;
                            }
                            levelSettings.ZeroLevel = defaultZeroLevel + AmandsGraphicsPlugin.CustomsFogLevel.Value;
                            break;
                        case "Lighthouse_Abadonned_pier":
                            if (AmandsGraphicsPlugin.MysticalGlow.Value == EEnabledFeature.On)
                            {
                                levelSettings.SkyColor = Color.white * AmandsGraphicsPlugin.MysticalGlowIntensity.Value * AmandsGraphicsPlugin.LighthouseMysticalGlowIntensity.Value;
                            }
                            else
                            {
                                levelSettings.SkyColor = defaultSkyColor;
                            }
                            levelSettings.ZeroLevel = defaultZeroLevel + AmandsGraphicsPlugin.LighthouseFogLevel.Value;
                            break;
                        case "Shopping_Mall_Terrain":
                            if (AmandsGraphicsPlugin.MysticalGlow.Value == EEnabledFeature.On)
                            {
                                levelSettings.SkyColor = Color.white * AmandsGraphicsPlugin.MysticalGlowIntensity.Value * AmandsGraphicsPlugin.InterchangeMysticalGlowIntensity.Value;
                            }
                            else
                            {
                                levelSettings.SkyColor = defaultSkyColor;
                            }
                            levelSettings.ZeroLevel = defaultZeroLevel + AmandsGraphicsPlugin.InterchangeFogLevel.Value;
                            break;
                        case "woods_combined":
                            if (AmandsGraphicsPlugin.MysticalGlow.Value == EEnabledFeature.On)
                            {
                                levelSettings.SkyColor = Color.white * AmandsGraphicsPlugin.MysticalGlowIntensity.Value * AmandsGraphicsPlugin.WoodsMysticalGlowIntensity.Value;
                            }
                            else
                            {
                                levelSettings.SkyColor = defaultSkyColor;
                            }
                            levelSettings.ZeroLevel = defaultZeroLevel + AmandsGraphicsPlugin.WoodsFogLevel.Value;
                            break;
                        case "Reserve_Base_DesignStuff":
                            if (AmandsGraphicsPlugin.MysticalGlow.Value == EEnabledFeature.On)
                            {
                                levelSettings.SkyColor = Color.white * AmandsGraphicsPlugin.MysticalGlowIntensity.Value * AmandsGraphicsPlugin.ReserveMysticalGlowIntensity.Value;
                            }
                            else
                            {
                                levelSettings.SkyColor = defaultSkyColor;
                            }
                            levelSettings.ZeroLevel = defaultZeroLevel + AmandsGraphicsPlugin.ReserveFogLevel.Value;
                            break;
                        case "shoreline_scripts":
                            if (AmandsGraphicsPlugin.MysticalGlow.Value == EEnabledFeature.On)
                            {
                                levelSettings.SkyColor = Color.white * AmandsGraphicsPlugin.MysticalGlowIntensity.Value * AmandsGraphicsPlugin.ShorelineMysticalGlowIntensity.Value;
                            }
                            else
                            {
                                levelSettings.SkyColor = defaultSkyColor;
                            }
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
                        case "Sandbox_Scripts":
                            levelSettings.ZeroLevel = defaultZeroLevel + AmandsGraphicsPlugin.GroundZeroFogLevel.Value;
                            break;
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
                FPSCameraCC_Vintage.enabled = AmandsGraphicsPlugin.UseBSGCC_Vintage.Value;
            }
            if (FPSCameraCC_Sharpen != null)
            {
                if (AmandsGraphicsPlugin.UseBSGCC_Sharpen.Value)
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
                if (AmandsGraphicsPlugin.UseBSGCustomGlobalFog.Value)
                {
                    FPSCameraCustomGlobalFog.enabled = defaultFPSCameraCustomGlobalFog;
                    FPSCameraCustomGlobalFog.FuncStart = 1f;
                    FPSCameraCustomGlobalFog.BlendMode = CustomGlobalFog.BlendModes.Lighten;
                }
                else
                {
                    FPSCameraCustomGlobalFog.enabled = (scene == "Factory_Day" || scene == "Factory_Night" || scene == "default") ? false : defaultFPSCameraCustomGlobalFog;
                    FPSCameraCustomGlobalFog.FuncStart = NVG ? AmandsGraphicsPlugin.NVGCustomGlobalFogIntensity.Value : AmandsGraphicsPlugin.CustomGlobalFogIntensity.Value;
                    FPSCameraCustomGlobalFog.BlendMode = AmandsGraphicsPlugin.SquidInkFix.Value == EEnabledFeature.On ? CustomGlobalFog.BlendModes.Lighten : CustomGlobalFog.BlendModes.Normal;
                }
            }
            if (FPSCameraGlobalFog != null)
            {
                FPSCameraGlobalFog.enabled = AmandsGraphicsPlugin.UseBSGGlobalFog.Value;
            }
            if (FPSCameraColorCorrectionCurves != null)
            {
                FPSCameraColorCorrectionCurves.enabled = AmandsGraphicsPlugin.UseBSGColorCorrectionCurves.Value;
            }
            // NVG FIX
            if (NVG && AmandsGraphicsPlugin.NVGOriginalColor.Value)
            {
                if (FPSCameraPrismEffects != null)
                {
                    FPSCameraPrismEffects.useLut = defaultuseLut;
                }
                if (levelSettings != null)
                {
                    //levelSettings.ZeroLevel = defaultZeroLevel;
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
                if (FPSCameraColorCorrectionCurves != null)
                {
                    FPSCameraColorCorrectionCurves.enabled = defaultFPSCameraColorCorrectionCurves;
                }
            }
        }
        private void ResetGraphics()
        {
            if (mBOIT_Scattering != null)
            {
                Traverse.Create(mBOIT_Scattering).Field("ZeroLevel").SetValue(defaultMBOITZeroLevel);
            }
            foreach (KeyValuePair<Light,float> changedLight in registeredLights)
            {
                changedLight.Key.range = changedLight.Value;
            }
            foreach (KeyValuePair<VolumetricLight, float> changedVolumetricLight in registeredVolumetricLights)
            {
                changedVolumetricLight.Key.ExtinctionCoef = changedVolumetricLight.Value;
                if (changedVolumetricLight.Key.VolumetricMaterial != null)
                {
                    changedVolumetricLight.Key.VolumetricMaterial.SetVector("_VolumetricLight", new Vector4(changedVolumetricLight.Key.ScatteringCoef, changedVolumetricLight.Key.ExtinctionCoef, AmandsGraphicsPlugin.FlashlightRange.Value, 1f - changedVolumetricLight.Key.SkyboxExtinctionCoef));
                }
            }
            GraphicsSettings.lightsUseLinearIntensity = defaultLightsUseLinearIntensity;
            if (FPSCameraHBAO != null)
            {
                FPSCameraHBAO.aoSettings = defaultFPSCameraHBAOAOSettings;
                FPSCameraHBAO.colorBleedingSettings = defaultFPSCameraHBAOColorBleedingSettings;
            }
            if (FPSCameraWeaponDepthOfField != null)
            {
                FPSCameraWeaponDepthOfField.enabled = defaultFPSCameraWeaponDepthOfField;
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
            if (weatherController != null && weatherController.TimeOfDayController != null)
            {
                weatherController.TimeOfDayController.LightColor.colorKeys = defaultGradientColorKeys;
            }
            if (toDController != null)
            {
                toDController.AmbientContrast = defaultAmbientContrast;
                toDController.ScatteringBrightnessMultiplier = defaultScatteringBrightnessMultiplier;
            }
            if (tOD_Sky != null)
            {
                tOD_Sky.Sun.MeshBrightness = defaultSunMeshBrightness;
                tOD_Sky.Moon.MeshBrightness = 0.8f;
                tOD_Sky.Moon.MeshContrast = 1f;
                tOD_Sky.Night.LightIntensity = defaultLightIntensity;
            }
            if (FPSCameraNightVision)
            {
                FPSCameraNightVision.NoiseIntensity = defaultNightVisionNoiseIntensity;
                FPSCameraNightVision.ApplySettings();
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
                    case "Sandbox_Scripts":
                        FPSCameraPrismEffects.toneValues = AmandsGraphicsPlugin.GroundZeroACES.Value;
                        FPSCameraPrismEffects.secondaryToneValues = AmandsGraphicsPlugin.GroundZeroACESS.Value;
                        break;
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
                    case "Sandbox_Scripts":
                        FPSCameraPrismEffects.toneValues = AmandsGraphicsPlugin.GroundZeroFilmic.Value;
                        FPSCameraPrismEffects.secondaryToneValues = AmandsGraphicsPlugin.GroundZeroFilmicS.Value;
                        break;
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
                    case "Sandbox_Scripts":
                        tonemap = AmandsGraphicsPlugin.GroundZeroTonemap.Value;
                        break;
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
            if ((AmandsGraphicsPlugin.SurroundDepthOfField.Value != EDepthOfField.Off || AmandsGraphicsPlugin.UIDepthOfField.Value != EUIDepthOfField.Off || AmandsGraphicsPlugin.OpticDepthOfField.Value != EDepthOfField.Off) && Graphics.activeTier == GraphicsTier.Tier2)
            {
                PreloaderUI.Instance.CloseErrorScreen();
                PreloaderUI.Instance.ShowErrorScreen("High-Quality Color is Off", "Enable High-Quality Color on Graphics Settings for SurroundDOF, UIDOF and OpticDOF to work as intended");
            }
            if (GraphicsMode)
            {
                UpdateAmandsGraphics();
            }
        }
        public void AmandsToggleText(bool Enabled)
        {
            if (AmandsToggleTextUIGameObject == null) return;
            if (amandsToggleText == null)
            {
                AmandsToggleTextGameObject = new GameObject("AmandsToggleTextGameObject");
                AmandsToggleTextGameObject.transform.SetParent(AmandsToggleTextUIGameObject.transform);
                amandsToggleText = AmandsToggleTextGameObject.AddComponent<AmandsToggleText>();
                amandsToggleText.text = "AMANDS " + (Enabled ? "<b>ON</b>" : "<b>OFF</b>");
            }
            else
            {
                amandsToggleText.UpdateText("AMANDS " + (Enabled ? "<b>ON</b>" : "<b>OFF</b>"));
            }
        }
        public static void CreateGameObjects(Transform parent)
        {
            AmandsToggleTextUIGameObject = new GameObject("killList");
            AmandsToggleTextUITransform = AmandsToggleTextUIGameObject.AddComponent<RectTransform>();
            AmandsToggleTextUIGameObject.transform.SetParent(parent);
            AmandsToggleTextUITransform.anchorMin = Vector2.zero;
            AmandsToggleTextUITransform.anchorMax = Vector2.zero;
            AmandsToggleTextUITransform.sizeDelta = new Vector2(0f, 0f);
            AmandsToggleTextUIVerticalLayoutGroup = AmandsToggleTextUIGameObject.AddComponent<VerticalLayoutGroup>();
            AmandsToggleTextUIVerticalLayoutGroup.childControlHeight = false;
            ContentSizeFitter contentSizeFitter = AmandsToggleTextUIGameObject.AddComponent<ContentSizeFitter>();
            contentSizeFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
            contentSizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
            AmandsToggleTextUITransform.localPosition = new Vector2((Screen.width / 2) - 100, -420.0f);
            AmandsToggleTextUITransform.pivot = new Vector2(1f, 0f);
        }
        public static void DestroyGameObjects()
        {
            if (AmandsToggleTextUIGameObject != null) Destroy(AmandsToggleTextUIGameObject);
        }
    }
    public enum EEnabledFeature
    {
        Off,
        On
    }
    public enum EDepthOfField
    {
        Off,
        On,
        HoldingBreathOnly
    }
    public enum EMedsDepthOfField
    {
        Off,
        On,
        SurgicalKitOnly
    }
    public enum EUIDepthOfField
    {
        Off,
        On
    }
    public enum EWeaponDepthOfField
    {
        Off,
        On
    }
    public enum EAimingMode
    {
        IronSight,
        Sight
    }
    public enum EWeaponDepthOfFieldState
    {
        Off,
        Weapon,
        IronSight,
        Sight,
        NVG
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
        SquidInkFix,
        Flashlight,
        NVG,
        NVGOriginalColor,
        NightAmbientLight,
        HBAO,
        MysticalGlow,
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
        SkyColor
    }
    public class AmandsHitEffectClass : MonoBehaviour
    {
        public void Start()
        {
        }
        public void Update()
        {
            if (AmandsGraphicsClass.ChromaticAberrationAnimation > 0)
            {
                AmandsGraphicsClass.ChromaticAberrationAnimation -= Time.deltaTime / AmandsGraphicsPlugin.HitCASpeed.Value;
                if (AmandsGraphicsClass.FPSCameraChromaticAberration != null)
                {
                    AmandsGraphicsClass.FPSCameraChromaticAberration.intensity.value = Mathf.Lerp(0f, AmandsGraphicsClass.ChromaticAberrationIntensity, AmandsGraphicsClass.ChromaticAberrationAnimation);
                    AmandsGraphicsClass.FPSCameraChromaticAberration.enabled.value = AmandsGraphicsClass.ChromaticAberrationAnimation > 0.0f;
                }
            }
        }
    }
    public class AmandsToggleText : MonoBehaviour
    {
        public TMP_Text tMP_Text;
        public string text = "";
        public Color color = new Color(0.84f, 0.88f, 0.95f, 0.69f);
        public int fontSize = 26;
        public float outlineWidth = 0.01f;
        public FontStyles fontStyles = FontStyles.SmallCaps;
        public TextAlignmentOptions textAlignmentOptions = TextAlignmentOptions.Right;
        public float time = 2f;
        public float lifeTime = 0f;
        public float OpacitySpeed = 0.08f;
        private float Opacity = 1f;
        private float StartOpacity = 0f;
        private bool UpdateOpacity = false;
        private bool UpdateStartOpacity = false;

        public void Start()
        {
            tMP_Text = gameObject.AddComponent<TextMeshProUGUI>();
            if (tMP_Text != null)
            {
                tMP_Text.text = text;
                tMP_Text.color = color;
                tMP_Text.fontSize = fontSize;
                tMP_Text.outlineWidth = outlineWidth;
                tMP_Text.fontStyle = fontStyles;
                tMP_Text.alignment = textAlignmentOptions;
                tMP_Text.alpha = 0f;
                UpdateStartOpacity = true;
            }
            else
            {
                Destroy(gameObject);
            }
        }
        public void UpdateText(string Text)
        {
            text = Text;
            if (tMP_Text != null)
            {
                tMP_Text.text = Text;
            }
            lifeTime = 0f;
            if (UpdateOpacity && tMP_Text != null)
            {
                Opacity = 1f;
                tMP_Text.alpha = Opacity;
                UpdateOpacity = false;
            }
        }
        public void Update()
        {
            lifeTime += Time.deltaTime;
            if (lifeTime > time)
            {
                UpdateOpacity = true;
            }
            if (UpdateOpacity && tMP_Text != null)
            {
                Opacity -= Math.Max(0.01f, OpacitySpeed);
                tMP_Text.alpha = Opacity;
                if (Opacity < 0)
                {
                    UpdateOpacity = false;
                    UpdateStartOpacity = false;
                    Destroy(gameObject);
                }
            }
            else if (UpdateStartOpacity && StartOpacity < 1f && tMP_Text != null)
            {
                StartOpacity += OpacitySpeed * 2f;
                tMP_Text.alpha = StartOpacity;
            }
        }
    }
}

