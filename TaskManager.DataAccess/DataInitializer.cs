using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.DataAccess
{
    public class DataInitializer : DropCreateDatabaseAlways<AppContext>
    {
        

        protected override void Seed(AppContext context)
        {
            base.Seed(context);
        }
    }
}
