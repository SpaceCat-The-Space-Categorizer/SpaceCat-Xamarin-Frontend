using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace SpaceCat_Xamarin_Frontend
{
    public interface IFileService
    {
        void SavePicture(string name, Stream data);
        Task<Stream> GetPicture(string name);
    }
}
