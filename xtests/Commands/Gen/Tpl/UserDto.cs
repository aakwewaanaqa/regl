//#!
#if DEV
using Codegen.Attributes;
#endif

namespace aimy_galaxy_proxy.User.Dto;

#if DEV
[GenDto]
#endif
//#!echo 'namespace Core.Apis {'
//#!copy 6
public class UserDto {
    public string email       { get; set; }
    public string uuid        { get; set; }
    public string profileName { get; set; }
    public string loginToken { get; set; }
}
//#!echo }