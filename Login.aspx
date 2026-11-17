<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Login.aspx.vb" Inherits="SistemaClientes.Login" %>

<!DOCTYPE html>
<html lang="es">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>Iniciar sesión | Sistema de Clientes</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.11.3/font/bootstrap-icons.css" rel="stylesheet" />
    <link href="Content/css/estilos.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server" class="login-fondo">
        <div class="card login-tarjeta">
            <div class="card-body p-4 p-md-5">

                <div class="text-center mb-4">
                    <i class="bi bi-person-circle login-icono"></i>
                    <h4 class="mt-2 mb-0">Sistema de Clientes</h4>
                    <small class="text-muted">Inicia sesión para continuar</small>
                </div>

                <!-- Mensaje de error -->
                <asp:Panel ID="pnlError" runat="server" Visible="false" CssClass="alert alert-danger" role="alert">
                    <i class="bi bi-exclamation-triangle-fill me-1"></i>
                    <asp:Label ID="lblError" runat="server"></asp:Label>
                </asp:Panel>

                <div class="mb-3">
                    <label for="txtUsuario" class="form-label">Usuario</label>
                    <div class="input-group">
                        <span class="input-group-text"><i class="bi bi-person"></i></span>
                        <asp:TextBox ID="txtUsuario" runat="server" CssClass="form-control" MaxLength="50" placeholder="Nombre de usuario"></asp:TextBox>
                    </div>
                    <asp:RequiredFieldValidator ID="rfvUsuario" runat="server" ControlToValidate="txtUsuario"
                        ErrorMessage="Ingresa tu usuario." Display="Dynamic" CssClass="text-danger small"
                        ValidationGroup="Login"></asp:RequiredFieldValidator>
                </div>

                <div class="mb-4">
                    <label for="txtContrasena" class="form-label">Contraseña</label>
                    <div class="input-group">
                        <span class="input-group-text"><i class="bi bi-lock"></i></span>
                        <asp:TextBox ID="txtContrasena" runat="server" CssClass="form-control" TextMode="Password" MaxLength="100" placeholder="Contraseña"></asp:TextBox>
                    </div>
                    <asp:RequiredFieldValidator ID="rfvContrasena" runat="server" ControlToValidate="txtContrasena"
                        ErrorMessage="Ingresa tu contraseña." Display="Dynamic" CssClass="text-danger small"
                        ValidationGroup="Login"></asp:RequiredFieldValidator>
                </div>

                <asp:Button ID="btnIngresar" runat="server" Text="Ingresar" CssClass="btn btn-primary w-100 py-2"
                    ValidationGroup="Login" />

            </div>
        </div>
    </form>
</body>
</html>