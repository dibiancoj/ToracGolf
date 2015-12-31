using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ToracGolf.MiddleLayer.Courses
{
    public class CourseImageFinder
    {

        #region Constructor

        public CourseImageFinder(string courseImageFilePath, string courseImageVirtualUrl)
        {
            FilePathMapping = FilePathMapBuilder(courseImageFilePath, courseImageVirtualUrl);
        }

        #endregion

        #region Properties

        private IDictionary<int, string> FilePathMapping { get; }

        #endregion

        #region Methods

        private static IDictionary<int, string> FilePathMapBuilder(string courseImageFilePath, string courseImageVirtualUrl)
        {
            //key is just the file name without attribute, value is the full path
            var dct = new Dictionary<int, string>();

            foreach (var fileToCheck in Directory.GetFiles(courseImageFilePath))
            {
                dct.Add(Convert.ToInt32(Path.GetFileNameWithoutExtension(fileToCheck)), Path.Combine(courseImageVirtualUrl, Path.GetFileName(fileToCheck)));
            }

            return dct;
        }

        public string FindCourseImage(int courseId)
        {
            string fullPath;

            if (FilePathMapping.TryGetValue(courseId, out fullPath))
            {
                return fullPath;
            }

            return string.Empty;
        }

        #endregion

    }
}
