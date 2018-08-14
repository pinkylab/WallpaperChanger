using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Unsplasharp;

namespace WallpaperChanger
{
    class Program
    {
        static void Main(string[] args)
        {
            Unsplash unsplash;

            if(args.Length == 0)
            {
                unsplash = new Unsplash();
            }
            else
            {
                unsplash = new Unsplash(args[0]);
            }

            WebClient wc = new WebClient();
            wc.DownloadFile(unsplash.DownloadLink, unsplash.SavePath);

            Wallpaper.SetWallpaper(unsplash.SavePath, WallpaperStyle.ResizeFill);
            // Wallpaper.UnsetWallpaper();

            Console.WriteLine("Set Wallpaper.");
        }
    }

    class Unsplash
    {
        private string download_link;
        private string save_path;

        public Unsplash()
        {
            download_link = "https://source.unsplash.com/random";

            string random = Guid.NewGuid().ToString("N").Substring(0, 16);
            save_path = Path.GetTempPath() + random + ".jpg";
        }

        public Unsplash(string apikey)
        {
            var client = new UnsplasharpClient(apikey);

            Unsplasharp.Models.Photo randomPhoto = null;

            Task task = Task.Factory.StartNew(async () =>
            {
                randomPhoto = await client.GetRandomPhoto();

            }).Unwrap();

            task.Wait();

            download_link = randomPhoto.Links.Download;
            save_path = Path.GetTempPath() + randomPhoto.Id + ".jpg"; // jpg決め打ちでいいのかな・・・
        }

        public string DownloadLink
        {
            set { this.download_link = value; }
            get { return this.download_link; }
        }

        public string SavePath
        {
            set { this.save_path = value; }
            get { return this.save_path; }
        }
    }
}