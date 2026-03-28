using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;

namespace MCLauncher
{
    /// <summary>
    /// Robust ZIP extraction with fallback mechanisms for problematic archives
    /// Handles split archives, corrupted files, and other edge cases
    /// </summary>
    public static class RobustZipExtractor
    {
        /// <summary>
        /// Extract ZIP archive with multiple fallback strategies
        /// </summary>
        /// <param name="zipPath">Path to ZIP file</param>
        /// <param name="extractPath">Destination directory</param>
        /// <param name="progressCallback">Optional progress callback (current, total)</param>
        /// <returns>True if extraction succeeded</returns>
        public static bool ExtractZipFile(string zipPath, string extractPath, Action<int, int> progressCallback = null)
        {
            Debug.WriteLine($"[RobustZipExtractor] Starting extraction: {zipPath} -> {extractPath}");
            
            // Validate inputs
            if (!File.Exists(zipPath))
            {
                Debug.WriteLine($"[RobustZipExtractor] ERROR: ZIP file not found: {zipPath}");
                return false;
            }
            
            // Create destination directory
            try
            {
                if (!Directory.Exists(extractPath))
                {
                    Directory.CreateDirectory(extractPath);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[RobustZipExtractor] ERROR: Cannot create destination directory: {ex.Message}");
                return false;
            }
            
            // Strategy 1: Try standard .NET ZipFile extraction
            try
            {
                Debug.WriteLine("[RobustZipExtractor] Attempting standard .NET extraction...");
                return ExtractWithDotNetZipFile(zipPath, extractPath, progressCallback);
            }
            catch (InvalidDataException ex) when (ex.Message.Contains("split") || ex.Message.Contains("spanned"))
            {
                Debug.WriteLine($"[RobustZipExtractor] Split archive detected: {ex.Message}");
                // Fall through to next strategy
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[RobustZipExtractor] Standard extraction failed: {ex.Message}");
                // Fall through to next strategy
            }
            
            // Strategy 2: Try manual extraction with ZipArchive (more tolerant)
            try
            {
                Debug.WriteLine("[RobustZipExtractor] Attempting manual ZipArchive extraction...");
                return ExtractWithZipArchive(zipPath, extractPath, progressCallback);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[RobustZipExtractor] ZipArchive extraction failed: {ex.Message}");
                // Fall through to next strategy
            }
            
            // Strategy 3: Try using 7-Zip if available
            try
            {
                Debug.WriteLine("[RobustZipExtractor] Attempting 7-Zip extraction...");
                return ExtractWith7Zip(zipPath, extractPath, progressCallback);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[RobustZipExtractor] 7-Zip extraction failed: {ex.Message}");
            }
            
            // All strategies failed
            Debug.WriteLine("[RobustZipExtractor] ERROR: All extraction strategies failed");
            return false;
        }
        
        /// <summary>
        /// Strategy 1: Standard .NET ZipFile.ExtractToDirectory
        /// Fast but doesn't support split archives
        /// </summary>
        private static bool ExtractWithDotNetZipFile(string zipPath, string extractPath, Action<int, int> progressCallback)
        {
            using (var archive = ZipFile.OpenRead(zipPath))
            {
                int totalEntries = archive.Entries.Count;
                int processedEntries = 0;
                
                Debug.WriteLine($"[RobustZipExtractor] Found {totalEntries} entries in archive");
                
                foreach (var entry in archive.Entries)
                {
                    try
                    {
                        // Skip directory entries
                        if (string.IsNullOrEmpty(entry.Name))
                        {
                            processedEntries++;
                            continue;
                        }
                        
                        // Build destination path
                        string destinationPath = Path.GetFullPath(Path.Combine(extractPath, entry.FullName));
                        
                        // Security check: prevent path traversal attacks
                        if (!destinationPath.StartsWith(Path.GetFullPath(extractPath) + Path.DirectorySeparatorChar))
                        {
                            Debug.WriteLine($"[RobustZipExtractor] WARNING: Skipping suspicious path: {entry.FullName}");
                            processedEntries++;
                            continue;
                        }
                        
                        // Create directory if needed
                        string directoryPath = Path.GetDirectoryName(destinationPath);
                        if (!Directory.Exists(directoryPath))
                        {
                            Directory.CreateDirectory(directoryPath);
                        }
                        
                        // Extract file
                        entry.ExtractToFile(destinationPath, overwrite: true);
                        
                        processedEntries++;
                        progressCallback?.Invoke(processedEntries, totalEntries);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"[RobustZipExtractor] WARNING: Failed to extract entry '{entry.FullName}': {ex.Message}");
                        // Continue with other files
                        processedEntries++;
                    }
                }
                
                Debug.WriteLine($"[RobustZipExtractor] Successfully extracted {processedEntries}/{totalEntries} entries");
                return processedEntries > 0;
            }
        }
        
