using Domain.DTO.Email;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services.IServices
{
    public interface IEmailHelpers
    {
        Task<bool> SeedGmail(SeedMailRequest request, string mailName, string appPass);
    }
}
