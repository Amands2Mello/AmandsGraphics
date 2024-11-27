
using SPT.Common.Utils;
using SPT.Reflection.Patching;
using BepInEx;
using BepInEx.Configuration;
using System;
using System.IO;
using System.Reflection;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using EFT.CameraControl;
using EFT.InventoryLogic;
using HarmonyLib;
using EFT.UI;
using System.Collections.Generic;
using EFT;
using System.Threading.Tasks;

namespace AmandsGraphics
{
    [BepInPlugin("com.Amanda.Graphics", "Graphics", "1.6.4")]
    public class AmandsGraphicsPlugin : BaseUnityPlugin
    {
        public static GameObject Hook;
        public static AmandsGraphicsClass AmandsGraphicsClassComponent;
        public static Type PainKillerEffectType;
        public static Type PainEffectType;

        public static ConfigEntry<KeyboardShortcut> GraphicsToggle { get; set; }
        public static ConfigEntry<EDebugMode> DebugMode { get; set; }

        public static ConfigEntry<EEnabledFeature> MotionBlur { get; set; }
        public static ConfigEntry<int> MotionBlurSampleCount { get; set; }
        public static ConfigEntry<float> MotionBlurShutterAngle { get; set; }

        public static ConfigEntry<EEnabledFeature> HBAO { get; set; }
        public static ConfigEntry<float> HBAOIntensity { get; set; }
        public static ConfigEntry<float> HBAOSaturation { get; set; }
        public static ConfigEntry<float> HBAOAlbedoMultiplier { get; set; }
        public static ConfigEntry<float> LabsHBAOIntensity { get; set; }
        public static ConfigEntry<float> LabsHBAOSaturation { get; set; }
        public static ConfigEntry<float> LabsHBAOAlbedoMultiplier { get; set; }

        public static ConfigEntry<EDepthOfField> SurroundDepthOfField { get; set; }
        public static ConfigEntry<float> SurroundDOFOpticZoom { get; set; }
        public static ConfigEntry<float> SurroundDOFAperture { get; set; }
        public static ConfigEntry<float> SurroundDOFSpeed { get; set; }
        public static ConfigEntry<float> SurroundDOFFocalLength { get; set; }
        public static ConfigEntry<float> SurroundDOFFocalLengthOff { get; set; }
        public static ConfigEntry<KernelSize> DOFKernelSize { get; set; }

        public static ConfigEntry<EUIDepthOfField> UIDepthOfField { get; set; }
        public static ConfigEntry<float> UIDOFAperture { get; set; }
        public static ConfigEntry<float> UIDOFDistance { get; set; }
        public static ConfigEntry<float> UIDOFSpeed { get; set; }
        public static ConfigEntry<float> UIDOFFocalLength { get; set; }

        public static ConfigEntry<EWeaponDepthOfField> WeaponDepthOfField { get; set; }
        public static ConfigEntry<float> WeaponDOFSpeed { get; set; }
        public static ConfigEntry<float> WeaponDOFWeaponMaxBlurSize { get; set; }
        public static ConfigEntry<float> WeaponDOFIronSightMaxBlurSize { get; set; }
        public static ConfigEntry<float> WeaponDOFSightMaxBlurSize { get; set; }
        public static ConfigEntry<float> WeaponDOFNVGMaxBlurSize { get; set; }
        public static ConfigEntry<float> WeaponDOFWeaponFocalLength { get; set; }
        public static ConfigEntry<float> WeaponDOFIronSightFocalLength { get; set; }
        public static ConfigEntry<float> WeaponDOFSightFocalLength { get; set; }
        public static ConfigEntry<float> WeaponDOFNVGFocalLength { get; set; }
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

        public static ConfigEntry<EEnabledFeature> Flashlight { get; set; }
        public static ConfigEntry<float> FlashlightRange { get; set; }
        public static ConfigEntry<float> FlashlightExtinctionCoef { get; set; }

        public static ConfigEntry<EEnabledFeature> NVG { get; set; }
        public static ConfigEntry<ETonemap> NVGTonemap { get; set; }
        public static ConfigEntry<float> NVGAmbientContrast { get; set; }
        public static ConfigEntry<float> InterchangeNVGAmbientContrast { get; set; }
        public static ConfigEntry<float> NVGNoiseIntensity { get; set; }
        public static ConfigEntry<float> InterchangeNVGNoiseIntensity { get; set; }
        public static ConfigEntry<bool> NVGOriginalColor { get; set; }
        public static ConfigEntry<float> NVGOriginalSkyColor { get; set; }
        public static ConfigEntry<float> InterchangeNVGOriginalSkyColor { get; set; }
        public static ConfigEntry<float> NVGMoonLightIntensity { get; set; }

        public static ConfigEntry<EEnabledFeature> NightAmbientLight { get; set; }
        public static ConfigEntry<float> NightAmbientContrast { get; set; }
        public static ConfigEntry<float> InterchangeNightAmbientContrast { get; set; }

        public static ConfigEntry<EEnabledFeature> MysticalGlow { get; set; }
        public static ConfigEntry<float> MysticalGlowIntensity { get; set; }
        public static ConfigEntry<float> GroundZeroMysticalGlowIntensity { get; set; }
        public static ConfigEntry<float> StreetsMysticalGlowIntensity { get; set; }
        public static ConfigEntry<float> CustomsMysticalGlowIntensity { get; set; }
        public static ConfigEntry<float> LighthouseMysticalGlowIntensity { get; set; }
        public static ConfigEntry<float> InterchangeMysticalGlowIntensity { get; set; }
        public static ConfigEntry<float> WoodsMysticalGlowIntensity { get; set; }
        public static ConfigEntry<float> ReserveMysticalGlowIntensity { get; set; }
        public static ConfigEntry<float> ShorelineMysticalGlowIntensity { get; set; }

        public static ConfigEntry<EEnabledFeature> HealthEffectHit { get; set; }
        public static ConfigEntry<float> HitCAIntensity { get; set; }
        public static ConfigEntry<float> HitCASpeed { get; set; }
        public static ConfigEntry<float> HitCAPower { get; set; }

        public static ConfigEntry<EEnabledFeature> HealthEffectPainkiller { get; set; }
        public static ConfigEntry<float> PainkillerSaturation { get; set; }
        //public static ConfigEntry<float> PainkillerCAIntensity { get; set; }
        public static ConfigEntry<EEnabledFeature> HealthEffectPain { get; set; }
        public static ConfigEntry<float> SunMeshBrightness { get; set; }
        public static ConfigEntry<float> SkyBrightness { get; set; }

        public static ConfigEntry<float> Brightness { get; set; }
        public static ConfigEntry<EGlobalTonemap> Tonemap { get; set; }
        public static ConfigEntry<bool> UseBSGLUT { get; set; }
        public static ConfigEntry<float> BloomIntensity { get; set; }
        public static ConfigEntry<bool> UseBSGCC_Vintage { get; set; }
        public static ConfigEntry<bool> UseBSGCC_Sharpen { get; set; }
        public static ConfigEntry<bool> UseBSGGlobalFog { get; set; }
        public static ConfigEntry<bool> UseBSGColorCorrectionCurves { get; set; }
        public static ConfigEntry<bool> LightsUseLinearIntensity { get; set; }
        public static ConfigEntry<bool> SunColor { get; set; }
        public static ConfigEntry<bool> SkyColor { get; set; }
        public static ConfigEntry<string> PresetName { get; set; }
        public static ConfigEntry<string> SavePreset { get; set; }
        public static ConfigEntry<string> LoadPreset { get; set; }

