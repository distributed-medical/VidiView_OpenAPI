using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VidiView.Api.DataModel;
public static class LinkExtension
{
    public static TemplatedLink AsTemplatedLink(this Link link)
    {
        return new TemplatedLink(link);
    }
}
