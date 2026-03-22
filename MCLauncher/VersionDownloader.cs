using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Collections.Generic;

namespace MCLauncher {

    class BadUpdateIdentityException: ArgumentException{
        public BadUpdateIdentityException() : base("Bad updateIdentity") { }
    }

    class VersionDownloader {

        private HttpClient client = new HttpClient();
        private WUProtocol protocol = new WUProtocol();
        
        private async Task<XDocument> PostXmlAsync(string url, XDocument data) {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url);
            using (var stringWriter = new StringWriter()) {
                using (XmlWriter xmlWriter = XmlWriter.Create(stringWriter, new XmlWriterSettings { Indent = false, OmitXmlDeclaration = true })) {
                    data.Save(xmlWriter);
                }
                request.Content = new StringContent(stringWriter.ToString(), Encoding.UTF8, "application/soap+xml");
            }
            using (var resp = await client.SendAsync(request)) {
                string str = await resp.Content.ReadAsStringAsync();
                return XDocument.Parse(str);
            }
        }

        private async Task DownloadFile(string url, string to, DownloadProgress progress, CancellationToken cancellationToken) {
            // Enhanced download with resume capability and infinite retry on network failure
            const int maxQuickRetries = 3; // Quick retries for transient errors
            const int retryDelayMs = 2000;
            int quickRetryCount = 0;
            
            long existingFileSize = 0;
            if (File.Exists(to))
            {
                existingFileSize = new FileInfo(to).Length;
                Debug.WriteLine($"Resuming download from byte {existingFileSize}");
            }

            while (true) // Infinite retry loop - will pause and wait for network
            {
                try
                {
                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url);
                    
                    // Add range header for resume support
                    if (existingFileSize > 0)
                    {
                        request.Headers.Range = new System.Net.Http.Headers.RangeHeaderValue(existingFileSize, null);
                    }

                    using (var resp = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken))
                    {
                        resp.EnsureSuccessStatusCode();
                        
                        using (var inStream = await resp.Content.ReadAsStreamAsync())
                        {
                            bool isPartial = resp.StatusCode == System.Net.HttpStatusCode.PartialContent;
                            if (existingFileSize > 0 && !isPartial)
                            {
                                Debug.WriteLine("Server ignored Range header, starting from scratch.");
                                existingFileSize = 0;
                            }
                            
                            FileMode fileMode = existingFileSize > 0 ? FileMode.Append : FileMode.Create;
                            using (var outStream = new FileStream(to, fileMode, FileAccess.Write, FileShare.None, 8192, useAsync: true))
                            {
                                long? totalSize = resp.Content.Headers.ContentLength;
                                if (totalSize.HasValue && existingFileSize > 0)
                                {
                                    totalSize = totalSize.Value + existingFileSize;
                                }
                                
                                progress(existingFileSize, totalSize);
                                long transferred = existingFileSize;
                                byte[] buf = new byte[1024 * 1024]; // 1MB buffer
                                
                                while (true)
                                {
                                    int n = await inStream.ReadAsync(buf, 0, buf.Length, cancellationToken);
                                    if (n == 0)
                                        break;
                                    
                                    await outStream.WriteAsync(buf, 0, n, cancellationToken);
                                    await outStream.FlushAsync(cancellationToken); // Ensure data is written
                                    
                                    transferred += n;
                                    progress(transferred, totalSize);
                                }
                            }
                        }
                    }
                    
                    // Download completed successfully
                    Debug.WriteLine("Download completed successfully");
                    return;
                }
                catch (HttpRequestException ex)
                {
                    quickRetryCount++;
                    Debug.WriteLine($"Network error during download (attempt {quickRetryCount}): {ex.Message}");
                    
                    // Update existing file size for resume
                    if (File.Exists(to))
                    {
                        existingFileSize = new FileInfo(to).Length;
                    }
                    
                    // For first few retries, use quick exponential backoff
                    if (quickRetryCount <= maxQuickRetries)
                    {
                        int delay = retryDelayMs * quickRetryCount;
                        Debug.WriteLine($"Quick retry in {delay}ms...");
                        await Task.Delay(delay, cancellationToken);
                    }
                    else
                    {
                        // After quick retries, assume network is down - wait longer
                        Debug.WriteLine("Network appears to be down. Waiting 10 seconds before retry...");
                        await Task.Delay(10000, cancellationToken); // Wait 10 seconds when network is down
                        // Don't reset quickRetryCount - keep trying indefinitely
                    }
                }
                catch (IOException ex)
                {
                    quickRetryCount++;
                    Debug.WriteLine($"IO error during download (attempt {quickRetryCount}): {ex.Message}");
                    
                    if (File.Exists(to))
                    {
                        existingFileSize = new FileInfo(to).Length;
                    }
                    
                    // IO errors might be disk issues - retry with backoff
                    int delay = Math.Min(retryDelayMs * quickRetryCount, 10000);
                    await Task.Delay(delay, cancellationToken);
                }
                catch (TaskCanceledException)
                {
                    Debug.WriteLine("Download cancelled by user");
                    throw;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Unexpected error during download: {ex.Message}");
                    // For unexpected errors, retry after a delay
                    await Task.Delay(5000, cancellationToken);
                    
                    if (File.Exists(to))
                    {
                        existingFileSize = new FileInfo(to).Length;
                    }
                }
            }
        }

        private async Task<string> GetDownloadUrl(string updateIdentity, string revisionNumber) {
            XDocument result = await PostXmlAsync(protocol.GetDownloadUrl(),
                protocol.BuildDownloadRequest(updateIdentity, revisionNumber));
            Debug.WriteLine($"GetDownloadUrl() response for updateIdentity {updateIdentity}, revision {revisionNumber}:\n{result.ToString()}");
            foreach (string s in protocol.ExtractDownloadResponseUrls(result)) {
                if (s.StartsWith("http://tlu.dl.delivery.mp.microsoft.com/"))
                    return s;
            }
            return null;
        }

        public void EnableUserAuthorization() {
            protocol.SetMSAUserToken(WUTokenHelper.GetWUToken());
        }

        public async Task DownloadAppx(string updateIdentity, string revisionNumber, string destination, DownloadProgress progress, CancellationToken cancellationToken) {
            string link = await GetDownloadUrl(updateIdentity, revisionNumber);
            if (link == null)
                throw new BadUpdateIdentityException();
            Debug.WriteLine("Resolved download link: " + link);
            await DownloadFile(link, destination, progress, cancellationToken);
        }

        public async Task DownloadMsixvc(List<string> downloadUrls, string destination, DownloadProgress progress, CancellationToken cancellationToken) {
            await DownloadFile(downloadUrls[0], destination, progress, cancellationToken);
        }

        public delegate void DownloadProgress(long current, long? total);



    }
}
