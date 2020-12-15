using System;
using System.Drawing;

namespace CygX1.Waxy.Http
{
    public class WebImage
    {
        public readonly Image Image;
        public readonly string Extension;
        public readonly DateTime TimeTaken;

        public string FileName
        {
            get
            {
                
                string fileName = 
                    EnsureTwoCharacters(TimeTaken.Hour) + "-" +
                    EnsureTwoCharacters(TimeTaken.Minute) + "-" +
                    EnsureTwoCharacters(TimeTaken.Second) + this.Extension; // ".jpg";

                return fileName;
            }
        }

        public WebImage(Image image, string extension, DateTime timeTaken)
        {
            this.Image = image;
            this.Extension = extension;
            this.TimeTaken = timeTaken;
        }

        private string EnsureTwoCharacters(int dayOrMonth)
        {
            if (dayOrMonth < 10)
                return "0" + dayOrMonth.ToString();
            else
                return dayOrMonth.ToString();
        }
    }
}
