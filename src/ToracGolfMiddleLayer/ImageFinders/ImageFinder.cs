using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ToracGolf.MiddleLayer
{
    public class ImageFinder
    {

        #region Constructor

        public ImageFinder(string imageFilePath, string imageVirtualUrl)
        {
            FilePathMapping = FilePathMapBuilder(imageFilePath, imageVirtualUrl);
        }

        #endregion

        #region Properties

        private IDictionary<int, string> FilePathMapping { get; }

        #endregion

        #region Methods

        private static IDictionary<int, string> FilePathMapBuilder(string imageFilePath, string imageVirtualUrl)
        {
            //key is just the file name without attribute, value is the full path
            var dct = new Dictionary<int, string>();

            foreach (var fileToCheck in Directory.GetFiles(imageFilePath))
            {
                dct.Add(Convert.ToInt32(Path.GetFileNameWithoutExtension(fileToCheck)), Path.Combine(imageVirtualUrl, Path.GetFileName(fileToCheck)));
            }

            return dct;
        }

        public string FindImage(int id)
        {
            string fullPath;

            if (FilePathMapping.TryGetValue(id, out fullPath))
            {
                return fullPath;
            }

            return string.Empty;
        }

        #endregion

    }
}
