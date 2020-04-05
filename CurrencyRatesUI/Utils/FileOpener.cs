using System;
using System.IO;

namespace CurrencyRatesUI.Utils {
    class FileOpener {
        readonly string path;

        internal FileOpener() {
            path = Tizen.Applications.Application.Current.DirectoryInfo.Data;
        }

        public Stream OpenFile(string filePath, FileMode fileMode) {
            if (filePath == null || filePath.Trim().Length == 0) {
                throw new ArgumentNullException("File path arguments are invalid.");
            }

            return new FileStream(Path.Combine(path, filePath), fileMode, FileAccess.ReadWrite, FileShare.Read);
        }
    }
}
