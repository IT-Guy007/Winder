namespace Winder.Repositories.Interfaces
{
    public interface IPhotosRepository
    {
        public bool AddPhoto(byte[] image, string email);
        public bool DeleteAllPhotos(string email);
        public byte[][] GetPhotos(string email);
    }
}
