using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hello.Domain.Models
{
    public class CommunityMember
    {
        public string MemberId { get; set; }
        public virtual User Member { get; set; }
        public int CommunityId { get; set; }
        public virtual Community Community { get; set; }
    }
}
