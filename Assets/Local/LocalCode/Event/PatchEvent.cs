using UniFramework.Event;
using YooAsset;

namespace LocalCode
{
    public class PatchEvent
    {
        /// <summary>
        /// initialize patch package failed
        /// </summary>
        public class InitializeFailed : EventItem
        {
        }

        /// <summary>
        /// patch steps changed
        /// </summary>
        public class PatchStepsChange : EventItem
        {
            public string tips;

            public PatchStepsChange(string tips)
            {
                this.tips = tips;
            }
        }

        /// <summary>
        /// found update files
        /// </summary>
        public class FoundUpdateFiles : EventItem
        {
            public int total_count;
            public long total_size_bytes;

            public FoundUpdateFiles(int total_count, long total_size_bytes)
            {
                this.total_count = total_count;
                this.total_size_bytes = total_size_bytes;
            }
        }

        /// <summary>
        /// Download progress update
        /// </summary>
        public class DownloadUpdate : EventItem
        {
            public int total_download_count;
            public int current_download_count;
            public long total_download_size_bytes;
            public long current_download_sizes;

            public DownloadUpdate(int current_count, int total_count,long current_download_sizes, long total_size_bytes)
            {
                this.current_download_count = current_count;
                this.total_download_count = total_count;
                this.current_download_sizes = current_download_sizes;
                this.total_download_size_bytes = total_size_bytes;
                
            }
        }

        /// <summary>
        /// package version request failed
        /// </summary>
        public class PackageVersionRequestFailed : EventItem
        {
        }

        /// <summary>
        /// package manifest update failed
        /// </summary>
        public class PackageManifestUpdateFailed : EventItem
        {
        }

        /// <summary>
        /// web file download failed
        /// </summary>
        public class WebFileDownloadFailed : EventItem
        {
            public string file_name;
            public string error;

            public WebFileDownloadFailed(string file_name, string error)
            {
                this.file_name = file_name;
                this.error = error;
            }
        }
    }
}