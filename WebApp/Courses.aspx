<%@ Page Title="Courses" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Courses.aspx.cs" Inherits="WebApp.Courses" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Courses by Department</h2>
    
<div class="form-group mb-4">
    <label>Select Department:</label>
    <asp:DropDownList ID="ddlDepartments" runat="server" 
        AutoPostBack="True" 
        OnSelectedIndexChanged="ddlDepartments_SelectedIndexChanged"
        CssClass="form-control" DataTextField="Name" DataValueField="DepartmentID">
    </asp:DropDownList>
</div>

<asp:GridView ID="CoursesGridView" runat="server" 
    AutoGenerateColumns="False" 
    CssClass="table table-striped"
    EmptyDataText="No courses found for selected department">
    <Columns>
        <asp:BoundField DataField="CourseID" HeaderText="Course ID" />
        <asp:BoundField DataField="Title" HeaderText="Title" />
        <asp:BoundField DataField="Credits" HeaderText="Credits" />
        <asp:BoundField DataField="Department.Name" HeaderText="Department" />
    </Columns>
</asp:GridView>
</asp:Content>