<%@ Page Title="Add Student" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="StudentsAdd.aspx.cs" Inherits="WebApp.StudentsAdd" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Add New Student</h2>
    
    <div class="form-horizontal">
        <div class="form-group">
            <label class="control-label col-md-2">Last Name:</label>
            <div class="col-md-10">
                <asp:TextBox ID="txtLastName" runat="server" CssClass="form-control" required="true"></asp:TextBox>
            </div>
        </div>
        
        <div class="form-group">
            <label class="control-label col-md-2">First Name:</label>
            <div class="col-md-10">
                <asp:TextBox ID="txtFirstName" runat="server" CssClass="form-control" required="true"></asp:TextBox>
            </div>
        </div>
        
<div class="form-group mb-4">
    <label class="control-label col-md-2">Enrollment Date:</label>
    <div class="col-md-10">
        <asp:TextBox ID="txtEnrollmentDate" runat="server" TextMode="Date" CssClass="form-control" required="true"></asp:TextBox>
    </div>
</div>

<div class="form-group">
    <div class="col-md-offset-2 col-md-10">
        <asp:Button ID="btnSave" runat="server" Text="Save Student" OnClick="btnSave_Click" CssClass="btn btn-primary" />
        <asp:Button ID="btnCancel" runat="server" Text="Cancel" PostBackUrl="~/Students.aspx" CssClass="btn btn-default" />
    </div>
</div>
        
        <div class="form-group">
            <div class="col-md-offset-2 col-md-10">
                <asp:Label ID="lblMessage" runat="server" CssClass="text-success" Visible="false"></asp:Label>
                <asp:Label ID="lblError" runat="server" CssClass="text-danger" Visible="false"></asp:Label>
            </div>
        </div>
    </div>
</asp:Content>