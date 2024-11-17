<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="WebApplication2.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Login</title>
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <style type="text/css">
        .login-container {
            width: 300px;
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
        .btn-login {
            background-color: #007bff;
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
        .signup-section {
            text-align: center;
            margin-top: 15px;
            padding-top: 15px;
            border-top: 1px solid #eee;
        }
        .signup-link {
            color: #007bff;
            text-decoration: none;
        }
        .signup-link:hover {
            text-decoration: underline;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="login-container">
            <h2>Login</h2>
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
                <asp:CheckBox ID="chkRememberMe" runat="server" Text="Remember Me" />
            </div>
            
            <asp:Button ID="btnLogin" runat="server" Text="Login" CssClass="btn-login" OnClick="btnLogin_Click" />

            <div class="signup-section">
                Don't have an account? 
                <asp:HyperLink ID="lnkSignup" runat="server" NavigateUrl="~/Signup.aspx" CssClass="signup-link">
                    Create one now
                </asp:HyperLink>
            </div>
        </div>
    </form>
</body>
</html>