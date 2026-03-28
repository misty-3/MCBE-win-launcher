using Newtonsoft.Json;

namespace MCLauncher {
    public class Preferences {
        public bool ShowInstalledOnly { get; set; } = false;

        public bool DeleteAppxAfterDownload { get; set; } = true;

        [JsonProperty("VersionsApi")]
        public string VersionsApiUWP { get; set; } = "";

        public string VersionsApiGDK { get; set; } = "";

        public bool HasPreviouslyUsedGDK { get; set; } = false;

        public bool ShowLegacyBetaTab { get; set; } = false;

        public string Language { get; set; } = "en";

        // Custom launcher data path - if empty, uses default LocalApplicationData
        public string LauncherDataPath { get; set; } = "";

        // Color customization preferences
        public string Color_DarkBg { get; set; } = "";
        public string Color_CardBg { get; set; } = "";
        public string Color_CardHover { get; set; } = "";
        public string Color_AccentGreen { get; set; } = "";
        public string Color_AccentBlue { get; set; } = "";
        public string Color_AccentRed { get; set; } = "";
        public string Color_TextPrimary { get; set; } = "";
        public string Color_TextSecondary { get; set; } = "";
        public string Color_Border { get; set; } = "";
    }
}
