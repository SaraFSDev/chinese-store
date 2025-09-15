using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using project.Models;
using project.Models.ModelsDTO;
using Microsoft.AspNetCore.Mvc;


namespace project.Services.IServices
{
    public interface IAuthService
    {
       Task RegisterAsync(Register model);

        Task<IActionResult> LoginAsync(Login model);
    }
}
