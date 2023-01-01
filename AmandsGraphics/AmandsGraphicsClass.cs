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

namespace AmandsGraphics
{
    public class AmandsGraphicsClass : MonoBehaviour
    {
        private static GameObject FPSCamera;
        private static Camera FPSCameraCamera;
        private static float FPSCameraCameraFOV;
        public static Camera BaseOpticCameraCamera;
        private static PostProcessVolume FPSCameraPostProcessVolume;
        private static PostProcessLayer FPSCameraPostProcessLayer;
        private static LocalPlayer localPlayer;
        private static UnityEngine.Rendering.PostProcessing.MotionBlur FPSCameraMotionBlur;
        private static UnityEngine.Rendering.PostProcessing.DepthOfField FPSCameraDepthOfField;
        private static float DepthOfFieldFocalLength;
        private static bool DepthOfFieldMode;
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
        private static Light sunLight;
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
            AmandsGraphicsPlugin.DepthOfField.SettingChanged += SettingsUpdated;
            AmandsGraphicsPlugin.DepthOfFieldFocusDistance.SettingChanged += SettingsUpdated;
            AmandsGraphicsPlugin.DepthOfFieldAperture.SettingChanged += SettingsUpdated;
            AmandsGraphicsPlugin.DepthOfFieldFocalLength.SettingChanged += SettingsUpdated;
            AmandsGraphicsPlugin.DepthOfFieldKernelSize.SettingChanged += SettingsUpdated;

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
            if (AmandsGraphicsPlugin.DepthOfField.Value && FPSCameraDepthOfField != null && FPSCameraCamera != null && localPlayer != null)
            {

                if (FPSCameraCameraFOV >= FPSCameraCamera.fieldOfView)
                {
                    DepthOfFieldMode = FPSCameraCamera.fieldOfView < (AmandsGraphicsPlugin.DepthOfFieldFOV.Value - 0.01f);
                }
                else
                {
                    DepthOfFieldMode = FPSCameraCamera.fieldOfView < 35.01f;
                }
                if (BaseOpticCameraCamera != null && BaseOpticCameraCamera.fieldOfView > AmandsGraphicsPlugin.DepthOfFieldOpticFOV.Value) DepthOfFieldMode = false;
                FPSCameraCameraFOV = FPSCameraCamera.fieldOfView;
                DepthOfFieldFocalLength += (((localPlayer.HandsController != null ? localPlayer.HandsController.IsAiming && DepthOfFieldMode : false) ? AmandsGraphicsPlugin.DepthOfFieldFocalLength.Value : AmandsGraphicsPlugin.DepthOfFieldFocalLengthOff.Value) - DepthOfFieldFocalLength) * AmandsGraphicsPlugin.DepthOfFieldSpeed.Value; //  FOV 35.99f
                FPSCameraDepthOfField.focalLength.value = DepthOfFieldFocalLength;
                FPSCameraDepthOfField.enabled.value = DepthOfFieldFocalLength > (AmandsGraphicsPlugin.DepthOfFieldFocalLengthOff.Value + 0.1f);
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
                        DepthOfFieldFocalLength = AmandsGraphicsPlugin.DepthOfFieldFocalLengthOff.Value;
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
        public void UpdateAmandsGraphics()
        {
            GraphicsSettings.lightsUseLinearIntensity = AmandsGraphicsPlugin.LightsUseLinearIntensity.Value;
            if (FPSCameraMotionBlur != null)
            {
                FPSCameraMotionBlur.enabled.Override(AmandsGraphicsPlugin.MotionBlur.Value);
                FPSCameraMotionBlur.sampleCount.Override(AmandsGraphicsPlugin.MotionBlurSampleCount.Value);
                FPSCameraMotionBlur.shutterAngle.Override(AmandsGraphicsPlugin.MotionBlurShutterAngle.Value);
            }
            if (FPSCameraDepthOfField != null)
            {
                FPSCameraDepthOfField.enabled.value = false;
                FPSCameraDepthOfField.focusDistance.value = AmandsGraphicsPlugin.DepthOfFieldFocusDistance.Value;
                FPSCameraDepthOfField.aperture.value = AmandsGraphicsPlugin.DepthOfFieldAperture.Value;
                FPSCameraDepthOfField.kernelSize.value = AmandsGraphicsPlugin.DepthOfFieldKernelSize.Value;
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

