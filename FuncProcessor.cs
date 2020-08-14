using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Plot {
    class FuncProcessor {
        public Func<double, double> SumFunctions(Func<double, double> f1, Func<double, double> f2) {
            return new Func<double, double>((x) => f1.Invoke(x) + f2.Invoke(x));
        }
        public Func<double, double> SubFunctions(Func<double, double> f1, Func<double, double> f2) {
            return new Func<double, double>((x) => f1.Invoke(x) - f2.Invoke(x));
        }
    }
}
