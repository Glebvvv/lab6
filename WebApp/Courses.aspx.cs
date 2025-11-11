using System;
using System.Linq;
using WebApp;

namespace WebApp
{
    public partial class Courses : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadDepartments();
                LoadCourses();
            }
        }

        private void LoadDepartments()
        {
            using (var context = new SchoolEntities2())
            {
                var departments = context.Departments.ToList();
                ddlDepartments.DataSource = departments;
                ddlDepartments.DataBind();

                // Добавляем пункт "All Departments"
                ddlDepartments.Items.Insert(0, new System.Web.UI.WebControls.ListItem("All Departments", "0"));
            }
        }

        private void LoadCourses()
        {
            using (var context = new SchoolEntities2())
            {
                IQueryable<Cours> coursesQuery = context.Courses.Include("Department");

                if (ddlDepartments.SelectedValue != "0")
                {
                    int departmentId = Convert.ToInt32(ddlDepartments.SelectedValue);
                    coursesQuery = coursesQuery.Where(c => c.DepartmentID == departmentId);
                }

                var courses = coursesQuery.ToList();
                CoursesGridView.DataSource = courses;
                CoursesGridView.DataBind();
            }
        }

        protected void ddlDepartments_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadCourses();
        }
    }
}