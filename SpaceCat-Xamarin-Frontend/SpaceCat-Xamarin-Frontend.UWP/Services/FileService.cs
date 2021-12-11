using SpaceCat_Xamarin_Frontend.UWP;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;
using Xamarin.Forms;

[assembly: Dependency(typeof(FileService))]
namespace SpaceCat_Xamarin_Frontend.UWP
{
    public class FileService : IFileService
    {
        public void SavePicture(string name, Stream data)
        {
            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            documentsPath = Path.Combine(documentsPath, "FloorplanImages");
            Directory.CreateDirectory(documentsPath);

            string filePath = Path.Combine(documentsPath, name + ".jpg");

            byte[] bArray = new byte[data.Length];
            using (FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate))
            {
                using (data)
                {
                    data.Read(bArray, 0, (int)data.Length);
                }
                int length = bArray.Length;
                fs.Write(bArray, 0, length);
            }
        }

        public async Task<Stream> GetPicture(string name)
        {
            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            documentsPath = Path.Combine(documentsPath, "FloorplanImages");

            if (Directory.Exists(documentsPath))
            {
                string root = ApplicationData.Current.LocalFolder.Path;
                string path = root + @"\FloorplanImages";
                StorageFolder imgFolder = await StorageFolder.GetFolderFromPathAsync(path);
                try
                {
                    StorageFile storageFile = await imgFolder.GetFileAsync(name + ".jpg");
                    IRandomAccessStreamWithContentType raStream = await storageFile.OpenReadAsync();
                    return raStream.AsStreamForRead();
                }
                catch (FileNotFoundException)
                {
                    System.Diagnostics.Debug.WriteLine("File not found!!!");
                    return null;
                }
            }
            else
                return null;
        }
    }
}
