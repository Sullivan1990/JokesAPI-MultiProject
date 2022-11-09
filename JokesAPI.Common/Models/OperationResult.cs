using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JokesAPI.Common.Models
{
    public class OperationResult<T>
    {
        public bool Succeeded { get; set; } = true;
        public string ErrorMessage { get; set; } = "";
        public T? Result { get; set; }
    }
}
