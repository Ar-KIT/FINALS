using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _07LabExe1
{
    public partial class FrmClubRegistration : Form
    {
        public FrmClubRegistration()
        {
            InitializeComponent();
        }
        private ClubRegistrationQuery clubRegistrationQuery;
        private int ID, Age, count;
        private string FirstName, MiddleName, LastName, Gender, Program;

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            FrmUpdateMember updateMemberForm = new FrmUpdateMember();
            updateMemberForm.FormClosed += (s, args) => RefreshListOfCLubMembers();
            updateMemberForm.ShowDialog();
        }

        private long StudentId;

        private void FrmClubRegistration_Load(object sender, EventArgs e)
        {
            clubRegistrationQuery = new ClubRegistrationQuery();
            RefreshListOfCLubMembers();
        }
        
        private void RefreshListOfCLubMembers()
        {
            clubRegistrationQuery.DisplayList();
            dataViewClubMembers.DataSource = clubRegistrationQuery.bindingSource;
        }
        private int RegistrationID()
        {
            return count++;
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            ID = RegistrationID();
            StudentId = Convert.ToInt64(txtStudentID.Text);
            FirstName = txtFirstName.Text;
            MiddleName = txtMiddleName.Text;
            LastName = txtLastName.Text;
            Age = Convert.ToInt32(txtAge.Text);
            Gender = cbGender.SelectedItem.ToString();
            Program = cbProgram.SelectedItem.ToString();

            clubRegistrationQuery.RegisterStudent(ID, StudentId, FirstName, MiddleName, LastName, Age, Gender, Program);
        }
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            RefreshListOfCLubMembers();
        }
    }
}
