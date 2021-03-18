using AO.AutomationFramework.Core.BusinessLogic.Variables;
using NUnit.Framework.Internal;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;

namespace AO.AutomationFramework.Core.BusinessLogic.Helpers
{
    //This structure shall be used to keep the size of the screen.
    internal struct SIZE
    {
        public int Cx;

        public int Cy;
    }

    public static class ScreenshotHelper
    {
        private static readonly string PathToScreenshot;

        private static string _testRunFolderName;

        static ScreenshotHelper()
        {
            PathToScreenshot = CommonVar.ScreenshotPath;
        }

        private static string SaveScreenshot(Bitmap image, string testMethodName)
        {
            var currentFileName = GetFileName(testMethodName);
            image.Save(currentFileName, ImageFormat.Png);
            return currentFileName;
        }

        private static string GetTestRunFolder()
        {
            string featureName = (string)TestExecutionContext.CurrentContext.CurrentTest.Properties["Category"][0];
            Console.WriteLine("GetTestRunFolder: " + featureName);
            string fullName = $"{featureName} {DateTime.Now.ToString("dd MM yy HH-mm-ss").Replace('/', '_').Replace('.', '_')}";
            string logSuffix = string.Empty;
            return Path.Combine(Path.GetFullPath(PathToScreenshot), ReplaceAllNotAvailableSymbols(fullName) + logSuffix);
        }

        private static string GetFullScreenshotFileName(string methodName)
        {
            var pref = new Random().Next(0, 9999).ToString();
            //string imageName = GetTestMethodIdByRegex(methodName);
            return string.Format("{0}{1}.png", Path.Combine(_testRunFolderName, ReplaceAllNotAvailableSymbols(methodName)), pref);
        }

        private static string ReplaceAllNotAvailableSymbols(string str)
        {
            var badSymbols = Path.GetInvalidFileNameChars();
            for (int index = 0; index < badSymbols.Length; index++)
            {
                var symbol = badSymbols[index];
                str = str.Replace(symbol, '_');
            }
            return str;
        }

        private static void CheckDirectoryAndCreateIfNotExist(string path)
        {
            if (!Directory.Exists(Path.GetFullPath(path)))
            {
                Directory.CreateDirectory(Path.GetFullPath(path));
            }
        }

        public static string GetFileName(string testMethodName)
        {
            CheckDirectoryAndCreateIfNotExist(PathToScreenshot);
            if (_testRunFolderName == null)
            {
                _testRunFolderName = GetTestRunFolder();
            }
            CheckDirectoryAndCreateIfNotExist(_testRunFolderName);
            return GetFullScreenshotFileName(testMethodName);
        }

        public static string MakeAndSaveScreenshot(string testMethodName)
        {
            return SaveScreenshot(MakeScreenshot(), testMethodName);
        }

