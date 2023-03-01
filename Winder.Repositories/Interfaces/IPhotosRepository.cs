using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Winder.Repositories.Interfaces
{
    internal interface IPhotosRepository
    {
        public void AddPhoto(byte[] image, string email)
        {
            throw new NotImplementedException();
        }

        public void DeleteAllPhotos(byte[] image, string email)
        {
            throw new NotImplementedException();
        }

        public byte[][] GetPhotos(string emai)
        {
            throw new NotImplementedException();
        }
    }
}