        public static ConfigEntry<float> GroundZeroFogLevel { get; set; }
        public static ConfigEntry<float> StreetsFogLevel { get; set; }
        public static ConfigEntry<float> CustomsFogLevel { get; set; }
        public static ConfigEntry<float> LighthouseFogLevel { get; set; }
        public static ConfigEntry<float> InterchangeFogLevel { get; set; }
        public static ConfigEntry<float> WoodsFogLevel { get; set; }
        public static ConfigEntry<float> ReserveFogLevel { get; set; }
        public static ConfigEntry<float> ShorelineFogLevel { get; set; }

        public static ConfigEntry<ETonemap> GroundZeroTonemap { get; set; }
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

        public static ConfigEntry<Vector3> GroundZeroACES { get; set; }
        public static ConfigEntry<Vector3> GroundZeroACESS { get; set; }
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

        public static ConfigEntry<Vector3> GroundZeroFilmic { get; set; }
        public static ConfigEntry<Vector3> GroundZeroFilmicS { get; set; }
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

        public static ConfigEntry<Vector4> HideoutSkyColor { get; set; }

        public static ConfigEntry<Vector4> LightColorIndex0 { get; set; }
        public static ConfigEntry<Vector4> LightColorIndex1 { get; set; }
        public static ConfigEntry<Vector4> LightColorIndex2 { get; set; }
        public static ConfigEntry<Vector4> LightColorIndex3 { get; set; }
        public static ConfigEntry<Vector4> LightColorIndex4 { get; set; }
        public static ConfigEntry<Vector4> LightColorIndex5 { get; set; }

        public static ConfigEntry<string> Version { get; set; }
        private static bool RequestDefaultValues = false;

        private void Awake()
        {
            Debug.LogError("Graphics Awake()");
            Hook = new GameObject("Graphics");
            AmandsGraphicsClassComponent = Hook.AddComponent<AmandsGraphicsClass>();
            DontDestroyOnLoad(Hook);
        }
        private void Start()
        {
            Version = Config.Bind("Versioning", "Version", "0.0.0", new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 1, ReadOnly = true, IsAdvanced = true }));

            if (Version.Value == "0.0.0")
            {
                // Using New Config File
                Version.Value = Info.Metadata.Version.ToString();
                RequestDefaultValues = true;
            }
            else if (Version.Value != Info.Metadata.Version.ToString())
            {
                // Using Old Config File
                Version.Value = Info.Metadata.Version.ToString();
                RequestDefaultValues = true;
            }
            else
            {
                // Valid Config File
            }

            string AmandsCinematic = "AmandsGraphics Cinematic";
            string AmandsExperimental = "AmandsGraphics Experimental";
            string AmandsFeatures = "AmandsGraphics Features";

