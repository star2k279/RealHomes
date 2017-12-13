using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RealHomes.Models
{
    public class PagedAgentCMSView
    {
        public IEnumerable<AgentViewCMSModel> CMSAgents;
        public int TotalRecords;
        public int TotalPages;
        public int CurrentPage;
        public int StartPage;
        public int EndPage;
    }
}