namespace SteveCtrl.Models
{
    public enum UserRole
    {
        Administrator,
        Default
    }

    public record UserModel(int Id, string Name, string Password, UserRole Role);
}
