using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorInventarioEmpresas.BackEnd.Commons
{
    public static class ImageHelper
    {
        public static void CreateThumbnailImage(int width, int height, Stream streamImg, string saveFilePath)
        {
            Bitmap sourceImage = new Bitmap(streamImg);
            using (Bitmap objBitmap = new Bitmap(width, height))
            {
                objBitmap.SetResolution(sourceImage.HorizontalResolution, sourceImage.VerticalResolution);
                using (Graphics objGraphics = Graphics.FromImage(objBitmap))
                {
                    // Set the graphic format for better result cropping   
                    objGraphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                    objGraphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    objGraphics.DrawImage(sourceImage, 0, 0, width, height);

                    // Save the file path, note we use png format to support png file   
                    objBitmap.Save(saveFilePath);
                }
            }
        }

        public static void CreateThumbnailImage(int width, int height, string fileNameInput, string fileNameOutput, ImageFormat outputFormat)
        {
            using (Image photo = new Bitmap(fileNameInput))
            {
                double aspectRatio = (double)photo.Width / photo.Height;
                double boxRatio = (double)width / height;
                double scaleFactor = 0;

                if (photo.Width < width && photo.Height < height)
                {
                    // keep the image the same size since it is already smaller than our max width/height
                    scaleFactor = 1.0;
                }
                else
                {
                    if (boxRatio > aspectRatio)
                        scaleFactor = (double)height / photo.Height;
                    else
                        scaleFactor = (double)width / photo.Width;
                }

                int newWidth = (int)(photo.Width * scaleFactor);
                int newHeight = (int)(photo.Height * scaleFactor);

                using (Bitmap bmp = new Bitmap(newWidth, newHeight))
                {
                    using (Graphics g = Graphics.FromImage(bmp))
                    {
                        g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        g.SmoothingMode = SmoothingMode.HighQuality;
                        g.CompositingQuality = CompositingQuality.HighQuality;
                        g.PixelOffsetMode = PixelOffsetMode.HighQuality;

                        g.DrawImage(photo, 0, 0, newWidth, newHeight);

                        if (ImageFormat.Png.Equals(outputFormat))
                        {
                            bmp.Save(fileNameOutput, outputFormat);
                        }
                        else if (ImageFormat.Jpeg.Equals(outputFormat))
                        {
                            ImageCodecInfo[] info = ImageCodecInfo.GetImageEncoders();
                            EncoderParameters encoderParameters;
                            using (encoderParameters = new EncoderParameters(1))
                            {
                                // use jpeg info[1] and set quality to 90
                                encoderParameters.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 90L);
                                bmp.Save(fileNameOutput, info[1], encoderParameters);
                            }
                        }
                    }
                }
            }
        }
    }
}
