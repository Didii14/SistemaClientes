<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="PaginaError.aspx.vb" Inherits="SistemaClientes.PaginaError" %>

<!DOCTYPE html>
<html lang="es">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>Error | Sistema de Clientes</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.11.3/font/bootstrap-icons.css" rel="stylesheet" />
    <link href="Content/css/estilos.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server" class="login-fondo">
        <div class="card login-tarjeta">
            <div class="card-body p-4 p-md-5 text-center">
                <i class="bi bi-exclamation-triangle login-icono"></i>
                <h4 class="mt-3">
                    <asp:Literal ID="litTitulo" runat="server"></asp:Literal>
                </h4>
                <p class="text-muted">
                    <asp:Literal ID="litMensaje" runat="server"></asp:Literal>
                </p>
                <a href="Default.aspx" class="btn btn-primary">
                    <i class="bi bi-house me-1"></i>Volver al inicio
                </a>
            </div>
        </div>
    </form>
</body>
</html>