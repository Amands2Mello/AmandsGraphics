
using BepInEx;
using BepInEx.Configuration;
using UnityEngine;

namespace AmandsGraphics
{
    [BepInPlugin("com.Amanda.Graphics", "Graphics", "1.0.0")]
    public class AmandsGraphicsPlugin : BaseUnityPlugin
    {
        public static GameObject Hook;
        public static ConfigEntry<bool> UnrealMotionBlurEnabled { get; set; }
        public static ConfigEntry<int> UnrealMotionBlurSampleCount { get; set; }
        public static ConfigEntry<float> UnrealMotionBlurShutterAngle { get; set; }
        public static ConfigEntry<KeyboardShortcut> GraphicsToggle { get; set; }
        public static ConfigEntry<bool> Labs { get; set; }
        public static ConfigEntry<Vector3> LabsToneValues { get; set; }
        public static ConfigEntry<Vector3> LabsSecondaryToneValues { get; set; }
        public static ConfigEntry<bool> Customs { get; set; }
        public static ConfigEntry<Vector3> CustomsToneValues { get; set; }
        public static ConfigEntry<Vector3> CustomsSecondaryToneValues { get; set; }
        public static ConfigEntry<bool> FactoryDay { get; set; }
        public static ConfigEntry<Vector3> FactoryDayToneValues { get; set; }
        public static ConfigEntry<Vector3> FactoryDaySecondaryToneValues { get; set; }
        public static ConfigEntry<Vector4> FactoryDaySkyColor { get; set; }
        public static ConfigEntry<Vector4> FactoryDayNVSkyColor { get; set; }
        public static ConfigEntry<bool> FactoryNight { get; set; }
        public static ConfigEntry<Vector3> FactoryNightToneValues { get; set; }
        public static ConfigEntry<Vector3> FactoryNightSecondaryToneValues { get; set; }
        public static ConfigEntry<Vector4> FactoryNightSkyColor { get; set; }
        public static ConfigEntry<Vector4> FactoryNightNVSkyColor { get; set; }
        public static ConfigEntry<bool> Lighthouse { get; set; }
        public static ConfigEntry<Vector3> LighthouseToneValues { get; set; }
        public static ConfigEntry<Vector3> LighthouseSecondaryToneValues { get; set; }
        public static ConfigEntry<bool> Interchange { get; set; }
        public static ConfigEntry<Vector3> InterchangeToneValues { get; set; }
        public static ConfigEntry<Vector3> InterchangeSecondaryToneValues { get; set; }
        public static ConfigEntry<bool> Woods { get; set; }
        public static ConfigEntry<Vector3> WoodsToneValues { get; set; }
        public static ConfigEntry<Vector3> WoodsSecondaryToneValues { get; set; }
        public static ConfigEntry<bool> Reserve { get; set; }
        public static ConfigEntry<Vector3> ReserveToneValues { get; set; }
        public static ConfigEntry<Vector3> ReserveSecondaryToneValues { get; set; }
        public static ConfigEntry<bool> Shoreline { get; set; }
        public static ConfigEntry<Vector3> ShorelineToneValues { get; set; }
        public static ConfigEntry<Vector3> ShorelineSecondaryToneValues { get; set; }
        public static ConfigEntry<bool> Hideout { get; set; }
        public static ConfigEntry<Vector3> HideoutToneValues { get; set; }
        public static ConfigEntry<Vector3> HideoutSecondaryToneValues { get; set; }
        public static ConfigEntry<Vector4> HideoutSkyColor { get; set; }
        public static ConfigEntry<bool> DefaultGlobalFog { get; set; }
        public static ConfigEntry<float> GlobalFogIntensity { get; set; }
        public static ConfigEntry<float> FogZeroLevel { get; set; }
        public static ConfigEntry<float> BloomIntensity { get; set; }
        public static ConfigEntry<bool> LightColorEnabled { get; set; }
        public static ConfigEntry<Vector4> LightColorIndex0 { get; set; }
        public static ConfigEntry<Vector4> LightColorIndex1 { get; set; }
        public static ConfigEntry<Vector4> LightColorIndex2 { get; set; }
        public static ConfigEntry<Vector4> LightColorIndex3 { get; set; }
        public static ConfigEntry<Vector4> LightColorIndex4 { get; set; }
        public static ConfigEntry<Vector4> LightColorIndex5 { get; set; }
        private void Awake()
        {
            Debug.LogError("Unreal Awake()");
            Hook = new GameObject();
            Hook.AddComponent<AmandsGraphicsClass>();
            Object.DontDestroyOnLoad(Hook);

        }
        private void Start()
        {
            UnrealMotionBlurEnabled = Config.Bind<bool>("MotionBlur", "Enabled", false, new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 130, IsAdvanced = true }));
            UnrealMotionBlurSampleCount = Config.Bind<int>("MotionBlur", "SampleCount", 20, new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 120, IsAdvanced = true }));
            UnrealMotionBlurShutterAngle = Config.Bind<float>("MotionBlur", "ShutterAngle", 360f, new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 110, IsAdvanced = true }));
            GraphicsToggle = Config.Bind<KeyboardShortcut>("Graphics", "GraphicsToggle", new KeyboardShortcut(KeyCode.Insert), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 210 }));
            GlobalFogIntensity = Config.Bind<float>("Graphics", "GlobalFogIntensity", 0.0f, new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 140 }));
            DefaultGlobalFog = Config.Bind<bool>("Graphics", "DefaultGlobalFog", false, new ConfigDescription("Use default global fog blending mode", null, new ConfigurationManagerAttributes { Order = 130, IsAdvanced = true }));
            FogZeroLevel = Config.Bind<float>("Graphics", "FogZeroLevel", -200.0f, new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 120 }));
            BloomIntensity = Config.Bind<float>("Graphics", "BloomIntensity", 0.5f, new ConfigDescription("Bloom is only evaluated on start of a raid", null, new ConfigurationManagerAttributes { Order = 110, IsAdvanced = true }));
            Labs = Config.Bind<bool>("Labs", "Enabled", true, new ConfigDescription("Enables on start of a raid", null, new ConfigurationManagerAttributes { Order = 130, IsAdvanced = true }));
            LabsToneValues = Config.Bind<Vector3>("Labs", "toneValues", new Vector3(20, 0.2f, 20), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 120, IsAdvanced = true }));
            LabsSecondaryToneValues = Config.Bind<Vector3>("Labs", "secondaryToneValues", new Vector3(0, 2, 0), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 110, IsAdvanced = true }));
            Customs = Config.Bind<bool>("Customs", "Enabled", true, new ConfigDescription("Enables on start of a raid", null, new ConfigurationManagerAttributes { Order = 130, IsAdvanced = true }));
            CustomsToneValues = Config.Bind<Vector3>("Customs", "toneValues", new Vector3(20, 0.2f, 20), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 120, IsAdvanced = true }));
            CustomsSecondaryToneValues = Config.Bind<Vector3>("Customs", "secondaryToneValues", new Vector3(0, 1, 0), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 110, IsAdvanced = true }));
            FactoryDay = Config.Bind<bool>("FactoryDay", "Enabled", true, new ConfigDescription("Enables on start of a raid", null, new ConfigurationManagerAttributes { Order = 150, IsAdvanced = true }));
            FactoryDayToneValues = Config.Bind<Vector3>("FactoryDay", "toneValues", new Vector3(15, 1, 15), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 140, IsAdvanced = true }));
            FactoryDaySecondaryToneValues = Config.Bind<Vector3>("FactoryDay", "secondaryToneValues", new Vector3(0, 1, 0), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 130, IsAdvanced = true }));
            FactoryDaySkyColor = Config.Bind<Vector4>("FactoryDay", "SkyColor", new Vector4(0.9f, 0.8f, 0.7f), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 120, IsAdvanced = true }));
            FactoryDayNVSkyColor = Config.Bind<Vector4>("FactoryDay", "NVSkyColor", new Vector4(0.9f, 0.8f, 0.7f), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 110, IsAdvanced = true }));
            FactoryNight = Config.Bind<bool>("FactoryNight", "Enabled", true, new ConfigDescription("Enables on start of a raid", null, new ConfigurationManagerAttributes { Order = 150, IsAdvanced = true }));
            FactoryNightToneValues = Config.Bind<Vector3>("FactoryNight", "toneValues", new Vector3(10, -0.2f, 10), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 140, IsAdvanced = true }));
            FactoryNightSecondaryToneValues = Config.Bind<Vector3>("FactoryNight", "secondaryToneValues", new Vector3(0, 1, 0), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 130, IsAdvanced = true }));
            FactoryNightSkyColor = Config.Bind<Vector4>("FactoryNight", "SkyColor", new Vector4(0.09f, 0.08f, 0.07f), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 120, IsAdvanced = true }));
            FactoryNightNVSkyColor = Config.Bind<Vector4>("FactoryNight", "NVSkyColor", new Vector4(0.25f, 0.25f, 0.25f), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 110, IsAdvanced = true }));
            Lighthouse = Config.Bind<bool>("Lighthouse", "Enabled", true, new ConfigDescription("Enables on start of a raid", null, new ConfigurationManagerAttributes { Order = 130, IsAdvanced = true }));
            LighthouseToneValues = Config.Bind<Vector3>("Lighthouse", "toneValues", new Vector3(20, 0.2f, 20), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 120, IsAdvanced = true }));
            LighthouseSecondaryToneValues = Config.Bind<Vector3>("Lighthouse", "secondaryToneValues", new Vector3(0, 1, 0), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 110, IsAdvanced = true }));
            Interchange = Config.Bind<bool>("Interchange", "Enabled", true, new ConfigDescription("Enables on start of a raid", null, new ConfigurationManagerAttributes { Order = 130, IsAdvanced = true }));
            InterchangeToneValues = Config.Bind<Vector3>("Interchange", "toneValues", new Vector3(20, 3, 20), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 120, IsAdvanced = true }));
            InterchangeSecondaryToneValues = Config.Bind<Vector3>("Interchange", "secondaryToneValues", new Vector3(0.50f, 1, 0), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 110, IsAdvanced = true }));
            Woods = Config.Bind<bool>("Woods", "Enabled", true, new ConfigDescription("Enables on start of a raid", null, new ConfigurationManagerAttributes { Order = 130, IsAdvanced = true }));
            WoodsToneValues = Config.Bind<Vector3>("Woods", "toneValues", new Vector3(20, 0.2f, 20), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 120, IsAdvanced = true }));
            WoodsSecondaryToneValues = Config.Bind<Vector3>("Woods", "secondaryToneValues", new Vector3(0, 1, 0), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 110, IsAdvanced = true }));
            Reserve = Config.Bind<bool>("Reserve", "Enabled", true, new ConfigDescription("Enables on start of a raid", null, new ConfigurationManagerAttributes { Order = 130, IsAdvanced = true }));
            ReserveToneValues = Config.Bind<Vector3>("Reserve", "toneValues", new Vector3(20, 0.2f, 20), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 120, IsAdvanced = true }));
            ReserveSecondaryToneValues = Config.Bind<Vector3>("Reserve", "secondaryToneValues", new Vector3(0, 1, 0), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 110, IsAdvanced = true }));
            Shoreline = Config.Bind<bool>("Shoreline", "Enabled", true, new ConfigDescription("Enables on start of a raid", null, new ConfigurationManagerAttributes { Order = 130, IsAdvanced = true }));
            ShorelineToneValues = Config.Bind<Vector3>("Shoreline", "toneValues", new Vector3(20, 0.2f, 20), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 120, IsAdvanced = true }));
            ShorelineSecondaryToneValues = Config.Bind<Vector3>("Shoreline", "secondaryToneValues", new Vector3(0, 1, 0), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 110, IsAdvanced = true }));
            Hideout = Config.Bind<bool>("Hideout", "Enabled", true, new ConfigDescription("Enables on start of a raid", null, new ConfigurationManagerAttributes { Order = 130, IsAdvanced = true }));
            HideoutToneValues = Config.Bind<Vector3>("Hideout", "toneValues", new Vector3(8, -0.2f, 8), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 130, IsAdvanced = true }));
            HideoutSecondaryToneValues = Config.Bind<Vector3>("Hideout", "secondaryToneValues", new Vector3(0, 1, 0), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 120, IsAdvanced = true }));
            HideoutSkyColor = Config.Bind<Vector4>("Hideout", "SkyColor", new Vector4(0.5f, 0.5f, 0.5f), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 110, IsAdvanced = true }));

            LightColorEnabled = Config.Bind<bool>("LightColor", "New sun colors", true, new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 170 }));
            LightColorIndex0 = Config.Bind<Vector4>("LightColor", "Index0", new Vector4(232.0f, 240.0f, 255.0f) / 255.0f, new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 160, IsAdvanced = true }));
            //Vector4(0.809f, 0.881f, 1.0f, 1.0f)); // Default values
            LightColorIndex1 = Config.Bind<Vector4>("LightColor", "Index1", new Vector4(0,0,0), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 150, IsAdvanced = true }));
            //Vector4(0.0f, 0.0f, 0.0f)); // Default values
            LightColorIndex2 = Config.Bind<Vector4>("LightColor", "Index2", new Vector4(255.0f, 186.0f, 168.0f) / 255.0f, new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 140, IsAdvanced = true }));
            //Vector4(1.0f, 0.457f, 0.322f, 1.0f)); // Default values
            LightColorIndex3 = Config.Bind<Vector4>("LightColor", "Index3", new Vector4(219.0f, 191.0f, 160.0f) / 255.0f, new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 130, IsAdvanced = true }));
            //Vector4(0.859f, 0.631f, 0.392f, 1.0f)); // Default values
            LightColorIndex4 = Config.Bind<Vector4>("LightColor", "Index4", new Vector4(255.0f, 238.0f, 196.0f) / 255.0f, new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 120, IsAdvanced = true }));
            //Vector4(1.0f, 0.867f, 0.537f, 1.0f)); // Default values
            LightColorIndex5 = Config.Bind<Vector4>("LightColor", "Index5", new Vector4(150.0f, 143.0f, 122.0f) / 255.0f, new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 110, IsAdvanced = true }));
            //Vector4(0.585f, 0.530f, 0.361f, 1.0f)); // Default values
        }
    }
}