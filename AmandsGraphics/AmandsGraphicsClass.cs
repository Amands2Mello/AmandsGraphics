using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;
using UnityStandardAssets.ImageEffects;
using UnityEngine.SceneManagement;
using System;
using EFT.Weather;
using DG.Tweening;


namespace AmandsGraphics
{
    internal class AmandsGraphicsClass : MonoBehaviour
    {
        private static GameObject Weather;
        private static WeatherController weatherController;
        private static GameObject FPSCamera;
        private static PostProcessVolume FPSCameraPostProcessVolume;
        private static UnityEngine.Rendering.PostProcessing.MotionBlur FPSCameraMotionBlur;
        private static CC_Sharpen FPSCameraSharpen;
        private static BloomAndFlares[] FPSCameraBloomAndFlares;
        private static PrismEffects FPSCameraPrismEffects;
        private static CC_Vintage FPSCameraCC_Vintage;
        private static CustomGlobalFog FPSCameraCustomGlobalFog;
        private static Component FPSCameraGlobalFog;
        private static ColorCorrectionCurves FPSCameraColorCorrectionCurves;
        private static string scene;
        private static bool sceneEnabled;
        private static LevelSettings levelSettings;
        private static Vector3 defaulttoneValues;
        private static Vector3 defaultsecondaryToneValues;
        private static bool defaultuseLut;
        private static float defaultrampOffsetR;
        private static float defaultrampOffsetG;
        private static float defaultrampOffsetB;
        private static float defaultZeroLevel;
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
        private static Light sunLight;
        private static GradientColorKey[] gradientColorKeys = { };
        private static GradientColorKey[] defaultGradientColorKeys = { };



