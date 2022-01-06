// QScreenCapture v2.0 (c) 2021-2022 Sensei (aka 'Q')
// Capture the screen to an image file.
//
// Usage:
// QScreenCapture [-h|--help|/?] [-v|--verbose] [-s <screen-id>] [-x <x>] [-y <y>] [--width <width>] [--height <height>] [-d|--datestamp] [-df|--dateformat <date-format>] filename
//
// Compilation:
// %SYSTEMROOT%\Microsoft.NET\Framework\v3.5\csc QScreenCapture.cs
//
// Example:
// QScreenCapture -v -d D:\picture.jpg
// QScreenCapture -v -d -df yyyyMMddHHmmss D:\picture.png
// QScreenCapture -v -x 200 -y 100 --width 500 --height 700 -d -df HHmmss pic.png
// QScreenCapture -v -s 2 -d C:\Users\[user-name]\Pictures\second_monitor.png
//
// TODO: add support for the mouse pointer!

using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections.Generic;

public class QScreenCapture {
/*
// Version v1.0 (just commented code, maybe somebody will find it also useful)
   private class GDI32 {
      public const int SRCCOPY = 0x00CC0020;
      [DllImport("gdi32.dll")]
      public static extern bool BitBlt( IntPtr hDCDst, int nXDst, int nYDst, int nWidth, int nHeight, IntPtr hDCSrc, int nXSrc, int nYSrc, int dwRop );
      [DllImport("gdi32.dll")]
      public static extern IntPtr CreateCompatibleBitmap( IntPtr hDC, int nWidth, int nHeight );
      [DllImport("gdi32.dll")]
      public static extern IntPtr CreateCompatibleDC( IntPtr hDC );
      [DllImport("gdi32.dll")]
      public static extern bool DeleteDC( IntPtr hDC );
      [DllImport("gdi32.dll")]
      public static extern bool DeleteObject( IntPtr hObject );
      [DllImport("gdi32.dll")]
      public static extern IntPtr SelectObject( IntPtr hDC, IntPtr hObject );
   }

   private class User32 {
      [StructLayout(LayoutKind.Sequential)]
      public struct Rect {
         public int left, top, right, bottom;
      }
      [DllImport("user32.dll")]
      public static extern IntPtr GetDesktopWindow();
      [DllImport("user32.dll")]
      public static extern IntPtr GetWindowDC( IntPtr hWnd );
      [DllImport("user32.dll")]
      public static extern IntPtr GetWindowRect( IntPtr hWnd, ref Rect rect );
      [DllImport("user32.dll")]
      public static extern IntPtr ReleaseDC( IntPtr hWnd, IntPtr hDC );
      [DllImport("user32.dll")]
      public static extern int SetProcessDPIAware();
   }

   public static Image CaptureScreen( Rectangle rectangle ) {
      return CaptureWindow( User32.GetDesktopWindow(), rectangle );
   }

   public static Image CaptureWindow( IntPtr hWnd, Rectangle rectangle ) {
      IntPtr hDCSrc = User32.GetWindowDC( hWnd );
      User32.Rect rect = new User32.Rect();
      User32.GetWindowRect( hWnd, ref rect );
      int width = Math.Min( rectangle.Width, rect.right - rect.left );
      int height = Math.Min( rectangle.Height, rect.bottom - rect.top );
      IntPtr hDCDst = GDI32.CreateCompatibleDC( hDCSrc );
      IntPtr hBitmap = GDI32.CreateCompatibleBitmap( hDCSrc, width, height );
      IntPtr hBitmapOld = GDI32.SelectObject( hDCDst, hBitmap );
      GDI32.BitBlt( hDCDst, 0, 0, width, height, hDCSrc, rectangle.X, rectangle.Y, GDI32.SRCCOPY );
      GDI32.SelectObject( hDCDst, hBitmapOld );
      GDI32.DeleteDC( hDCDst );
      User32.ReleaseDC( hWnd, hDCSrc );
      Image image = Image.FromHbitmap( hBitmap );
      GDI32.DeleteObject( hBitmap );
      return( image );
   }

   public static void CaptureScreenToFile( string filename, Rectangle rectangle ) {
      string extension = Path.GetExtension( filename );
      if( String.IsNullOrEmpty( extension ) ) throw( new Exception( "Missing extension" ) );
      ImageFormat format = GetImageFormatByExtension( extension );
      Image image = CaptureScreen( rectangle );
      image.Save( filename, format );
   }
*/

// Version v2.0
   private class User32 {
      [DllImport("user32.dll")]
      public static extern int SetProcessDPIAware(); // Added in Windows Vista.
   }

