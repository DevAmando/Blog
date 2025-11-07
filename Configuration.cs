namespace Blog;

public static class Configuration
{
    public static string JwtKey  = "awdujiniawdyudawtawdbhbudawniwdaniadw=";
    public static string ApiKeyName  = "api_key";
    public static string ApiKey = "curso_api_Isadjkashd6==";
    public static SmtpConfiguration Smtp  = new ();

    public class SmtpConfiguration
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
    

}