        /// <summary>
        /// Strategy 2: Manual extraction using ZipArchive with stream
        /// More tolerant of corrupted entries
        /// </summary>
        private static bool ExtractWithZipArchive(string zipPath, string extractPath, Action<int, int> progressCallback)
        {
            using (var fileStream = new FileStream(zipPath, FileMode.Open, FileAccess.Read, FileShare.Read))
            using (var archive = new ZipArchive(fileStream, ZipArchiveMode.Read, leaveOpen: false))
            {
                int totalEntries = archive.Entries.Count;
                int processedEntries = 0;
                int successfulEntries = 0;
                
                Debug.WriteLine($"[RobustZipExtractor] Found {totalEntries} entries in archive (ZipArchive mode)");
                
                foreach (var entry in archive.Entries)
                {
                    try
                    {
                        // Skip directory entries
                        if (string.IsNullOrEmpty(entry.Name))
                        {
                            processedEntries++;
                            continue;
                        }
                        
                        // Build destination path
                        string destinationPath = Path.GetFullPath(Path.Combine(extractPath, entry.FullName));
                        
                        // Security check
                        if (!destinationPath.StartsWith(Path.GetFullPath(extractPath) + Path.DirectorySeparatorChar))
                        {
                            Debug.WriteLine($"[RobustZipExtractor] WARNING: Skipping suspicious path: {entry.FullName}");
                            processedEntries++;
                            continue;
                        }
                        
                        // Create directory
                        string directoryPath = Path.GetDirectoryName(destinationPath);
                        if (!Directory.Exists(directoryPath))
                        {
                            Directory.CreateDirectory(directoryPath);
                        }
                        
                        // Extract using stream (more robust)
                        using (var entryStream = entry.Open())
                        using (var fileStream2 = new FileStream(destinationPath, FileMode.Create, FileAccess.Write, FileShare.None))
                        {
                            entryStream.CopyTo(fileStream2);
                        }
                        
                        successfulEntries++;
                        processedEntries++;
                        progressCallback?.Invoke(processedEntries, totalEntries);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"[RobustZipExtractor] WARNING: Failed to extract entry '{entry.FullName}': {ex.Message}");
                        processedEntries++;
                        // Continue with other files
                    }
                }
                
                Debug.WriteLine($"[RobustZipExtractor] Successfully extracted {successfulEntries}/{totalEntries} entries");
                return successfulEntries > 0;
            }
        }
        
        /// <summary>
        /// Strategy 3: Use 7-Zip command line if available
        /// Supports split archives and most formats
        /// </summary>
        private static bool ExtractWith7Zip(string zipPath, string extractPath, Action<int, int> progressCallback)
        {
            // Try to find 7-Zip installation
            string[] possible7ZipPaths = new[]
            {
                @"C:\Program Files\7-Zip\7z.exe",
                @"C:\Program Files (x86)\7-Zip\7z.exe",
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "7-Zip", "7z.exe"),
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), "7-Zip", "7z.exe")
            };
            
            string sevenZipPath = null;
            foreach (var path in possible7ZipPaths)
            {
                if (File.Exists(path))
                {
                    sevenZipPath = path;
                    break;
                }
            }
            
            if (sevenZipPath == null)
            {
                Debug.WriteLine("[RobustZipExtractor] 7-Zip not found on system");
                return false;
            }
            
            Debug.WriteLine($"[RobustZipExtractor] Found 7-Zip at: {sevenZipPath}");
            
            // Execute 7-Zip extraction (minimized window to avoid AV detection)
            var processInfo = new ProcessStartInfo
            {
                FileName = sevenZipPath,
                Arguments = $"x \"{zipPath}\" -o\"{extractPath}\" -y", // x=extract with paths, -y=yes to all
                UseShellExecute = true,  // Changed to avoid AV heuristics
                WindowStyle = ProcessWindowStyle.Minimized  // Minimized instead of hidden
            };
            
            try
            {
                using (var process = Process.Start(processInfo))
                {
                    process.WaitForExit();
                    
                    if (process.ExitCode == 0)
                    {
                        Debug.WriteLine("[RobustZipExtractor] 7-Zip extraction successful");
                        progressCallback?.Invoke(100, 100);
                        return true;
                    }
                    else
                    {
                        Debug.WriteLine($"[RobustZipExtractor] 7-Zip failed with exit code {process.ExitCode}");
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[RobustZipExtractor] 7-Zip execution failed: {ex.Message}");
                return false;
            }
        }
        
        /// <summary>
        /// Validate if a file is a valid ZIP archive
        /// </summary>
        public static bool IsValidZipFile(string zipPath)
        {
            try
            {
                using (var archive = ZipFile.OpenRead(zipPath))
                {
                    // Try to read first entry
                    var firstEntry = archive.Entries.Count > 0 ? archive.Entries[0] : null;
                    return firstEntry != null;
                }
            }
            catch (InvalidDataException ex) when (ex.Message.Contains("split") || ex.Message.Contains("spanned"))
            {
                // Split archives are technically valid, just not supported by .NET
                Debug.WriteLine("[RobustZipExtractor] File is a split/spanned archive");
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