   public static void CaptureScreenToFile( string filename, Rectangle rectangle ) {
      string extension = Path.GetExtension( filename );
      if( String.IsNullOrEmpty( extension ) ) throw( new Exception( "Missing extension" ) );
      ImageFormat format = GetImageFormatByExtension( extension );
      Bitmap bitmap = new Bitmap( rectangle.Width, rectangle.Height );
      Graphics graphics = Graphics.FromImage( bitmap );
      graphics.CopyFromScreen( rectangle.Location, Point.Empty, rectangle.Size );
      bitmap.Save( filename, format );
   }

   public static ImageFormat GetImageFormatByExtension( string extension ) {
      Dictionary<String, ImageFormat> formats = new Dictionary<String, ImageFormat>();
      formats.Add( "bmp", ImageFormat.Bmp );
      formats.Add( "emf", ImageFormat.Emf );
      formats.Add( "exif", ImageFormat.Exif );
      formats.Add( "jpg", ImageFormat.Jpeg );
      formats.Add( "jpeg", ImageFormat.Jpeg );
      formats.Add( "gif", ImageFormat.Gif );
      formats.Add( "png", ImageFormat.Png );
      formats.Add( "tiff", ImageFormat.Tiff );
      formats.Add( "wmf", ImageFormat.Wmf );

      try {
         return( formats[ extension.Substring( 1 ).ToLower() ] );
      } catch( Exception e ) {
         throw( new Exception( "Unknown extension " + extension, e ) );
      }
   }

   public static void Help() {
      Console.WriteLine( "QScreenCapture v2.0 (c) 2021-2022 Sensei (aka 'Q')" );
      Console.WriteLine( "Capture the screen to an image file." );
      Console.WriteLine( "" );
      Console.WriteLine( "Usage:" );
      Console.WriteLine( "QScreenCapture [-h|--help|/?] [-v|--verbose] [-s <screen-id>] [-x <x>] [-y <y>] [--width <width>] [--height <height>] [-d|--datestamp] [-df|--dateformat <date-format>] filename" );
      Console.WriteLine( "" );
      Console.WriteLine( "Examples:" );
      Console.WriteLine( "QScreenCapture -v -d D:\\picture.jpg" );
      Console.WriteLine( "QScreenCapture -v -d -df yyyyMMddHHmmss D:\\picture.png" );
      Console.WriteLine( "QScreenCapture -v -x 200 -y 100 --width 500 --height 700 -d -df HHmmss pic.png" );
      Console.WriteLine( "QScreenCapture -v -s 2 -d C:\\Users\\[user-name]\\Pictures\\second_monitor.png" );

      int index = 1;
      foreach( System.Windows.Forms.Screen screen in System.Windows.Forms.Screen.AllScreens ) {
         Console.WriteLine( "Screen #{0} X={1} Y={2} Width={3} Height={4}",
            index,
            screen.WorkingArea.Location.X, screen.WorkingArea.Location.Y,
            screen.Bounds.Size.Width, screen.Bounds.Size.Height );
         index++;
      }
   }

