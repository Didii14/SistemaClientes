<%@ Page Title="Bitácora" Language="vb" MasterPageFile="~/Site.Master" AutoEventWireup="false" CodeBehind="Bitacora.aspx.vb" Inherits="SistemaClientes.Bitacora" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cphContenido" runat="server">

    <div class="mb-3">
        <h2 class="mb-0">Bitácora de operaciones</h2>
        <p class="text-muted mb-0">Historial de altas, ediciones y eliminaciones de clientes, y quién las realizó.</p>
    </div>

    <!-- Mensajes -->
    <asp:Panel ID="pnlMensaje" runat="server" Visible="false" CssClass="alert alert-warning" role="alert">
        <asp:Literal ID="litMensaje" runat="server"></asp:Literal>
    </asp:Panel>

    <!-- Filtros -->
    <div class="card tarjeta-dashboard mb-3">
        <div class="card-body">
            <div class="row g-3 align-items-end">
                <div class="col-md-3">
                    <label class="form-label">Acción</label>
                    <asp:DropDownList ID="ddlAccion" runat="server" CssClass="form-select">
                        <asp:ListItem Value="" Text="Todas"></asp:ListItem>
                        <asp:ListItem Value="AGREGAR" Text="Agregar"></asp:ListItem>
                        <asp:ListItem Value="EDITAR" Text="Editar"></asp:ListItem>
                        <asp:ListItem Value="ELIMINAR" Text="Eliminar"></asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="col-md-3">
                    <label class="form-label">Usuario</label>
                    <asp:DropDownList ID="ddlUsuario" runat="server" CssClass="form-select"></asp:DropDownList>
                </div>
                <div class="col-md-2">
                    <label class="form-label">Desde</label>
                    <asp:TextBox ID="txtDesde" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                </div>
                <div class="col-md-2">
                    <label class="form-label">Hasta</label>
                    <asp:TextBox ID="txtHasta" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                </div>
                <div class="col-md-2 d-flex gap-2">
                    <asp:LinkButton ID="btnFiltrar" runat="server" CssClass="btn btn-primary" CausesValidation="false" ToolTip="Buscar">
                        <i class="bi bi-search"></i>
                    </asp:LinkButton>
                    <asp:LinkButton ID="btnLimpiar" runat="server" CssClass="btn btn-outline-secondary" CausesValidation="false" ToolTip="Limpiar filtros">
                        <i class="bi bi-eraser"></i>
                    </asp:LinkButton>
                </div>
            </div>
        </div>
    </div>

    <!-- Resultados -->
    <div class="card tarjeta-dashboard">
        <div class="card-body">
            <div class="text-muted small mb-2">
                <asp:Label ID="lblTotal" runat="server"></asp:Label>
            </div>
            <div class="table-responsive">
                <asp:GridView ID="gvBitacora" runat="server" AutoGenerateColumns="False"
                    CssClass="table table-hover align-middle mb-0" GridLines="None"
                    AllowPaging="True" PageSize="15"
                    EmptyDataText="No se encontraron registros en la bitácora.">
                    <HeaderStyle CssClass="table-dark" />
                    <PagerStyle CssClass="text-center pt-3" />
                    <Columns>
                        <asp:BoundField DataField="IDBitacora" HeaderText="ID" />
                        <asp:BoundField DataField="FechaHora" HeaderText="Fecha y hora" DataFormatString="{0:dd/MM/yyyy HH:mm:ss}" HtmlEncode="False" />
                        <asp:TemplateField HeaderText="Acción">
                            <ItemTemplate>
                                <span class="badge <%# ClaseBadge(Eval("Accion").ToString()) %>"><%# Server.HtmlEncode(Eval("Accion").ToString()) %></span>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="IDCliente" HeaderText="ID cliente" />
                        <asp:BoundField DataField="NombreUsuario" HeaderText="Usuario" />
                        <asp:BoundField DataField="Descripcion" HeaderText="Descripción" />
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </div>

</asp:Content>