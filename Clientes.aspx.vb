Public Class Clientes
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            CargarClientes()
        End If
    End Sub

    ' LISTADO Y BUSQUEDA 

    ' Llena la tabla con los clientes
    Private Sub CargarClientes()
        Try
            gvClientes.DataSource = ClienteDAL.Listar(txtBuscar.Text.Trim())
            gvClientes.DataBind()
        Catch ex As Exception
            RegistroErrores.Registrar(ex)
            MostrarMensaje("No se pudo cargar la lista de clientes. Intenta de nuevo más tarde.", "danger")
        End Try
    End Sub

    Protected Sub btnBuscar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnBuscar.Click
        OcultarMensaje()
        gvClientes.PageIndex = 0
        CargarClientes()
    End Sub

    Protected Sub btnLimpiar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnLimpiar.Click
        OcultarMensaje()
        txtBuscar.Text = ""
        gvClientes.PageIndex = 0
        CargarClientes()
    End Sub

    Protected Sub gvClientes_PageIndexChanging(ByVal sender As Object, ByVal e As GridViewPageEventArgs) Handles gvClientes.PageIndexChanging
        gvClientes.PageIndex = e.NewPageIndex
        CargarClientes()
    End Sub

    '  ACCIONES DE LA TABLA EDITAR / ELIMINAR

    Protected Sub gvClientes_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs) Handles gvClientes.RowCommand
        If e.CommandName <> "Editar" AndAlso e.CommandName <> "Eliminar" Then Return

        OcultarMensaje()
        Dim idCliente As Integer = Convert.ToInt32(e.CommandArgument)

        Try
            If e.CommandName = "Editar" Then
                Dim c As Cliente = ClienteDAL.ObtenerPorId(idCliente)
                If c Is Nothing Then
                    MostrarMensaje("El cliente ya no existe.", "warning")
                    CargarClientes()
                    Return
                End If

                hfIDCliente.Value = c.IDCliente.ToString()
                txtNombre.Text = c.Nombre
                txtApellido.Text = c.Apellido
                txtDUI.Text = c.DUI
                txtTelefono.Text = c.Telefono
                txtCorreo.Text = c.Correo
                txtDireccion.Text = c.Direccion
                lblTituloFormulario.Text = "Editar cliente"
                MostrarFormulario(True)

            ElseIf e.CommandName = "Eliminar" Then
                ' El usuario que realiza la operacion sale de la sesion
                Dim idUsuario As Integer = Autenticacion.ObtenerIDUsuario()
                Dim nombreUsuario As String = Autenticacion.ObtenerNombreUsuario()

                If ClienteDAL.Eliminar(idCliente, idUsuario, nombreUsuario) Then
                    MostrarMensaje("Cliente eliminado correctamente.", "success")
                Else
                    MostrarMensaje("El cliente ya no existe.", "warning")
                End If
                CargarClientes()
            End If

        Catch ex As Exception
            RegistroErrores.Registrar(ex)
            MostrarMensaje("No se pudo completar la operación. Intenta de nuevo más tarde.", "danger")
        End Try
    End Sub

    ' FORMULARIO

    Protected Sub btnNuevo_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnNuevo.Click
        OcultarMensaje()
        LimpiarFormulario()
        lblTituloFormulario.Text = "Nuevo cliente"
        MostrarFormulario(True)
    End Sub

    Protected Sub btnCancelar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancelar.Click
        OcultarMensaje()
        LimpiarFormulario()
        MostrarFormulario(False)
    End Sub

    Protected Sub btnGuardar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnGuardar.Click
        If Not Page.IsValid Then Return

        Dim c As New Cliente()
        c.IDCliente = Convert.ToInt32(hfIDCliente.Value)
        c.Nombre = txtNombre.Text.Trim()
        c.Apellido = txtApellido.Text.Trim()
        c.DUI = txtDUI.Text.Trim()
        c.Telefono = Validaciones.NormalizarTelefono(txtTelefono.Text.Trim())
        c.Correo = txtCorreo.Text.Trim()
        c.Direccion = txtDireccion.Text.Trim()

        ' Validacion propia del servidor
        Dim errores As List(Of String) = Validaciones.ValidarCliente(c)
        If errores.Count > 0 Then
            MostrarErrores(errores)
            Return
        End If

        ' El usuario que realiza la operacion sale de la sesion
        Dim idUsuario As Integer = Autenticacion.ObtenerIDUsuario()
        Dim nombreUsuario As String = Autenticacion.ObtenerNombreUsuario()

        Try
            ' El DUI no puede repetirse
            If ClienteDAL.ExisteDUI(c.DUI, c.IDCliente) Then
                MostrarMensaje("Ya existe otro cliente registrado con ese DUI.", "danger")
                Return
            End If

            If c.IDCliente = 0 Then
                ClienteDAL.Insertar(c, idUsuario, nombreUsuario)
                MostrarMensaje("Cliente agregado correctamente.", "success")
            Else
                If ClienteDAL.Actualizar(c, idUsuario, nombreUsuario) Then
                    MostrarMensaje("Cliente actualizado correctamente.", "success")
                Else
                    MostrarMensaje("El cliente ya no existe.", "warning")
                End If
            End If

            LimpiarFormulario()
            MostrarFormulario(False)
            CargarClientes()

        Catch ex As Exception
            RegistroErrores.Registrar(ex)
            MostrarMensaje("No se pudo guardar el cliente. Intenta de nuevo más tarde.", "danger")
        End Try
    End Sub

    ' METODOS DE APOYO

    Private Sub MostrarFormulario(ByVal mostrar As Boolean)
        pnlFormulario.Visible = mostrar
        pnlLista.Visible = Not mostrar
    End Sub

    Private Sub LimpiarFormulario()
        hfIDCliente.Value = "0"
        txtNombre.Text = ""
        txtApellido.Text = ""
        txtDUI.Text = ""
        txtTelefono.Text = ""
        txtCorreo.Text = ""
        txtDireccion.Text = ""
    End Sub

    Private Sub MostrarMensaje(ByVal mensaje As String, ByVal tipo As String)
        litMensaje.Text = Server.HtmlEncode(mensaje)
        pnlMensaje.CssClass = "alert alert-" & tipo
        pnlMensaje.Visible = True
    End Sub

    ' Muestra varios errores de validacion, uno por linea
    Private Sub MostrarErrores(ByVal errores As List(Of String))
        Dim codificados As New List(Of String)()
        For Each err As String In errores
            codificados.Add(Server.HtmlEncode(err))
        Next
        litMensaje.Text = String.Join("<br />", codificados)
        pnlMensaje.CssClass = "alert alert-danger"
        pnlMensaje.Visible = True
    End Sub

    Private Sub OcultarMensaje()
        pnlMensaje.Visible = False
    End Sub

End Class