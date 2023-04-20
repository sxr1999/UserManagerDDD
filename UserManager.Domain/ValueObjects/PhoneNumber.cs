using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserManager.Domain.ValueObjects
{
    public record PhoneNumber(int RegionNumber,string Number);
}
