using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace AO.AutomationFramework.Core.BusinessLogic.Helpers
{
    //Taken from main LogiLease code
    public static class ImageHelper
    {
        public static Bitmap Crop(Bitmap bitmap)
        {
            var w = bitmap.Width;
            var h = bitmap.Height;

            bool isAllWhiteOrTransparentRow(int row)
            {
                for (int i = 0; i < w; i++)
                {
                    var pixel = bitmap.GetPixel(i, row);

                    if (pixel.A == 0)
                    {
                        continue;
                    }

                    if (pixel.R != 255)
                    {
                        return false;
                    }
                }

                return true;
            }

            bool isAllWhiteOrTransparentColumn(int col)
            {
                for (int i = 0; i < h; i++)
                {
                    var pixel = bitmap.GetPixel(col, i);

                    if (pixel.A == 0)
                    {
                        continue;
                    }

                    if (pixel.R != 255)
                    {
                        return false;
                    }
                }

                return true;
            }

            int leftMost = 0;
            for (int col = 0; col < w; col++)
            {
                if (isAllWhiteOrTransparentColumn(col))
                {
                    leftMost = col + 1;
                }
                else
                {
                    break;
                }
            }

            int rightMost = w - 1;
            for (int col = rightMost; col > 0; col--)
            {
                if (isAllWhiteOrTransparentColumn(col))
                {
                    rightMost = col - 1;
                }
                else
                {
                    break;
                }
            }

            int topMost = 0;
            for (int row = 0; row < h; row++)
            {
                if (isAllWhiteOrTransparentRow(row))
                {
                    topMost = row + 1;
                }
                else
                {
                    break;
                }
            }

            int bottomMost = h - 1;
            for (int row = bottomMost; row > 0; row--)
            {
                if (isAllWhiteOrTransparentRow(row))
                {
                    bottomMost = row - 1;
                }
                else
                {
                    break;
                }
            }

            if (rightMost == 0 && bottomMost == 0 && leftMost == w && topMost == h)
            {
                return bitmap;
            }

            var croppedWidth = rightMost - leftMost + 1;
            var croppedHeight = bottomMost - topMost + 1;

            var target = new Bitmap(croppedWidth, croppedHeight);
            using (var g = Graphics.FromImage(target))
            {
                g.DrawImage(
                    bitmap,
                    new RectangleF(0, 0, croppedWidth, croppedHeight),
                    new RectangleF(leftMost, topMost, croppedWidth, croppedHeight),
                    GraphicsUnit.Pixel);
            }

            return target;
        }

        /// <summary>
        /// Resize the image to the specified width and height.
        /// </summary>
        /// <param name="bitmap">The image to resize.</param>
        /// <param name="width">The width to resize to.</param>
        /// <param name="height">The height to resize to.</param>
        /// <returns>The resized image.</returns>
        private static Bitmap ResizeImage(Bitmap bitmap, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(bitmap.HorizontalResolution, bitmap.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using var wrapMode = new ImageAttributes();
                wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                graphics.DrawImage(bitmap, destRect, 0, 0, bitmap.Width, bitmap.Height, GraphicsUnit.Pixel, wrapMode);
            }

            return destImage;
        }

        private static Bitmap LeftCentreAlignAndPad(Bitmap bitmap, int width, int height, Color paddingColour)
        {
            if (bitmap.Width > width)
            {
                throw new ArgumentException("Width is too large");
            }

            if (bitmap.Height > height)
            {
                throw new ArgumentException("Height is too large");
            }

            var destImage = new Bitmap(width, height);

            var marginTop = (float)((height - bitmap.Height) / 2.0);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                graphics.Clear(paddingColour);
                graphics.DrawImage(bitmap, 0, marginTop);
            }

            return destImage;
        }

        public static Bitmap CropResizeAndLeftCentreAlignAndPad(Bitmap bitmap, int width, int height, Color paddingColour)
        {
            // crop any whitespace/transparency surrounding the image
            bitmap = Crop(bitmap);

            var croppedWidth = bitmap.Width;
            var croppedHeight = bitmap.Height;

            // resize whichever is the relevant dimension, preserving the aspect ratio, so that one dimension of the resulting image matches one of the desired output dimensions
            var ratioWidth = width / (double)croppedWidth;
            var ratioHeight = height / (double)croppedHeight;

            // both width and height are too small vs the desired output dimensions
            if (ratioWidth > 1 && ratioHeight > 1)
            {
                // the width requires a smaller resize increase than the height, use the width ratio to resize the height
                if (ratioWidth < ratioHeight)
                {
                    bitmap = ResizeImage(bitmap, width, (int)(croppedHeight * ratioWidth));
                }

                // the height requires a smaller resize increase than the width, use the height ratio to resize the width
                else
                {
                    bitmap = ResizeImage(bitmap, (int)(croppedWidth * ratioHeight), height);
                }
            }

            // both width and height are too large vs the desired output dimensions
            else if (ratioWidth < 1 && ratioHeight < 1)
            {
                // the width requires a greater resize reduction than the height, use the width ratio to resize the height
                if (ratioWidth < ratioHeight)
                {
                    bitmap = ResizeImage(bitmap, width, (int)(croppedHeight * ratioWidth));
                }

                // the height requires a greater resize reduction than the width, use the height ratio to resize the width
                else
                {
                    bitmap = ResizeImage(bitmap, (int)(croppedWidth * ratioHeight), height);
                }
            }

            // width is too small, height is too large vs the desired output dimensions, use the height ratio to resize reduce the width
            else if (ratioWidth > 1)
            {
                bitmap = ResizeImage(bitmap, (int)(croppedWidth * ratioHeight), height);
            }

            // width is too large, height is too small vs the desired output dimensions, use the width ratio to resize reduce the height
            else
            {
                bitmap = ResizeImage(bitmap, width, (int)(croppedHeight * ratioWidth));
            }

            // horizontally left-align, vertically-centre, and pad the resulting image with transparency
            bitmap = LeftCentreAlignAndPad(bitmap, width, height, paddingColour);

            return bitmap;
        }

        public static string BitmapToBase64(Bitmap bitmap, ImageFormat format)
        {
            using var ms = new MemoryStream();
            bitmap.Save(ms, format);
            return Convert.ToBase64String(ms.ToArray());
        }

        public static int CompareColours(Color pixelA, Color pixelB)
        {
            return (int)(Math.Pow((int)pixelA.R - pixelB.R, 2) + Math.Pow((int)pixelA.B - pixelB.B, 2) + Math.Pow((int)pixelA.G - pixelB.G, 2));
        }

        private static int AreBothBlack(Color pixelA, Color pixelB)
        {
            if (pixelA == Color.FromArgb(0, 0, 0))
            {
                if (pixelB == Color.FromArgb(0, 0, 0))
                {
                    return 0;
                }
                else
                {
                    return 1;
                }
            }
            else
            {
                return 0;
            }
        }

        public static bool DoImagesMatch(Bitmap bmp1, Bitmap bmp2, bool useThumbnail, decimal tolerance = 0.0000009M)
        {
            if (useThumbnail)
            {
                bmp1 = new Bitmap(bmp1.GetThumbnailImage(67, 25, null, IntPtr.Zero));
                bmp2 = new Bitmap(bmp2.GetThumbnailImage(67, 25, null, IntPtr.Zero));
            }
            if (bmp1.Size != bmp2.Size)
                return false;
            else
            {
                decimal totalError = 0;
                for (int x = 0; x < bmp1.Width; x++)
                {
                    for (int y = 0; y < bmp1.Height; y++)
                    {
                        totalError += AreBothBlack(bmp1.GetPixel(x, y), bmp2.GetPixel(x, y)) / (decimal)198608D;
                    }
                }

                decimal averageError = totalError / (bmp1.Width * bmp1.Height);
                return averageError <= tolerance;
            }
        }
    }
}