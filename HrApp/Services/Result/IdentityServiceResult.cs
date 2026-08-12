using Microsoft.AspNetCore.Identity;

namespace HrApp.Services.Result
{
    public class IdentityServiceResult
    {
        List<string> _errors = new List<string>();

        public IEnumerable<string> Errors => _errors;
        public IdentityUser IdentityUser { get; set; }
        public bool Succeeded { get; set; }

        public void AddError(string error)
        {
            _errors.Add(error);
        }
    }
}
