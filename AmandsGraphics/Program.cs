
using Aki.Common.Utils;
using Aki.Reflection.Patching;
using Aki.Reflection.Utils;
using BepInEx;
using BepInEx.Configuration;
using System;
using System.IO;
using System.Reflection;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using EFT.CameraControl;

namespace AmandsGraphics
{
    [BepInPlugin("com.Amanda.Graphics", "Graphics", "1.3.0")]
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

        public static ConfigEntry<EDepthOfField> SurroundDepthOfField { get; set; }
        public static ConfigEntry<float> SurroundDOFOpticZoom { get; set; }
        public static ConfigEntry<float> SurroundDOFAperture { get; set; }
        public static ConfigEntry<float> SurroundDOFSpeed { get; set; }
        public static ConfigEntry<float> SurroundDOFFocalLength { get; set; }
        public static ConfigEntry<float> SurroundDOFFocalLengthOff { get; set; }
        public static ConfigEntry<KernelSize> SurroundDOFKernelSize { get; set; }

        public static ConfigEntry<EWeaponDepthOfField> WeaponDepthOfField { get; set; }
        public static ConfigEntry<float> WeaponDOFSpeed { get; set; }
        public static ConfigEntry<float> WeaponDOFWeaponMaxBlurSize { get; set; }
        public static ConfigEntry<float> WeaponDOFAimingMaxBlurSize { get; set; }
        public static ConfigEntry<float> WeaponDOFOpticMaxBlurSize { get; set; }
        public static ConfigEntry<float> WeaponDOFWeaponFocalLength { get; set; }
        public static ConfigEntry<float> WeaponDOFAimingFocalLength { get; set; }
        public static ConfigEntry<float> WeaponDOFOpticFocalLength { get; set; }
        public static ConfigEntry<float> WeaponDOFAperture { get; set; }
        public static ConfigEntry<UnityStandardAssets.ImageEffects.DepthOfField.BlurSampleCount> WeaponDOFBlurSampleCount { get; set; }

        public static ConfigEntry<EDepthOfField> OpticDepthOfField { get; set; }
        public static ConfigEntry<float> OpticDOFOpticZoom { get; set; }
        public static ConfigEntry<float> OpticDOFAperture1x { get; set; }
        public static ConfigEntry<float> OpticDOFAperture2x { get; set; }
        public static ConfigEntry<float> OpticDOFAperture4x { get; set; }
        public static ConfigEntry<float> OpticDOFAperture6x { get; set; }
        public static ConfigEntry<float> OpticDOFSpeed { get; set; }
        public static ConfigEntry<EOpticDOFFocalLengthMode> OpticDOFFocalLengthMode { get; set; }
        public static ConfigEntry<float> OpticDOFFocalLength { get; set; }
        public static ConfigEntry<KernelSize> OpticDOFKernelSize { get; set; }
        public static ConfigEntry<ERaycastQuality> OpticDOFRaycastQuality { get; set; }
        public static ConfigEntry<float> OpticDOFDistanceSpeed { get; set; }
        public static ConfigEntry<float> OpticDOFRaycastDistance { get; set; }
        public static ConfigEntry<float> OpticDOFTargetDistance { get; set; }


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

        public static ConfigEntry<float> StreetsFogLevel { get; set; }
        public static ConfigEntry<float> CustomsFogLevel { get; set; }
        public static ConfigEntry<float> LighthouseFogLevel { get; set; }
        public static ConfigEntry<float> InterchangeFogLevel { get; set; }
        public static ConfigEntry<float> WoodsFogLevel { get; set; }
        public static ConfigEntry<float> ReserveFogLevel { get; set; }
        public static ConfigEntry<float> ShorelineFogLevel { get; set; }

        public static ConfigEntry<ETonemap> StreetsTonemap { get; set; }
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

        public static ConfigEntry<Vector3> StreetsACES { get; set; }
        public static ConfigEntry<Vector3> StreetsACESS { get; set; }
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

        public static ConfigEntry<Vector3> StreetsFilmic { get; set; }
        public static ConfigEntry<Vector3> StreetsFilmicS { get; set; }
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

