using LinqToTwitter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitterAssignment.Abstract
{
    public interface IMvcAuthorization
    {
        MvcAuthorizer Auth { get; }
    }
}
