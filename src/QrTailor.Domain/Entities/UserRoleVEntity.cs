using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QrTailor.Domain.Entities
{
    public class UserRoleVEntity
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ClaimId { get; set; }
        public string OperationName { get; set; }
    }
}
