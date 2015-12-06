using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication2
{
    

    class ImageHandler
    {
        public static String CARD_BORDER = "basic_card_blood.png";

        private Dictionary<string, System.Drawing.Image> images;

        public ImageHandler()
        {
            images = new Dictionary<string, System.Drawing.Image>();
        }
        
        public System.Drawing.Image getImage(String imageName)
        {
            if (!images.ContainsKey(imageName))
            {
                images[imageName] = System.Drawing.Image.FromFile(Environment.CurrentDirectory + "\\" + imageName);
            }
            
            return images[imageName];
        }
    }
}
