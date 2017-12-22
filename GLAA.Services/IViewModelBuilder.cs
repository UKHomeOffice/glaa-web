using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GLAA.Services
{
    public interface IViewModelBuilder<out T>
    {
        T New();
    }
}
