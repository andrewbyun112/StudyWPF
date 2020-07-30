namespace ThirdCaliburnApp
{
    public class Commons
    {
        internal static readonly string CONNSTRING = 
            "Data source=localhost;Port=3306;Database=testtbl;Uid=root;Password=mysql_p@ssw0rd";
    }

    public class EmployeesTbl
    {
        public static string SELECT_EMPLOYEES = "SELECT   id, " +
                                                                           "                EmpName, " +
                                                                           "                Salary, " +
                                                                           "                DeptName, " +
                                                                           "                Destination " +
                                                                           "    FROM   employeestbl ";

        internal static string INSERT_EMPLOYEE = "";

        internal static string UPDATE_EMPLOYEE = "UPDATE    employeestbl " +
                                                                            "       SET " +
                                                                            "                 EmpName = @EmpName, " +
                                                                            "                 Salary = @Salary, " +
                                                                            "                 DeptName = @DeptName, " +
                                                                            "                 Destination = @Destination " +
                                                                            "WHERE     id = @id ";
    }
}