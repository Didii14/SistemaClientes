<%@ Page Title="Inicio" Language="vb" MasterPageFile="~/Site.Master" AutoEventWireup="false" CodeBehind="Default.aspx.vb" Inherits="SistemaClientes._Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cphContenido" runat="server">

    <div class="mb-4">
        <h2 class="mb-0">Bienvenido, <asp:Label ID="lblNombre" runat="server"></asp:Label></h2>
        <p class="text-muted">Panel principal del sistema de gestión de clientes.</p>
    </div>

    <div class="row g-4">
        <!-- Total de clientes -->
        <div class="col-md-4">
            <div class="card tarjeta-dashboard h-100">
                <div class="card-body d-flex align-items-center">
                    <i class="bi bi-people tarjeta-icono me-3"></i>
                    <div>
                        <div class="text-muted small">Clientes registrados</div>
                        <div class="fs-3 fw-bold"><asp:Label ID="lblTotalClientes" runat="server" Text="0"></asp:Label></div>
                    </div>
                </div>
            </div>
        </div>

        <!-- Operaciones en bitacora -->
        <div class="col-md-4">
            <div class="card tarjeta-dashboard h-100">
                <div class="card-body d-flex align-items-center">
                    <i class="bi bi-journal-check tarjeta-icono me-3"></i>
                    <div>
                        <div class="text-muted small">Operaciones en bitácora</div>
                        <div class="fs-3 fw-bold"><asp:Label ID="lblTotalBitacora" runat="server" Text="0"></asp:Label></div>
                    </div>
                </div>
            </div>
        </div>

        <!-- Usuario actual -->
        <div class="col-md-4">
            <div class="card tarjeta-dashboard h-100">
                <div class="card-body d-flex align-items-center">
                    <i class="bi bi-person-badge tarjeta-icono me-3"></i>
                    <div>
                        <div class="text-muted small">Usuario activo</div>
                        <div class="fs-5 fw-bold"><asp:Label ID="lblUsuarioActivo" runat="server"></asp:Label></div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <!-- Accesos rapidos -->
    <h5 class="mt-5 mb-3">Accesos rápidos</h5>
    <div class="row g-4">
        <div class="col-md-6">
            <a href="Clientes.aspx" class="text-decoration-none">
                <div class="card tarjeta-dashboard">
                    <div class="card-body text-center py-4">
                        <i class="bi bi-person-lines-fill tarjeta-icono"></i>
                        <h5 class="mt-2 text-dark">Mantenimiento de clientes</h5>
                        <p class="text-muted mb-0">Agregar, editar, buscar y eliminar clientes.</p>
                    </div>
                </div>
            </a>
        </div>
        <div class="col-md-6">
            <a href="Bitacora.aspx" class="text-decoration-none">
                <div class="card tarjeta-dashboard">
                    <div class="card-body text-center py-4">
                        <i class="bi bi-journal-text tarjeta-icono"></i>
                        <h5 class="mt-2 text-dark">Bitácora</h5>
                        <p class="text-muted mb-0">Consulta el historial de operaciones y quién las realizó.</p>
                    </div>
                </div>
            </a>
        </div>
    </div>

</asp:Content>