            PresetName = Config.Bind("AmandsGraphics Preset", "PresetName", "Experimental", new ConfigDescription("EXPERIMENTAL", null, new ConfigurationManagerAttributes { Order = 120 }));
            SavePreset = Config.Bind("AmandsGraphics Preset", "SavePreset", "", new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 110, HideSettingName = true, CustomDrawer = SavePresetDrawer }));
            LoadPreset = Config.Bind("AmandsGraphics Preset", "LoadPreset", "", new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 100, HideSettingName = true, CustomDrawer = LoadPresetDrawer }));

            GraphicsToggle = Config.Bind(AmandsFeatures, "GraphicsToggle", new KeyboardShortcut(KeyCode.Insert), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 510 }));
            DebugMode = Config.Bind(AmandsFeatures, "DebugMode", EDebugMode.HBAO, new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 500 }));

            MotionBlur = Config.Bind(AmandsCinematic, "MotionBlur", false, new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 480 }));
            MotionBlurSampleCount = Config.Bind(AmandsCinematic, "MotionBlur SampleCount", 10, new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 470, IsAdvanced = true }));
            MotionBlurShutterAngle = Config.Bind(AmandsCinematic, "MotionBlur ShutterAngle", 270f, new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 460, IsAdvanced = true }));

            HBAO = Config.Bind(AmandsCinematic, "HBAO", true, new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 450 }));
            HBAOIntensity = Config.Bind(AmandsCinematic, "HBAO Intensity", 1.0f, new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 440, IsAdvanced = true }));
            HBAOSaturation = Config.Bind(AmandsCinematic, "HBAO Saturation", 1.5f, new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 430, IsAdvanced = true }));
            HBAOAlbedoMultiplier = Config.Bind(AmandsCinematic, "HBAO Albedo Multiplier", 2f, new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 420, IsAdvanced = true }));

            SurroundDepthOfField = Config.Bind(AmandsCinematic, "SurroundDepthOfField", EDepthOfField.Off, new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 410 }));
            SurroundDOFOpticZoom = Config.Bind(AmandsCinematic, "SurroundDOF OpticZoom", 2f, new ConfigDescription("DepthOfField will be enabled if the zoom is greater than or equal to this value", new AcceptableValueRange<float>(1f, 25f), new ConfigurationManagerAttributes { Order = 400, IsAdvanced = true }));
            SurroundDOFAperture = Config.Bind(AmandsCinematic, "SurroundDOF Aperture", 5.6f, new ConfigDescription("The smaller the value is, the shallower the depth of field is", new AcceptableValueRange<float>(0.01f, 128f), new ConfigurationManagerAttributes { Order = 390, IsAdvanced = true }));
            SurroundDOFSpeed = Config.Bind(AmandsCinematic, "SurroundDOF Speed", 4f, new ConfigDescription("Animation speed", new AcceptableValueRange<float>(0.1f, 32f), new ConfigurationManagerAttributes { Order = 380, IsAdvanced = true }));
            SurroundDOFFocalLength = Config.Bind(AmandsCinematic, "SurroundDOF FocalLength", 25f, new ConfigDescription("The larger the value is, the shallower the depth of field is", new AcceptableValueRange<float>(0f, 100f), new ConfigurationManagerAttributes { Order = 370, IsAdvanced = true }));
            SurroundDOFFocalLengthOff = Config.Bind(AmandsCinematic, "SurroundDOF FocalLength Off", 4f, new ConfigDescription("The larger the value is, the shallower the depth of field is. Used by animation to determinate what's considered off", new AcceptableValueRange<float>(0f, 100f), new ConfigurationManagerAttributes { Order = 360, IsAdvanced = true }));
            SurroundDOFKernelSize = Config.Bind(AmandsCinematic, "SurroundDOF KernelSize", KernelSize.Medium, new ConfigDescription("This setting determines the maximum radius of bokeh", null, new ConfigurationManagerAttributes { Order = 350, IsAdvanced = true }));

            WeaponDepthOfField = Config.Bind(AmandsCinematic, "WeaponDepthOfField", EWeaponDepthOfField.Off, new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 340 }));
            WeaponDOFSpeed = Config.Bind(AmandsCinematic, "WeaponDOF Speed", 6.0f, new ConfigDescription("Animation speed", new AcceptableValueRange<float>(0.1f, 32f), new ConfigurationManagerAttributes { Order = 330, IsAdvanced = true }));
            WeaponDOFWeaponMaxBlurSize = Config.Bind(AmandsCinematic, "WeaponDOF Weapon Blur", 3f, new ConfigDescription("Max Blur Size", new AcceptableValueRange<float>(0.01f, 10f), new ConfigurationManagerAttributes { Order = 320, IsAdvanced = true }));
            WeaponDOFAimingMaxBlurSize = Config.Bind(AmandsCinematic, "WeaponDOF Aiming Blur", 4f, new ConfigDescription("Max Blur Size when aiming with iron sights/collimators", new AcceptableValueRange<float>(0.01f, 10f), new ConfigurationManagerAttributes { Order = 310, IsAdvanced = true }));
            WeaponDOFOpticMaxBlurSize = Config.Bind(AmandsCinematic, "WeaponDOF Optic Blur", 6f, new ConfigDescription("Max Blur Size when aiming with optics", new AcceptableValueRange<float>(0.01f, 10f), new ConfigurationManagerAttributes { Order = 300, IsAdvanced = true }));
            WeaponDOFWeaponFocalLength = Config.Bind(AmandsCinematic, "WeaponDOF Weapon FocalLength", 25f, new ConfigDescription("", new AcceptableValueRange<float>(0.01f, 100f), new ConfigurationManagerAttributes { Order = 290, IsAdvanced = true }));
            WeaponDOFAimingFocalLength = Config.Bind(AmandsCinematic, "WeaponDOF Aiming FocalLength", 30f, new ConfigDescription("", new AcceptableValueRange<float>(0.01f, 100f), new ConfigurationManagerAttributes { Order = 280, IsAdvanced = true }));
            WeaponDOFOpticFocalLength = Config.Bind(AmandsCinematic, "WeaponDOF Optic FocalLength", 90f, new ConfigDescription("", new AcceptableValueRange<float>(0.01f, 100f), new ConfigurationManagerAttributes { Order = 270, IsAdvanced = true }));
            WeaponDOFAperture = Config.Bind(AmandsCinematic, "WeaponDOF Aperture", 0.25f, new ConfigDescription("", new AcceptableValueRange<float>(0.01f, 2f), new ConfigurationManagerAttributes { Order = 260, IsAdvanced = true }));
            WeaponDOFBlurSampleCount = Config.Bind(AmandsCinematic, "WeaponDOF BlurSampleCount", UnityStandardAssets.ImageEffects.DepthOfField.BlurSampleCount.High, new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 250, IsAdvanced = true }));

            OpticDepthOfField = Config.Bind(AmandsCinematic, "OpticDepthOfField", EDepthOfField.Off, new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 240 }));
            OpticDOFOpticZoom = Config.Bind(AmandsCinematic, "OpticDOF OpticZoom", 2f, new ConfigDescription("OpticDepthOfField will be enabled if the zoom is greater than or equal to this value", new AcceptableValueRange<float>(1f, 25f), new ConfigurationManagerAttributes { Order = 230, IsAdvanced = true }));
            OpticDOFAperture1x = Config.Bind(AmandsCinematic, "OpticDOF Aperture 1x", 64f, new ConfigDescription("The smaller the value is, the shallower the depth of field is at 1x zoom", new AcceptableValueRange<float>(0.01f, 128f), new ConfigurationManagerAttributes { Order = 220, IsAdvanced = true }));
            OpticDOFAperture2x = Config.Bind(AmandsCinematic, "OpticDOF Aperture 2x", 32f, new ConfigDescription("The smaller the value is, the shallower the depth of field is at 2x zoom", new AcceptableValueRange<float>(0.01f, 128f), new ConfigurationManagerAttributes { Order = 210, IsAdvanced = true }));
            OpticDOFAperture4x = Config.Bind(AmandsCinematic, "OpticDOF Aperture 4x", 8f, new ConfigDescription("The smaller the value is, the shallower the depth of field is at 4x zoom", new AcceptableValueRange<float>(0.01f, 128f), new ConfigurationManagerAttributes { Order = 200, IsAdvanced = true }));
            OpticDOFAperture6x = Config.Bind(AmandsCinematic, "OpticDOF Aperture 6x", 5.6f, new ConfigDescription("The smaller the value is, the shallower the depth of field is at 6x zoom and above", new AcceptableValueRange<float>(0.01f, 128f), new ConfigurationManagerAttributes { Order = 190, IsAdvanced = true }));
            OpticDOFSpeed = Config.Bind(AmandsCinematic, "OpticDOF Speed", 4f, new ConfigDescription("Animation speed", new AcceptableValueRange<float>(0.1f, 32f), new ConfigurationManagerAttributes { Order = 180, IsAdvanced = true }));
            OpticDOFFocalLengthMode = Config.Bind(AmandsCinematic, "OpticDOF FocalLength Mode", EOpticDOFFocalLengthMode.Math, new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 170, IsAdvanced = true }));
            OpticDOFFocalLength = Config.Bind(AmandsCinematic, "OpticDOF FocalLength", 100f, new ConfigDescription("", new AcceptableValueRange<float>(1f, 500f), new ConfigurationManagerAttributes { Order = 160, IsAdvanced = true }));
            OpticDOFKernelSize = Config.Bind(AmandsCinematic, "OpticDOF KernelSize", KernelSize.Medium, new ConfigDescription("This setting determines the maximum radius of bokeh", null, new ConfigurationManagerAttributes { Order = 150, IsAdvanced = true }));
            OpticDOFRaycastQuality = Config.Bind(AmandsCinematic, "OpticDOF Raycast Quality", ERaycastQuality.High, new ConfigDescription("Low: Simple LowPoly Raycast\n High: HighPoly Raycast and Target Detection\n Foliage: 2 Raycasts Targets have priority behind foliage", null, new ConfigurationManagerAttributes { Order = 140, IsAdvanced = true }));
            OpticDOFDistanceSpeed = Config.Bind(AmandsCinematic, "OpticDOF Distance Speed", 15f, new ConfigDescription("Distance animation speed", new AcceptableValueRange<float>(0.1f, 25f), new ConfigurationManagerAttributes { Order = 130, IsAdvanced = true }));
            OpticDOFRaycastDistance = Config.Bind(AmandsCinematic, "OpticDOF Raycast Distance", 1000f, new ConfigDescription("The max distance the ray should check for collisions", new AcceptableValueRange<float>(10f, 10000f), new ConfigurationManagerAttributes { Order = 120, IsAdvanced = true }));
            OpticDOFTargetDistance = Config.Bind(AmandsCinematic, "OpticDOF Target Distance", 50.0f, new ConfigDescription("Aiming distance window on spotted target in cm", new AcceptableValueRange<float>(0.0f, 200f), new ConfigurationManagerAttributes { Order = 110, IsAdvanced = true }));

            Brightness = Config.Bind(AmandsFeatures, "Brightness", 0.5f, new ConfigDescription("EXPERIMENTAL", new AcceptableValueRange<float>(0f, 1f), new ConfigurationManagerAttributes { Order = 340 }));
            Tonemap = Config.Bind(AmandsFeatures, "Tonemap", EGlobalTonemap.ACES, new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 330 }));
            useLut = Config.Bind(AmandsFeatures, "useLut", false, new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 320 }));
            BloomIntensity = Config.Bind(AmandsFeatures, "Bloom Intensity", 0.5f, new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 310 }));
            CC_Vintage = Config.Bind(AmandsFeatures, "CC_Vintage", false, new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 300 }));
            CC_Sharpen = Config.Bind(AmandsFeatures, "CC_Sharpen", false, new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 290 }));
            CustomGlobalFog = Config.Bind(AmandsFeatures, "CustomGlobalFog", false, new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 280 }));
            CustomGlobalFogIntensity = Config.Bind(AmandsFeatures, "CustomGlobalFog Intensity", 0.1f, new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 270 }));
            GlobalFog = Config.Bind(AmandsFeatures, "GlobalFog", false, new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 260 }));
            ColorCorrectionCurves = Config.Bind(AmandsFeatures, "ColorCorrectionCurves", false, new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 250 }));
            LightsUseLinearIntensity = Config.Bind(AmandsFeatures, "LightsUseLinearIntensity", false, new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 240 }));
            SunColor = Config.Bind(AmandsFeatures, "SunColor", true, new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 230 }));
            SkyColor = Config.Bind(AmandsFeatures, "SkyColor", true, new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 220 }));
            NVGColorsFix = Config.Bind(AmandsFeatures, "NVGColorsFix", true, new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 210 }));

            StreetsFogLevel = Config.Bind("Streets", "Fog Level", -250.0f, new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 160 }));
            StreetsTonemap = Config.Bind("Streets", "Tonemap", ETonemap.ACES, new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 150 }));
            StreetsACES = Config.Bind("Streets", "ACES", new Vector3(25, 0.2f, 25), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 140, IsAdvanced = true }));
            StreetsACESS = Config.Bind("Streets", "ACESS", new Vector3(0, 1.4f, 0), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 130, IsAdvanced = true }));
            StreetsFilmic = Config.Bind("Streets", "Filmic", new Vector3(1f, 2f, 1.75f), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 120, IsAdvanced = true }));
            StreetsFilmicS = Config.Bind("Streets", "FilmicS", new Vector3(0, 0.35f, 0), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 110, IsAdvanced = true }));

            LabsTonemap = Config.Bind("Labs", "Tonemap", ETonemap.ACES, new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 150 }));
            LabsACES = Config.Bind("Labs", "ACES", new Vector3(20, 0.2f, 20), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 140, IsAdvanced = true }));
            LabsACESS = Config.Bind("Labs", "ACESS", new Vector3(0, 2, 0), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 130, IsAdvanced = true }));
            LabsFilmic = Config.Bind("Labs", "Filmic", new Vector3(2f, 2f, 1.75f), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 120, IsAdvanced = true }));
            LabsFilmicS = Config.Bind("Labs", "FilmicS", new Vector3(0, 0.5f, 0), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 110, IsAdvanced = true }));

            CustomsFogLevel = Config.Bind("Customs", "Fog Level", -100.0f, new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 160 }));
            CustomsTonemap = Config.Bind("Customs", "Tonemap", ETonemap.ACES, new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 150 }));
            CustomsACES = Config.Bind("Customs", "ACES", new Vector3(20, 0.2f, 20), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 140, IsAdvanced = true }));
            CustomsACESS = Config.Bind("Customs", "ACESS", new Vector3(0, 1, 0), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 130, IsAdvanced = true }));
            CustomsFilmic = Config.Bind("Customs", "Filmic", new Vector3(1f, 2f, 1.75f), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 120, IsAdvanced = true }));
            CustomsFilmicS = Config.Bind("Customs", "FilmicS", new Vector3(0, 0.25f, 0), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 110, IsAdvanced = true }));

            FactoryTonemap = Config.Bind("Factory", "Tonemap", ETonemap.ACES, new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 170 }));
            FactoryACES = Config.Bind("Factory", "ACES", new Vector3(15, 1, 15), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 160, IsAdvanced = true }));
            FactoryACESS = Config.Bind("Factory", "ACESS", new Vector3(0, 1.25f, 0), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 150, IsAdvanced = true }));
            FactoryFilmic = Config.Bind("Factory", "Filmic", new Vector3(1f, 2f, 1.75f), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 140, IsAdvanced = true }));
            FactoryFilmicS = Config.Bind("Factory", "FilmicS", new Vector3(0, 0.3f, 0), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 130, IsAdvanced = true }));
            FactorySkyColor = Config.Bind("Factory", "SkyColor", new Vector4(0.9f, 0.8f, 0.7f), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 120, IsAdvanced = true }));
            FactoryNVSkyColor = Config.Bind("Factory", "NVSkyColor", new Vector4(0.9f, 0.8f, 0.7f), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 110, IsAdvanced = true }));

            FactoryNightTonemap = Config.Bind("FactoryNight", "Tonemap", ETonemap.ACES, new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 170 }));
            FactoryNightACES = Config.Bind("FactoryNight", "ACES", new Vector3(10, -0.2f, 10), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 160, IsAdvanced = true }));
            FactoryNightACESS = Config.Bind("FactoryNight", "ACESS", new Vector3(0, 1, 0), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 150, IsAdvanced = true }));
            FactoryNightFilmic = Config.Bind("FactoryNight", "Filmic", new Vector3(1f, 2f, 1.75f), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 140, IsAdvanced = true }));
            FactoryNightFilmicS = Config.Bind("FactoryNight", "FilmicS", new Vector3(0, 0.5f, 0), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 130, IsAdvanced = true }));
            FactoryNightSkyColor = Config.Bind("FactoryNight", "SkyColor", new Vector4(0.09f, 0.08f, 0.07f), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 120, IsAdvanced = true }));
            FactoryNightNVSkyColor = Config.Bind("FactoryNight", "NVSkyColor", new Vector4(0.25f, 0.25f, 0.25f), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 110, IsAdvanced = true }));

            LighthouseFogLevel = Config.Bind("Lighthouse", "Fog Level", -100.0f, new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 160 }));
            LighthouseTonemap = Config.Bind("Lighthouse", "Tonemap", ETonemap.ACES, new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 150 }));
            LighthouseACES = Config.Bind("Lighthouse", "ACES", new Vector3(20, 0.2f, 20), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 140, IsAdvanced = true }));
            LighthouseACESS = Config.Bind("Lighthouse", "ACESS", new Vector3(0, 1, 0), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 130, IsAdvanced = true }));
            LighthouseFilmic = Config.Bind("Lighthouse", "Filmic", new Vector3(1f, 2f, 1.75f), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 120, IsAdvanced = true }));
            LighthouseFilmicS = Config.Bind("Lighthouse", "FilmicS", new Vector3(0, 0.25f, 0), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 110, IsAdvanced = true }));

            InterchangeFogLevel = Config.Bind("Interchange", "Fog Level", -100.0f, new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 160 }));
            InterchangeTonemap = Config.Bind("Interchange", "Tonemap", ETonemap.ACES, new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 150 }));
            InterchangeACES = Config.Bind("Interchange", "ACES", new Vector3(20, 3, 20), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 140, IsAdvanced = true }));
            InterchangeACESS = Config.Bind("Interchange", "ACESS", new Vector3(0.50f, 1, 0), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 130, IsAdvanced = true }));
            InterchangeFilmic = Config.Bind("Interchange", "Filmic", new Vector3(1f, 2f, 1.75f), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 120, IsAdvanced = true }));
            InterchangeFilmicS = Config.Bind("Interchange", "FilmicS", new Vector3(-0.01f, 0.18f, 0), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 110, IsAdvanced = true }));

            WoodsFogLevel = Config.Bind("Woods", "Fog Level", -100.0f, new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 160 }));
            WoodsTonemap = Config.Bind("Woods", "Tonemap", ETonemap.ACES, new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 150 }));
            WoodsACES = Config.Bind("Woods", "ACES", new Vector3(20, 0.2f, 20), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 140, IsAdvanced = true }));
            WoodsACESS = Config.Bind("Woods", "ACESS", new Vector3(0, 1, 0), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 130, IsAdvanced = true }));
            WoodsFilmic = Config.Bind("Woods", "Filmic", new Vector3(1f, 2f, 1.75f), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 120, IsAdvanced = true }));
            WoodsFilmicS = Config.Bind("Woods", "FilmicS", new Vector3(0, 0.25f, 0), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 110, IsAdvanced = true }));

            ReserveFogLevel = Config.Bind("Reserve", "Fog Level", -100.0f, new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 160 }));
            ReserveTonemap = Config.Bind("Reserve", "Tonemap", ETonemap.ACES, new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 150 }));
            ReserveACES = Config.Bind("Reserve", "ACES", new Vector3(20, 0.2f, 20), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 140, IsAdvanced = true }));
            ReserveACESS = Config.Bind("Reserve", "ACESS", new Vector3(0, 1, 0), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 130, IsAdvanced = true }));
            ReserveFilmic = Config.Bind("Reserve", "Filmic", new Vector3(1f, 2f, 1.75f), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 120, IsAdvanced = true }));
            ReserveFilmicS = Config.Bind("Reserve", "FilmicS", new Vector3(0, 0.25f, 0), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 110, IsAdvanced = true }));

            ShorelineFogLevel = Config.Bind("Shoreline", "Fog Level", -100.0f, new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 160 }));
            ShorelineTonemap = Config.Bind("Shoreline", "Tonemap", ETonemap.ACES, new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 150 }));
            ShorelineACES = Config.Bind("Shoreline", "ACES", new Vector3(20, 0.2f, 20), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 140, IsAdvanced = true }));
            ShorelineACESS = Config.Bind("Shoreline", "ACESS", new Vector3(0, 1, 0), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 130, IsAdvanced = true }));
            ShorelineFilmic = Config.Bind("Shoreline", "Filmic", new Vector3(1f, 2f, 1.75f), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 120, IsAdvanced = true }));
            ShorelineFilmicS = Config.Bind("Shoreline", "FilmicS", new Vector3(0, 0.25f, 0), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 110, IsAdvanced = true }));

            HideoutTonemap = Config.Bind("Hideout", "Tonemap", ETonemap.ACES, new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 160 }));
            HideoutACES = Config.Bind("Hideout", "ACES", new Vector3(8, -0.2f, 8), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 150, IsAdvanced = true }));
            HideoutACESS = Config.Bind("Hideout", "ACESS", new Vector3(0, 1, 0), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 140, IsAdvanced = true }));
            HideoutFilmic = Config.Bind("Hideout", "Filmic", new Vector3(1f, 2f, 1.75f), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 130, IsAdvanced = true }));
            HideoutFilmicS = Config.Bind("Hideout", "FilmicS", new Vector3(0, 0.7f, 0), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 120, IsAdvanced = true }));
            HideoutSkyColor = Config.Bind("Hideout", "SkyColor", new Vector4(0.5f, 0.5f, 0.5f), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 110, IsAdvanced = true }));

            LightColorIndex0 = Config.Bind("AmandsGraphics LightColor", "Index0", new Vector4(232.0f, 240.0f, 255.0f) / 255.0f, new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 160, IsAdvanced = true }));
            LightColorIndex1 = Config.Bind("AmandsGraphics LightColor", "Index1", new Vector4(0, 0, 0), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 150, IsAdvanced = true }));
            //LightColorIndex2 = Config.Bind("AmandsGraphics LightColor", "Index2", new Vector4(255.0f, 186.0f, 168.0f) / 255.0f, new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 140, IsAdvanced = true }));
            LightColorIndex2 = Config.Bind("AmandsGraphics LightColor", "Index2", new Vector4(1.0f, 0.457f, 0.322f, 1.0f), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 140, IsAdvanced = true }));
            LightColorIndex3 = Config.Bind("AmandsGraphics LightColor", "Index3", new Vector4(219.0f, 191.0f, 160.0f) / 255.0f, new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 130, IsAdvanced = true }));
            LightColorIndex4 = Config.Bind("AmandsGraphics LightColor", "Index4", new Vector4(255.0f, 238.0f, 196.0f) / 255.0f, new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 120, IsAdvanced = true }));
            LightColorIndex5 = Config.Bind("AmandsGraphics LightColor", "Index5", new Vector4(150.0f, 143.0f, 122.0f) / 255.0f, new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 110, IsAdvanced = true }));

            new AmandsGraphicsNVGPatch().Enable();
            new AmandsGraphicsHBAOPatch().Enable();
            new AmandsGraphicsPrismEffectsPatch().Enable();
            new AmandsGraphicsOpticPatch().Enable();
            new AmandsGraphicsOpticSightPatch().Enable();
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
                    HBAO = HBAO.Value,
                    HBAOIntensity = HBAOIntensity.Value,
                    HBAOSaturation = HBAOSaturation.Value,
                    HBAOAlbedoMultiplier = HBAOAlbedoMultiplier.Value,

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
                    StreetsFogLevel = StreetsFogLevel.Value,
                    CustomsFogLevel = CustomsFogLevel.Value,
                    LighthouseFogLevel = LighthouseFogLevel.Value,
                    InterchangeFogLevel = InterchangeFogLevel.Value,
                    WoodsFogLevel = WoodsFogLevel.Value,
                    ReserveFogLevel = ReserveFogLevel.Value,
                    ShorelineFogLevel = ShorelineFogLevel.Value,

                    StreetsTonemap = StreetsTonemap.Value,
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

                    StreetsACES = StreetsACES.Value,
                    StreetsACESS = StreetsACESS.Value,
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

                    StreetsFilmic = StreetsFilmic.Value,
                    StreetsFilmicS = StreetsFilmicS.Value,
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
                HBAO.Value = preset.HBAO;
                HBAOIntensity.Value = preset.HBAOIntensity;
                HBAOSaturation.Value = preset.HBAOSaturation;
                HBAOAlbedoMultiplier.Value = preset.HBAOAlbedoMultiplier;

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

                StreetsFogLevel.Value = preset.StreetsFogLevel;
                CustomsFogLevel.Value = preset.CustomsFogLevel;
                LighthouseFogLevel.Value = preset.LighthouseFogLevel;
                InterchangeFogLevel.Value = preset.InterchangeFogLevel;
                WoodsFogLevel.Value = preset.WoodsFogLevel;
                ReserveFogLevel.Value = preset.ReserveFogLevel;
                ShorelineFogLevel.Value = preset.ShorelineFogLevel;

                LabsTonemap.Value = preset.LabsTonemap;
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
        public bool HBAO { get; set; }
        public float HBAOIntensity { get; set; }
        public float HBAOSaturation { get; set; }
        public float HBAOAlbedoMultiplier { get; set; }

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

        public float StreetsFogLevel { get; set; }
        public float CustomsFogLevel { get; set; }
        public float LighthouseFogLevel { get; set; }
        public float InterchangeFogLevel { get; set; }
        public float WoodsFogLevel { get; set; }
        public float ReserveFogLevel { get; set; }
        public float ShorelineFogLevel { get; set; }

        public ETonemap StreetsTonemap { get; set; }
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

        public Vector3 StreetsACES { get; set; }
        public Vector3 StreetsACESS { get; set; }
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

        public Vector3 StreetsFilmic { get; set; }
        public Vector3 StreetsFilmicS { get; set; }
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
    public class AmandsGraphicsPrismEffectsPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return typeof(PrismEffects).GetMethod("OnEnable", BindingFlags.Instance | BindingFlags.NonPublic);
        }
        [PatchPostfix]
        private static void PatchPostFix(ref PrismEffects __instance)
        {
            if (__instance.gameObject.name == "FPS Camera") AmandsGraphicsPlugin.AmandsGraphicsClassComponent.ActivateAmandsGraphics(__instance.gameObject, __instance);
        }
    }
    public class AmandsGraphicsOpticPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return typeof(OpticComponentUpdater).GetMethod("Awake", BindingFlags.Instance | BindingFlags.NonPublic);
        }
        [PatchPostfix]
        private static void PatchPostFix(ref OpticComponentUpdater __instance)
        {
            if (__instance.gameObject.name == "BaseOpticCamera(Clone)")
            {
                AmandsGraphicsPlugin.AmandsGraphicsClassComponent.ActivateAmandsOpticDepthOfField(__instance.gameObject);
                AmandsGraphicsClass.OpticCameraCamera = __instance.GetComponent<Camera>();
                AmandsGraphicsClass.OpticCameraThermalVision = __instance.GetComponent<ThermalVision>();
            }
        }
    }
    public class AmandsGraphicsOpticSightPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return typeof(OpticSight).GetMethod("OnEnable", BindingFlags.Instance | BindingFlags.NonPublic);
        }
        [PatchPostfix]
        private static void PatchPostFix(ref OpticSight __instance)
        {
            //AmandsGraphicsClass.opticSight = __instance;
            foreach (Transform transform in __instance.gameObject.transform.GetChildren())
            {
                if (transform.name.Contains("backLens")) AmandsGraphicsClass.backLens = transform;
            }
            SightModVisualControllers sightModVisualControllers = __instance.gameObject.GetComponentInParent<SightModVisualControllers>();
            if (sightModVisualControllers != null)
            {
                AmandsGraphicsClass.sightComponent = sightModVisualControllers.SightMod;
            }
        }
    }
}
