namespace Shared.Responses;

public class PasswordSignInResponse(
    bool succeeded,
    bool isLockedOut,
    bool isNotAllowed,
    bool requiresTwoFactor,
    string data)
    : BaseIdentityResponse(succeeded, isLockedOut, isNotAllowed, requiresTwoFactor, data);