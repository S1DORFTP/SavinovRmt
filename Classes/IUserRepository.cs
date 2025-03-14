namespace rmt.Classes
{
    public interface IUserRepository
    {
        User GetUserById(int id);
        void AddUser(User user);
        void UpdateUser(User user);
        void DeleteUser(int id);
    }
}
