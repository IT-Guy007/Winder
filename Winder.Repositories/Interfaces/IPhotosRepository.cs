namespace Winder.Repositories.Interfaces
{
    internal interface IPhotosRepository {
        public void AddPhoto(byte[] image, string email);
        public void DeleteAllPhotos(string email);
        public byte[][] GetPhotos(string email);
    }
}
