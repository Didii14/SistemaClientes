<%@ Page Title="Clientes" Language="vb" MasterPageFile="~/Site.Master" AutoEventWireup="false" CodeBehind="Clientes.aspx.vb" Inherits="SistemaClientes.Clientes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cphContenido" runat="server">

    <div class="d-flex justify-content-between align-items-center mb-3">
        <div>
            <h2 class="mb-0">Mantenimiento de clientes</h2>
            <p class="text-muted mb-0">Agrega, edita, busca y elimina clientes.</p>
        </div>
    </div>

    <!-- Mensajes de exito o error -->
    <asp:Panel ID="pnlMensaje" runat="server" Visible="false" CssClass="alert alert-success" role="alert">
        <asp:Literal ID="litMensaje" runat="server"></asp:Literal>
    </asp:Panel>

    <!-- LISTADO -->
    <asp:Panel ID="pnlLista" runat="server">

        <div class="card tarjeta-dashboard mb-3">
            <div class="card-body">
                <div class="row g-2 align-items-center">
                    <div class="col-md-6">
                        <div class="input-group">
                            <span class="input-group-text"><i class="bi bi-search"></i></span>
                            <asp:TextBox ID="txtBuscar" runat="server" CssClass="form-control"
                                placeholder="Buscar por nombre o DUI" MaxLength="80"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-6 d-flex gap-2 justify-content-md-end">
                        <asp:Button ID="btnBuscar" runat="server" Text="Buscar" CssClass="btn btn-primary" CausesValidation="false" />
                        <asp:Button ID="btnLimpiar" runat="server" Text="Limpiar" CssClass="btn btn-outline-secondary" CausesValidation="false" />
                        <asp:LinkButton ID="btnNuevo" runat="server" CssClass="btn btn-success" CausesValidation="false">
                            <i class="bi bi-plus-circle me-1"></i>Nuevo cliente
                        </asp:LinkButton>
                    </div>
                </div>
            </div>
        </div>

        <div class="card tarjeta-dashboard">
            <div class="card-body">
                <div class="table-responsive">
                    <asp:GridView ID="gvClientes" runat="server" AutoGenerateColumns="False"
                        CssClass="table table-hover align-middle mb-0" GridLines="None"
                        DataKeyNames="IDCliente" AllowPaging="True" PageSize="10"
                        EmptyDataText="No se encontraron clientes.">
                        <HeaderStyle CssClass="table-dark" />
                        <PagerStyle CssClass="text-center pt-3" />
                        <Columns>
                            <asp:BoundField DataField="IDCliente" HeaderText="ID" />
                            <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                            <asp:BoundField DataField="Apellido" HeaderText="Apellido" />
                            <asp:BoundField DataField="DUI" HeaderText="DUI" />
                            <asp:BoundField DataField="Telefono" HeaderText="Teléfono" />
                            <asp:BoundField DataField="Correo" HeaderText="Correo" />
                            <asp:BoundField DataField="Direccion" HeaderText="Dirección" />
                            <asp:BoundField DataField="FechaRegistro" HeaderText="Registro" DataFormatString="{0:dd/MM/yyyy}" HtmlEncode="False" />
                            <asp:TemplateField HeaderText="Acciones">
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnEditar" runat="server" CommandName="Editar"
                                        CommandArgument='<%# Eval("IDCliente") %>'
                                        CssClass="btn btn-sm btn-outline-primary me-1" CausesValidation="false" ToolTip="Editar">
                                        <i class="bi bi-pencil-square"></i>
                                    </asp:LinkButton>
                                    <asp:LinkButton ID="btnEliminar" runat="server" CommandName="Eliminar"
                                        CommandArgument='<%# Eval("IDCliente") %>'
                                        CssClass="btn btn-sm btn-outline-danger" CausesValidation="false" ToolTip="Eliminar"
                                        OnClientClick="return confirm('¿Seguro que deseas eliminar este cliente?');">
                                        <i class="bi bi-trash"></i>
                                    </asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </div>

    </asp:Panel>

    <!-- ============ FORMULARIO (agregar / editar) ============ -->
    <asp:Panel ID="pnlFormulario" runat="server" Visible="false">

        <div class="card tarjeta-dashboard">
            <div class="card-header bg-white">
                <h5 class="mb-0"><asp:Label ID="lblTituloFormulario" runat="server" Text="Nuevo cliente"></asp:Label></h5>
            </div>
            <div class="card-body">

                <asp:HiddenField ID="hfIDCliente" runat="server" Value="0" />

                <div class="row g-3">
                    <div class="col-md-6">
                        <label class="form-label">Nombre</label>
                        <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control" MaxLength="80"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvNombre" runat="server" ControlToValidate="txtNombre"
                            ErrorMessage="El nombre es obligatorio." Display="Dynamic" CssClass="text-danger small"
                            ValidationGroup="Cliente"></asp:RequiredFieldValidator>
                    </div>

                    <div class="col-md-6">
                        <label class="form-label">Apellido</label>
                        <asp:TextBox ID="txtApellido" runat="server" CssClass="form-control" MaxLength="80"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvApellido" runat="server" ControlToValidate="txtApellido"
                            ErrorMessage="El apellido es obligatorio." Display="Dynamic" CssClass="text-danger small"
                            ValidationGroup="Cliente"></asp:RequiredFieldValidator>
                    </div>

                    <div class="col-md-4">
                        <label class="form-label">DUI</label>
                        <asp:TextBox ID="txtDUI" runat="server" CssClass="form-control" MaxLength="10" placeholder="00000000-0"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvDUI" runat="server" ControlToValidate="txtDUI"
                            ErrorMessage="El DUI es obligatorio." Display="Dynamic" CssClass="text-danger small"
                            ValidationGroup="Cliente"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="revDUI" runat="server" ControlToValidate="txtDUI"
                            ValidationExpression="^[0-9]{8}-[0-9]$"
                            ErrorMessage="Formato de DUI: 00000000-0." Display="Dynamic" CssClass="text-danger small"
                            ValidationGroup="Cliente"></asp:RegularExpressionValidator>
                    </div>

                    <div class="col-md-4">
                        <label class="form-label">Teléfono</label>
                        <asp:TextBox ID="txtTelefono" runat="server" CssClass="form-control" MaxLength="9" placeholder="7777-8888"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvTelefono" runat="server" ControlToValidate="txtTelefono"
                            ErrorMessage="El teléfono es obligatorio." Display="Dynamic" CssClass="text-danger small"
                            ValidationGroup="Cliente"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="revTelefono" runat="server" ControlToValidate="txtTelefono"
                            ValidationExpression="^[0-9]{4}-?[0-9]{4}$"
                            ErrorMessage="Debe tener 8 dígitos (ej. 7777-8888)." Display="Dynamic" CssClass="text-danger small"
                            ValidationGroup="Cliente"></asp:RegularExpressionValidator>
                    </div>

                    <div class="col-md-4">
                        <label class="form-label">Correo electrónico</label>
                        <asp:TextBox ID="txtCorreo" runat="server" CssClass="form-control" MaxLength="100" placeholder="nombre@correo.com"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvCorreo" runat="server" ControlToValidate="txtCorreo"
                            ErrorMessage="El correo es obligatorio." Display="Dynamic" CssClass="text-danger small"
                            ValidationGroup="Cliente"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="revCorreo" runat="server" ControlToValidate="txtCorreo"
                            ValidationExpression="^[^@\s]+@[^@\s]+\.[^@\s]+$"
                            ErrorMessage="Correo no válido." Display="Dynamic" CssClass="text-danger small"
                            ValidationGroup="Cliente"></asp:RegularExpressionValidator>
                    </div>

                    <div class="col-12">
                        <label class="form-label">Dirección</label>
                        <asp:TextBox ID="txtDireccion" runat="server" CssClass="form-control" MaxLength="200"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvDireccion" runat="server" ControlToValidate="txtDireccion"
                            ErrorMessage="La dirección es obligatoria." Display="Dynamic" CssClass="text-danger small"
                            ValidationGroup="Cliente"></asp:RequiredFieldValidator>
                    </div>
                </div>

                <div class="mt-4 d-flex gap-2">
                    <asp:LinkButton ID="btnGuardar" runat="server" CssClass="btn btn-primary" ValidationGroup="Cliente">
                        <i class="bi bi-check-circle me-1"></i>Guardar
                    </asp:LinkButton>
                    <asp:LinkButton ID="btnCancelar" runat="server" CssClass="btn btn-outline-secondary" CausesValidation="false">
                        <i class="bi bi-x-circle me-1"></i>Cancelar
                    </asp:LinkButton>
                </div>

            </div>
        </div>

    </asp:Panel>

</asp:Content>