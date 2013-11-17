using test.Models.FileCabinet;
using System.Collections.Generic;
using System.IO;

namespace test.Stuff
{
    public class FileCabinetUtil
    {
        public static List<string> text = new List<string>() { "txt" };
        public static List<string> document = new List<string>() { "doc", "docx", "pdf" };
        public static List<string> image = new List<string>() { "jpg", "gif", "png" };
        public static List<string> audio = new List<string>() { "mp3" };
        public static List<string> video = new List<string>() { "avi", "wmv", "mp4" };

        public static FileType GetFileType(string fileName)
        {
            string extension = Path.GetExtension(fileName).Replace(".", "");

            if (text.Contains(extension))
            {
                return FileType.Text;
            }
            if (document.Contains(extension))
            {
                return FileType.Document;
            }
            if (image.Contains(extension))
            {
                return FileType.Image;
            }
            if (audio.Contains(extension))
            {
                return FileType.Audio;
            }
            if (video.Contains(extension))
            {
                return FileType.Video;
            }

            return FileType.File;
        }
    }
}