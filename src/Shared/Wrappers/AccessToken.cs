namespace CourtBooker.Auth.Shared.Wrappers;

public class AccessToken
{
    public AccessToken(string bearer)
    {
        Bearer = bearer;
    }

    public string Bearer { get; set; }
}