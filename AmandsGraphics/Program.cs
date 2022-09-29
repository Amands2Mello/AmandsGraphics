
using Aki.Common.Utils;
using Aki.Reflection.Patching;
using BepInEx;
using BepInEx.Configuration;
using System;
using System.IO;
using System.Reflection;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace AmandsGraphics
{
    [BepInPlugin("com.Amanda.Graphics", "Graphics", "1.1.0")]
    public class AmandsGraphicsPlugin : BaseUnityPlugin
    {
        public static GameObject Hook;
        public static AmandsGraphicsClass AmandsGraphicsClassComponent;

        public static ConfigEntry<KeyboardShortcut> GraphicsToggle { get; set; }
        public static ConfigEntry<EDebugMode> DebugMode { get; set; }

        public static ConfigEntry<bool> MotionBlur { get; set; }
        public static ConfigEntry<int> MotionBlurSampleCount { get; set; }
        public static ConfigEntry<float> MotionBlurShutterAngle { get; set; }
        public static ConfigEntry<bool> HBAO { get; set; }
        public static ConfigEntry<float> HBAOIntensity { get; set; }
        public static ConfigEntry<float> HBAOSaturation { get; set; }
        public static ConfigEntry<float> HBAOAlbedoMultiplier { get; set; }
        public static ConfigEntry<bool> DepthOfField { get; set; }
        public static ConfigEntry<float> DepthOfFieldFocusDistance { get; set; }
        public static ConfigEntry<float> DepthOfFieldAperture { get; set; }
        public static ConfigEntry<float> DepthOfFieldFocalLength { get; set; }
        public static ConfigEntry<float> DepthOfFieldFocalLengthOff { get; set; }
        public static ConfigEntry<KernelSize> DepthOfFieldKernelSize { get; set; }
        public static ConfigEntry<float> DepthOfFieldSpeed { get; set; }
        public static ConfigEntry<float> DepthOfFieldFOV { get; set; }

        public static ConfigEntry<float> Brightness { get; set; }
        public static ConfigEntry<EGlobalTonemap> Tonemap { get; set; }
        public static ConfigEntry<bool> useLut { get; set; }
        public static ConfigEntry<float> BloomIntensity { get; set; }
        public static ConfigEntry<bool> CC_Vintage { get; set; }
        public static ConfigEntry<bool> CC_Sharpen { get; set; }
        public static ConfigEntry<bool> CustomGlobalFog { get; set; }
        public static ConfigEntry<float> CustomGlobalFogIntensity { get; set; }
        public static ConfigEntry<bool> GlobalFog { get; set; }
        public static ConfigEntry<bool> ColorCorrectionCurves { get; set; }
        public static ConfigEntry<bool> LightsUseLinearIntensity { get; set; }
        public static ConfigEntry<bool> SunColor { get; set; }
        public static ConfigEntry<bool> SkyColor { get; set; }
        public static ConfigEntry<bool> NVGColorsFix { get; set; }
        public static ConfigEntry<string> PresetName { get; set; }
        public static ConfigEntry<string> SavePreset { get; set; }
        public static ConfigEntry<string> LoadPreset { get; set; }

        public static ConfigEntry<float> CustomsFogLevel { get; set; }
        public static ConfigEntry<float> LighthouseFogLevel { get; set; }
        public static ConfigEntry<float> InterchangeFogLevel { get; set; }
        public static ConfigEntry<float> WoodsFogLevel { get; set; }
        public static ConfigEntry<float> ReserveFogLevel { get; set; }
        public static ConfigEntry<float> ShorelineFogLevel { get; set; }

        public static ConfigEntry<ETonemap> LabsTonemap { get; set; }
        public static ConfigEntry<ETonemap> CustomsTonemap { get; set; }
        public static ConfigEntry<ETonemap> FactoryTonemap { get; set; }
        public static ConfigEntry<ETonemap> FactoryNightTonemap { get; set; }
        public static ConfigEntry<ETonemap> LighthouseTonemap { get; set; }
        public static ConfigEntry<ETonemap> InterchangeTonemap { get; set; }
        public static ConfigEntry<ETonemap> WoodsTonemap { get; set; }
        public static ConfigEntry<ETonemap> ReserveTonemap { get; set; }
        public static ConfigEntry<ETonemap> ShorelineTonemap { get; set; }
        public static ConfigEntry<ETonemap> HideoutTonemap { get; set; }

        public static ConfigEntry<Vector3> LabsACES { get; set; }
        public static ConfigEntry<Vector3> LabsACESS { get; set; }
        public static ConfigEntry<Vector3> CustomsACES { get; set; }
        public static ConfigEntry<Vector3> CustomsACESS { get; set; }
        public static ConfigEntry<Vector3> FactoryACES { get; set; }
        public static ConfigEntry<Vector3> FactoryACESS { get; set; }
        public static ConfigEntry<Vector3> FactoryNightACES { get; set; }
        public static ConfigEntry<Vector3> FactoryNightACESS { get; set; }
        public static ConfigEntry<Vector3> LighthouseACES { get; set; }
        public static ConfigEntry<Vector3> LighthouseACESS { get; set; }
        public static ConfigEntry<Vector3> InterchangeACES { get; set; }
        public static ConfigEntry<Vector3> InterchangeACESS { get; set; }
        public static ConfigEntry<Vector3> WoodsACES { get; set; }
        public static ConfigEntry<Vector3> WoodsACESS { get; set; }
        public static ConfigEntry<Vector3> ReserveACES { get; set; }
        public static ConfigEntry<Vector3> ReserveACESS { get; set; }
        public static ConfigEntry<Vector3> ShorelineACES { get; set; }
        public static ConfigEntry<Vector3> ShorelineACESS { get; set; }
        public static ConfigEntry<Vector3> HideoutACES { get; set; }
        public static ConfigEntry<Vector3> HideoutACESS { get; set; }

        public static ConfigEntry<Vector3> LabsFilmic { get; set; }
        public static ConfigEntry<Vector3> LabsFilmicS { get; set; }
        public static ConfigEntry<Vector3> CustomsFilmic { get; set; }
        public static ConfigEntry<Vector3> CustomsFilmicS { get; set; }
        public static ConfigEntry<Vector3> FactoryFilmic { get; set; }
        public static ConfigEntry<Vector3> FactoryFilmicS { get; set; }
        public static ConfigEntry<Vector3> FactoryNightFilmic { get; set; }
        public static ConfigEntry<Vector3> FactoryNightFilmicS { get; set; }
        public static ConfigEntry<Vector3> LighthouseFilmic { get; set; }
        public static ConfigEntry<Vector3> LighthouseFilmicS { get; set; }
        public static ConfigEntry<Vector3> InterchangeFilmic { get; set; }
        public static ConfigEntry<Vector3> InterchangeFilmicS { get; set; }
        public static ConfigEntry<Vector3> WoodsFilmic { get; set; }
        public static ConfigEntry<Vector3> WoodsFilmicS { get; set; }
        public static ConfigEntry<Vector3> ReserveFilmic { get; set; }
        public static ConfigEntry<Vector3> ReserveFilmicS { get; set; }
        public static ConfigEntry<Vector3> ShorelineFilmic { get; set; }
        public static ConfigEntry<Vector3> ShorelineFilmicS { get; set; }
        public static ConfigEntry<Vector3> HideoutFilmic { get; set; }
        public static ConfigEntry<Vector3> HideoutFilmicS { get; set; }

        public static ConfigEntry<Vector4> FactorySkyColor { get; set; }
        public static ConfigEntry<Vector4> FactoryNVSkyColor { get; set; }
        public static ConfigEntry<Vector4> FactoryNightSkyColor { get; set; }
        public static ConfigEntry<Vector4> FactoryNightNVSkyColor { get; set; }
        public static ConfigEntry<Vector4> HideoutSkyColor { get; set; }

        public static ConfigEntry<Vector4> LightColorIndex0 { get; set; }
        public static ConfigEntry<Vector4> LightColorIndex1 { get; set; }
        public static ConfigEntry<Vector4> LightColorIndex2 { get; set; }
        public static ConfigEntry<Vector4> LightColorIndex3 { get; set; }
        public static ConfigEntry<Vector4> LightColorIndex4 { get; set; }
        public static ConfigEntry<Vector4> LightColorIndex5 { get; set; }

        private void Awake()
        {
            Debug.LogError("Graphics Awake()");
            Hook = new GameObject();
            AmandsGraphicsClassComponent = Hook.AddComponent<AmandsGraphicsClass>();
            DontDestroyOnLoad(Hook);
        }
        private static void SavePresetDrawer(ConfigEntryBase entry)
        {
            bool button = GUILayout.Button("Save Preset", GUILayout.ExpandWidth(true));
            if (button) SaveAmandsGraphicsPreset();
        }
        private static void LoadPresetDrawer(ConfigEntryBase entry)
        {
            bool button = GUILayout.Button("Load Preset", GUILayout.ExpandWidth(true));
            if (button) LoadAmandsGraphicsPreset();
        }
        private void Start()
        {
            string AmandsFeatures = "AmandsGraphics Features";
            string AmandsCinematic = "AmandsGraphics Cinematic";

            PresetName = Config.Bind<string>("AmandsGraphics Preset", "PresetName", "Experimental", new ConfigDescription("EXPERIMENTAL", null, new ConfigurationManagerAttributes { Order = 120 }));
            SavePreset = Config.Bind<string>("AmandsGraphics Preset", "SavePreset", "", new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 110, HideSettingName = true, CustomDrawer = SavePresetDrawer }));
            LoadPreset = Config.Bind<string>("AmandsGraphics Preset", "LoadPreset", "", new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 100, HideSettingName = true, CustomDrawer = LoadPresetDrawer }));

            GraphicsToggle = Config.Bind<KeyboardShortcut>(AmandsFeatures, "GraphicsToggle", new KeyboardShortcut(KeyCode.Insert), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 510 }));
            DebugMode = Config.Bind<EDebugMode>(AmandsFeatures, "DebugMode", EDebugMode.HBAO, new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 500 }));

            MotionBlur = Config.Bind<bool>(AmandsCinematic, "MotionBlur", false, new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 250, IsAdvanced = true }));
            MotionBlurSampleCount = Config.Bind<int>(AmandsCinematic, "MotionBlur SampleCount", 20, new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 240, IsAdvanced = true }));
            MotionBlurShutterAngle = Config.Bind<float>(AmandsCinematic, "MotionBlur ShutterAngle", 360f, new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 230, IsAdvanced = true }));
            HBAO = Config.Bind<bool>(AmandsCinematic, "HBAO", true, new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 220, IsAdvanced = true }));
            HBAOIntensity = Config.Bind<float>(AmandsCinematic, "HBAO Intensity", 1.25f, new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 210, IsAdvanced = true }));
            HBAOSaturation = Config.Bind<float>(AmandsCinematic, "HBAO Saturation", 1.5f, new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 200, IsAdvanced = true }));
            HBAOAlbedoMultiplier = Config.Bind<float>(AmandsCinematic, "HBAO Albedo Multiplier", 2f, new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 190, IsAdvanced = true }));
            DepthOfField = Config.Bind<bool>(AmandsCinematic, "DepthOfField", false, new ConfigDescription("EXPERIMENTAL Use FOV 51-75", null, new ConfigurationManagerAttributes { Order = 180, IsAdvanced = true }));
            DepthOfFieldFocusDistance = Config.Bind<float>(AmandsCinematic, "DepthOfField FocusDistance", 0.11f, new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 170, IsAdvanced = true }));
            DepthOfFieldAperture = Config.Bind<float>(AmandsCinematic, "DepthOfField Aperture", 100f, new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 160, IsAdvanced = true }));
            DepthOfFieldFocalLength = Config.Bind<float>(AmandsCinematic, "DepthOfField Focal Length", 50f, new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 150, IsAdvanced = true }));
            DepthOfFieldFocalLengthOff = Config.Bind<float>(AmandsCinematic, "DepthOfField Focal Length OFF", 25f, new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 140, IsAdvanced = true }));
            DepthOfFieldKernelSize = Config.Bind<KernelSize>(AmandsCinematic, "DepthOfField Kernel Size", KernelSize.VeryLarge, new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 130, IsAdvanced = true }));
            DepthOfFieldSpeed = Config.Bind<float>(AmandsCinematic, "DepthOfField Speed", 0.1f, new ConfigDescription("", new AcceptableValueRange<float>(0.01f, 1f), new ConfigurationManagerAttributes { Order = 120, IsAdvanced = true }));
            DepthOfFieldFOV = Config.Bind<float>(AmandsCinematic, "DepthOfField FOV", 36f, new ConfigDescription("Activates DepthOfField when fov zoom is lower than this value. Recommended values for different FOVs: FOV 51 = 36, FOV 75 = 60", new AcceptableValueRange<float>(36f, 60f), new ConfigurationManagerAttributes { Order = 110, IsAdvanced = true }));

            Brightness = Config.Bind<float>(AmandsFeatures, "Brightness", 0.5f, new ConfigDescription("EXPERIMENTAL", new AcceptableValueRange<float>(0f,1f), new ConfigurationManagerAttributes { Order = 340 }));
            Tonemap = Config.Bind<EGlobalTonemap>(AmandsFeatures, "Tonemap", EGlobalTonemap.ACES, new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 330 }));
            useLut = Config.Bind<bool>(AmandsFeatures, "useLut", false, new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 320 }));
            BloomIntensity = Config.Bind<float>(AmandsFeatures, "Bloom Intensity", 0.5f, new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 310 }));
            CC_Vintage = Config.Bind<bool>(AmandsFeatures, "CC_Vintage", false, new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 300 }));
            CC_Sharpen = Config.Bind<bool>(AmandsFeatures, "CC_Sharpen", false, new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 290 }));
            CustomGlobalFog = Config.Bind<bool>(AmandsFeatures, "CustomGlobalFog", false, new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 280 }));
            CustomGlobalFogIntensity = Config.Bind<float>(AmandsFeatures, "CustomGlobalFog Intensity", 0.1f, new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 270 }));
            GlobalFog = Config.Bind<bool>(AmandsFeatures, "GlobalFog", false, new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 260 }));
            ColorCorrectionCurves = Config.Bind<bool>(AmandsFeatures, "ColorCorrectionCurves", false, new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 250 }));
            LightsUseLinearIntensity = Config.Bind<bool>(AmandsFeatures, "LightsUseLinearIntensity", false, new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 240 }));
            SunColor = Config.Bind<bool>(AmandsFeatures, "SunColor", true, new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 230 }));
            SkyColor = Config.Bind<bool>(AmandsFeatures, "SkyColor", true, new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 220 }));
            NVGColorsFix = Config.Bind<bool>(AmandsFeatures, "NVGColorsFix", true, new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 210 }));

            LabsTonemap = Config.Bind<ETonemap>("Labs", "Tonemap", ETonemap.ACES, new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 150 }));
            LabsACES = Config.Bind<Vector3>("Labs", "ACES", new Vector3(20, 0.2f, 20), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 140, IsAdvanced = true }));
            LabsACESS = Config.Bind<Vector3>("Labs", "ACESS", new Vector3(0, 2, 0), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 130, IsAdvanced = true }));
            LabsFilmic = Config.Bind<Vector3>("Labs", "Filmic", new Vector3(2f, 2f, 1.75f), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 120, IsAdvanced = true }));
            LabsFilmicS = Config.Bind<Vector3>("Labs", "FilmicS", new Vector3(0, 0.5f, 0), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 110, IsAdvanced = true }));

            CustomsFogLevel = Config.Bind<float>("Customs", "Fog Level", -100.0f, new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 160 }));
            CustomsTonemap = Config.Bind<ETonemap>("Customs", "Tonemap", ETonemap.ACES, new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 150 }));
            CustomsACES = Config.Bind<Vector3>("Customs", "ACES", new Vector3(20, 0.2f, 20), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 140, IsAdvanced = true }));
            CustomsACESS = Config.Bind<Vector3>("Customs", "ACESS", new Vector3(0, 1, 0), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 130, IsAdvanced = true }));
            CustomsFilmic = Config.Bind<Vector3>("Customs", "Filmic", new Vector3(1f, 2f, 1.75f), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 120, IsAdvanced = true }));
            CustomsFilmicS = Config.Bind<Vector3>("Customs", "FilmicS", new Vector3(0, 0.25f, 0), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 110, IsAdvanced = true }));

            FactoryTonemap = Config.Bind<ETonemap>("Factory", "Tonemap", ETonemap.ACES, new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 170 }));
            FactoryACES = Config.Bind<Vector3>("Factory", "ACES", new Vector3(15, 1, 15), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 160, IsAdvanced = true }));
            FactoryACESS = Config.Bind<Vector3>("Factory", "ACESS", new Vector3(0, 1, 0), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 150, IsAdvanced = true }));
            FactoryFilmic = Config.Bind<Vector3>("Factory", "Filmic", new Vector3(1f, 2f, 1.75f), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 140, IsAdvanced = true }));
            FactoryFilmicS = Config.Bind<Vector3>("Factory", "FilmicS", new Vector3(0, 0.22f, 0), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 130, IsAdvanced = true }));
            FactorySkyColor = Config.Bind<Vector4>("Factory", "SkyColor", new Vector4(0.9f, 0.8f, 0.7f), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 120, IsAdvanced = true }));
            FactoryNVSkyColor = Config.Bind<Vector4>("Factory", "NVSkyColor", new Vector4(0.9f, 0.8f, 0.7f), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 110, IsAdvanced = true }));

            FactoryNightTonemap = Config.Bind<ETonemap>("FactoryNight", "Tonemap", ETonemap.ACES, new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 170 }));
            FactoryNightACES = Config.Bind<Vector3>("FactoryNight", "ACES", new Vector3(10, -0.2f, 10), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 160, IsAdvanced = true }));
            FactoryNightACESS = Config.Bind<Vector3>("FactoryNight", "ACESS", new Vector3(0, 1, 0), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 150, IsAdvanced = true }));
            FactoryNightFilmic = Config.Bind<Vector3>("FactoryNight", "Filmic", new Vector3(1f, 2f, 1.75f), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 140, IsAdvanced = true }));
            FactoryNightFilmicS = Config.Bind<Vector3>("FactoryNight", "FilmicS", new Vector3(0, 0.5f, 0), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 130, IsAdvanced = true }));
            FactoryNightSkyColor = Config.Bind<Vector4>("FactoryNight", "SkyColor", new Vector4(0.09f, 0.08f, 0.07f), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 120, IsAdvanced = true }));
            FactoryNightNVSkyColor = Config.Bind<Vector4>("FactoryNight", "NVSkyColor", new Vector4(0.25f, 0.25f, 0.25f), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 110, IsAdvanced = true }));

            LighthouseFogLevel = Config.Bind<float>("Lighthouse", "Fog Level", -100.0f, new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 160 }));
            LighthouseTonemap = Config.Bind<ETonemap>("Lighthouse", "Tonemap", ETonemap.ACES, new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 150 }));
            LighthouseACES = Config.Bind<Vector3>("Lighthouse", "ACES", new Vector3(20, 0.2f, 20), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 140, IsAdvanced = true }));
            LighthouseACESS = Config.Bind<Vector3>("Lighthouse", "ACESS", new Vector3(0, 1, 0), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 130, IsAdvanced = true }));
            LighthouseFilmic = Config.Bind<Vector3>("Lighthouse", "Filmic", new Vector3(1f, 2f, 1.75f), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 120, IsAdvanced = true }));
            LighthouseFilmicS = Config.Bind<Vector3>("Lighthouse", "FilmicS", new Vector3(0, 0.25f, 0), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 110, IsAdvanced = true }));

            InterchangeFogLevel = Config.Bind<float>("Interchange", "Fog Level", -100.0f, new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 160 }));
            InterchangeTonemap = Config.Bind<ETonemap>("Interchange", "Tonemap", ETonemap.ACES, new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 150 }));
            InterchangeACES = Config.Bind<Vector3>("Interchange", "ACES", new Vector3(20, 3, 20), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 140, IsAdvanced = true }));
            InterchangeACESS = Config.Bind<Vector3>("Interchange", "ACESS", new Vector3(0.50f, 1, 0), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 130, IsAdvanced = true }));
            InterchangeFilmic = Config.Bind<Vector3>("Interchange", "Filmic", new Vector3(1f, 2f, 1.75f), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 120, IsAdvanced = true }));
            InterchangeFilmicS = Config.Bind<Vector3>("Interchange", "FilmicS", new Vector3(-0.01f, 0.18f, 0), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 110, IsAdvanced = true }));

            WoodsFogLevel = Config.Bind<float>("Woods", "Fog Level", -100.0f, new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 160 }));
            WoodsTonemap = Config.Bind<ETonemap>("Woods", "Tonemap", ETonemap.ACES, new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 150 }));
            WoodsACES = Config.Bind<Vector3>("Woods", "ACES", new Vector3(20, 0.2f, 20), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 140, IsAdvanced = true }));
            WoodsACESS = Config.Bind<Vector3>("Woods", "ACESS", new Vector3(0, 1, 0), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 130, IsAdvanced = true }));
            WoodsFilmic = Config.Bind<Vector3>("Woods", "Filmic", new Vector3(1f, 2f, 1.75f), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 120, IsAdvanced = true }));
            WoodsFilmicS = Config.Bind<Vector3>("Woods", "FilmicS", new Vector3(0, 0.25f, 0), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 110, IsAdvanced = true }));

            ReserveFogLevel = Config.Bind<float>("Reserve", "Fog Level", -100.0f, new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 160 }));
            ReserveTonemap = Config.Bind<ETonemap>("Reserve", "Tonemap", ETonemap.ACES, new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 150 }));
            ReserveACES = Config.Bind<Vector3>("Reserve", "ACES", new Vector3(20, 0.2f, 20), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 140, IsAdvanced = true }));
            ReserveACESS = Config.Bind<Vector3>("Reserve", "ACESS", new Vector3(0, 1, 0), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 130, IsAdvanced = true }));
            ReserveFilmic = Config.Bind<Vector3>("Reserve", "Filmic", new Vector3(1f, 2f, 1.75f), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 120, IsAdvanced = true }));
            ReserveFilmicS = Config.Bind<Vector3>("Reserve", "FilmicS", new Vector3(0, 0.25f, 0), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 110, IsAdvanced = true }));

            ShorelineFogLevel = Config.Bind<float>("Shoreline", "Fog Level", -100.0f, new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 160 }));
            ShorelineTonemap = Config.Bind<ETonemap>("Shoreline", "Tonemap", ETonemap.ACES, new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 150 }));
            ShorelineACES = Config.Bind<Vector3>("Shoreline", "ACES", new Vector3(20, 0.2f, 20), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 140, IsAdvanced = true }));
            ShorelineACESS = Config.Bind<Vector3>("Shoreline", "ACESS", new Vector3(0, 1, 0), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 130, IsAdvanced = true }));
            ShorelineFilmic = Config.Bind<Vector3>("Shoreline", "Filmic", new Vector3(1f, 2f, 1.75f), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 120, IsAdvanced = true }));
            ShorelineFilmicS = Config.Bind<Vector3>("Shoreline", "FilmicS", new Vector3(0, 0.25f, 0), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 110, IsAdvanced = true }));

            HideoutTonemap = Config.Bind<ETonemap>("Hideout", "Tonemap", ETonemap.ACES, new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 160 }));
            HideoutACES = Config.Bind<Vector3>("Hideout", "ACES", new Vector3(8, -0.2f, 8), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 150, IsAdvanced = true }));
            HideoutACESS = Config.Bind<Vector3>("Hideout", "ACESS", new Vector3(0, 1, 0), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 140, IsAdvanced = true }));
            HideoutFilmic = Config.Bind<Vector3>("Hideout", "Filmic", new Vector3(1f, 2f, 1.75f), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 130, IsAdvanced = true }));
            HideoutFilmicS = Config.Bind<Vector3>("Hideout", "FilmicS", new Vector3(0, 0.7f, 0), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 120, IsAdvanced = true }));
            HideoutSkyColor = Config.Bind<Vector4>("Hideout", "SkyColor", new Vector4(0.5f, 0.5f, 0.5f), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 110, IsAdvanced = true }));

            LightColorIndex0 = Config.Bind<Vector4>("AmandsGraphics LightColor", "Index0", new Vector4(232.0f, 240.0f, 255.0f) / 255.0f, new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 160, IsAdvanced = true }));
            LightColorIndex1 = Config.Bind<Vector4>("AmandsGraphics LightColor", "Index1", new Vector4(0, 0, 0), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 150, IsAdvanced = true }));
            //LightColorIndex2 = Config.Bind<Vector4>("AmandsGraphics LightColor", "Index2", new Vector4(255.0f, 186.0f, 168.0f) / 255.0f, new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 140, IsAdvanced = true }));
            LightColorIndex2 = Config.Bind<Vector4>("AmandsGraphics LightColor", "Index2", new Vector4(1.0f, 0.457f, 0.322f, 1.0f), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 140, IsAdvanced = true }));
            LightColorIndex3 = Config.Bind<Vector4>("AmandsGraphics LightColor", "Index3", new Vector4(219.0f, 191.0f, 160.0f) / 255.0f, new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 130, IsAdvanced = true }));
            LightColorIndex4 = Config.Bind<Vector4>("AmandsGraphics LightColor", "Index4", new Vector4(255.0f, 238.0f, 196.0f) / 255.0f, new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 120, IsAdvanced = true }));
            LightColorIndex5 = Config.Bind<Vector4>("AmandsGraphics LightColor", "Index5", new Vector4(150.0f, 143.0f, 122.0f) / 255.0f, new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 110, IsAdvanced = true }));

            new AmandsGraphicsNVGPatch().Enable();
            new AmandsGraphicsHBAOPatch().Enable();
        }

        public static void SaveAmandsGraphicsPreset()
        {
            if (PresetName.Value != "")
            {
                if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "/BepInEx/plugins/AmandsGraphics/"))
                {
                    Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "/BepInEx/plugins/AmandsGraphics/");
                }
                AmandsGraphicsPreset preset = new AmandsGraphicsPreset()
                {
                    MotionBlur = MotionBlur.Value,
                    MotionBlurSampleCount = MotionBlurSampleCount.Value,
                    MotionBlurShutterAngle = MotionBlurShutterAngle.Value,
                    HBAO = HBAO.Value,
                    HBAOIntensity = HBAOIntensity.Value,
                    HBAOSaturation = HBAOSaturation.Value,
                    HBAOAlbedoMultiplier = HBAOAlbedoMultiplier.Value,
                    DepthOfField = DepthOfField.Value,
                    DepthOfFieldFocusDistance = DepthOfFieldFocusDistance.Value,
                    DepthOfFieldAperture = DepthOfFieldAperture.Value,
                    DepthOfFieldFocalLength = DepthOfFieldFocalLength.Value,
                    DepthOfFieldFocalLengthOff = DepthOfFieldFocalLengthOff.Value,
                    DepthOfFieldKernelSize = DepthOfFieldKernelSize.Value,
                    DepthOfFieldSpeed = DepthOfFieldSpeed.Value,

                    Brightness = Brightness.Value,
                    Tonemap = Tonemap.Value,
                    useLut = useLut.Value,
                    BloomIntensity = BloomIntensity.Value,
                    CC_Vintage = CC_Vintage.Value,
                    CC_Sharpen = CC_Sharpen.Value,
                    CustomGlobalFog = CustomGlobalFog.Value,
                    CustomGlobalFogIntensity = CustomGlobalFogIntensity.Value,
                    GlobalFog = GlobalFog.Value,
                    ColorCorrectionCurves = ColorCorrectionCurves.Value,
                    LightsUseLinearIntensity = LightsUseLinearIntensity.Value,
                    SunColor = SunColor.Value,
                    SkyColor = SkyColor.Value,
                    NVGColorsFix = NVGColorsFix.Value,

                    CustomsFogLevel = CustomsFogLevel.Value,
                    LighthouseFogLevel = LighthouseFogLevel.Value,
                    InterchangeFogLevel = InterchangeFogLevel.Value,
                    WoodsFogLevel = WoodsFogLevel.Value,
                    ReserveFogLevel = ReserveFogLevel.Value,
                    ShorelineFogLevel = ShorelineFogLevel.Value,

                    LabsTonemap = LabsTonemap.Value,
                    CustomsTonemap = CustomsTonemap.Value,
                    FactoryTonemap = FactoryTonemap.Value,
                    FactoryNightTonemap = FactoryNightTonemap.Value,
                    LighthouseTonemap = LighthouseTonemap.Value,
                    InterchangeTonemap = InterchangeTonemap.Value,
                    WoodsTonemap = WoodsTonemap.Value,
                    ReserveTonemap = ReserveTonemap.Value,
                    ShorelineTonemap = ShorelineTonemap.Value,
                    HideoutTonemap = HideoutTonemap.Value,

                    LabsACES = LabsACES.Value,
                    LabsACESS = LabsACESS.Value,
                    CustomsACES = CustomsACES.Value,
                    CustomsACESS = CustomsACESS.Value,
                    FactoryACES = FactoryACES.Value,
                    FactoryACESS = FactoryACESS.Value,
                    FactoryNightACES = FactoryNightACES.Value,
                    FactoryNightACESS = FactoryNightACESS.Value,
                    LighthouseACES = LighthouseACES.Value,
                    LighthouseACESS = LighthouseACESS.Value,
                    InterchangeACES = InterchangeACES.Value,
                    InterchangeACESS = InterchangeACESS.Value,
                    WoodsACES = WoodsACES.Value,
                    WoodsACESS = WoodsACESS.Value,
                    ReserveACES = ReserveACES.Value,
                    ReserveACESS = ReserveACESS.Value,
                    ShorelineACES = ShorelineACES.Value,
                    ShorelineACESS = ShorelineACESS.Value,
                    HideoutACES = HideoutACES.Value,
                    HideoutACESS = HideoutACESS.Value,

                    LabsFilmic = LabsFilmic.Value,
                    LabsFilmicS = LabsFilmicS.Value,
                    CustomsFilmic = CustomsFilmic.Value,
                    CustomsFilmicS = CustomsFilmicS.Value,
                    FactoryFilmic = FactoryFilmic.Value,
                    FactoryFilmicS = FactoryFilmicS.Value,
                    FactoryNightFilmic = FactoryNightFilmic.Value,
                    FactoryNightFilmicS = FactoryNightFilmicS.Value,
                    LighthouseFilmic = LighthouseFilmic.Value,
                    LighthouseFilmicS = LighthouseFilmicS.Value,
                    InterchangeFilmic = InterchangeFilmic.Value,
                    InterchangeFilmicS = InterchangeFilmicS.Value,
                    WoodsFilmic = WoodsFilmic.Value,
                    WoodsFilmicS = WoodsFilmicS.Value,
                    ReserveFilmic = ReserveFilmic.Value,
                    ReserveFilmicS = ReserveFilmicS.Value,
                    ShorelineFilmic = ShorelineFilmic.Value,
                    ShorelineFilmicS = ShorelineFilmicS.Value,
                    HideoutFilmic = HideoutFilmic.Value,
                    HideoutFilmicS = HideoutFilmicS.Value,

                    FactorySkyColor = FactorySkyColor.Value,
                    FactoryNVSkyColor = FactoryNVSkyColor.Value,
                    FactoryNightSkyColor = FactoryNightSkyColor.Value,
                    FactoryNightNVSkyColor = FactoryNightNVSkyColor.Value,
                    HideoutSkyColor = HideoutSkyColor.Value,

                    LightColorIndex0 = LightColorIndex0.Value,
                    LightColorIndex1 = LightColorIndex1.Value,
                    LightColorIndex2 = LightColorIndex2.Value,
                    LightColorIndex3 = LightColorIndex3.Value,
                    LightColorIndex4 = LightColorIndex4.Value,
                    LightColorIndex5 = LightColorIndex5.Value,
                };
                WriteToJsonFile<AmandsGraphicsPreset>((AppDomain.CurrentDomain.BaseDirectory + "/BepInEx/plugins/AmandsGraphics/" + PresetName.Value + ".json"), preset, false);
            }
        }
        public static void LoadAmandsGraphicsPreset()
        {
            if (File.Exists((AppDomain.CurrentDomain.BaseDirectory + "/BepInEx/plugins/AmandsGraphics/" + PresetName.Value + ".json")))
            {
                AmandsGraphicsPreset preset = ReadFromJsonFile<AmandsGraphicsPreset>((AppDomain.CurrentDomain.BaseDirectory + "/BepInEx/plugins/AmandsGraphics/" + PresetName.Value + ".json"));
                MotionBlur.Value = preset.MotionBlur;
                MotionBlurSampleCount.Value = preset.MotionBlurSampleCount;
                MotionBlurShutterAngle.Value = preset.MotionBlurShutterAngle;
                HBAO.Value = preset.HBAO;
                HBAOIntensity.Value = preset.HBAOIntensity;
                HBAOSaturation.Value = preset.HBAOSaturation;
                HBAOAlbedoMultiplier.Value = preset.HBAOAlbedoMultiplier;
                DepthOfField.Value = preset.DepthOfField;
                DepthOfFieldFocusDistance.Value = preset.DepthOfFieldFocusDistance;
                DepthOfFieldAperture.Value = preset.DepthOfFieldAperture;
                DepthOfFieldFocalLength.Value = preset.DepthOfFieldFocalLength;
                DepthOfFieldFocalLengthOff.Value = preset.DepthOfFieldFocalLengthOff;
                DepthOfFieldKernelSize.Value = preset.DepthOfFieldKernelSize;
                DepthOfFieldSpeed.Value = preset.DepthOfFieldSpeed;

                Brightness.Value = preset.Brightness;
                Tonemap.Value = preset.Tonemap;
                useLut.Value = preset.useLut;
                BloomIntensity.Value = preset.BloomIntensity;
                CC_Vintage.Value = preset.CC_Vintage;
                CC_Sharpen.Value = preset.CC_Sharpen;
                CustomGlobalFog.Value = preset.CustomGlobalFog;
                CustomGlobalFogIntensity.Value = preset.CustomGlobalFogIntensity;
                GlobalFog.Value = preset.GlobalFog;
                ColorCorrectionCurves.Value = preset.ColorCorrectionCurves;
                LightsUseLinearIntensity.Value = preset.LightsUseLinearIntensity;
                SunColor.Value = preset.SunColor;
                SkyColor.Value = preset.SkyColor;
                NVGColorsFix.Value = preset.NVGColorsFix;

                CustomsFogLevel.Value = preset.CustomsFogLevel;
                LighthouseFogLevel.Value = preset.LighthouseFogLevel;
                InterchangeFogLevel.Value = preset.InterchangeFogLevel;
                WoodsFogLevel.Value = preset.WoodsFogLevel;
                ReserveFogLevel.Value = preset.ReserveFogLevel;
                ShorelineFogLevel.Value = preset.ShorelineFogLevel;

                LabsTonemap.Value = preset.LabsTonemap;
                CustomsTonemap.Value = preset.CustomsTonemap;
                FactoryTonemap.Value = preset.FactoryTonemap;
                FactoryNightTonemap.Value = preset.FactoryNightTonemap;
                LighthouseTonemap.Value = preset.LighthouseTonemap;
                InterchangeTonemap.Value = preset.InterchangeTonemap;
                WoodsTonemap.Value = preset.WoodsTonemap;
                ReserveTonemap.Value = preset.ReserveTonemap;
                ShorelineTonemap.Value = preset.ShorelineTonemap;
                HideoutTonemap.Value = preset.HideoutTonemap;

                LabsACES.Value = preset.LabsACES;
                LabsACESS.Value = preset.LabsACESS;
                CustomsACES.Value = preset.CustomsACES;
                CustomsACESS.Value = preset.CustomsACESS;
                FactoryACES.Value = preset.FactoryACES;
                FactoryACESS.Value = preset.FactoryACESS;
                FactoryNightACES.Value = preset.FactoryNightACES;
                FactoryNightACESS.Value = preset.FactoryNightACESS;
                LighthouseACES.Value = preset.LighthouseACES;
                LighthouseACESS.Value = preset.LighthouseACESS;
                InterchangeACES.Value = preset.InterchangeACES;
                InterchangeACESS.Value = preset.InterchangeACESS;
                WoodsACES.Value = preset.WoodsACES;
                WoodsACESS.Value = preset.WoodsACESS;
                ReserveACES.Value = preset.ReserveACES;
                ReserveACESS.Value = preset.ReserveACESS;
                ShorelineACES.Value = preset.ShorelineACES;
                ShorelineACESS.Value = preset.ShorelineACESS;
                HideoutACES.Value = preset.HideoutACES;
                HideoutACESS.Value = preset.HideoutACESS;

                LabsFilmic.Value = preset.LabsFilmic;
                LabsFilmicS.Value = preset.LabsFilmicS;
                CustomsFilmic.Value = preset.CustomsFilmic;
                CustomsFilmicS.Value = preset.CustomsFilmicS;
                FactoryFilmic.Value = preset.FactoryFilmic;
                FactoryFilmicS.Value = preset.FactoryFilmicS;
                FactoryNightFilmic.Value = preset.FactoryNightFilmic;
                FactoryNightFilmicS.Value = preset.FactoryNightFilmicS;
                LighthouseFilmic.Value = preset.LighthouseFilmic;
                LighthouseFilmicS.Value = preset.LighthouseFilmicS;
                InterchangeFilmic.Value = preset.InterchangeFilmic;
                InterchangeFilmicS.Value = preset.InterchangeFilmicS;
                WoodsFilmic.Value = preset.WoodsFilmic;
                WoodsFilmicS.Value = preset.WoodsFilmicS;
                ReserveFilmic.Value = preset.ReserveFilmic;
                ReserveFilmicS.Value = preset.ReserveFilmicS;
                ShorelineFilmic.Value = preset.ShorelineFilmic;
                ShorelineFilmicS.Value = preset.ShorelineFilmicS;
                HideoutFilmic.Value = preset.HideoutFilmic;
                HideoutFilmicS.Value = preset.HideoutFilmicS;

                FactorySkyColor.Value = preset.FactorySkyColor;
                FactoryNVSkyColor.Value = preset.FactoryNVSkyColor;
                FactoryNightSkyColor.Value = preset.FactoryNightSkyColor;
                FactoryNightNVSkyColor.Value = preset.FactoryNightNVSkyColor;
                HideoutSkyColor.Value = preset.HideoutSkyColor;

                LightColorIndex0.Value = preset.LightColorIndex0;
                LightColorIndex1.Value = preset.LightColorIndex1;
                LightColorIndex2.Value = preset.LightColorIndex2;
                LightColorIndex3.Value = preset.LightColorIndex3;
                LightColorIndex4.Value = preset.LightColorIndex4;
                LightColorIndex5.Value = preset.LightColorIndex5;
            }
        }
        public static void WriteToJsonFile<T>(string filePath, T objectToWrite, bool append = false) where T : new()
        {
            TextWriter writer = null;
            try
            {
                var contentsToWriteToFile = Json.Serialize(objectToWrite);
                writer = new StreamWriter(filePath, append);
                writer.Write(contentsToWriteToFile);
            }
            finally
            {
                if (writer != null)
                    writer.Close();
            }
        }
        public static T ReadFromJsonFile<T>(string filePath) where T : new()
        {
            TextReader reader = null;
            try
            {
                reader = new StreamReader(filePath);
                var fileContents = reader.ReadToEnd();
                return Json.Deserialize<T>(fileContents);
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
        }
    }
    internal class AmandsGraphicsPreset
    {
        public bool MotionBlur { get; set; }
        public int MotionBlurSampleCount { get; set; }
        public float MotionBlurShutterAngle { get; set; }
        public bool HBAO { get; set; }
        public float HBAOIntensity { get; set; }
        public float HBAOSaturation { get; set; }
        public float HBAOAlbedoMultiplier { get; set; }
        public bool DepthOfField { get; set; }
        public float DepthOfFieldFocusDistance { get; set; }
        public float DepthOfFieldAperture { get; set; }
        public float DepthOfFieldFocalLength { get; set; }
        public float DepthOfFieldFocalLengthOff { get; set; }
        public KernelSize DepthOfFieldKernelSize { get; set; }
        public float DepthOfFieldSpeed { get; set; }

        public float Brightness { get; set; }
        public EGlobalTonemap Tonemap { get; set; }
        public bool useLut { get; set; }
        public float BloomIntensity { get; set; }
        public bool CC_Vintage { get; set; }
        public bool CC_Sharpen { get; set; }
        public bool CustomGlobalFog { get; set; }
        public float CustomGlobalFogIntensity { get; set; }
        public bool GlobalFog { get; set; }
        public bool ColorCorrectionCurves { get; set; }
        public bool LightsUseLinearIntensity { get; set; }
        public bool SunColor { get; set; }
        public bool SkyColor { get; set; }
        public bool NVGColorsFix { get; set; }

        public float CustomsFogLevel { get; set; }
        public float LighthouseFogLevel { get; set; }
        public float InterchangeFogLevel { get; set; }
        public float WoodsFogLevel { get; set; }
        public float ReserveFogLevel { get; set; }
        public float ShorelineFogLevel { get; set; }

        public ETonemap LabsTonemap { get; set; }
        public ETonemap CustomsTonemap { get; set; }
        public ETonemap FactoryTonemap { get; set; }
        public ETonemap FactoryNightTonemap { get; set; }
        public ETonemap LighthouseTonemap { get; set; }
        public ETonemap InterchangeTonemap { get; set; }
        public ETonemap WoodsTonemap { get; set; }
        public ETonemap ReserveTonemap { get; set; }
        public ETonemap ShorelineTonemap { get; set; }
        public ETonemap HideoutTonemap { get; set; }

        public Vector3 LabsACES { get; set; }
        public Vector3 LabsACESS { get; set; }
        public Vector3 CustomsACES { get; set; }
        public Vector3 CustomsACESS { get; set; }
        public Vector3 FactoryACES { get; set; }
        public Vector3 FactoryACESS { get; set; }
        public Vector3 FactoryNightACES { get; set; }
        public Vector3 FactoryNightACESS { get; set; }
        public Vector3 LighthouseACES { get; set; }
        public Vector3 LighthouseACESS { get; set; }
        public Vector3 InterchangeACES { get; set; }
        public Vector3 InterchangeACESS { get; set; }
        public Vector3 WoodsACES { get; set; }
        public Vector3 WoodsACESS { get; set; }
        public Vector3 ReserveACES { get; set; }
        public Vector3 ReserveACESS { get; set; }
        public Vector3 ShorelineACES { get; set; }
        public Vector3 ShorelineACESS { get; set; }
        public Vector3 HideoutACES { get; set; }
        public Vector3 HideoutACESS { get; set; }

        public Vector3 LabsFilmic { get; set; }
        public Vector3 LabsFilmicS { get; set; }
        public Vector3 CustomsFilmic { get; set; }
        public Vector3 CustomsFilmicS { get; set; }
        public Vector3 FactoryFilmic { get; set; }
        public Vector3 FactoryFilmicS { get; set; }
        public Vector3 FactoryNightFilmic { get; set; }
        public Vector3 FactoryNightFilmicS { get; set; }
        public Vector3 LighthouseFilmic { get; set; }
        public Vector3 LighthouseFilmicS { get; set; }
        public Vector3 InterchangeFilmic { get; set; }
        public Vector3 InterchangeFilmicS { get; set; }
        public Vector3 WoodsFilmic { get; set; }
        public Vector3 WoodsFilmicS { get; set; }
        public Vector3 ReserveFilmic { get; set; }
        public Vector3 ReserveFilmicS { get; set; }
        public Vector3 ShorelineFilmic { get; set; }
        public Vector3 ShorelineFilmicS { get; set; }
        public Vector3 HideoutFilmic { get; set; }
        public Vector3 HideoutFilmicS { get; set; }

        public Vector4 FactorySkyColor { get; set; }
        public Vector4 FactoryNVSkyColor { get; set; }
        public Vector4 FactoryNightSkyColor { get; set; }
        public Vector4 FactoryNightNVSkyColor { get; set; }
        public Vector4 HideoutSkyColor { get; set; }

        public Vector4 LightColorIndex0 { get; set; }
        public Vector4 LightColorIndex1 { get; set; }
        public Vector4 LightColorIndex2 { get; set; }
        public Vector4 LightColorIndex3 { get; set; }
        public Vector4 LightColorIndex4 { get; set; }
        public Vector4 LightColorIndex5 { get; set; }

    }
    public class AmandsGraphicsNVGPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return typeof(SSAA).GetMethod("SetNightVisionEnabled", BindingFlags.Instance | BindingFlags.Public);
        }
        [PatchPostfix]
        private static void PatchPostFix(ref SSAA __instance, bool enabled)
        {
            if (AmandsGraphicsPlugin.NVGColorsFix.Value && AmandsGraphicsPlugin.AmandsGraphicsClassComponent.GraphicsMode && AmandsGraphicsClass.NVG != enabled)
            {
                AmandsGraphicsClass.NVG = enabled;
                AmandsGraphicsPlugin.AmandsGraphicsClassComponent.UpdateAmandsGraphics();
            }
        }
    }
    public class AmandsGraphicsHBAOPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return typeof(HBAO).GetMethod("ApplyPreset", BindingFlags.Instance | BindingFlags.Public);
        }
        [PatchPostfix]
        private static void PatchPostFix(ref HBAO __instance, HBAO_Core.Preset preset)
        {
            AmandsGraphicsClass.defaultFPSCameraHBAOAOSettings = __instance.aoSettings;
            AmandsGraphicsClass.defaultFPSCameraHBAOColorBleedingSettings = __instance.colorBleedingSettings;
            AmandsGraphicsClass.FPSCameraHBAOAOSettings = __instance.aoSettings;
            AmandsGraphicsClass.FPSCameraHBAOColorBleedingSettings = __instance.colorBleedingSettings;
        }
    }
}
