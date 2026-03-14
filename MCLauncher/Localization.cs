using System.Collections.Generic;

namespace MCLauncher
{
    public static class Localization
    {
        private static string _currentLanguage = "en";
        
        private static Dictionary<string, Dictionary<string, string>> _translations = new Dictionary<string, Dictionary<string, string>>
        {
            // English translations
            ["en"] = new Dictionary<string, string>
            {
                // Window titles
                ["AppTitle"] = "MCBE Launcher",
                ["SettingsTitle"] = "Settings",
                
                // Navigation
                ["MyVersions"] = "🏠 My Versions",
                ["Browse"] = "📦 Browse",
                ["Navigation"] = "NAVIGATION",
                
                // My Versions tab
                ["MyVersionsHeader"] = "My Versions",
                ["MyVersionsSubtitle"] = "Manage your installed Minecraft versions",
                ["ImportFile"] = "➕ Import File",
                ["Play"] = "▶️ Play",
                ["Remove"] = "🗑️",
                ["RemoveTooltip"] = "Remove",
                
                // Empty state
                ["NoVersionsInstalled"] = "No versions installed",
                ["NoVersionsSubtitle"] = "Browse available versions to get started",
                ["BrowseVersionsButton"] = "📦 Browse Versions",
                
                // Browse tab
                ["BrowseVersionsHeader"] = "Browse Versions",
                ["BrowseVersionsSubtitle"] = "Download any Minecraft version",
                ["Refresh"] = "🔄 Refresh",
                ["SearchPlaceholder"] = "🔍 Search versions...",
                ["All"] = "All",
                ["Release"] = "⭐ Release",
                ["Preview"] = "✨ Preview",
                ["Download"] = "⬇️ Download",
                
                // Version types
                ["StableRelease"] = "⭐ Stable Release - Recommended!",
                ["PreviewVersion"] = "✨ Preview - Try new features early!",
                ["BetaVersion"] = "🧪 Beta - Old test version",
                ["ImportedVersion"] = "📦 Imported from file",
                
                // Status messages
                ["ReadyToPlay"] = "Ready to play!",
                ["NotInstalled"] = "Not installed",
                ["TypeXboxGDK"] = "Type: Xbox/GDK",
                ["TypeWindowsStore"] = "Type: Windows Store",
                
                // Loading
                ["LoadingVersions"] = "Loading versions...",
                ["LoadingCached"] = "Loading cached versions...",
                ["DownloadingList"] = "Downloading latest version list...",
                ["LoadingImported"] = "Loading imported versions...",
                
                // Messages
                ["LanguageChanged"] = "Language Changed",
                ["LanguageChangedMessage"] = "Language changed to English. Full translation will be applied in a future update.",
                ["MinecraftStarting"] = "Minecraft is starting! 🎮",
                ["MinecraftStartingMessage"] = "{0} is launching!\n\nYour worlds are ready to play!",
                ["VersionRemoved"] = "Removed!",
                ["VersionRemovedMessage"] = "{0} has been removed.",
                ["DownloadComplete"] = "Download complete!",
                ["DownloadCompleteMessage"] = "{0} is ready to play!\n\nGo to 'My Versions' to launch it.",
                ["VersionAdded"] = "Version added!",
                ["VersionAddedMessage"] = "{0} is ready to play!",
                
                // Error messages
                ["Error"] = "Error",
                ["CouldntUpdateList"] = "Couldn't update version list",
                ["CouldntUpdateListMessage"] = "We couldn't download the latest versions from the internet. You can still use cached versions, but some new ones might be missing.",
                ["CouldntPrepareWorlds"] = "Couldn't prepare worlds",
                ["CouldntPrepareWorldsMessage"] = "We had trouble preparing your worlds for this version.\n\nYour worlds are safe, but you might need to move them manually.",
                ["CouldntRegister"] = "Couldn't register version",
                ["CouldntRegisterMessage"] = "We couldn't register this Minecraft version with Windows.\n\nMake sure Minecraft isn't already running and try again.",
                ["CouldntLaunch"] = "Couldn't launch Minecraft",
                ["CouldntLaunchMessage"] = "Minecraft didn't start. Make sure:\n• Minecraft isn't already running\n• You have the Store version installed\n\nTry restarting the launcher if the problem continues.",
                ["CouldntRemove"] = "Couldn't remove",
                ["CouldntRemoveMessage"] = "We couldn't remove this version. Make sure Minecraft isn't running and try again.",
                ["DownloadFailed"] = "Download failed",
                ["DownloadFailedMessage"] = "We couldn't download this version. Check your internet connection and try again.",
                ["ExtractionFailed"] = "Extraction failed",
                ["ExtractionFailedMessage"] = "The file might be corrupted or not a valid Minecraft package. Please try a different file.",
                ["PleaseWait"] = "Please wait",
                ["PleaseWaitMessage"] = "This version is currently being modified. Please wait a moment and try again.",
                
                // Dialogs
                ["RemoveVersion"] = "Remove version?",
                ["RemoveVersionMessage"] = "Are you sure you want to remove {0}?\n\nYour worlds will be safe, but you'll need to download this version again if you want to play it later.",
                ["ReplaceExisting"] = "Replace existing version?",
                ["ReplaceExistingMessage"] = "You already have a version with this name. Do you want to replace it?",
                ["CouldntRemoveOld"] = "Couldn't remove old version",
                ["CouldntRemoveOldMessage"] = "We couldn't remove the existing version. Import cancelled.",
                ["WrongFileType"] = "Wrong file type",
                ["WrongFileTypeMessage"] = "This file type {0} isn't supported. Please choose a .appx or .msixvc file.",
                
                // GDK Warning
                ["FirstTimeSetup"] = "First Time Setup",
                ["GDKWarningMessage"] = "⚠️ Important: First time using Xbox/GDK versions!\n\nBefore you continue, make sure:\n✓ You have Minecraft installed from the Microsoft Store\n✓ You have at least 2GB of free space\n\nDuring installation, you might see some windows pop up briefly - this is normal!\n\nYour worlds will be automatically backed up.\n\nReady to continue?",
                
                // Multiple world locations
                ["MultipleWorldLocations"] = "Multiple World Locations",
                ["MultipleWorldLocationsMessage"] = "Worlds were found in multiple locations:\n{0}\n\nThe version will look for worlds in: {1}\n\nSome worlds may not be visible. Continue anyway?",
                
                // Download status
                ["StatusPreparing"] = "Preparing...",
                ["StatusDownloading"] = "Downloading... {0}MiB/{1}MiB",
                ["StatusExtracting"] = "Extracting...",
                ["StatusRegistering"] = "Registering package...",
                ["StatusLaunching"] = "Launching...",
                ["StatusUnregistering"] = "Unregistering package...",
                ["StatusCleaningUp"] = "Cleaning up...",
                ["StatusStaging"] = "Staging package... (this might take a few minutes)",
                ["StatusDecrypting"] = "Copying decrypted Minecraft.Windows.exe...",
                ["StatusMoving"] = "Copying other game files...",
                ["StatusMovingData"] = "Restoring Minecraft worlds...",
                
                // Buttons
                ["ViewFiles"] = "📁 View Files",
                ["Pause"] = "⏸️ Pause",
                ["Resume"] = "▶️ Resume",
                ["Cancel"] = "❌ Cancel",
                
                // Status Indicators
                ["DeveloperMode"] = "Developer Mode",
                ["DecryptionKeys"] = "Decryption Keys",
                ["DevModeRequired"] = "Developer Mode Required",
                ["DevModeRequiredMessage"] = "Developer Mode must be enabled to install Minecraft versions.\n\nThis is a Windows requirement for installing apps outside the Store.\n\nWould you like to enable it now? (requires admin)",
                ["TurnOn"] = "Turn On",
                ["DecryptKeysRequired"] = "Decryption Keys Required",
                ["DecryptKeysRequiredMessage"] = "To install Xbox/GDK versions of Minecraft, you need decryption keys.\n\nThese keys are installed automatically when you run Minecraft from the Microsoft Store at least once.\n\nWould you like to open the Store to install Minecraft?",
                ["TakeMeThere"] = "Take Me There",
                ["DevModeEnabled"] = "Developer Mode Enabled!",
                ["DevModeEnabledMessage"] = "Developer Mode has been enabled successfully!\n\nYou can now install Minecraft versions.",
                ["DevModeEnableFailed"] = "Couldn't Enable Developer Mode",
                ["DevModeEnableFailedMessage"] = "We couldn't enable Developer Mode automatically.\n\nPlease enable it manually:\n1. Open Windows Settings\n2. Go to 'Update & Security' (or 'Privacy & Security' on Windows 11)\n3. Click 'For developers'\n4. Turn on 'Developer Mode'\n5. Wait for it to install",
            },
            
            // Arabic (Iraqi dialect) translations
            ["ar"] = new Dictionary<string, string>
            {
                // Window titles
                ["AppTitle"] = "مشغّل ماين كرافت",
                ["SettingsTitle"] = "الإعدادات",
                
                // Navigation
                ["MyVersions"] = "🏠 نسخي",
                ["Browse"] = "📦 تصفح",
                ["Navigation"] = "القائمة",
                
                // My Versions tab
                ["MyVersionsHeader"] = "نسخي المثبتة",
                ["MyVersionsSubtitle"] = "دير نسخ ماين كرافت المثبتة عندك",
                ["ImportFile"] = "➕ استورد ملف",
                ["Play"] = "▶️ العب",
                ["Remove"] = "🗑️",
                ["RemoveTooltip"] = "احذف",
                
                // Empty state
                ["NoVersionsInstalled"] = "ما عندك نسخ مثبتة",
                ["NoVersionsSubtitle"] = "روح تصفح النسخ المتاحة وابدأ",
                ["BrowseVersionsButton"] = "📦 تصفح النسخ",
                
                // Browse tab
                ["BrowseVersionsHeader"] = "تصفح النسخ",
                ["BrowseVersionsSubtitle"] = "نزّل أي نسخة من ماين كرافت",
                ["Refresh"] = "🔄 حدّث",
                ["SearchPlaceholder"] = "🔍 دوّر على نسخة...",
                ["All"] = "الكل",
                ["Release"] = "⭐ مستقرة",
                ["Preview"] = "✨ تجريبية",
                ["Download"] = "⬇️ نزّل",
                
                // Version types
                ["StableRelease"] = "⭐ نسخة مستقرة - ننصح بيها!",
                ["PreviewVersion"] = "✨ نسخة تجريبية - جرب المميزات الجديدة!",
                ["BetaVersion"] = "🧪 بيتا - نسخة تجريبية قديمة",
                ["ImportedVersion"] = "📦 مستوردة من ملف",
                
                // Status messages
                ["ReadyToPlay"] = "جاهزة للعب!",
                ["NotInstalled"] = "مو مثبتة",
                ["TypeXboxGDK"] = "النوع: Xbox/GDK",
                ["TypeWindowsStore"] = "النوع: متجر ويندوز",
                
                // Loading
                ["LoadingVersions"] = "يحمّل النسخ...",
                ["LoadingCached"] = "يحمّل النسخ المحفوظة...",
                ["DownloadingList"] = "ينزّل آخر قائمة نسخ...",
                ["LoadingImported"] = "يحمّل النسخ المستوردة...",
                
                // Messages
                ["LanguageChanged"] = "تغيرت اللغة",
                ["LanguageChangedMessage"] = "تم تغيير اللغة للعربية بنجاح! 🎉",
                ["MinecraftStarting"] = "ماين كرافت يشتغل! 🎮",
                ["MinecraftStartingMessage"] = "{0} يشتغل الحين!\n\nعوالمك جاهزة للعب!",
                ["VersionRemoved"] = "انحذفت!",
                ["VersionRemovedMessage"] = "{0} انحذفت بنجاح.",
                ["DownloadComplete"] = "التنزيل خلص!",
                ["DownloadCompleteMessage"] = "{0} جاهزة للعب!\n\nروح لـ 'نسخي' وشغّلها.",
                ["VersionAdded"] = "انضافت النسخة!",
                ["VersionAddedMessage"] = "{0} جاهزة للعب!",
                
                // Error messages
                ["Error"] = "خطأ",
                ["CouldntUpdateList"] = "ما قدرنا نحدّث القائمة",
                ["CouldntUpdateListMessage"] = "ما قدرنا ننزّل آخر النسخ من النت. تكدر تستخدم النسخ المحفوظة، بس ممكن بعض النسخ الجديدة ما تطلع.",
                ["CouldntPrepareWorlds"] = "ما قدرنا نجهز العوالم",
                ["CouldntPrepareWorldsMessage"] = "صارت مشكلة بتجهيز عوالمك لهاي النسخة.\n\nعوالمك بأمان، بس ممكن تحتاج تنقلهم يدوياً.",
                ["CouldntRegister"] = "ما قدرنا نسجل النسخة",
                ["CouldntRegisterMessage"] = "ما قدرنا نسجل نسخة ماين كرافت هاي بالويندوز.\n\nتأكد إن ماين كرافت مو شغال وحاول مرة ثانية.",
                ["CouldntLaunch"] = "ما قدرنا نشغّل ماين كرافت",
                ["CouldntLaunchMessage"] = "ماين كرافت ما اشتغل. تأكد من:\n• ماين كرافت مو شغال أصلاً\n• عندك نسخة المتجر مثبتة\n\nحاول تعيد تشغيل المشغّل إذا المشكلة باقية.",
                ["CouldntRemove"] = "ما قدرنا نحذف",
                ["CouldntRemoveMessage"] = "ما قدرنا نحذف هاي النسخة. تأكد إن ماين كرافت مو شغال وحاول مرة ثانية.",
                ["DownloadFailed"] = "التنزيل فشل",
                ["DownloadFailedMessage"] = "ما قدرنا ننزّل هاي النسخة. تحقق من اتصال النت وحاول مرة ثانية.",
                ["ExtractionFailed"] = "فك الضغط فشل",
                ["ExtractionFailedMessage"] = "الملف ممكن يكون تالف أو مو ملف ماين كرافت صحيح. جرب ملف ثاني.",
                ["PleaseWait"] = "استنى شوية",
                ["PleaseWaitMessage"] = "هاي النسخة يصير عليها تعديل الحين. استنى شوية وحاول مرة ثانية.",
                
                // Dialogs
                ["RemoveVersion"] = "تحذف النسخة؟",
                ["RemoveVersionMessage"] = "متأكد تريد تحذف {0}؟\n\nعوالمك راح تبقى بأمان، بس راح تحتاج تنزّل هاي النسخة مرة ثانية إذا تريد تلعبها بعدين.",
                ["ReplaceExisting"] = "تبدّل النسخة الموجودة؟",
                ["ReplaceExistingMessage"] = "عندك نسخة بنفس الاسم. تريد تبدّلها؟",
                ["CouldntRemoveOld"] = "ما قدرنا نحذف النسخة القديمة",
                ["CouldntRemoveOldMessage"] = "ما قدرنا نحذف النسخة الموجودة. الاستيراد انلغى.",
                ["WrongFileType"] = "نوع الملف غلط",
                ["WrongFileTypeMessage"] = "نوع الملف {0} مو مدعوم. اختر ملف .appx أو .msixvc.",
                
                // GDK Warning
                ["FirstTimeSetup"] = "الإعداد الأولي",
                ["GDKWarningMessage"] = "⚠️ مهم: أول مرة تستخدم نسخ Xbox/GDK!\n\nقبل ما تكمل، تأكد من:\n✓ عندك ماين كرافت مثبت من متجر مايكروسوفت\n✓ عندك على الأقل 2 جيجا مساحة فاضية\n\nوقت التثبيت، ممكن تشوف شبابيك تطلع وتختفي - هذا طبيعي!\n\nعوالمك راح تنحفظ تلقائياً.\n\nجاهز تكمل؟",
                
                // Multiple world locations
                ["MultipleWorldLocations"] = "عوالم بأماكن متعددة",
                ["MultipleWorldLocationsMessage"] = "لقينا عوالم بأماكن متعددة:\n{0}\n\nالنسخة راح تدور على العوالم بـ: {1}\n\nممكن بعض العوالم ما تظهر. تكمل على أي حال؟",
                
                // Download status
                ["StatusPreparing"] = "يجهز...",
                ["StatusDownloading"] = "ينزّل... {0} ميجا/{1} ميجا",
                ["StatusExtracting"] = "يفك الضغط...",
                ["StatusRegistering"] = "يسجل الحزمة...",
                ["StatusLaunching"] = "يشغّل...",
                ["StatusUnregistering"] = "يلغي تسجيل الحزمة...",
                ["StatusCleaningUp"] = "ينظف...",
                ["StatusStaging"] = "يجهز الحزمة... ممكن ياخذ شوية وقت",
                ["StatusDecrypting"] = "ينسخ Minecraft.Windows.exe...",
                ["StatusMoving"] = "ينسخ ملفات اللعبة الثانية...",
                ["StatusMovingData"] = "يرجع عوالم ماين كرافت...",
                
                // Buttons
                ["ViewFiles"] = "📁 شوف الملفات",
                ["Pause"] = "⏸️ وقف",
                ["Resume"] = "▶️ كمّل",
                ["Cancel"] = "❌ ألغي",
                
                // Status Indicators
                ["DeveloperMode"] = "وضع المطور",
                ["DecryptionKeys"] = "مفاتيح فك التشفير",
                ["DevModeRequired"] = "وضع المطور مطلوب",
                ["DevModeRequiredMessage"] = "لازم تفعّل وضع المطور عشان تثبت نسخ ماين كرافت.\n\nهذا شرط من الويندوز لتثبيت التطبيقات من برّا المتجر.\n\nتريد تفعّله الحين؟ يحتاج صلاحيات مدير",
                ["TurnOn"] = "فعّل",
                ["DecryptKeysRequired"] = "مفاتيح فك التشفير مطلوبة",
                ["DecryptKeysRequiredMessage"] = "عشان تثبت نسخ Xbox/GDK من ماين كرافت، تحتاج مفاتيح فك التشفير.\n\nهاي المفاتيح تنثبت تلقائياً لما تشغّل ماين كرافت من متجر مايكروسوفت مرة وحدة على الأقل.\n\nتريد نفتح لك المتجر عشان تثبت ماين كرافت؟",
                ["TakeMeThere"] = "وديني",
                ["DevModeEnabled"] = "وضع المطور انفعّل!",
                ["DevModeEnabledMessage"] = "وضع المطور انفعّل بنجاح!\n\nتكدر الحين تثبت نسخ ماين كرافت.",
                ["DevModeEnableFailed"] = "ما قدرنا نفعّل وضع المطور",
                ["DevModeEnableFailedMessage"] = "ما قدرنا نفعّل وضع المطور تلقائياً.\n\nفعّله يدوياً:\n1. افتح إعدادات الويندوز\n2. روح لـ 'التحديث والأمان' أو 'الخصوصية والأمان' بويندوز 11\n3. اضغط 'للمطورين'\n4. فعّل 'وضع المطور'\n5. استنى لين يخلص التثبيت",
            }
        };
        
        public static void SetLanguage(string languageCode)
        {
            _currentLanguage = languageCode;
        }
        
        public static string Get(string key)
        {
            if (_translations.ContainsKey(_currentLanguage) && _translations[_currentLanguage].ContainsKey(key))
            {
                return _translations[_currentLanguage][key];
            }
            
            // Fallback to English
            if (_translations["en"].ContainsKey(key))
            {
                return _translations["en"][key];
            }
            
            return key; // Return key if translation not found
        }
        
        public static string Format(string key, params object[] args)
        {
            string template = Get(key);
            return string.Format(template, args);
        }
    }
}
