using System;
using System.Linq;
using System.Web.UI.WebControls;
using WebApp;

namespace WebApp
{
    public partial class Students : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindStudents();
            }
        }

        private void BindStudents()
        {
            using (var context = new SchoolEntities2())
            {
                var students = context.Students.ToList();
                StudentsGridView.DataSource = students;
                StudentsGridView.DataBind();
            }
        }

        protected void StudentsGridView_RowEditing(object sender, GridViewEditEventArgs e)
        {
            StudentsGridView.EditIndex = e.NewEditIndex;
            BindStudents();
        }

        protected void StudentsGridView_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int studentId = Convert.ToInt32(StudentsGridView.DataKeys[e.RowIndex].Value);

            using (var context = new SchoolEntities2())
            {
                var student = context.Students.Find(studentId);

                if (student != null)
                {
                    // Получаем значения из TextBox'ов
                    var row = StudentsGridView.Rows[e.RowIndex];
                    student.LastName = (row.FindControl("txtLastName") as TextBox)?.Text;
                    student.FirstName = (row.FindControl("txtFirstName") as TextBox)?.Text;

                    var dateText = (row.FindControl("txtEnrollmentDate") as TextBox)?.Text;
                    if (DateTime.TryParse(dateText, out DateTime enrollmentDate))
                    {
                        student.EnrollmentDate = enrollmentDate;
                    }

                    context.SaveChanges();
                }
            }

            StudentsGridView.EditIndex = -1;
            BindStudents();
        }

        protected void StudentsGridView_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            StudentsGridView.EditIndex = -1;
            BindStudents();
        }

        protected void StudentsGridView_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int studentId = Convert.ToInt32(StudentsGridView.DataKeys[e.RowIndex].Value);

            using (var context = new SchoolEntities2())
            {
                var student = context.Students.Find(studentId);
                if (student != null)
                {
                    context.Students.Remove(student);
                    context.SaveChanges();
                }
            }

            BindStudents();
        }

        protected void StudentsGridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            StudentsGridView.PageIndex = e.NewPageIndex;
            BindStudents();
        }

        protected void StudentsGridView_Sorting(object sender, GridViewSortEventArgs e)
        {
            using (var context = new SchoolEntities2())
            {
                var students = context.Students.ToList();

                switch (e.SortExpression)
                {
                    case "LastName":
                        students = e.SortDirection == SortDirection.Ascending ?
                            students.OrderBy(s => s.LastName).ToList() :
                            students.OrderByDescending(s => s.LastName).ToList();
                        break;
                    case "FirstName":
                        students = e.SortDirection == SortDirection.Ascending ?
                            students.OrderBy(s => s.FirstName).ToList() :
                            students.OrderByDescending(s => s.FirstName).ToList();
                        break;
                    case "EnrollmentDate":
                        students = e.SortDirection == SortDirection.Ascending ?
                            students.OrderBy(s => s.EnrollmentDate).ToList() :
                            students.OrderByDescending(s => s.EnrollmentDate).ToList();
                        break;
                }

                StudentsGridView.DataSource = students;
                StudentsGridView.DataBind();
            }
        }
    }
}