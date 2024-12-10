using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _07LabExe1
{
    public partial class FrmUpdateMember : Form
    {
        private SqlConnection sqlConnect;
        private SqlCommand sqlCommand;
        private SqlDataAdapter sqlAdapter;
        private SqlDataReader sqlReader;

        private ClubRegistrationQuery clubRegistrationQuery;

        public DataTable dataTable;
        public BindingSource bindingSource;
        private string connectionString;

        private int ID, Age;
        private string FirstName, MiddleName, LastName, Gender, Program;
        private long StudentId;
        public FrmUpdateMember()
        {
            InitializeComponent();
            LoadStudentIDs();
            cbStudentID.SelectedIndexChanged += new EventHandler(cbStudentID_SelectedIndexChanged);
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            ID = Convert.ToInt32(cbStudentID.SelectedValue);
            FirstName = txtFirstName.Text;
            MiddleName = txtMiddleName.Text;
            LastName = txtLastName.Text;
            Age = Convert.ToInt32(txtAge.Text);
            Gender = cbGender.SelectedItem.ToString();
            Program = cbProgram.SelectedItem.ToString();

            UpdateMemberDetails(ID, FirstName, MiddleName, LastName, Age, Gender, Program);
            ClubRegistrationQuery clubRegistrationQuery = new ClubRegistrationQuery();
            this.Close();
        }

        private void UpdateMemberDetails(int id, string firstName, string middleName, string lastName, int age, string gender, string program)
        {
            sqlCommand = new SqlCommand("UPDATE ClubMembers SET FirstName = @FirstName, MiddleName = @MiddleName, LastName = @LastName, Age = @Age, Gender = @Gender, Program = @Program WHERE StudentID = @StudentID", sqlConnect);
            sqlCommand.Parameters.AddWithValue("@FirstName", firstName);
            sqlCommand.Parameters.AddWithValue("@MiddleName", middleName);
            sqlCommand.Parameters.AddWithValue("@LastName", lastName);
            sqlCommand.Parameters.AddWithValue("@Age", age);
            sqlCommand.Parameters.AddWithValue("@Gender", gender);
            sqlCommand.Parameters.AddWithValue("@Program", program);
            sqlCommand.Parameters.AddWithValue("@StudentID", id);

            sqlConnect.Open();
            sqlCommand.ExecuteNonQuery();
            sqlConnect.Close();
        }

        private void FrmUpdateMember_Load(object sender, EventArgs e)
        {
            clubRegistrationQuery = new ClubRegistrationQuery();
            LoadStudentIDs();
        }

        private void cbStudentID_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbStudentID.SelectedValue != null)
            {
                int selectedID = Convert.ToInt32(cbStudentID.SelectedValue);
                PopulateMemberDetails(selectedID);
            }
        }

        private void PopulateMemberDetails(int id)
        {
            sqlCommand = new SqlCommand("SELECT FirstName, MiddleName, LastName, Age, Gender, Program FROM ClubMembers WHERE StudentID = @StudentID", sqlConnect);
            sqlCommand.Parameters.AddWithValue("@StudentID", id);

            sqlConnect.Open();
            sqlReader = sqlCommand.ExecuteReader();

            if (sqlReader.Read())
            {
                txtFirstName.Text = sqlReader["FirstName"].ToString();
                txtMiddleName.Text = sqlReader["MiddleName"].ToString();
                txtLastName.Text = sqlReader["LastName"].ToString();
                txtAge.Text = sqlReader["Age"].ToString();
                cbGender.SelectedItem = sqlReader["Gender"].ToString();
                cbProgram.SelectedItem = sqlReader["Program"].ToString();
            }

            sqlReader.Close();
            sqlConnect.Close();
        }

        private void LoadStudentIDs()
        {
            // Fetch all StudentIDs to populate in the ComboBox
            connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\azilr\\source\\repos\\07LabExe1\\07LabExe1\\ClubDB.mdf;Integrated Security=True";
            sqlConnect = new SqlConnection(connectionString);
            dataTable = new DataTable();
            bindingSource = new BindingSource();
            sqlCommand = new SqlCommand("SELECT StudentId FROM ClubMembers", sqlConnect);
            sqlConnect.Open();
            sqlAdapter = new SqlDataAdapter(sqlCommand);
            sqlAdapter.Fill(dataTable);
            sqlConnect.Close();

            cbStudentID.DisplayMember = "StudentId";
            cbStudentID.ValueMember = "StudentId";
            cbStudentID.DataSource = dataTable;
        }
    }
}
