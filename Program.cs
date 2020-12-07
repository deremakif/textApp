using System;

using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using SkiaSharp;


namespace textApp
{
    class Program
    {
        static void Main(string[] args)
        {
            String text = "The quick brown fox jumps over a lazy dog.";

            #region With System.Drawing
            Bitmap result = new Bitmap(500, 500);
            var g = Graphics.FromImage(result);
            g.Clear(Color.Green);

            Font font = new Font("Arial", 16, FontStyle.Underline);
            SolidBrush solidBrush = new SolidBrush(Color.Black);
            Rectangle rectangle = new Rectangle(0, 0, 500, 500);
            StringFormat stringFormat = new StringFormat();

            stringFormat.LineAlignment = StringAlignment.Center; // Far, Center, Near
            stringFormat.Alignment = StringAlignment.Center; // Far, Center, Near

            g.DrawString(text, font, solidBrush, rectangle, stringFormat);

            var jpgEncoder = GetEncoder(ImageFormat.Jpeg);
            var myEncoder = Encoder.Quality;
            var myEncoderParameters = new EncoderParameters(1);

            myEncoderParameters.Param[0] = new EncoderParameter(myEncoder, 100);
            // result.Save("assets/outputImageWithSystemDrawingCenter.jpg", jpgEncoder, myEncoderParameters);
            #endregion

            #region With SkiaSharp

            var info = new SKImageInfo(500, 500);
            using (var surface = SKSurface.Create(info))
            {
                // the the canvas and properties
                var canvas = surface.Canvas;
            
                canvas.Clear(SKColor.Parse("008000")); // Green Hex Code.

                var paint = new SKPaint
                {
                    Color = SKColors.Black,
                    TextSize = 16,
                    TextAlign = SKTextAlign.Right, // Left,Right,Center                   
                };

                canvas.DrawText(text, new SKPoint(250, 250), paint);

                // save the file
                using (var image = surface.Snapshot())
                using (var data = image.Encode(SKEncodedImageFormat.Jpeg, 100))
                using (var stream = System.IO.File.OpenWrite("assets/outputImageWithSkiaSharpRight.jpg"))
                {
                    data.SaveTo(stream);
                }
            }

            #endregion
        }

        public static ImageCodecInfo GetEncoder(ImageFormat format)
        {
            try
            {
                var codecs = ImageCodecInfo.GetImageDecoders();
                foreach (var codec in codecs)
                    if (codec.FormatID == format.Guid)
                        return codec;
            }
            catch (System.Exception) { }
            return null;
        }
    }

}