        public static Bitmap MakeScreenshot()
        {
            //In size variable we shall keep the size of the screen.
            SIZE size;

            //Variable to keep the handle to bitmap.

            //Here we get the handle to the desktop device context.
            IntPtr hDc = PlatformInvokeUser32.GetDC
                (PlatformInvokeUser32.GetDesktopWindow());

            //Here we make a compatible device context in memory for screen
            //device context.
            IntPtr hMemDc = PlatformInvokeGDI32.CreateCompatibleDC(hDc);

            //We pass SM_CXSCREEN constant to GetSystemMetrics to get the
            //X coordinates of the screen.
            size.Cx = PlatformInvokeUser32.GetSystemMetrics
                (PlatformInvokeUser32.SmCxscreen);

            //We pass SM_CYSCREEN constant to GetSystemMetrics to get the
            //Y coordinates of the screen.
            size.Cy = PlatformInvokeUser32.GetSystemMetrics
                (PlatformInvokeUser32.SmCyscreen);

            //We create a compatible bitmap of the screen size and using
            //the screen device context.
            IntPtr hBitmap = PlatformInvokeGDI32.CreateCompatibleBitmap
                (hDc, size.Cx, size.Cy);

            //As hBitmap is IntPtr, we cannot check it against null.
            //For this purpose, IntPtr.Zero is used.
            if (hBitmap != IntPtr.Zero)
            {
                //Here we select the compatible bitmap in the memeory device
                //context and keep the refrence to the old bitmap.
                IntPtr hOld = PlatformInvokeGDI32.SelectObject
                    (hMemDc, hBitmap);
                //We copy the Bitmap to the memory device context.
                PlatformInvokeGDI32.BitBlt(hMemDc, 0, 0, size.Cx, size.Cy, hDc,
                                           0, 0, PlatformInvokeGDI32.Srccopy);
                //We select the old bitmap back to the memory device context.
                PlatformInvokeGDI32.SelectObject(hMemDc, hOld);
                //We delete the memory device context.
                PlatformInvokeGDI32.DeleteDC(hMemDc);
                //We release the screen device context.
                PlatformInvokeUser32.ReleaseDC(PlatformInvokeUser32.
                                                   GetDesktopWindow(), hDc);

                //Image is created by Image bitmap handle and stored in
                //local variable.
                Bitmap bmp = Image.FromHbitmap(hBitmap);

                //Release the memory to avoid memory leaks.
                PlatformInvokeGDI32.DeleteObject(hBitmap);
                //This statement runs the garbage collector manually.
                GC.Collect();
                //Return the bitmap
                return bmp;
            }
            //If hBitmap is null, retun null.
            return null;
        }
    }

    public class PlatformInvokeGDI32
    {
        #region Class Variables

        public const int Srccopy = 13369376;

        #endregion Class Variables

        #region Class Functions

        [DllImport("gdi32.dll", EntryPoint = "DeleteDC")]
        public static extern IntPtr DeleteDC(IntPtr hDc);

        [DllImport("gdi32.dll", EntryPoint = "DeleteObject")]
        public static extern IntPtr DeleteObject(IntPtr hDc);

        [DllImport("gdi32.dll", EntryPoint = "BitBlt")]
        public static extern bool BitBlt(IntPtr hdcDest, int xDest,
                                         int yDest, int wDest,
                                         int hDest, IntPtr hdcSource,
                                         int xSrc, int ySrc, int rasterOp);

        [DllImport("gdi32.dll", EntryPoint = "CreateCompatibleBitmap")]
        public static extern IntPtr CreateCompatibleBitmap
                                    (IntPtr hdc, int nWidth, int nHeight);

        [DllImport("gdi32.dll", EntryPoint = "CreateCompatibleDC")]
        public static extern IntPtr CreateCompatibleDC(IntPtr hdc);

        [DllImport("gdi32.dll", EntryPoint = "SelectObject")]
        public static extern IntPtr SelectObject(IntPtr hdc, IntPtr bmp);

        [DllImport("msvcrt.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int Memcmp(IntPtr b1, IntPtr b2, long count);

        #endregion Class Functions
    }

    /// <summary>
    ///
    /// </summary>
    /// This class shall keep the User32 APIs used in our program.
    public class PlatformInvokeUser32
    {
        #region Class Variables

        public const int SmCxscreen = 0;

        public const int SmCyscreen = 1;

        #endregion Class Variables

        #region Class Functions

        [DllImport("user32.dll", EntryPoint = "GetDesktopWindow")]
        public static extern IntPtr GetDesktopWindow();

        [DllImport("user32.dll", EntryPoint = "GetDC")]
        public static extern IntPtr GetDC(IntPtr ptr);

        [DllImport("user32.dll", EntryPoint = "GetSystemMetrics")]
        public static extern int GetSystemMetrics(int abc);

        [DllImport("user32.dll", EntryPoint = "GetWindowDC")]
        public static extern IntPtr GetWindowDC(Int32 ptr);

        [DllImport("user32.dll", EntryPoint = "ReleaseDC")]
        public static extern IntPtr ReleaseDC(IntPtr hWnd, IntPtr hDc);

        #endregion Class Functions
    }
}