   public static void Main( string [] args ) {
      if( Environment.OSVersion.Version.Major >= 6 ) {
         User32.SetProcessDPIAware(); // Added in Windows Vista.
      }

      if( args.Length > 0 ) {
        Rectangle rectangle = new Rectangle();
        if( System.Windows.Forms.Screen.AllScreens.Length > 0 ) { // Exaggeration?
           System.Windows.Forms.Screen screen = System.Windows.Forms.Screen.AllScreens[ 0 ];
           rectangle.Location = screen.WorkingArea.Location;
           rectangle.Size = screen.Bounds.Size;
        } else {
           Console.Error.WriteLine( "No suitable displays found!" );
           Environment.Exit( 20 );
        }

        string dateformat = "_yyyyMMdd_HHmmss";
        bool datestamp = false;
        bool verbose = false;
        string filename = "";

        for( int i = 0; i < args.Length; i++ ) {
           string arg = args[i];
           if( arg.Equals( "-h" ) || arg.Equals( "--help" ) || arg.Equals( "/?" ) ) {
              Help();
              Environment.Exit( 0 );
           } else if( arg.Equals( "-v" ) || arg.Equals( "--verbose" ) ) {
              verbose = true;
           } else if( arg.Equals( "-d" ) || arg.Equals( "--datestamp" ) ) {
              datestamp = true;
           } else if( arg.Equals( "-df" ) || arg.Equals( "--dateformat" ) ) {
              i++;
              try {
                 dateformat = args[ i ];
              } catch( Exception e ) {
                 Console.Error.WriteLine( e.Message );
                 Environment.Exit( 20 );
              }
           } else if( arg.Equals( "-s" ) || arg.Equals( "--screen" ) ) {
              i++;
              try {
                 int screen_id = Int32.Parse( args[ i ] );
                 if( screen_id < 1 ) throw( new ArgumentException() );
                 screen_id--;
                 if( screen_id >= System.Windows.Forms.Screen.AllScreens.Length ) throw( new ArgumentException() );
                 System.Windows.Forms.Screen screen = System.Windows.Forms.Screen.AllScreens[ screen_id ];
                 rectangle = screen.Bounds; // Careful. Overwrites -x, -y, --width and --height at once.
              } catch( Exception e ) {
                 Console.Error.WriteLine( e.Message );
                 Environment.Exit( 20 );
              }
           } else if( arg.Equals( "-x" ) ) {
              i++;
              try {
                 rectangle.X = Int32.Parse( args[ i ] );
                 if( rectangle.X < 0 ) throw( new FormatException() );
              } catch( FormatException e ) {
                 Console.Error.WriteLine( e.Message );
                 Environment.Exit( 20 );
				  }
           } else if( arg.Equals( "-y" ) ) {
              i++;
              try {
                 rectangle.Y = Int32.Parse( args[ i ] );
                 if( rectangle.Y < 0 ) throw( new FormatException() );
              } catch( FormatException e ) {
                 Console.Error.WriteLine( e.Message );
                 Environment.Exit( 20 );
				  }
           } else if( arg.Equals( "--width" ) ) {
              i++;
              try {
                 rectangle.Width = Int32.Parse( args[ i ] );
                 if( rectangle.Width <= 0 ) throw( new FormatException() );
              } catch( FormatException e ) {
                 Console.Error.WriteLine( e.Message );
                 Environment.Exit( 20 );
				  }
           } else if( arg.Equals( "--height" ) ) {
              i++;
              try {
                 rectangle.Height = Int32.Parse( args[ i ] );
                 if( rectangle.Height <= 0 ) throw( new FormatException() );
              } catch( FormatException e ) {
                 Console.Error.WriteLine( e.Message );
                 Environment.Exit( 20 );
				  }
           } else if( i == args.Length - 1 ) {
              filename = args[ i ];
           } else {
               Console.Error.WriteLine( "Unknown argument \"{0}\"!", arg );
               Environment.Exit( 20 );
			  }
        }

        if( !String.IsNullOrEmpty( filename ) ) {
	        if( datestamp ) {
	           string extension = Path.GetExtension( filename );
	           filename = filename.Substring( 0, filename.Length - extension.Length );
	           filename = filename + DateTime.Now.ToString( dateformat ) + extension;
	        }
	
	        try {
	           CaptureScreenToFile( filename, rectangle );
	           if( verbose ) Console.WriteLine( "Screen captured to " + filename );
	        } catch( Exception e ) {
	           Console.Error.WriteLine( e.Message );
	           Environment.Exit( 20 );
	        }
	     } else {
           Console.Error.WriteLine( "Required argument is missing!" );
           Environment.Exit( 20 );
		  }
     } else {
        Help();
     }
  }
}
