using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecondCaliburnApp.Helpers
{
    public class Commons
    {
        public static string STRCONNSTRING = "Data Source=localhost;Port=3306;Database=testtbl;uid=root;password=mysql_p@ssw0rd";

        public static string SELECTEDPEOPLEQUERY =
            "SELECT id, FirstName, LastName " +
            "    From peopletbl " +
            "ORDER BY id ";
    }
}
