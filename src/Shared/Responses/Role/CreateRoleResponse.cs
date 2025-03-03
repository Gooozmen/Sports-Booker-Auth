namespace Shared.Responses.Role;

public class CreateRoleResponse(
    bool succeeded,
    bool isLockedOut,
    bool isNotAllowed,
    bool requiresTwoFactor,
    string data)
    : BaseIdentityResponse(succeeded, isLockedOut, isNotAllowed, requiresTwoFactor, data);