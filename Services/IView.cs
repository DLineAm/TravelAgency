namespace TravelAgency.Services
{
    public interface IView
    {
        void Close();
        void Minimize();
        void Maximize();
        void UpdateResource(string resourceName, object value);
    }
}