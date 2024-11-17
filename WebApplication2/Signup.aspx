<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Signup.aspx.cs" Inherits="WebApplication2.Signup" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Sign Up</title>
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <style type="text/css">
        .signup-container {
            width: 400px;
            margin: 100px auto;
            padding: 20px;
            border: 1px solid #ccc;
            border-radius: 5px;
            box-shadow: 0 0 10px rgba(0,0,0,0.1);
        }
        .form-group {
            margin-bottom: 15px;
        }
        .form-control {
            width: 100%;
            padding: 8px;
            border: 1px solid #ddd;
            border-radius: 4px;
            box-sizing: border-box;
        }
        .btn-signup {
            background-color: #28a745;
            color: white;
            padding: 10px 15px;
            border: none;
            border-radius: 4px;
            cursor: pointer;
            width: 100%;
        }
        .error-message {
            color: red;
            margin-bottom: 10px;
        }
        .success-message {
            color: green;
            margin-bottom: 10px;
        }
        .login-link {
            text-align: center;
            margin-top: 15px;
            padding-top: 15px;
            border-top: 1px solid #eee;
        }
        .login-link a {
            color: #007bff;
            text-decoration: none;
        }
        .login-link a:hover {
            text-decoration: underline;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="signup-container">
            <h2>Create Account</h2>
            <asp:Label ID="lblMessage" runat="server" CssClass="error-message"></asp:Label>
            
            <div class="form-group">
                <asp:Label ID="lblUsername" runat="server" Text="Username:"></asp:Label>
                <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control" placeholder="Enter username"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvUsername" runat="server" 
                    ControlToValidate="txtUsername"
                    ErrorMessage="Username is required" 
                    Display="Dynamic" 
                    ForeColor="Red">
                </asp:RequiredFieldValidator>
            </div>
            
            <div class="form-group">
                <asp:Label ID="lblPassword" runat="server" Text="Password:"></asp:Label>
                <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="form-control" placeholder="Enter password"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvPassword" runat="server" 
                    ControlToValidate="txtPassword"
                    ErrorMessage="Password is required" 
                    Display="Dynamic" 
                    ForeColor="Red">
                </asp:RequiredFieldValidator>
            </div>
            
            <div class="form-group">
                <asp:Label ID="lblConfirmPassword" runat="server" Text="Confirm Password:"></asp:Label>
                <asp:TextBox ID="txtConfirmPassword" runat="server" TextMode="Password" CssClass="form-control" placeholder="Confirm password"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvConfirmPassword" runat="server" 
                    ControlToValidate="txtConfirmPassword"
                    ErrorMessage="Please confirm your password" 
                    Display="Dynamic" 
                    ForeColor="Red">
                </asp:RequiredFieldValidator>
                <asp:CompareValidator ID="cvPassword" runat="server"
                    ControlToValidate="txtConfirmPassword"
                    ControlToCompare="txtPassword"
                    ErrorMessage="Passwords do not match"
                    Display="Dynamic"
                    ForeColor="Red">
                </asp:CompareValidator>
            </div>
            
            <div class="form-group">
                <asp:Label ID="lblEmail" runat="server" Text="Email:"></asp:Label>
                <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" placeholder="Enter email"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvEmail" runat="server" 
                    ControlToValidate="txtEmail"
                    ErrorMessage="Email is required" 
                    Display="Dynamic" 
                    ForeColor="Red">
                </asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="revEmail" runat="server"
                    ControlToValidate="txtEmail"
                    ErrorMessage="Invalid email format"
                    ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                    Display="Dynamic"
                    ForeColor="Red">
                </asp:RegularExpressionValidator>
            </div>
            
            <asp:Button ID="btnSignup" runat="server" Text="Sign Up" CssClass="btn-signup" OnClick="btnSignup_Click" />
            
            <div class="login-link">
                Already have an account? 
                <asp:HyperLink ID="lnkLogin" runat="server" NavigateUrl="~/Login.aspx">Login here</asp:HyperLink>
            </div>
        </div>
    </form>
</body>
</html>