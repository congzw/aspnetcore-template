using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foo.Web._Demos.Lifetimes
{
    public interface ILifetimeDesc
    {

    }

    public interface ISingletonDesc: ILifetimeDesc
    {

    }

    public interface IScopedDesc: ILifetimeDesc
    {

    }

    public interface ITransientDesc: ILifetimeDesc
    {

    }

    public class LifetimeDesc : ITransientDesc, ISingletonDesc, IScopedDesc, ILifetimeDesc
    {
        public override string ToString()
        {
            return this.GetHashCode().ToString();
        }

        public static string ShowDiff(ILifetimeDesc desc, ILifetimeDesc desc2)
        {
            return string.Format("[{0}, {1}] Same: {2}", desc, desc2, object.ReferenceEquals(desc, desc2));
        }
    }
}
