<%@ Page Title="Students" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Students.aspx.cs" Inherits="WebApp.Students" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Students Management</h2>
    
    <asp:GridView ID="StudentsGridView" runat="server" 
        AutoGenerateColumns="False" 
        DataKeyNames="StudentID"
        AllowPaging="True" 
        AllowSorting="True"
        PageSize="5"
        OnRowEditing="StudentsGridView_RowEditing"
        OnRowUpdating="StudentsGridView_RowUpdating" 
        OnRowCancelingEdit="StudentsGridView_RowCancelingEdit"
        OnRowDeleting="StudentsGridView_RowDeleting"
        OnPageIndexChanging="StudentsGridView_PageIndexChanging"
        OnSorting="StudentsGridView_Sorting"
        CssClass="table table-striped">
        <Columns>
            <asp:BoundField DataField="StudentID" HeaderText="ID" ReadOnly="True" SortExpression="StudentID" />
            <asp:TemplateField HeaderText="Last Name" SortExpression="LastName">
                <EditItemTemplate>
                    <asp:TextBox ID="txtLastName" runat="server" Text='<%# Bind("LastName") %>' CssClass="form-control"></asp:TextBox>
                </EditItemTemplate>
                <ItemTemplate>
                    <asp:Label ID="lblLastName" runat="server" Text='<%# Eval("LastName") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="First Name" SortExpression="FirstName">
                <EditItemTemplate>
                    <asp:TextBox ID="txtFirstName" runat="server" Text='<%# Bind("FirstName") %>' CssClass="form-control"></asp:TextBox>
                </EditItemTemplate>
                <ItemTemplate>
                    <asp:Label ID="lblFirstName" runat="server" Text='<%# Eval("FirstName") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Enrollment Date" SortExpression="EnrollmentDate">
                <EditItemTemplate>
                    <asp:TextBox ID="txtEnrollmentDate" runat="server" Text='<%# Bind("EnrollmentDate", "{0:yyyy-MM-dd}") %>' 
                        TextMode="Date" CssClass="form-control"></asp:TextBox>
                </EditItemTemplate>
                <ItemTemplate>
                    <asp:Label ID="lblEnrollmentDate" runat="server" Text='<%# Eval("EnrollmentDate", "{0:dd.MM.yyyy}") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:CommandField ShowEditButton="True" ButtonType="Button" ControlStyle-CssClass="btn btn-primary btn-sm" />
            <asp:CommandField ShowDeleteButton="True" ButtonType="Button" ControlStyle-CssClass="btn btn-danger btn-sm" />
        </Columns>
        <PagerStyle CssClass="grid-pager" />
    </asp:GridView>

    <div class="mt-3">
        <asp:Button ID="btnAddNew" runat="server" Text="Add New Student" 
            PostBackUrl="~/StudentsAdd.aspx" CssClass="btn btn-success" />
    </div>
</asp:Content>