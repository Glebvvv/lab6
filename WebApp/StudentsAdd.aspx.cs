using System;
using WebApp;

namespace WebApp
{
    public partial class StudentsAdd : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtEnrollmentDate.Text = DateTime.Today.ToString("yyyy-MM-dd");
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                using (var context = new SchoolEntities2())
                {
                    var newStudent = new Student
                    {
                        LastName = txtLastName.Text.Trim(),
                        FirstName = txtFirstName.Text.Trim(),
                        EnrollmentDate = DateTime.Parse(txtEnrollmentDate.Text)
                    };

                    context.Students.Add(newStudent);
                    context.SaveChanges();

                    lblMessage.Text = $"Student {newStudent.FirstName} {newStudent.LastName} added successfully!";
                    lblMessage.Visible = true;
                    lblError.Visible = false;

                    // Очищаем поля после успешного сохранения
                    txtLastName.Text = "";
                    txtFirstName.Text = "";
                    txtEnrollmentDate.Text = DateTime.Today.ToString("yyyy-MM-dd");
                }
            }
            catch (Exception ex)
            {
                lblError.Text = $"Error adding student: {ex.Message}";
                lblError.Visible = true;
                lblMessage.Visible = false;
            }
        }
    }
}