        private bool DebugMode = false;
        private int UpdateInterval;
        public void Start()
        {
            AmandsGraphicsPlugin.UnrealMotionBlurEnabled.SettingChanged += MotionBlurUpdated;
            AmandsGraphicsPlugin.UnrealMotionBlurSampleCount.SettingChanged += MotionBlurUpdated;
            AmandsGraphicsPlugin.UnrealMotionBlurShutterAngle.SettingChanged += MotionBlurUpdated;

            AmandsGraphicsPlugin.Labs.SettingChanged += sceneEnabledUpdated;
            AmandsGraphicsPlugin.Customs.SettingChanged += sceneEnabledUpdated;
            AmandsGraphicsPlugin.FactoryDay.SettingChanged += sceneEnabledUpdated;
            AmandsGraphicsPlugin.FactoryNight.SettingChanged += sceneEnabledUpdated;
            AmandsGraphicsPlugin.Lighthouse.SettingChanged += sceneEnabledUpdated;
            AmandsGraphicsPlugin.Interchange.SettingChanged += sceneEnabledUpdated;
            AmandsGraphicsPlugin.Woods.SettingChanged += sceneEnabledUpdated;
            AmandsGraphicsPlugin.Reserve.SettingChanged += sceneEnabledUpdated;
            AmandsGraphicsPlugin.Shoreline.SettingChanged += sceneEnabledUpdated;
            AmandsGraphicsPlugin.Hideout.SettingChanged += sceneEnabledUpdated;

            AmandsGraphicsPlugin.LabsToneValues.SettingChanged += GraphicsUpdated;
            AmandsGraphicsPlugin.LabsSecondaryToneValues.SettingChanged += GraphicsUpdated;
            AmandsGraphicsPlugin.CustomsToneValues.SettingChanged += GraphicsUpdated;
            AmandsGraphicsPlugin.CustomsSecondaryToneValues.SettingChanged += GraphicsUpdated;
            AmandsGraphicsPlugin.FactoryDayToneValues.SettingChanged += GraphicsUpdated;
            AmandsGraphicsPlugin.FactoryDaySecondaryToneValues.SettingChanged += GraphicsUpdated;
            AmandsGraphicsPlugin.FactoryDaySkyColor.SettingChanged += GraphicsUpdated;
            AmandsGraphicsPlugin.FactoryDayNVSkyColor.SettingChanged += GraphicsUpdated;
            AmandsGraphicsPlugin.FactoryNightToneValues.SettingChanged += GraphicsUpdated;
            AmandsGraphicsPlugin.FactoryNightSecondaryToneValues.SettingChanged += GraphicsUpdated;
            AmandsGraphicsPlugin.FactoryNightSkyColor.SettingChanged += GraphicsUpdated;
            AmandsGraphicsPlugin.FactoryNightNVSkyColor.SettingChanged += GraphicsUpdated;
            AmandsGraphicsPlugin.LighthouseToneValues.SettingChanged += GraphicsUpdated;
            AmandsGraphicsPlugin.LighthouseSecondaryToneValues.SettingChanged += GraphicsUpdated;
            AmandsGraphicsPlugin.InterchangeToneValues.SettingChanged += GraphicsUpdated;
            AmandsGraphicsPlugin.InterchangeSecondaryToneValues.SettingChanged += GraphicsUpdated;
            AmandsGraphicsPlugin.WoodsToneValues.SettingChanged += GraphicsUpdated;
            AmandsGraphicsPlugin.WoodsSecondaryToneValues.SettingChanged += GraphicsUpdated;
            AmandsGraphicsPlugin.ReserveToneValues.SettingChanged += GraphicsUpdated;
            AmandsGraphicsPlugin.ReserveSecondaryToneValues.SettingChanged += GraphicsUpdated;
            AmandsGraphicsPlugin.ShorelineToneValues.SettingChanged += GraphicsUpdated;
            AmandsGraphicsPlugin.ShorelineSecondaryToneValues.SettingChanged += GraphicsUpdated;
            AmandsGraphicsPlugin.HideoutToneValues.SettingChanged += GraphicsUpdated;
            AmandsGraphicsPlugin.HideoutSecondaryToneValues.SettingChanged += GraphicsUpdated;
            AmandsGraphicsPlugin.HideoutSkyColor.SettingChanged += GraphicsUpdated;
            AmandsGraphicsPlugin.GlobalFogIntensity.SettingChanged += GraphicsUpdated;
            AmandsGraphicsPlugin.DefaultGlobalFog.SettingChanged += GraphicsUpdated;
            AmandsGraphicsPlugin.FogZeroLevel.SettingChanged += GraphicsUpdated;

            AmandsGraphicsPlugin.LightColorEnabled.SettingChanged += LightColorUpdated;
            AmandsGraphicsPlugin.LightColorIndex0.SettingChanged += LightColorUpdated;
            AmandsGraphicsPlugin.LightColorIndex1.SettingChanged += LightColorUpdated;
            AmandsGraphicsPlugin.LightColorIndex2.SettingChanged += LightColorUpdated;
            AmandsGraphicsPlugin.LightColorIndex3.SettingChanged += LightColorUpdated;
            AmandsGraphicsPlugin.LightColorIndex4.SettingChanged += LightColorUpdated;
            AmandsGraphicsPlugin.LightColorIndex5.SettingChanged += LightColorUpdated;
        }
        public void MotionBlurUpdated(object sender, EventArgs e)
        {
            if (FPSCameraMotionBlur != null)
            {
                FPSCameraMotionBlur.enabled.Override(AmandsGraphicsPlugin.UnrealMotionBlurEnabled.Value);
                FPSCameraMotionBlur.sampleCount.Override(AmandsGraphicsPlugin.UnrealMotionBlurSampleCount.Value);
                FPSCameraMotionBlur.shutterAngle.Override(AmandsGraphicsPlugin.UnrealMotionBlurShutterAngle.Value);
            }
        }
        public void sceneEnabledUpdated(object sender, EventArgs e)
        {
            switch (scene)
            {
                case "Laboratory_Scripts":
                    sceneEnabled = AmandsGraphicsPlugin.Labs.Value;
                    break;
                case "custom_Light":
                    sceneEnabled = AmandsGraphicsPlugin.Customs.Value;
                    break;
                case "Factory_Day":
                    sceneEnabled = AmandsGraphicsPlugin.FactoryDay.Value;
                    break;
                case "Factory_Night":
                    sceneEnabled = AmandsGraphicsPlugin.FactoryNight.Value;
                    break;
                case "Lighthouse_Abadonned_pier":
                    sceneEnabled = AmandsGraphicsPlugin.Lighthouse.Value;
                    break;
                case "Shopping_Mall_Terrain":
                    sceneEnabled = AmandsGraphicsPlugin.Interchange.Value;
                    break;
                case "woods_combined":
                    sceneEnabled = AmandsGraphicsPlugin.Woods.Value;
                    break;
                case "Reserve_Base_DesignStuff":
                    sceneEnabled = AmandsGraphicsPlugin.Reserve.Value;
                    break;
                case "shoreline_scripts":
                    sceneEnabled = AmandsGraphicsPlugin.Shoreline.Value;
                    break;
                default:
                    sceneEnabled = AmandsGraphicsPlugin.Hideout.Value;
                    break;
            }
            UpdateInterval = 200;
        }
        public void LightColorUpdated(object sender, EventArgs e)
        {
            if (!DebugMode)
            {
                gradientColorKeys[0] = new GradientColorKey(AmandsGraphicsPlugin.LightColorIndex0.Value, 0.0f);
                gradientColorKeys[1] = new GradientColorKey(AmandsGraphicsPlugin.LightColorIndex1.Value, 0.5115129f);
                gradientColorKeys[2] = new GradientColorKey(AmandsGraphicsPlugin.LightColorIndex2.Value, 0.5266652f);
                gradientColorKeys[3] = new GradientColorKey(AmandsGraphicsPlugin.LightColorIndex3.Value, 0.5535668f);
                gradientColorKeys[4] = new GradientColorKey(AmandsGraphicsPlugin.LightColorIndex4.Value, 0.6971694f);
                gradientColorKeys[5] = new GradientColorKey(AmandsGraphicsPlugin.LightColorIndex5.Value, 0.9992523f);
                if (Weather != null)
                {
                    if (weatherController != null && weatherController.TimeOfDayController != null)
                    {
                        if (AmandsGraphicsPlugin.LightColorEnabled.Value)
                        {
                            weatherController.TimeOfDayController.LightColor.colorKeys = gradientColorKeys;
                        }
                        else
                        {
                            weatherController.TimeOfDayController.LightColor.colorKeys = defaultGradientColorKeys;
                        }
                    }
                }
            }
        }
        public void GraphicsUpdated(object sender, EventArgs e)
        {
            if (!DebugMode)
            {
                if (FPSCameraPrismEffects != null)
                {
                    switch (scene)
                    {
                        case "Laboratory_Scripts":
                            FPSCameraPrismEffects.toneValues = AmandsGraphicsPlugin.LabsToneValues.Value;
                            FPSCameraPrismEffects.secondaryToneValues = AmandsGraphicsPlugin.LabsSecondaryToneValues.Value;
                            break;
                        case "custom_Light":
                            FPSCameraPrismEffects.toneValues = AmandsGraphicsPlugin.CustomsToneValues.Value;
                            FPSCameraPrismEffects.secondaryToneValues = AmandsGraphicsPlugin.CustomsSecondaryToneValues.Value;
                            break;
                        case "Factory_Day":
                            FPSCameraPrismEffects.toneValues = AmandsGraphicsPlugin.FactoryDayToneValues.Value;
                            FPSCameraPrismEffects.secondaryToneValues = AmandsGraphicsPlugin.FactoryDaySecondaryToneValues.Value;
                            if (levelSettings != null)
                            {
                                levelSettings.SkyColor = AmandsGraphicsPlugin.FactoryDaySkyColor.Value / 10;
                                levelSettings.EquatorColor = AmandsGraphicsPlugin.FactoryDaySkyColor.Value / 10;
                                levelSettings.GroundColor = AmandsGraphicsPlugin.FactoryDaySkyColor.Value / 10;
                                levelSettings.NightVisionSkyColor = AmandsGraphicsPlugin.FactoryDayNVSkyColor.Value / 10;
                                levelSettings.NightVisionEquatorColor = AmandsGraphicsPlugin.FactoryDayNVSkyColor.Value / 10;
                                levelSettings.NightVisionGroundColor = AmandsGraphicsPlugin.FactoryDayNVSkyColor.Value / 10;
                            }
                            break;
                        case "Factory_Night":
                            FPSCameraPrismEffects.toneValues = AmandsGraphicsPlugin.FactoryNightToneValues.Value;
                            FPSCameraPrismEffects.secondaryToneValues = AmandsGraphicsPlugin.FactoryNightSecondaryToneValues.Value;
                            if (levelSettings != null)
                            {
                                levelSettings.SkyColor = AmandsGraphicsPlugin.FactoryNightSkyColor.Value / 10;
                                levelSettings.EquatorColor = AmandsGraphicsPlugin.FactoryNightSkyColor.Value / 10;
                                levelSettings.GroundColor = AmandsGraphicsPlugin.FactoryNightSkyColor.Value / 10;
                                levelSettings.NightVisionSkyColor = AmandsGraphicsPlugin.FactoryNightNVSkyColor.Value / 10;
                                levelSettings.NightVisionEquatorColor = AmandsGraphicsPlugin.FactoryNightNVSkyColor.Value / 10;
                                levelSettings.NightVisionGroundColor = AmandsGraphicsPlugin.FactoryNightNVSkyColor.Value / 10;
                            }
                            break;
                        case "Lighthouse_Abadonned_pier":
                            FPSCameraPrismEffects.toneValues = AmandsGraphicsPlugin.LighthouseToneValues.Value;
                            FPSCameraPrismEffects.secondaryToneValues = AmandsGraphicsPlugin.LighthouseSecondaryToneValues.Value;
                            break;
                        case "Shopping_Mall_Terrain":
                            FPSCameraPrismEffects.toneValues = AmandsGraphicsPlugin.InterchangeToneValues.Value;
                            FPSCameraPrismEffects.secondaryToneValues = AmandsGraphicsPlugin.InterchangeSecondaryToneValues.Value;
                            break;
                        case "woods_combined":
                            FPSCameraPrismEffects.toneValues = AmandsGraphicsPlugin.WoodsToneValues.Value;
                            FPSCameraPrismEffects.secondaryToneValues = AmandsGraphicsPlugin.WoodsSecondaryToneValues.Value;
                            break;
                        case "Reserve_Base_DesignStuff":
                            FPSCameraPrismEffects.toneValues = AmandsGraphicsPlugin.ReserveToneValues.Value;
                            FPSCameraPrismEffects.secondaryToneValues = AmandsGraphicsPlugin.ReserveSecondaryToneValues.Value;
                            break;
                        case "shoreline_scripts":
                            FPSCameraPrismEffects.toneValues = AmandsGraphicsPlugin.ShorelineToneValues.Value;
                            FPSCameraPrismEffects.secondaryToneValues = AmandsGraphicsPlugin.ShorelineSecondaryToneValues.Value;
                            break;
                       default:
                                FPSCameraPrismEffects.toneValues = AmandsGraphicsPlugin.HideoutToneValues.Value;
                                FPSCameraPrismEffects.secondaryToneValues = AmandsGraphicsPlugin.HideoutSecondaryToneValues.Value;
                                if (levelSettings != null)
                                {
                                    levelSettings.SkyColor = AmandsGraphicsPlugin.HideoutSkyColor.Value / 10;
                                    levelSettings.EquatorColor = AmandsGraphicsPlugin.HideoutSkyColor.Value / 10;
                                    levelSettings.GroundColor = AmandsGraphicsPlugin.HideoutSkyColor.Value / 10;
                                    levelSettings.NightVisionSkyColor = Color.black;
                                    levelSettings.NightVisionEquatorColor = Color.black;
                                    levelSettings.NightVisionGroundColor = Color.black;
                                }
                                break;
                    }
                }
                if (levelSettings != null)
                {
                    levelSettings.ZeroLevel = defaultZeroLevel + AmandsGraphicsPlugin.FogZeroLevel.Value;
                }
                if (FPSCameraCustomGlobalFog != null)
                {
                    FPSCameraCustomGlobalFog.FuncStart = AmandsGraphicsPlugin.GlobalFogIntensity.Value;
                    FPSCameraCustomGlobalFog.BlendMode = AmandsGraphicsPlugin.DefaultGlobalFog.Value ? CustomGlobalFog.BlendModes.Lighten : CustomGlobalFog.BlendModes.Normal;
                }
            }
        }
        public void Update()
        {
            if (Input.GetKeyDown(AmandsGraphicsPlugin.GraphicsToggle.Value.MainKey) && FPSCamera != null)
            {
                if (DebugMode)
                {
                    DebugMode = false;
                    if (FPSCameraPrismEffects != null)
                    {
                        FPSCameraPrismEffects.tonemapType = Prism.Utils.TonemapType.ACES;
                        switch (scene)
                        {
                            case "Laboratory_Scripts": 
                                FPSCameraPrismEffects.toneValues = AmandsGraphicsPlugin.LabsToneValues.Value;
                                FPSCameraPrismEffects.secondaryToneValues = AmandsGraphicsPlugin.LabsSecondaryToneValues.Value;
                                FPSCameraPrismEffects.useLut = false;
                                break;
                            case "custom_Light":
                                FPSCameraPrismEffects.toneValues = AmandsGraphicsPlugin.CustomsToneValues.Value;
                                FPSCameraPrismEffects.secondaryToneValues = AmandsGraphicsPlugin.CustomsSecondaryToneValues.Value;
                                FPSCameraPrismEffects.useLut = false;
                                break;
                            case "Factory_Day":
                                FPSCameraPrismEffects.toneValues = AmandsGraphicsPlugin.FactoryDayToneValues.Value;
                                FPSCameraPrismEffects.secondaryToneValues = AmandsGraphicsPlugin.FactoryDaySecondaryToneValues.Value;
                                FPSCameraPrismEffects.useLut = false;
                                if (levelSettings != null)
                                {
                                    levelSettings.SkyColor = AmandsGraphicsPlugin.FactoryDaySkyColor.Value / 10;//new Color(0.09f, 0.08f, 0.07f);
                                    levelSettings.EquatorColor = AmandsGraphicsPlugin.FactoryDaySkyColor.Value / 10;
                                    levelSettings.GroundColor = AmandsGraphicsPlugin.FactoryDaySkyColor.Value / 10;
                                    levelSettings.NightVisionSkyColor = AmandsGraphicsPlugin.FactoryDayNVSkyColor.Value / 10;
                                    levelSettings.NightVisionEquatorColor = AmandsGraphicsPlugin.FactoryDayNVSkyColor.Value / 10;
                                    levelSettings.NightVisionGroundColor = AmandsGraphicsPlugin.FactoryDayNVSkyColor.Value / 10;
                                }
                                break;
                            case "Factory_Night":
                                FPSCameraPrismEffects.toneValues = AmandsGraphicsPlugin.FactoryNightToneValues.Value;
                                FPSCameraPrismEffects.secondaryToneValues = AmandsGraphicsPlugin.FactoryNightSecondaryToneValues.Value;
                                FPSCameraPrismEffects.useLut = false;
                                if (levelSettings != null)
                                {
                                    levelSettings.SkyColor = AmandsGraphicsPlugin.FactoryNightSkyColor.Value / 10;//Color.black;
                                    levelSettings.EquatorColor = AmandsGraphicsPlugin.FactoryNightSkyColor.Value / 10;
                                    levelSettings.GroundColor = AmandsGraphicsPlugin.FactoryNightSkyColor.Value / 10;
                                    levelSettings.NightVisionSkyColor = AmandsGraphicsPlugin.FactoryNightNVSkyColor.Value / 10;
                                    levelSettings.NightVisionEquatorColor = AmandsGraphicsPlugin.FactoryNightNVSkyColor.Value / 10;
                                    levelSettings.NightVisionGroundColor = AmandsGraphicsPlugin.FactoryNightNVSkyColor.Value / 10;
                                }
                                break;
                            case "Lighthouse_Abadonned_pier":
                                FPSCameraPrismEffects.toneValues = AmandsGraphicsPlugin.LighthouseToneValues.Value;
                                FPSCameraPrismEffects.secondaryToneValues = AmandsGraphicsPlugin.LighthouseSecondaryToneValues.Value;
                                FPSCameraPrismEffects.useLut = false;
                                break;
                            case "Shopping_Mall_Terrain":
                                FPSCameraPrismEffects.toneValues = AmandsGraphicsPlugin.InterchangeToneValues.Value;
                                FPSCameraPrismEffects.secondaryToneValues = AmandsGraphicsPlugin.InterchangeSecondaryToneValues.Value;
                                FPSCameraPrismEffects.useLut = false;
                                break;
                            case "woods_combined":
                                FPSCameraPrismEffects.toneValues = AmandsGraphicsPlugin.WoodsToneValues.Value;
                                FPSCameraPrismEffects.secondaryToneValues = AmandsGraphicsPlugin.WoodsSecondaryToneValues.Value;
                                FPSCameraPrismEffects.useLut = false;
                                break;
                            case "Reserve_Base_DesignStuff":
                                FPSCameraPrismEffects.toneValues = AmandsGraphicsPlugin.ReserveToneValues.Value;
                                FPSCameraPrismEffects.secondaryToneValues = AmandsGraphicsPlugin.ReserveSecondaryToneValues.Value;
                                FPSCameraPrismEffects.useLut = false;
                                break;
                            case "shoreline_scripts":
                                FPSCameraPrismEffects.toneValues = AmandsGraphicsPlugin.ShorelineToneValues.Value;
                                FPSCameraPrismEffects.secondaryToneValues = AmandsGraphicsPlugin.ShorelineSecondaryToneValues.Value;
                                FPSCameraPrismEffects.useLut = false;
                                break;
                            default:
                                FPSCameraPrismEffects.toneValues = AmandsGraphicsPlugin.HideoutToneValues.Value;
                                FPSCameraPrismEffects.secondaryToneValues = AmandsGraphicsPlugin.HideoutSecondaryToneValues.Value;
                                FPSCameraPrismEffects.useLut = false;
                                if (levelSettings != null)
                                {
                                    levelSettings.SkyColor = AmandsGraphicsPlugin.HideoutSkyColor.Value / 10;
                                    levelSettings.EquatorColor = AmandsGraphicsPlugin.HideoutSkyColor.Value / 10;
                                    levelSettings.GroundColor = AmandsGraphicsPlugin.HideoutSkyColor.Value / 10;
                                    levelSettings.NightVisionSkyColor = Color.black;
                                    levelSettings.NightVisionEquatorColor = Color.black;
                                    levelSettings.NightVisionGroundColor = Color.black;
                                }
                                break;
                        }
                    }
                    if (levelSettings != null)
                    {
                        levelSettings.ZeroLevel = defaultZeroLevel + AmandsGraphicsPlugin.FogZeroLevel.Value;
                    }
                    if (FPSCameraSharpen != null)
                    {
                        FPSCameraSharpen.rampOffsetR = 0;
                        FPSCameraSharpen.rampOffsetG = 0;
                        FPSCameraSharpen.rampOffsetB = 0;
                    }
                    if (FPSCameraCC_Vintage != null)
                    {
                        FPSCameraCC_Vintage.SetEnabledUniversal(false);
                    }
                    if (FPSCameraCustomGlobalFog != null)
                    {
                        FPSCameraCustomGlobalFog.FuncStart = AmandsGraphicsPlugin.GlobalFogIntensity.Value;
                        FPSCameraCustomGlobalFog.BlendMode = AmandsGraphicsPlugin.DefaultGlobalFog.Value ? CustomGlobalFog.BlendModes.Lighten : CustomGlobalFog.BlendModes.Normal;
                        if (scene != "Laboratory_Scripts" && scene != "custom_Light" && scene != "Lighthouse_Abadonned_pier" && scene != "Shopping_Mall_Terrain" && scene != "woods_combined" && scene != "Reserve_Base_DesignStuff" && scene != "shoreline_scripts")
                        {
                            FPSCameraCustomGlobalFog.SetEnabledUniversal(false);
                        }
                    }
                    if (FPSCameraGlobalFog != null)
                    {
                        FPSCameraGlobalFog.SetEnabledUniversal(false);
                    }
                    if (FPSCameraColorCorrectionCurves != null)
                    {
                        FPSCameraColorCorrectionCurves.SetEnabledUniversal(false);
                    }
                    if (Weather != null)
                    {
                        if (weatherController != null && weatherController.TimeOfDayController != null && AmandsGraphicsPlugin.LightColorEnabled.Value)
                        {
                            weatherController.TimeOfDayController.LightColor.colorKeys = gradientColorKeys;
                        }
                    }
                }
                else
                {
                    DebugMode = true;
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
                        if (scene != "Laboratory_Scripts" && scene != "custom_Light" && scene != "Lighthouse_Abadonned_pier" && scene != "Shopping_Mall_Terrain" && scene != "woods_combined" && scene != "Reserve_Base_DesignStuff" && scene != "shoreline_scripts")
                        {
                            levelSettings.SkyColor = defaultSkyColor;
                            levelSettings.EquatorColor = defaultEquatorColor;
                            levelSettings.GroundColor = defaultGroundColor;
                            levelSettings.NightVisionSkyColor = defaultNightVisionSkyColor;
                            levelSettings.NightVisionEquatorColor = defaultNightVisionEquatorColor;
                            levelSettings.NightVisionGroundColor = defaultNightVisionGroundColor;
                        }
                    }
                    if (FPSCameraSharpen != null)
                    {
                        FPSCameraSharpen.rampOffsetR = defaultrampOffsetR;
                        FPSCameraSharpen.rampOffsetG = defaultrampOffsetG;
                        FPSCameraSharpen.rampOffsetB = defaultrampOffsetB;
                    }
                    if (FPSCameraCC_Vintage != null)
                    {
                        FPSCameraCC_Vintage.SetEnabledUniversal(defaultFPSCameraCC_Vintage);
                    }
                    if (FPSCameraCustomGlobalFog != null)
                    {
                        FPSCameraCustomGlobalFog.FuncStart = 1;
                        FPSCameraCustomGlobalFog.BlendMode = CustomGlobalFog.BlendModes.Lighten;
                        FPSCameraCustomGlobalFog.SetEnabledUniversal(defaultFPSCameraCustomGlobalFog);
                    }
                    if (FPSCameraGlobalFog != null)
                    {
                        FPSCameraGlobalFog.SetEnabledUniversal(defaultFPSCameraGlobalFog);
                    }
                    if (FPSCameraColorCorrectionCurves != null)
                    {
                        FPSCameraColorCorrectionCurves.SetEnabledUniversal(defaultFPSCameraColorCorrectionCurves);
                    }
                    if (Weather != null)
                    {
                        if (weatherController != null && weatherController.TimeOfDayController != null)
                        {
                            weatherController.TimeOfDayController.LightColor.colorKeys = defaultGradientColorKeys;
                        }
                    }
                }
            }
            UpdateInterval += 1;
            if (UpdateInterval > 200)
            {
                UpdateInterval = 0;
                if (FPSCamera == null)
                {
                    scene = SceneManager.GetActiveScene().name;
                    switch (scene)
                    {
                        case "Laboratory_Scripts":
                            sceneEnabled = AmandsGraphicsPlugin.Labs.Value;
                            break;
                        case "custom_Light":
                            sceneEnabled = AmandsGraphicsPlugin.Customs.Value;
                            break;
                        case "Factory_Day":
                            sceneEnabled = AmandsGraphicsPlugin.FactoryDay.Value;
                            break;
                        case "Factory_Night":
                            sceneEnabled = AmandsGraphicsPlugin.FactoryNight.Value;
                            break;
                        case "Lighthouse_Abadonned_pier":
                            sceneEnabled = AmandsGraphicsPlugin.Lighthouse.Value;
                            break;
                        case "Shopping_Mall_Terrain":
                            sceneEnabled = AmandsGraphicsPlugin.Interchange.Value;
                            break;
                        case "woods_combined":
                            sceneEnabled = AmandsGraphicsPlugin.Woods.Value;
                            break;
                        case "Reserve_Base_DesignStuff":
                            sceneEnabled = AmandsGraphicsPlugin.Reserve.Value;
                            break;
                        case "shoreline_scripts":
                            sceneEnabled = AmandsGraphicsPlugin.Shoreline.Value;
                            break;
                        default:
                            sceneEnabled = AmandsGraphicsPlugin.Hideout.Value;
                            break;
                    }
                    if (!sceneEnabled) return;
                    FPSCamera = GameObject.Find("FPS Camera");
                    if (FPSCamera != null)
                    {
                        DebugMode = false;
                        FPSCameraPostProcessVolume = FPSCamera.GetComponent<PostProcessVolume>();
                        if (FPSCameraPostProcessVolume != null)
                        {
                            FPSCameraPostProcessVolume.profile.TryGetSettings<UnityEngine.Rendering.PostProcessing.MotionBlur>(out FPSCameraMotionBlur);
                            if (FPSCameraMotionBlur != null)
                            {
                                FPSCameraMotionBlur.enabled.Override(AmandsGraphicsPlugin.UnrealMotionBlurEnabled.Value);
                                FPSCameraMotionBlur.sampleCount.Override(AmandsGraphicsPlugin.UnrealMotionBlurSampleCount.Value);
                                FPSCameraMotionBlur.shutterAngle.Override(AmandsGraphicsPlugin.UnrealMotionBlurShutterAngle.Value);
                            }
                            else
                            {
                                FPSCameraPostProcessVolume.profile.AddSettings<UnityEngine.Rendering.PostProcessing.MotionBlur>();
                                FPSCameraPostProcessVolume.profile.TryGetSettings<UnityEngine.Rendering.PostProcessing.MotionBlur>(out FPSCameraMotionBlur);
                                if (FPSCameraMotionBlur != null)
                                {
                                    FPSCameraMotionBlur.enabled.Override(AmandsGraphicsPlugin.UnrealMotionBlurEnabled.Value);
                                    FPSCameraMotionBlur.sampleCount.Override(AmandsGraphicsPlugin.UnrealMotionBlurSampleCount.Value);
                                    FPSCameraMotionBlur.shutterAngle.Override(AmandsGraphicsPlugin.UnrealMotionBlurShutterAngle.Value);
                                }
                            }
                        }
                        FPSCameraSharpen = FPSCamera.GetComponent<CC_Sharpen>();
                        if (FPSCameraSharpen != null)
                        {
                            defaultrampOffsetR = FPSCameraSharpen.rampOffsetR;
                            defaultrampOffsetG = FPSCameraSharpen.rampOffsetG;
                            defaultrampOffsetB = FPSCameraSharpen.rampOffsetB;
                            FPSCameraSharpen.SetEnabledUniversal(true);
                            FPSCameraSharpen.rampOffsetR = 0;
                            FPSCameraSharpen.rampOffsetG = 0;
                            FPSCameraSharpen.rampOffsetB = 0;
                        }
                        FPSCameraBloomAndFlares = FPSCamera.GetComponents<BloomAndFlares>();
                        foreach (BloomAndFlares bloomAndFlares in FPSCameraBloomAndFlares)
                        {
                            bloomAndFlares.bloomIntensity *= AmandsGraphicsPlugin.BloomIntensity.Value;
                        }
                        FPSCameraPrismEffects = FPSCamera.GetComponent<PrismEffects>();
                        if (FPSCameraPrismEffects != null)
                        {
                            defaulttoneValues = FPSCameraPrismEffects.toneValues;
                            defaultsecondaryToneValues = FPSCameraPrismEffects.secondaryToneValues;
                            defaultuseLut = FPSCameraPrismEffects.useLut;
                            switch (scene)
                            {
                                case "Laboratory_Scripts":
                                    FPSCameraPrismEffects.tonemapType = Prism.Utils.TonemapType.ACES;
                                    FPSCameraPrismEffects.toneValues = AmandsGraphicsPlugin.LabsToneValues.Value;
                                    FPSCameraPrismEffects.secondaryToneValues = AmandsGraphicsPlugin.LabsSecondaryToneValues.Value;
                                    FPSCameraPrismEffects.useLut = false;
                                    levelSettings = GameObject.Find("---Laboratory_levelsettings---").GetComponent<LevelSettings>();
                                    if (levelSettings != null)
                                    {
                                        defaultZeroLevel = levelSettings.ZeroLevel;
                                        levelSettings.ZeroLevel = defaultZeroLevel + AmandsGraphicsPlugin.FogZeroLevel.Value;
                                    }
                                    break;
                                case "custom_Light":
                                    FPSCameraPrismEffects.tonemapType = Prism.Utils.TonemapType.ACES;
                                    FPSCameraPrismEffects.toneValues = AmandsGraphicsPlugin.CustomsToneValues.Value;
                                    FPSCameraPrismEffects.secondaryToneValues = AmandsGraphicsPlugin.CustomsSecondaryToneValues.Value;
                                    FPSCameraPrismEffects.useLut = false;
                                    levelSettings = GameObject.Find("---Custom_levelsettings---").GetComponent<LevelSettings>();
                                    if (levelSettings != null)
                                    {
                                        defaultZeroLevel = levelSettings.ZeroLevel;
                                        levelSettings.ZeroLevel = defaultZeroLevel + AmandsGraphicsPlugin.FogZeroLevel.Value;
                                    }
                                    break;
                                case "Factory_Day":
                                    FPSCameraPrismEffects.tonemapType = Prism.Utils.TonemapType.ACES;
                                    FPSCameraPrismEffects.toneValues = AmandsGraphicsPlugin.FactoryDayToneValues.Value;
                                    FPSCameraPrismEffects.secondaryToneValues = AmandsGraphicsPlugin.FactoryDaySecondaryToneValues.Value;
                                    FPSCameraPrismEffects.useLut = false;
                                    levelSettings = GameObject.Find("---FactoryDay_levelsettings---").GetComponent<LevelSettings>();
                                    if (levelSettings != null)
                                    {
                                        defaultZeroLevel = levelSettings.ZeroLevel;
                                        levelSettings.ZeroLevel = defaultZeroLevel + AmandsGraphicsPlugin.FogZeroLevel.Value;
                                        defaultSkyColor = levelSettings.SkyColor;
                                        defaultEquatorColor = levelSettings.EquatorColor;
                                        defaultGroundColor = levelSettings.GroundColor;
                                        defaultNightVisionSkyColor = levelSettings.NightVisionSkyColor;
                                        defaultNightVisionEquatorColor = levelSettings.NightVisionEquatorColor;
                                        defaultNightVisionGroundColor = levelSettings.NightVisionGroundColor;
                                        levelSettings.SkyColor = AmandsGraphicsPlugin.FactoryDaySkyColor.Value / 10;//new Color(0.09f, 0.08f, 0.07f);
                                        levelSettings.EquatorColor = AmandsGraphicsPlugin.FactoryDaySkyColor.Value / 10;
                                        levelSettings.GroundColor = AmandsGraphicsPlugin.FactoryDaySkyColor.Value / 10;
                                        levelSettings.NightVisionSkyColor = AmandsGraphicsPlugin.FactoryDayNVSkyColor.Value / 10;
                                        levelSettings.NightVisionEquatorColor = AmandsGraphicsPlugin.FactoryDayNVSkyColor.Value / 10;
                                        levelSettings.NightVisionGroundColor = AmandsGraphicsPlugin.FactoryDayNVSkyColor.Value / 10;
                                    }
                                    sunLight = GameObject.Find("sun").GetComponent<Light>();
                                    if (sunLight != null)
                                    {
                                    }
                                    break;
                                case "Factory_Night":
                                    FPSCameraPrismEffects.tonemapType = Prism.Utils.TonemapType.ACES;
                                    FPSCameraPrismEffects.toneValues = AmandsGraphicsPlugin.FactoryNightToneValues.Value;
                                    FPSCameraPrismEffects.secondaryToneValues = AmandsGraphicsPlugin.FactoryNightSecondaryToneValues.Value;
                                    FPSCameraPrismEffects.useLut = false;
                                    levelSettings = GameObject.Find("---FactoryNight_levelsettings---").GetComponent<LevelSettings>();
                                    if (levelSettings != null)
                                    {
                                        defaultZeroLevel = levelSettings.ZeroLevel;
                                        levelSettings.ZeroLevel = defaultZeroLevel + AmandsGraphicsPlugin.FogZeroLevel.Value;
                                        defaultSkyColor = levelSettings.SkyColor;
                                        defaultEquatorColor = levelSettings.EquatorColor;
                                        defaultGroundColor = levelSettings.GroundColor;
                                        defaultNightVisionSkyColor = levelSettings.NightVisionSkyColor;
                                        defaultNightVisionEquatorColor = levelSettings.NightVisionEquatorColor;
                                        defaultNightVisionGroundColor = levelSettings.NightVisionGroundColor;
                                        levelSettings.SkyColor = AmandsGraphicsPlugin.FactoryNightSkyColor.Value / 10;//Color.black;
                                        levelSettings.EquatorColor = AmandsGraphicsPlugin.FactoryNightSkyColor.Value / 10;
                                        levelSettings.GroundColor = AmandsGraphicsPlugin.FactoryNightSkyColor.Value / 10;
                                        levelSettings.NightVisionSkyColor = AmandsGraphicsPlugin.FactoryNightNVSkyColor.Value / 10;
                                        levelSettings.NightVisionEquatorColor = AmandsGraphicsPlugin.FactoryNightNVSkyColor.Value / 10;
                                        levelSettings.NightVisionGroundColor = AmandsGraphicsPlugin.FactoryNightNVSkyColor.Value / 10;
                                    }
                                    break;
                                case "Lighthouse_Abadonned_pier":
                                    FPSCameraPrismEffects.tonemapType = Prism.Utils.TonemapType.ACES;
                                    FPSCameraPrismEffects.toneValues = AmandsGraphicsPlugin.LighthouseToneValues.Value;
                                    FPSCameraPrismEffects.secondaryToneValues = AmandsGraphicsPlugin.LighthouseSecondaryToneValues.Value;
                                    FPSCameraPrismEffects.useLut = false;
                                    levelSettings = GameObject.Find("---Lighthouse_levelsettings---").GetComponent<LevelSettings>();
                                    if (levelSettings != null)
                                    {
                                        defaultZeroLevel = levelSettings.ZeroLevel;
                                        levelSettings.ZeroLevel = defaultZeroLevel + AmandsGraphicsPlugin.FogZeroLevel.Value;
                                    }
                                    break;
                                case "Shopping_Mall_Terrain":
                                    FPSCameraPrismEffects.tonemapType = Prism.Utils.TonemapType.ACES;
                                    FPSCameraPrismEffects.toneValues = AmandsGraphicsPlugin.InterchangeToneValues.Value;
                                    FPSCameraPrismEffects.secondaryToneValues = AmandsGraphicsPlugin.InterchangeSecondaryToneValues.Value;
                                    FPSCameraPrismEffects.useLut = false;
                                    levelSettings = GameObject.Find("---Interchange_levelsettings---").GetComponent<LevelSettings>();
                                    if (levelSettings != null)
                                    {
                                        defaultZeroLevel = levelSettings.ZeroLevel;
                                        levelSettings.ZeroLevel = defaultZeroLevel + AmandsGraphicsPlugin.FogZeroLevel.Value;
                                    }
                                    break;
                                case "woods_combined":
                                    FPSCameraPrismEffects.tonemapType = Prism.Utils.TonemapType.ACES;
                                    FPSCameraPrismEffects.toneValues = AmandsGraphicsPlugin.WoodsToneValues.Value;
                                    FPSCameraPrismEffects.secondaryToneValues = AmandsGraphicsPlugin.WoodsSecondaryToneValues.Value;
                                    FPSCameraPrismEffects.useLut = false;
                                    levelSettings = GameObject.Find("---Woods_levelsettings---").GetComponent<LevelSettings>();
                                    if (levelSettings != null)
                                    {
                                        defaultZeroLevel = levelSettings.ZeroLevel;
                                        levelSettings.ZeroLevel = defaultZeroLevel + AmandsGraphicsPlugin.FogZeroLevel.Value;
                                    }
                                    break;
                                case "Reserve_Base_DesignStuff":
                                    FPSCameraPrismEffects.tonemapType = Prism.Utils.TonemapType.ACES;
                                    FPSCameraPrismEffects.toneValues = AmandsGraphicsPlugin.ReserveToneValues.Value;
                                    FPSCameraPrismEffects.secondaryToneValues = AmandsGraphicsPlugin.ReserveSecondaryToneValues.Value;
                                    FPSCameraPrismEffects.useLut = false;
                                    levelSettings = GameObject.Find("---Reserve_levelsettings---").GetComponent<LevelSettings>();
                                    if (levelSettings != null)
                                    {
                                        defaultZeroLevel = levelSettings.ZeroLevel;
                                        levelSettings.ZeroLevel = defaultZeroLevel + AmandsGraphicsPlugin.FogZeroLevel.Value;
                                    }
                                    break;
                                case "shoreline_scripts":
                                    FPSCameraPrismEffects.tonemapType = Prism.Utils.TonemapType.ACES;
                                    FPSCameraPrismEffects.toneValues = AmandsGraphicsPlugin.ShorelineToneValues.Value;
                                    FPSCameraPrismEffects.secondaryToneValues = AmandsGraphicsPlugin.ShorelineSecondaryToneValues.Value;
                                    FPSCameraPrismEffects.useLut = false;
                                    levelSettings = GameObject.Find("---ShoreLine_levelsettings---").GetComponent<LevelSettings>();
                                    if (levelSettings != null)
                                    {
                                        defaultZeroLevel = levelSettings.ZeroLevel;
                                        levelSettings.ZeroLevel = defaultZeroLevel + AmandsGraphicsPlugin.FogZeroLevel.Value;
                                    }
                                    break;
                                default:
                                    FPSCameraPrismEffects.tonemapType = Prism.Utils.TonemapType.ACES;
                                    FPSCameraPrismEffects.toneValues = AmandsGraphicsPlugin.HideoutToneValues.Value;
                                    FPSCameraPrismEffects.secondaryToneValues = AmandsGraphicsPlugin.HideoutSecondaryToneValues.Value;
                                    FPSCameraPrismEffects.useLut = false;
                                    levelSettings = GameObject.Find("!settings").GetComponent<LevelSettings>();
                                    if (levelSettings != null)
                                    {
                                        defaultZeroLevel = levelSettings.ZeroLevel;
                                        levelSettings.ZeroLevel = defaultZeroLevel + AmandsGraphicsPlugin.FogZeroLevel.Value;
                                        defaultSkyColor = levelSettings.SkyColor;
                                        defaultEquatorColor = levelSettings.EquatorColor;
                                        defaultGroundColor = levelSettings.GroundColor;
                                        defaultNightVisionSkyColor = levelSettings.NightVisionSkyColor;
                                        defaultNightVisionEquatorColor = levelSettings.NightVisionEquatorColor;
                                        defaultNightVisionGroundColor = levelSettings.NightVisionGroundColor;
                                        levelSettings.SkyColor = AmandsGraphicsPlugin.HideoutSkyColor.Value / 10;
                                        levelSettings.EquatorColor = AmandsGraphicsPlugin.HideoutSkyColor.Value / 10;
                                        levelSettings.GroundColor = AmandsGraphicsPlugin.HideoutSkyColor.Value / 10;
                                        levelSettings.NightVisionSkyColor = Color.black;
                                        levelSettings.NightVisionEquatorColor = Color.black;
                                        levelSettings.NightVisionGroundColor = Color.black;
                                    }
                                    break;
                            }
                        }
                        FPSCameraCC_Vintage = FPSCamera.GetComponent<CC_Vintage>();
                        if (FPSCameraCC_Vintage != null)
                        {
                            defaultFPSCameraCC_Vintage = FPSCameraCC_Vintage.IsEnabledUniversal();
                            FPSCameraCC_Vintage.SetEnabledUniversal(false);
                        }
                        FPSCameraCustomGlobalFog = FPSCamera.GetComponent<CustomGlobalFog>();
                        if (FPSCameraCustomGlobalFog != null)
                        {
                            FPSCameraCustomGlobalFog.FuncStart = AmandsGraphicsPlugin.GlobalFogIntensity.Value;
                            FPSCameraCustomGlobalFog.BlendMode = AmandsGraphicsPlugin.DefaultGlobalFog.Value ? CustomGlobalFog.BlendModes.Lighten : CustomGlobalFog.BlendModes.Normal;
                            defaultFPSCameraCustomGlobalFog = FPSCameraCustomGlobalFog.IsEnabledUniversal();
                            if (scene != "Laboratory_Scripts" && scene != "custom_Light" && scene != "Lighthouse_Abadonned_pier" && scene != "Shopping_Mall_Terrain" && scene != "woods_combined" && scene != "Reserve_Base_DesignStuff" && scene != "shoreline_scripts")
                            {
                                FPSCameraCustomGlobalFog.SetEnabledUniversal(false);
                            }
                        }
                        foreach (Component component in FPSCamera.GetComponents<Component>())
                        {
                            if (component.ToString() == "FPS Camera (UnityStandardAssets.ImageEffects.GlobalFog)")
                            {
                                FPSCameraGlobalFog = component;
                                defaultFPSCameraGlobalFog = FPSCameraGlobalFog.IsEnabledUniversal();
                                FPSCameraGlobalFog.SetEnabledUniversal(false);
                                break;
                            }
                        }
                        FPSCameraColorCorrectionCurves = FPSCamera.GetComponent<ColorCorrectionCurves>();
                        if (FPSCameraColorCorrectionCurves != null)
                        {
                            defaultFPSCameraColorCorrectionCurves = FPSCameraColorCorrectionCurves.IsEnabledUniversal();
                            FPSCameraColorCorrectionCurves.SetEnabledUniversal(false);
                        }
                        Weather = GameObject.Find("Weather");
                        if (Weather != null)
                        {
                            weatherController = Weather.GetComponent<WeatherController>();
                            if (weatherController.TimeOfDayController != null)
                            {
                                defaultGradientColorKeys = weatherController.TimeOfDayController.LightColor.colorKeys;
                                gradientColorKeys = weatherController.TimeOfDayController.LightColor.colorKeys;//new GradientColorKey[] { new GradientColorKey(Color.black, 0.0f), new GradientColorKey(Color.black, 0.5115129f), new GradientColorKey(Color.black, 0.5266652f), new GradientColorKey(Color.black, 0.5535668f), new GradientColorKey(Color.black, 0.6971694f), new GradientColorKey(Color.black, 0.9992523f) };
                                gradientColorKeys[0] = new GradientColorKey(AmandsGraphicsPlugin.LightColorIndex0.Value, 0.0f);
                                gradientColorKeys[1] = new GradientColorKey(AmandsGraphicsPlugin.LightColorIndex1.Value, 0.5115129f);
                                gradientColorKeys[2] = new GradientColorKey(AmandsGraphicsPlugin.LightColorIndex2.Value, 0.5266652f);
                                gradientColorKeys[3] = new GradientColorKey(AmandsGraphicsPlugin.LightColorIndex3.Value, 0.5535668f);
                                gradientColorKeys[4] = new GradientColorKey(AmandsGraphicsPlugin.LightColorIndex4.Value, 0.6971694f);
                                gradientColorKeys[5] = new GradientColorKey(AmandsGraphicsPlugin.LightColorIndex5.Value, 0.9992523f);
                                if (AmandsGraphicsPlugin.LightColorEnabled.Value)
                                {
                                    weatherController.TimeOfDayController.LightColor.colorKeys = gradientColorKeys;
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}

