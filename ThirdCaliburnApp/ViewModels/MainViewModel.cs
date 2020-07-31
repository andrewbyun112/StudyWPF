using Caliburn.Micro;
using MvvmDialogs;
using MySql.Data.MySqlClient;
using System.Windows;
using ThirdCaliburnApp.Models;

namespace ThirdCaliburnApp.ViewModels
{
    public class MainViewModel : Conductor<object>, IHaveDisplayName
    {
        #region 속성 영역
        readonly IWindowManager windowManager;
        readonly IDialogService dialogService;

        int id;
        public int Id
        {
            get => id;
            set
            {
                id = value;
                NotifyOfPropertyChange(() => Id);
                NotifyOfPropertyChange(() => CanDeleteEmployee);
            }
        }

        string empName;
        public string EmpName
        {
            get => empName;
            set
            {
                empName = value;
                NotifyOfPropertyChange(() => EmpName);
                NotifyOfPropertyChange(() => CanSaveEmployee);
            }
        }

        decimal salary;
        public decimal Salary
        {
            get => salary;
            set
            {
                salary = value;
                NotifyOfPropertyChange(() => Salary);
                NotifyOfPropertyChange(() => CanSaveEmployee);
            }
        }

        string deptName;
        public string DeptName
        {
            get => deptName;
            set
            {
                deptName = value;
                NotifyOfPropertyChange(() => DeptName);
                NotifyOfPropertyChange(() => CanSaveEmployee);
            }
        }

        string destination;
        public string Destination
        {
            get => destination;
            set
            {
                destination = value;
                NotifyOfPropertyChange(() => Destination);
                NotifyOfPropertyChange(() => CanSaveEmployee);
            }
        }

        public MainViewModel(IWindowManager windowManager, IDialogService dialogService)
        {
            this.windowManager = windowManager;
            this.dialogService = dialogService;

            GetEmployees();
        }

        //전체 Employees 리스트
        BindableCollection<EmployeesModel> employees;
        public BindableCollection<EmployeesModel> Employees { 
            get => employees;
            set
            {
                employees = value;
                NotifyOfPropertyChange(() => Employees);
            }
        }

        EmployeesModel selectedEmployee;
        //리스트 중 선택된 하나 Employee
        public EmployeesModel SelectedEmployee
        {
            get => selectedEmployee;
            set
            {
                selectedEmployee = value;

                if (value != null)
                {
                    Id = value.id;
                    EmpName = value.EmpName;
                    Salary = value.Salary;
                    DeptName = value.DeptName;
                    Destination = value.Destination;
                }
                NotifyOfPropertyChange(() => SelectedEmployee);
            }
        }

        public bool CanSaveEmployee
        {
            #region 같은방법
            //get
            //{
            //    if (string.IsNullOrEmpty(EmpName) || string.IsNullOrEmpty(DeptName) ||
            //        string.IsNullOrEmpty(Destination) || Salary == 0)
            //        return false;
            //    else
            //        return true;
            //}

            //get
            //{
            //    return !(string.IsNullOrEmpty(EmpName) || string.IsNullOrEmpty(DeptName) ||
            //        string.IsNullOrEmpty(Destination) || Salary == 0);

            //}
            #endregion
            get => !(string.IsNullOrEmpty(EmpName) || string.IsNullOrEmpty(DeptName) ||
                    string.IsNullOrEmpty(Destination) || Salary == 0);
        }

        
        #endregion

        public MainViewModel()
        {
            GetEmployees();
        }

        private void GetEmployees()
        {
            using (MySqlConnection conn = new MySqlConnection(Commons.CONNSTRING))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(EmployeesTbl.SELECT_EMPLOYEES, conn);
                //cmd.Connection = conn;
                MySqlDataReader reader = cmd.ExecuteReader();
                Employees = new BindableCollection<EmployeesModel>();
                while (reader.Read())
                {
                    var temp = new EmployeesModel
                    {
                        id = (int)reader["id"],
                        EmpName = reader["EmpName"].ToString(),
                        Salary = (decimal)reader["Salary"],
                        DeptName = reader["DeptName"].ToString(),
                        Destination = reader["Destination"].ToString()
                    };
                    Employees.Add(temp);
                }
            }
        }

        public void SaveEmployee()
        {
            int resultRow = 0;

            try
            {
                using (MySqlConnection conn = new MySqlConnection(Commons.CONNSTRING))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = conn;
                    if (Id == 0) //insert
                        cmd.CommandText = EmployeesTbl.INSERT_EMPLOYEE;
                    else // update
                        cmd.CommandText = EmployeesTbl.UPDATE_EMPLOYEE;

                    MySqlParameter paramEmpName = new MySqlParameter("@EmpName", MySqlDbType.VarChar, 45);
                    paramEmpName.Value = EmpName;
                    cmd.Parameters.Add(paramEmpName);

                    MySqlParameter paramSalary = new MySqlParameter("@Salary", MySqlDbType.Decimal);
                    paramSalary.Value = Salary;
                    cmd.Parameters.Add(paramSalary);

                    MySqlParameter paramDeptName = new MySqlParameter("@DeptName", MySqlDbType.VarChar, 45);
                    paramDeptName.Value = DeptName;
                    cmd.Parameters.Add(paramDeptName);

                    MySqlParameter paramDestination = new MySqlParameter("@Destination", MySqlDbType.VarChar, 45);
                    paramDestination.Value = Destination;
                    cmd.Parameters.Add(paramDestination);

                    if (Id != 0)
                    {
                        MySqlParameter paramId = new MySqlParameter("@Id", MySqlDbType.Int32);
                        paramId.Value = Id;
                        cmd.Parameters.Add(paramId);
                    }
                    resultRow = cmd.ExecuteNonQuery();
                }
                if (resultRow > 0)
                {
                    //MessageBox.Show("저장했습니다.");
                    DialogViewModel dialogVM = new DialogViewModel();
                    dialogVM.DisplayName = "저장되었습니다.";
                    bool? success = windowManager.ShowDialog(dialogVM);

                    NewEmployee();
                    GetEmployees();
                }
            }
            catch (System.Exception)
            {
                
                throw;
            }
        }

        public void NewEmployee()
        {
            Id = 0;
            EmpName = string.Empty;
            Salary = 0;
            DeptName = Destination = string.Empty;
        }

        
        public bool CanDeleteEmployee
        {
            get => !(Id == 0);
        }

        public void DeleteEmployee()
        {
            int resultRow = 0;
            using (MySqlConnection conn = new MySqlConnection(Commons.CONNSTRING))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conn;
               
                cmd.CommandText = EmployeesTbl.DELETE_EMPLOYEE;

                MySqlParameter paramId = new MySqlParameter("@Id", MySqlDbType.Int32);
                paramId.Value = Id;
                cmd.Parameters.Add(paramId);
                
                resultRow = cmd.ExecuteNonQuery();
            }

            if (resultRow > 0)
            {
                DialogViewModel dialogVM = new DialogViewModel();
                dialogVM.DisplayName = "삭제되었습니다.";
                bool? success = windowManager.ShowDialog(dialogVM);

                NewEmployee();
                GetEmployees();
            }
        }
    }
}