            GraphicsToggle = Config.Bind(AmandsFeatures, "GraphicsToggle", new KeyboardShortcut(KeyCode.Insert), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 610 }));
            DebugMode = Config.Bind(AmandsFeatures, "DebugMode", EDebugMode.HBAO, new ConfigDescription("Shift + Insert will toggle only the selected DebugMode feature", null, new ConfigurationManagerAttributes { Order = 600 }));

            MotionBlur = Config.Bind(AmandsCinematic, "MotionBlur", EEnabledFeature.Off, new ConfigDescription("Motion Blur needs anti-aliasing set to TAA to work as intended", null, new ConfigurationManagerAttributes { Order = 670 }));
            MotionBlurSampleCount = Config.Bind(AmandsCinematic, "MotionBlur SampleCount", 10, new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 660, IsAdvanced = true }));
            MotionBlurShutterAngle = Config.Bind(AmandsCinematic, "MotionBlur ShutterAngle", 270f, new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 650, IsAdvanced = true }));

            HBAO = Config.Bind(AmandsCinematic, "HBAO", EEnabledFeature.Off, new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 640 }));
            HBAOIntensity = Config.Bind(AmandsCinematic, "HBAO Intensity", 1.0f, new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 630, IsAdvanced = true }));
            HBAOSaturation = Config.Bind(AmandsCinematic, "HBAO Saturation", 1.5f, new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 620, IsAdvanced = true }));
            HBAOAlbedoMultiplier = Config.Bind(AmandsCinematic, "HBAO Albedo Multiplier", 2f, new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 610, IsAdvanced = true }));

            SurroundDepthOfField = Config.Bind(AmandsCinematic, "SurroundDepthOfField", EDepthOfField.Off, new ConfigDescription("High-Quality Color needs to be Enabled on Graphics Settings", null, new ConfigurationManagerAttributes { Order = 600 }));
            SurroundDOFOpticZoom = Config.Bind(AmandsCinematic, "SurroundDOF OpticZoom", 2f, new ConfigDescription("DepthOfField will be enabled if the zoom is greater than or equal to this value", new AcceptableValueRange<float>(1f, 25f), new ConfigurationManagerAttributes { Order = 590, IsAdvanced = true }));
            SurroundDOFAperture = Config.Bind(AmandsCinematic, "SurroundDOF Aperture", 5.6f, new ConfigDescription("The smaller the value is, the shallower the depth of field is", new AcceptableValueRange<float>(0.01f, 128f), new ConfigurationManagerAttributes { Order = 580, IsAdvanced = true }));
            SurroundDOFSpeed = Config.Bind(AmandsCinematic, "SurroundDOF Speed", 16f, new ConfigDescription("Animation speed", new AcceptableValueRange<float>(0.1f, 32f), new ConfigurationManagerAttributes { Order = 570, IsAdvanced = true }));
            SurroundDOFFocalLength = Config.Bind(AmandsCinematic, "SurroundDOF FocalLength", 25f, new ConfigDescription("The larger the value is, the shallower the depth of field is", new AcceptableValueRange<float>(0f, 100f), new ConfigurationManagerAttributes { Order = 560, IsAdvanced = true }));
            SurroundDOFFocalLengthOff = Config.Bind(AmandsCinematic, "SurroundDOF FocalLength Off", 4f, new ConfigDescription("The larger the value is, the shallower the depth of field is. Used by animation to determinate what's considered off", new AcceptableValueRange<float>(0f, 100f), new ConfigurationManagerAttributes { Order = 550, IsAdvanced = true }));
            DOFKernelSize = Config.Bind(AmandsCinematic, "DOF KernelSize", KernelSize.Medium, new ConfigDescription("This setting determines the maximum radius of bokeh", null, new ConfigurationManagerAttributes { Order = 540, IsAdvanced = true }));

            UIDepthOfField = Config.Bind(AmandsCinematic, "UIDepthOfField", EUIDepthOfField.Off, new ConfigDescription("High-Quality Color needs to be Enabled on Graphics Settings\"", null, new ConfigurationManagerAttributes { Order = 390 }));
            UIDOFDistance = Config.Bind(AmandsCinematic, "UIDOF Distance", 0.2f, new ConfigDescription("Focus point distance", new AcceptableValueRange<float>(0.01f, 1f), new ConfigurationManagerAttributes { Order = 380, IsAdvanced = true }));
            UIDOFAperture = Config.Bind(AmandsCinematic, "UIDOF Aperture", 5.6f, new ConfigDescription("The smaller the value is, the shallower the depth of field is", new AcceptableValueRange<float>(0.01f, 128f), new ConfigurationManagerAttributes { Order = 370, IsAdvanced = true }));
            UIDOFSpeed = Config.Bind(AmandsCinematic, "UIDOF Speed", 4f, new ConfigDescription("Animation speed", new AcceptableValueRange<float>(0.1f, 32f), new ConfigurationManagerAttributes { Order = 360, IsAdvanced = true }));
            UIDOFFocalLength = Config.Bind(AmandsCinematic, "UIDOF FocalLength", 25f, new ConfigDescription("The larger the value is, the shallower the depth of field is", new AcceptableValueRange<float>(0f, 100f), new ConfigurationManagerAttributes { Order = 350, IsAdvanced = true }));

            WeaponDepthOfField = Config.Bind(AmandsCinematic, "WeaponDepthOfField", EWeaponDepthOfField.Off, new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 340 }));
            WeaponDOFSpeed = Config.Bind(AmandsCinematic, "WeaponDOF Speed", 6.0f, new ConfigDescription("Animation speed", new AcceptableValueRange<float>(0.1f, 32f), new ConfigurationManagerAttributes { Order = 330, IsAdvanced = true }));
            WeaponDOFWeaponMaxBlurSize = Config.Bind(AmandsCinematic, "WeaponDOF Weapon Blur", 2f, new ConfigDescription("Max Blur Size", new AcceptableValueRange<float>(0.01f, 10f), new ConfigurationManagerAttributes { Order = 320, IsAdvanced = true }));
            WeaponDOFIronSightMaxBlurSize = Config.Bind(AmandsCinematic, "WeaponDOF IronSight Blur", 3f, new ConfigDescription("Max Blur Size when aiming with iron sights", new AcceptableValueRange<float>(0.01f, 10f), new ConfigurationManagerAttributes { Order = 310, IsAdvanced = true }));
            WeaponDOFSightMaxBlurSize = Config.Bind(AmandsCinematic, "WeaponDOF Sight Blur", 4f, new ConfigDescription("Max Blur Size when aiming with sights", new AcceptableValueRange<float>(0.01f, 10f), new ConfigurationManagerAttributes { Order = 300, IsAdvanced = true }));
            WeaponDOFNVGMaxBlurSize = Config.Bind(AmandsCinematic, "WeaponDOF NVG Blur", 4f, new ConfigDescription("Max Blur Size when aiming with sights", new AcceptableValueRange<float>(0.01f, 10f), new ConfigurationManagerAttributes { Order = 298, IsAdvanced = true }));
            WeaponDOFWeaponFocalLength = Config.Bind(AmandsCinematic, "WeaponDOF Weapon FocalLength", 25f, new ConfigDescription("", new AcceptableValueRange<float>(0.01f, 100f), new ConfigurationManagerAttributes { Order = 290, IsAdvanced = true }));
            WeaponDOFIronSightFocalLength = Config.Bind(AmandsCinematic, "WeaponDOF IronSight FocalLength", 30f, new ConfigDescription("", new AcceptableValueRange<float>(0.01f, 100f), new ConfigurationManagerAttributes { Order = 280, IsAdvanced = true }));
            WeaponDOFSightFocalLength = Config.Bind(AmandsCinematic, "WeaponDOF Sight FocalLength", 90f, new ConfigDescription("", new AcceptableValueRange<float>(0.01f, 100f), new ConfigurationManagerAttributes { Order = 270, IsAdvanced = true }));
            WeaponDOFNVGFocalLength = Config.Bind(AmandsCinematic, "WeaponDOF NVG FocalLength", 100f, new ConfigDescription("", new AcceptableValueRange<float>(0.01f, 100f), new ConfigurationManagerAttributes { Order = 268, IsAdvanced = true }));
            WeaponDOFAperture = Config.Bind(AmandsCinematic, "WeaponDOF Aperture", 0.25f, new ConfigDescription("", new AcceptableValueRange<float>(0.01f, 2f), new ConfigurationManagerAttributes { Order = 260, IsAdvanced = true }));
            WeaponDOFBlurSampleCount = Config.Bind(AmandsCinematic, "WeaponDOF BlurSampleCount", UnityStandardAssets.ImageEffects.DepthOfField.BlurSampleCount.High, new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 250, IsAdvanced = true }));

            OpticDepthOfField = Config.Bind(AmandsCinematic, "OpticDepthOfField", EDepthOfField.Off, new ConfigDescription("High-Quality Color needs to be Enabled on Graphics Settings\"", null, new ConfigurationManagerAttributes { Order = 240 }));
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

            Flashlight = Config.Bind(AmandsExperimental, "Flashlight", EEnabledFeature.On, new ConfigDescription("EXPERIMENTAL", null, new ConfigurationManagerAttributes { Order = 170 }));
            FlashlightRange = Config.Bind(AmandsExperimental, "Flashlight Range", 2f, new ConfigDescription("Flashlights range multiplier", new AcceptableValueRange<float>(0.5f, 4f), new ConfigurationManagerAttributes { Order = 160 }));
            FlashlightExtinctionCoef = Config.Bind(AmandsExperimental, "Flashlight ExtinctionCoef", 0.2f, new ConfigDescription("Volumetric extinction coefficient", new AcceptableValueRange<float>(0.001f, 1f), new ConfigurationManagerAttributes { Order = 150 }));

            NVG = Config.Bind(AmandsExperimental, "NVG", EEnabledFeature.On, new ConfigDescription("EXPERIMENTAL", null, new ConfigurationManagerAttributes { Order = 150 }));
            NVGTonemap = Config.Bind(AmandsExperimental, "NVG Tonemap", ETonemap.ACES, new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 142, IsAdvanced = true }));
            NVGAmbientContrast = Config.Bind(AmandsExperimental, "NVG AmbientContrast", 1f, new ConfigDescription("", new AcceptableValueRange<float>(1f, 1.5f), new ConfigurationManagerAttributes { Order = 140, IsAdvanced = true }));
            NVGNoiseIntensity = Config.Bind(AmandsExperimental, "NVG Noise Intensity", 0.8f, new ConfigDescription("", new AcceptableValueRange<float>(0f, 2f), new ConfigurationManagerAttributes { Order = 130 }));
            NVGOriginalColor = Config.Bind(AmandsExperimental, "NVG Original Color", false, new ConfigDescription("Enables back all default color filters", null, new ConfigurationManagerAttributes { Order = 120 }));
            NVGOriginalSkyColor = Config.Bind(AmandsExperimental, "NVG Original Sky Color", 0.1f, new ConfigDescription("Enables back the default sky color for NVG", new AcceptableValueRange<float>(0.001f, 1f), new ConfigurationManagerAttributes { Order = 110 }));
            NVGMoonLightIntensity = Config.Bind(AmandsExperimental, "NVG Moon LightIntensity", 1f, new ConfigDescription("", new AcceptableValueRange<float>(0.5f, 2f), new ConfigurationManagerAttributes { Order = 90 }));

            NightAmbientLight = Config.Bind(AmandsExperimental, "Night AmbientLight", EEnabledFeature.Off, new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 80 }));
            NightAmbientContrast = Config.Bind(AmandsExperimental, "Night AmbientAmbientContrast", 1.1f, new ConfigDescription("", new AcceptableValueRange<float>(1.1f, 1.15f), new ConfigurationManagerAttributes { Order = 70, IsAdvanced = true }));
            MysticalGlow = Config.Bind(AmandsExperimental, "MysticalGlow", EEnabledFeature.Off, new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 60 }));
            MysticalGlowIntensity = Config.Bind(AmandsExperimental, "MysticalGlow Intensity", 0.05f, new ConfigDescription("", new AcceptableValueRange<float>(0.0f, 0.1f), new ConfigurationManagerAttributes { Order = 50, IsAdvanced = true }));

            HealthEffectHit = Config.Bind(AmandsExperimental, "HealthEffect Hit", EEnabledFeature.Off, new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 40 }));
            HitCAIntensity = Config.Bind(AmandsExperimental, "ChromaticAberration", 0.5f, new ConfigDescription("", new AcceptableValueRange<float>(0.0f, 2.0f), new ConfigurationManagerAttributes { Order = 30 }));
            HitCASpeed = Config.Bind(AmandsExperimental, "ChromaticAberration Speed", 1.0f, new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 20, IsAdvanced = true }));
            HitCAPower = Config.Bind(AmandsExperimental, "ChromaticAberration Power", 100.0f, new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 10, IsAdvanced = true }));

            HealthEffectPainkiller = Config.Bind(AmandsExperimental, "HealthEffect Painkiller", EEnabledFeature.Off, new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 8 }));
            PainkillerSaturation = Config.Bind(AmandsExperimental, "Painkiller Saturation", 0.5f, new ConfigDescription("", new AcceptableValueRange<float>(0.0f, 2.0f), new ConfigurationManagerAttributes { Order = 6 }));
            //PainkillerCAIntensity = Config.Bind(AmandsExperimental, "Painkiller ChromaticAberration", 0.5f, new ConfigDescription("", new AcceptableValueRange<float>(0.0f, 2.0f), new ConfigurationManagerAttributes { Order = 4 }));
            HealthEffectPain = Config.Bind(AmandsExperimental, "HealthEffect Pain", EEnabledFeature.Off, new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 4 }));
            SunMeshBrightness = Config.Bind(AmandsExperimental, "SunMeshBrightness", 2.0f, new ConfigDescription("", new AcceptableValueRange<float>(0.25f, 3.0f), new ConfigurationManagerAttributes { Order = 2 }));
            SkyBrightness = Config.Bind(AmandsExperimental, "SkyBrightness", 1.0f, new ConfigDescription("", new AcceptableValueRange<float>(0.1f, 2.0f), new ConfigurationManagerAttributes { Order = 1 }));

            Brightness = Config.Bind(AmandsFeatures, "Brightness", 0.5f, new ConfigDescription("EXPERIMENTAL", new AcceptableValueRange<float>(0f, 1f), new ConfigurationManagerAttributes { Order = 340 }));
            Tonemap = Config.Bind(AmandsFeatures, "Tonemap", EGlobalTonemap.ACES, new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 330 }));
            UseBSGLUT = Config.Bind(AmandsFeatures, "Use BSG LUT", false, new ConfigDescription("Enabling this will revert the mod changes", null, new ConfigurationManagerAttributes { Order = 320, IsAdvanced = true }));
            BloomIntensity = Config.Bind(AmandsFeatures, "Bloom Intensity", 0.5f, new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 310 }));
            UseBSGCC_Vintage = Config.Bind(AmandsFeatures, "Use BSG CC_Vintage", false, new ConfigDescription("Enabling this will revert the mod changes", null, new ConfigurationManagerAttributes { Order = 300, IsAdvanced = true }));
            UseBSGCC_Sharpen = Config.Bind(AmandsFeatures, "Use BSG CC_Sharpen", false, new ConfigDescription("Enabling this will revert the mod changes", null, new ConfigurationManagerAttributes { Order = 290, IsAdvanced = true }));
            UseBSGGlobalFog = Config.Bind(AmandsFeatures, "Use BSG GlobalFog", false, new ConfigDescription("Enabling this will revert the mod changes", null, new ConfigurationManagerAttributes { Order = 260, IsAdvanced = true }));
            UseBSGColorCorrectionCurves = Config.Bind(AmandsFeatures, "Use BSG ColorCorrection", false, new ConfigDescription("Enabling this will revert the mod changes", null, new ConfigurationManagerAttributes { Order = 250, IsAdvanced = true }));
            LightsUseLinearIntensity = Config.Bind(AmandsFeatures, "LightsUseLinearIntensity", false, new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 240 }));
            SunColor = Config.Bind(AmandsFeatures, "SunColor", true, new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 230 }));
            SkyColor = Config.Bind(AmandsFeatures, "SkyColor", true, new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 220 }));

            GroundZeroFogLevel = Config.Bind("GroundZero", "Fog Level", -250.0f, new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 210 }));
            GroundZeroTonemap = Config.Bind("GroundZero", "Tonemap", ETonemap.ACES, new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 209 }));
            GroundZeroACES = Config.Bind("GroundZero", "ACES", new Vector3(25, 0.2f, 25), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 208, IsAdvanced = true }));
            GroundZeroACESS = Config.Bind("GroundZero", "ACESS", new Vector3(0, 1.1f, 0), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 200, IsAdvanced = true }));
            GroundZeroFilmic = Config.Bind("GroundZero", "Filmic", new Vector3(10f, 2f, 1.75f), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 190, IsAdvanced = true }));
            GroundZeroFilmicS = Config.Bind("GroundZero", "FilmicS", new Vector3(0, 0.4f, 0), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 180, IsAdvanced = true }));
            GroundZeroMysticalGlowIntensity = Config.Bind("GroundZero", "MysticalGlow Intensity", 0.75f, new ConfigDescription("", new AcceptableValueRange<float>(0.0f, 2.0f), new ConfigurationManagerAttributes { Order = 170, IsAdvanced = true }));

            StreetsFogLevel = Config.Bind("Streets", "Fog Level", -250.0f, new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 160 }));
            StreetsTonemap = Config.Bind("Streets", "Tonemap", ETonemap.ACES, new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 150 }));
            StreetsACES = Config.Bind("Streets", "ACES", new Vector3(25, 0.2f, 25), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 140, IsAdvanced = true }));
            StreetsACESS = Config.Bind("Streets", "ACESS", new Vector3(0, 1.1f, 0), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 130, IsAdvanced = true }));
            StreetsFilmic = Config.Bind("Streets", "Filmic", new Vector3(10f, 2f, 1.75f), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 120, IsAdvanced = true }));
            StreetsFilmicS = Config.Bind("Streets", "FilmicS", new Vector3(0, 0.4f, 0), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 110, IsAdvanced = true }));
            StreetsMysticalGlowIntensity = Config.Bind("Streets", "MysticalGlow Intensity", 0.75f, new ConfigDescription("", new AcceptableValueRange<float>(0.0f, 2.0f), new ConfigurationManagerAttributes { Order = 100, IsAdvanced = true }));

            LabsTonemap = Config.Bind("Labs", "Tonemap", ETonemap.ACES, new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 150 }));
            LabsACES = Config.Bind("Labs", "ACES", new Vector3(20, 0.4f, 20), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 140, IsAdvanced = true }));
            LabsACESS = Config.Bind("Labs", "ACESS", new Vector3(0, 1f, 0), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 130, IsAdvanced = true }));
            LabsFilmic = Config.Bind("Labs", "Filmic", new Vector3(8f, 2f, 1.75f), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 120, IsAdvanced = true }));
            LabsFilmicS = Config.Bind("Labs", "FilmicS", new Vector3(0, 0.4f, 0), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 110, IsAdvanced = true }));
            LabsHBAOIntensity = Config.Bind("Labs", "HBAO Intensity", 0.5f, new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 100, IsAdvanced = true }));
            LabsHBAOSaturation = Config.Bind("Labs", "HBAO Saturation", 1.5f, new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 90, IsAdvanced = true }));
            LabsHBAOAlbedoMultiplier = Config.Bind("Labs", "HBAO Albedo Multiplier", 2f, new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 80, IsAdvanced = true }));

            CustomsFogLevel = Config.Bind("Customs", "Fog Level", -100.0f, new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 160 }));
            CustomsTonemap = Config.Bind("Customs", "Tonemap", ETonemap.ACES, new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 150 }));
            CustomsACES = Config.Bind("Customs", "ACES", new Vector3(20, 0.2f, 20), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 140, IsAdvanced = true }));
            CustomsACESS = Config.Bind("Customs", "ACESS", new Vector3(0, 1, 0), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 130, IsAdvanced = true }));
            CustomsFilmic = Config.Bind("Customs", "Filmic", new Vector3(10f, 2f, 1.75f), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 120, IsAdvanced = true }));
            CustomsFilmicS = Config.Bind("Customs", "FilmicS", new Vector3(0, 0.4f, 0), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 110, IsAdvanced = true }));
            CustomsMysticalGlowIntensity = Config.Bind("Customs", "MysticalGlow Intensity", 1.0f, new ConfigDescription("", new AcceptableValueRange<float>(0.0f, 2.0f), new ConfigurationManagerAttributes { Order = 100, IsAdvanced = true }));

            FactoryTonemap = Config.Bind("Factory", "Tonemap", ETonemap.ACES, new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 170 }));
            FactoryACES = Config.Bind("Factory", "ACES", new Vector3(20, 0.8f, 20), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 160, IsAdvanced = true }));
            FactoryACESS = Config.Bind("Factory", "ACESS", new Vector3(0, 1f, 0), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 150, IsAdvanced = true }));
            FactoryFilmic = Config.Bind("Factory", "Filmic", new Vector3(5f, 2f, 1.75f), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 140, IsAdvanced = true }));
            FactoryFilmicS = Config.Bind("Factory", "FilmicS", new Vector3(0, 0.3f, 0), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 130, IsAdvanced = true }));

            FactoryNightTonemap = Config.Bind("FactoryNight", "Tonemap", ETonemap.ACES, new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 170 }));
            FactoryNightACES = Config.Bind("FactoryNight", "ACES", new Vector3(20, 0.8f, 20), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 160, IsAdvanced = true }));
            FactoryNightACESS = Config.Bind("FactoryNight", "ACESS", new Vector3(0, 1f, 0), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 150, IsAdvanced = true }));
            FactoryNightFilmic = Config.Bind("FactoryNight", "Filmic", new Vector3(5f, 2f, 1.75f), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 140, IsAdvanced = true }));
            FactoryNightFilmicS = Config.Bind("FactoryNight", "FilmicS", new Vector3(0, 0.26f, 0), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 130, IsAdvanced = true }));

            LighthouseFogLevel = Config.Bind("Lighthouse", "Fog Level", -100.0f, new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 160 }));
            LighthouseTonemap = Config.Bind("Lighthouse", "Tonemap", ETonemap.ACES, new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 150 }));
            LighthouseACES = Config.Bind("Lighthouse", "ACES", new Vector3(20, 0.2f, 20), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 140, IsAdvanced = true }));
            LighthouseACESS = Config.Bind("Lighthouse", "ACESS", new Vector3(0, 1, 0), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 130, IsAdvanced = true }));
            LighthouseFilmic = Config.Bind("Lighthouse", "Filmic", new Vector3(10f, 2f, 1.75f), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 120, IsAdvanced = true }));
            LighthouseFilmicS = Config.Bind("Lighthouse", "FilmicS", new Vector3(0, 0.4f, 0), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 110, IsAdvanced = true }));
            LighthouseMysticalGlowIntensity = Config.Bind("Lighthouse", "MysticalGlow Intensity", 0.75f, new ConfigDescription("", new AcceptableValueRange<float>(0.0f, 2.0f), new ConfigurationManagerAttributes { Order = 100, IsAdvanced = true }));

            InterchangeNightAmbientContrast = Config.Bind("Interchange", "Night AmbientAmbientContrast", 1.14f, new ConfigDescription("", new AcceptableValueRange<float>(1.1f, 1.15f), new ConfigurationManagerAttributes { Order = 170 }));
            InterchangeFogLevel = Config.Bind("Interchange", "Fog Level", -100.0f, new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 160 }));
            InterchangeNVGNoiseIntensity = Config.Bind("Interchange", "NVG Noise Intensity", 0.25f, new ConfigDescription("", new AcceptableValueRange<float>(0f, 1f), new ConfigurationManagerAttributes { Order = 158 }));
            InterchangeNVGAmbientContrast = Config.Bind("Interchange", "NVG AmbientContrast", 1.1f, new ConfigDescription("", new AcceptableValueRange<float>(1f, 1.5f), new ConfigurationManagerAttributes { Order = 156 }));
            InterchangeNVGOriginalSkyColor = Config.Bind("Interchange", "NVG Original Sky Color", 0.7f, new ConfigDescription("Enables back the default sky color for NVG", new AcceptableValueRange<float>(0.001f, 1f), new ConfigurationManagerAttributes { Order = 154, IsAdvanced = true }));
            InterchangeTonemap = Config.Bind("Interchange", "Tonemap", ETonemap.ACES, new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 150 }));
            InterchangeACES = Config.Bind("Interchange", "ACES", new Vector3(20, 0.2f, 18), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 140, IsAdvanced = true }));
            InterchangeACESS = Config.Bind("Interchange", "ACESS", new Vector3(0, 1f, 0), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 130, IsAdvanced = true }));
            InterchangeFilmic = Config.Bind("Interchange", "Filmic", new Vector3(10f, 2f, 1.75f), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 120, IsAdvanced = true }));
            InterchangeFilmicS = Config.Bind("Interchange", "FilmicS", new Vector3(0, 0.4f, 0), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 110, IsAdvanced = true }));
            InterchangeMysticalGlowIntensity = Config.Bind("Interchange", "MysticalGlow Intensity", 0.45f, new ConfigDescription("", new AcceptableValueRange<float>(0.0f, 2.0f), new ConfigurationManagerAttributes { Order = 100, IsAdvanced = true }));

            WoodsFogLevel = Config.Bind("Woods", "Fog Level", -100.0f, new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 160 }));
            WoodsTonemap = Config.Bind("Woods", "Tonemap", ETonemap.ACES, new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 150 }));
            WoodsACES = Config.Bind("Woods", "ACES", new Vector3(20, 0.2f, 20), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 140, IsAdvanced = true }));
            WoodsACESS = Config.Bind("Woods", "ACESS", new Vector3(0, 1, 0), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 130, IsAdvanced = true }));
            WoodsFilmic = Config.Bind("Woods", "Filmic", new Vector3(10f, 2f, 1.75f), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 120, IsAdvanced = true }));
            WoodsFilmicS = Config.Bind("Woods", "FilmicS", new Vector3(0, 0.4f, 0), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 110, IsAdvanced = true }));
            WoodsMysticalGlowIntensity = Config.Bind("Woods", "MysticalGlow Intensity", 1.25f, new ConfigDescription("", new AcceptableValueRange<float>(0.0f, 2.0f), new ConfigurationManagerAttributes { Order = 100, IsAdvanced = true }));

            ReserveFogLevel = Config.Bind("Reserve", "Fog Level", -100.0f, new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 160 }));
            ReserveTonemap = Config.Bind("Reserve", "Tonemap", ETonemap.ACES, new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 150 }));
            ReserveACES = Config.Bind("Reserve", "ACES", new Vector3(20, 0.2f, 20), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 140, IsAdvanced = true }));
            ReserveACESS = Config.Bind("Reserve", "ACESS", new Vector3(0, 0.85f, 0), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 130, IsAdvanced = true }));
            ReserveFilmic = Config.Bind("Reserve", "Filmic", new Vector3(10f, 2f, 1.75f), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 120, IsAdvanced = true }));
            ReserveFilmicS = Config.Bind("Reserve", "FilmicS", new Vector3(0, 0.4f, 0), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 110, IsAdvanced = true }));
            ReserveMysticalGlowIntensity = Config.Bind("Reserve", "MysticalGlow Intensity", 0.75f, new ConfigDescription("", new AcceptableValueRange<float>(0.0f, 2.0f), new ConfigurationManagerAttributes { Order = 100, IsAdvanced = true }));

            ShorelineFogLevel = Config.Bind("Shoreline", "Fog Level", -100.0f, new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 160 }));
            ShorelineTonemap = Config.Bind("Shoreline", "Tonemap", ETonemap.ACES, new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 150 }));
            ShorelineACES = Config.Bind("Shoreline", "ACES", new Vector3(20, 0.2f, 20), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 140, IsAdvanced = true }));
            ShorelineACESS = Config.Bind("Shoreline", "ACESS", new Vector3(0, 1, 0), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 130, IsAdvanced = true }));
            ShorelineFilmic = Config.Bind("Shoreline", "Filmic", new Vector3(10f, 2f, 1.75f), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 120, IsAdvanced = true }));
            ShorelineFilmicS = Config.Bind("Shoreline", "FilmicS", new Vector3(0, 0.4f, 0), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 110, IsAdvanced = true }));
            ShorelineMysticalGlowIntensity = Config.Bind("Shoreline", "MysticalGlow Intensity", 0.5f, new ConfigDescription("", new AcceptableValueRange<float>(0.0f, 2.0f), new ConfigurationManagerAttributes { Order = 100, IsAdvanced = true }));

            HideoutTonemap = Config.Bind("Hideout", "Tonemap", ETonemap.ACES, new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 160 }));
            HideoutACES = Config.Bind("Hideout", "ACES", new Vector3(8, -0.1f, 8), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 150, IsAdvanced = true }));
            HideoutACESS = Config.Bind("Hideout", "ACESS", new Vector3(0, 0.85f, 0), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 140, IsAdvanced = true }));
            HideoutFilmic = Config.Bind("Hideout", "Filmic", new Vector3(4f, 2.1f, 1.75f), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 130, IsAdvanced = true }));
            HideoutFilmicS = Config.Bind("Hideout", "FilmicS", new Vector3(0, 0.65f, 0), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 120, IsAdvanced = true }));
            HideoutSkyColor = Config.Bind("Hideout", "SkyColor", new Vector4(0.6f, 0.6f, 0.6f), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 110, IsAdvanced = true }));

            LightColorIndex0 = Config.Bind("AmandsGraphics LightColor", "Index0", new Vector4(232.0f, 240.0f, 255.0f) / 255.0f, new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 160, IsAdvanced = true }));
            LightColorIndex1 = Config.Bind("AmandsGraphics LightColor", "Index1", new Vector4(0, 0, 0), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 150, IsAdvanced = true }));
            //LightColorIndex2 = Config.Bind("AmandsGraphics LightColor", "Index2", new Vector4(255.0f, 186.0f, 168.0f) / 255.0f, new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 140, IsAdvanced = true }));
            LightColorIndex2 = Config.Bind("AmandsGraphics LightColor", "Index2", new Vector4(1.0f, 0.457f, 0.322f, 1.0f), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 140, IsAdvanced = true }));
            LightColorIndex3 = Config.Bind("AmandsGraphics LightColor", "Index3", new Vector4(219.0f, 191.0f, 160.0f) / 255.0f, new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 130, IsAdvanced = true }));
            LightColorIndex4 = Config.Bind("AmandsGraphics LightColor", "Index4", new Vector4(255.0f, 238.0f, 196.0f) / 255.0f, new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 120, IsAdvanced = true }));
            LightColorIndex5 = Config.Bind("AmandsGraphics LightColor", "Index5", new Vector4(150.0f, 143.0f, 122.0f) / 255.0f, new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 110, IsAdvanced = true }));

            if (RequestDefaultValues) DefaultValues();

            new AmandsPlayerPatch().Enable();
            new AmandsGraphicsNVGPatch().Enable();
            new AmandsGraphicsApplyNVGPatch().Enable();
            new AmandsGraphicsHBAOPatch().Enable();
            new AmandsGraphicsPrismEffectsPatch().Enable();
            new AmandsGraphicsOpticPatch().Enable();
            new AmandsGraphicsOpticSightPatch().Enable();
            new AmandsGraphicsCameraClassPatch().Enable();
            new AmandsGraphicsmethod_25Patch().Enable();
            new AmandsGraphicsTacticalComboVisualControllerPatch().Enable();
            new AmandsGraphicsFastBlurPatch().Enable();
            new AmandsGraphicsMethod_7Patch().Enable();
            new AmandsGraphicsFastBlurHitPatch().Enable();
            new AmandsBattleUIScreenPatch().Enable();
            new AmandsEffectsControllerPatch().Enable();
        }
        private void DefaultValues()
        {
            LabsACES.Value = (Vector3)LabsACES.DefaultValue;
            LabsACESS.Value = (Vector3)LabsACESS.DefaultValue;
            LabsFilmicS.Value = (Vector3)LabsFilmicS.DefaultValue;
            FactoryACES.Value = (Vector3)FactoryACES.DefaultValue;
            FactoryACESS.Value = (Vector3)FactoryACESS.DefaultValue;
            FactoryFilmic.Value = (Vector3)FactoryFilmic.DefaultValue;
            FactoryNightACES.Value = (Vector3)FactoryNightACES.DefaultValue;
            FactoryNightACESS.Value = (Vector3)FactoryNightACESS.DefaultValue;
            FactoryNightFilmic.Value = (Vector3)FactoryNightFilmic.DefaultValue;
            FactoryNightFilmicS.Value = (Vector3)FactoryNightFilmicS.DefaultValue;
        }
    }
    public class AmandsPlayerPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return typeof(Player).GetMethod("Init", BindingFlags.Instance | BindingFlags.Public);
        }
        [PatchPostfix]
        private static void PatchPostFix(ref Player __instance)
        {
            if (__instance != null && __instance.IsYourPlayer)
            {
                AmandsGraphicsClass.Player = __instance;
            }
        }
    }
    public class AmandsGraphicsNVGPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return typeof(BSG.CameraEffects.NightVision).GetMethods(BindingFlags.Instance | BindingFlags.Public).First(x => x.GetParameters().Count() == 1 && x.GetParameters()[0].Name == "on" && x.Name != "StartSwitch");
        }
        [PatchPostfix]
        private static void PatchPostFix(ref BSG.CameraEffects.NightVision __instance, bool on)
        {
            if (AmandsGraphicsPlugin.AmandsGraphicsClassComponent.GraphicsMode && AmandsGraphicsClass.Player != null && AmandsGraphicsClass.NVG != on && AmandsGraphicsClass.FPSCameraNightVision != null)
            {
                AmandsGraphicsClass.NVG = on;
                AmandsGraphicsPlugin.AmandsGraphicsClassComponent.UpdateAmandsGraphics();
            }
            AmandsGraphicsClass.NVG = on;
        }
    }
    public class AmandsGraphicsApplyNVGPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return typeof(BSG.CameraEffects.NightVision).GetMethod("StartSwitch", BindingFlags.Instance | BindingFlags.Public);
        }
        [PatchPostfix]
        private static void PatchPostFix(ref BSG.CameraEffects.NightVision __instance)
        {
            if (AmandsGraphicsPlugin.AmandsGraphicsClassComponent.GraphicsMode && AmandsGraphicsClass.Player != null)
            {
                AmandsGraphicsClass.defaultNightVisionNoiseIntensity = __instance.NoiseIntensity;
                switch (AmandsGraphicsClass.scene)
                {
                    case "Shopping_Mall_Terrain":
                        __instance.NoiseIntensity = AmandsGraphicsClass.defaultNightVisionNoiseIntensity * AmandsGraphicsPlugin.InterchangeNVGNoiseIntensity.Value;
                        break;
                    default:
                        __instance.NoiseIntensity = AmandsGraphicsClass.defaultNightVisionNoiseIntensity * AmandsGraphicsPlugin.NVGNoiseIntensity.Value;
                        break;
                }
                __instance.ApplySettings();
            }
        }
    }
    public class AmandsGraphicsHBAOPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return typeof(HBAO_Core).GetMethod("ApplyPreset", BindingFlags.Instance | BindingFlags.Public);
        }
        [PatchPostfix]
        private static void PatchPostFix(ref HBAO_Core __instance, HBAO_Core.Preset preset)
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
            return typeof(PrismEffects).GetMethod("OnEnable", BindingFlags.Instance | BindingFlags.Public);
        }
        [PatchPostfix]
        private static void PatchPostFix(ref PrismEffects __instance)
        {
            if (__instance.gameObject.name == "FPS Camera")
            {
                AmandsGraphicsPlugin.AmandsGraphicsClassComponent.GraphicsMode = false;
                OnEnableASync(__instance);
            }
        }
        private async static void OnEnableASync(PrismEffects instance)
        {
            await Task.Delay(100);
            AmandsGraphicsPlugin.AmandsGraphicsClassComponent.ActivateAmandsGraphics(instance.gameObject, instance);
        }
    }
    public class AmandsGraphicsOpticPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return typeof(OpticComponentUpdater).GetMethod("Awake", BindingFlags.Instance | BindingFlags.Public);
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
            return typeof(OpticSight).GetMethod("OnEnable", BindingFlags.Instance | BindingFlags.Public);
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
    public class AmandsGraphicsCameraClassPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return typeof(CameraClass).GetMethod("Blur", BindingFlags.Instance | BindingFlags.Public);
        }
        [PatchPrefix]
        private static bool PatchPrefix(ref CameraClass __instance, bool isActive, float time)
        {
            AmandsGraphicsClass.CameraClassBlur = isActive;
            if (!isActive && __instance.IsActive)
            {
                return true;
            }
            return AmandsGraphicsPlugin.UIDepthOfField.Value == EUIDepthOfField.Off;
        }
    }
    public class AmandsGraphicsmethod_25Patch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return typeof(EFT.Animations.ProceduralWeaponAnimation).GetMethods(BindingFlags.Instance | BindingFlags.Public).First(x => x.GetParameters().Count() == 1 && x.GetParameters()[0].Name == "currentScopeIndex");
        }
        [PatchPostfix]
        private static void PatchPostFix(ref EFT.Animations.ProceduralWeaponAnimation __instance)
        {
            if (AmandsGraphicsClass.Player != null && AmandsGraphicsClass.Player.ProceduralWeaponAnimation == __instance)
            {
                object CurrentScope = Traverse.Create(AmandsGraphicsClass.Player.ProceduralWeaponAnimation).Property("CurrentScope").GetValue<object>();
                if (CurrentScope != null)
                {
                    ScopePrefabCache scopePrefabCache = Traverse.Create(CurrentScope).Field("ScopePrefabCache").GetValue<ScopePrefabCache>();
                    if (scopePrefabCache != null)
                    {
                        SightComponent Mod = Traverse.Create(CurrentScope).Field("Mod").GetValue<SightComponent>();
                        if (Mod != null && Mod.SelectedScopeIndex == 0)
                        {
                            AmandsGraphicsClass.aimingMode = EAimingMode.Sight;
                        }
                        else
                        {
                            AmandsGraphicsClass.aimingMode = EAimingMode.IronSight;
                        }
                    }
                    else
                    {
                        AmandsGraphicsClass.aimingMode = EAimingMode.IronSight;
                    }
                }
            }
        }
    }
    public class AmandsGraphicsTacticalComboVisualControllerPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return typeof(TacticalComboVisualController).GetMethod("UpdateBeams", BindingFlags.Instance | BindingFlags.Public);
        }
        [PatchPostfix]
        private static void PatchPostFix(ref TacticalComboVisualController __instance)
        {
            if (AmandsGraphicsPlugin.Flashlight.Value == EEnabledFeature.On && AmandsGraphicsClass.Player != null && Vector3.Distance(__instance.transform.position, AmandsGraphicsClass.Player.Position) < 5f && AmandsGraphicsClass.Player.HandsController != null && __instance.transform.IsChildOf(AmandsGraphicsClass.Player.HandsController.WeaponRoot))
            {
                foreach (Light light in Traverse.Create(__instance).Field("light_0").GetValue<Light[]>())
                {
                    if (!AmandsGraphicsClass.registeredLights.ContainsKey(light))
                    {
                        AmandsGraphicsClass.registeredLights.Add(light,light.range);
                    }
                    if (AmandsGraphicsPlugin.AmandsGraphicsClassComponent.GraphicsMode) light.range = AmandsGraphicsClass.registeredLights[light] * AmandsGraphicsPlugin.FlashlightRange.Value;
                    VolumetricLight volumetricLight = light.GetComponent<VolumetricLight>();
                    if (volumetricLight != null)
                    {
                        if (!AmandsGraphicsClass.registeredVolumetricLights.ContainsKey(volumetricLight))
                        {
                            AmandsGraphicsClass.registeredVolumetricLights.Add(volumetricLight, volumetricLight.ExtinctionCoef);
                        }
                        if (AmandsGraphicsPlugin.AmandsGraphicsClassComponent.GraphicsMode)
                        {
                            volumetricLight.ExtinctionCoef = AmandsGraphicsPlugin.FlashlightExtinctionCoef.Value;
                            if (volumetricLight.VolumetricMaterial != null)
                            {
                                volumetricLight.VolumetricMaterial.SetVector("_VolumetricLight", new Vector4(volumetricLight.ScatteringCoef, volumetricLight.ExtinctionCoef, AmandsGraphicsPlugin.FlashlightRange.Value, 1f - volumetricLight.SkyboxExtinctionCoef));
                            }
                        }
                    }
                }
            }
        }
    }
    public class AmandsGraphicsFastBlurPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return typeof(FastBlur).GetMethod("Start", BindingFlags.Instance | BindingFlags.Public);
        }
        [PatchPostfix]
        private static void PatchPostFix(ref FastBlur __instance)
        {
            if (__instance.gameObject.name == "FPS Camera")
            {
                AmandsGraphicsClass.fastBlur = __instance;
                PostProcessLayer FPSCameraPostProcessLayer = __instance.gameObject.GetComponent<PostProcessLayer>();
                if (FPSCameraPostProcessLayer != null)
                {
                    AmandsGraphicsClass.FPSCameraChromaticAberration = Traverse.Create(FPSCameraPostProcessLayer).Field("m_Bundles").GetValue<Dictionary<Type, PostProcessBundle>>()[typeof(UnityEngine.Rendering.PostProcessing.ChromaticAberration)].settings as UnityEngine.Rendering.PostProcessing.ChromaticAberration;
                    AmandsGraphicsClass.FPSCameraChromaticAberration.enabled.value = false;
                }
            }
        }
    }
    public class AmandsGraphicsMethod_7Patch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return typeof(EffectsController).GetMethod("method_7", BindingFlags.Instance | BindingFlags.Public);
        }
        [PatchPostfix]
        private static void PatchPostFix(ref EffectsController __instance)
        {
            if (AmandsGraphicsClass.fastBlur != null && AmandsGraphicsPlugin.HealthEffectHit.Value == EEnabledFeature.On)
            {
                AmandsGraphicsClass.fastBlur.enabled = false;
            }
        }
    }
    public class AmandsGraphicsFastBlurHitPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return typeof(FastBlur).GetMethod("Hit", BindingFlags.Instance | BindingFlags.Public);
        }
        [PatchPostfix]
        private static void PatchPostFix(ref FastBlur __instance, float power)
        {
            if (AmandsGraphicsPlugin.HealthEffectHit.Value == EEnabledFeature.On)
            {
                AmandsGraphicsPlugin.AmandsGraphicsClassComponent.AmandsGraphicsHitEffect(power);
            }
        }
    }
    public class AmandsBattleUIScreenPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return typeof(EFT.UI.EftBattleUIScreen).GetMethods(BindingFlags.Instance | BindingFlags.Public).First(x => x.Name == "Show" && x.GetParameters()[0].Name == "owner");
        }
        [PatchPostfix]
        private static void PatchPostFix(ref EFT.UI.EftBattleUIScreen __instance)
        {
            if (AmandsGraphicsClass.ActiveUIScreen == __instance.gameObject) return;
            AmandsGraphicsClass.ActiveUIScreen = __instance.gameObject;
            AmandsGraphicsClass.DestroyGameObjects();
            AmandsGraphicsClass.CreateGameObjects(__instance.transform);
        }
    }
    public class AmandsEffectsControllerPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return typeof(EffectsController).GetMethod("Awake", BindingFlags.Instance | BindingFlags.Public);
        }
        [PatchPostfix]
        private static void PatchPostFix(ref EffectsController __instance)
        {
            if (AmandsGraphicsPlugin.PainKillerEffectType == null || AmandsGraphicsPlugin.PainEffectType == null)
            {
                object EffectsList = Traverse.Create(__instance).Field("list_0").GetValue<object>();
                object[] EffectsListItems = Traverse.Create(EffectsList).Field("_items").GetValue<object[]>();
                if (EffectsListItems != null)
                {
                    foreach (object Effect in EffectsListItems)
                    {
                        if (AmandsGraphicsPlugin.PainKillerEffectType == null && Traverse.Create(Effect).Field("float_4").FieldExists())
                        {
                            CC_Sharpen cc_Sharpen_0 = Traverse.Create(Effect).Field("cc_Sharpen_0").GetValue<CC_Sharpen>();
                            if (cc_Sharpen_0 != null)
                            {
                                AmandsGraphicsPlugin.PainKillerEffectType = Effect.GetType();
                                new AmandsPainkillerAddEffectPatch().Enable();
                                new AmandsPainkillerDeleteEffectPatch().Enable();
                                continue;
                            }
                        }
                        if (AmandsGraphicsPlugin.PainEffectType == null && Traverse.Create(Effect).Field("cc_RadialBlur_0").FieldExists())
                        {
                            AmandsGraphicsPlugin.PainEffectType = Effect.GetType();
                            new AmandsPainTogglePatch().Enable();
                            continue;
                        }
                    }
                }
            }
        }
    }
    public class AmandsPainkillerAddEffectPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return AmandsGraphicsPlugin.PainKillerEffectType.GetMethod("AddEffect", BindingFlags.Instance | BindingFlags.Public);
        }
        [PatchPostfix]
        private static void PatchPostFix(ref object __instance)
        {
            if (AmandsGraphicsPlugin.HealthEffectPainkiller.Value == EEnabledFeature.On)
            {
                List<IEffect> ActiveEffects = Traverse.Create(__instance).Field("ActiveEffects").GetValue<List<IEffect>>();
                if (ActiveEffects != null)
                {
                    /*bool bool_1 = Traverse.Create(__instance).Field("bool_1").GetValue<bool>();
                    float float_2 = Traverse.Create(__instance).Field("float_2").GetValue<float>();*/

                    float maxEffectValue;
                    if (ActiveEffects.Count <= 0)
                    {
                        maxEffectValue = 0f;
                    }
                    else
                    {
                        maxEffectValue = Mathf.Min(1.0f * AmandsGraphicsPlugin.PainkillerSaturation.Value, 1f);
                    }
                    Traverse.Create(__instance).Field("MaxEffectValue").SetValue(maxEffectValue);
                    /*if (bool_1)
                    {
                        Traverse.Create(__instance).Field("float_3").SetValue((ActiveEffects.Count > 0) ? 0.015f * AmandsGraphicsPlugin.PainkillerCAIntensity.Value : float_2);
                    }*/
                }
            }
        }
    }
    public class AmandsPainkillerDeleteEffectPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return AmandsGraphicsPlugin.PainKillerEffectType.GetMethod("DeleteEffect", BindingFlags.Instance | BindingFlags.Public);
        }
        [PatchPostfix]
        private static void PatchPostFix(ref object __instance)
        {
            if (AmandsGraphicsPlugin.HealthEffectPainkiller.Value == EEnabledFeature.On)
            {
                List<IEffect> ActiveEffects = Traverse.Create(__instance).Field("ActiveEffects").GetValue<List<IEffect>>();
                if (ActiveEffects != null)
                {
                    /*bool bool_1 = Traverse.Create(__instance).Field("bool_1").GetValue<bool>();
                    float float_2 = Traverse.Create(__instance).Field("float_2").GetValue<float>();*/

                    float maxEffectValue;
                    if (ActiveEffects.Count <= 0)
                    {
                        maxEffectValue = 0f;
                    }
                    else
                    {
                        maxEffectValue = Mathf.Min(1.0f * AmandsGraphicsPlugin.PainkillerSaturation.Value, 1f);
                    }
                    Traverse.Create(__instance).Field("MaxEffectValue").SetValue(maxEffectValue);
                    /*if (bool_1)
                    {
                        Traverse.Create(__instance).Field("float_3").SetValue((ActiveEffects.Count > 0) ? 0.015f * AmandsGraphicsPlugin.PainkillerCAIntensity.Value : float_2);
                    }*/
                }
            }
        }
    }
    public class AmandsPainTogglePatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return AmandsGraphicsPlugin.PainEffectType.GetMethod("Toggle", BindingFlags.Instance | BindingFlags.Public);
        }
        [PatchPrefix]
        private static bool PatchPreFix(ref object __instance, ref bool value)
        {
            if (AmandsGraphicsPlugin.HealthEffectPain.Value == EEnabledFeature.On)
            {
                value = false;
            }
            return true;
        }
    }
}
