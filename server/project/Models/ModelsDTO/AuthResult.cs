using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace project.Models.ModelsDTO
{
    public class AuthResult
    {
        public bool IsSuccess { get; set; } 
        public string Message { get; set; }
        public string Token { get; set; }
    }
}
