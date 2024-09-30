using System.Resources;

namespace YourIdentityProject.Resources
{
    public class Messages
    {
        private static ResourceManager resourceManager = new ResourceManager("YourIdentityProject.Resources.Messages", typeof(Messages).Assembly);

        public static string UsernameRequired => resourceManager.GetString("UsernameRequired");
        public static string PasswordRequired => resourceManager.GetString("PasswordRequired");
        public static string EmailRequired => resourceManager.GetString("EmailRequired");
        public static string InvalidEmailFormat => resourceManager.GetString("InvalidEmailFormat");
    }
}