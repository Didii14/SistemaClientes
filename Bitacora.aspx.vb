Public Class Bitacora
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            CargarUsuarios()
            CargarBitacora()
        End If
    End Sub

    ' Llena la lista desplegable de usuarios
    Private Sub CargarUsuarios()
        Try
            ddlUsuario.Items.Clear()
            ddlUsuario.Items.Add(New ListItem("Todos", "0"))
            For Each u As Usuario In UsuarioDAL.ListarUsuarios()
                ddlUsuario.Items.Add(New ListItem(u.NombreUsuario, u.IDUsuario.ToString()))
            Next
        Catch ex As Exception
            RegistroErrores.Registrar(ex)
            MostrarMensaje("No se pudo cargar la lista de usuarios.")
        End Try
    End Sub

    Private Sub CargarBitacora()
        pnlMensaje.Visible = False

        Dim accion As String = ddlAccion.SelectedValue
        Dim idUsuario As Integer = Convert.ToInt32(ddlUsuario.SelectedValue)
        Dim desde As DateTime? = ParsearFecha(txtDesde.Text)
        Dim hasta As DateTime? = ParsearFecha(txtHasta.Text)

        If desde.HasValue AndAlso hasta.HasValue AndAlso desde.Value > hasta.Value Then
            MostrarMensaje("La fecha inicial no puede ser mayor que la fecha final.")
            gvBitacora.DataSource = New List(Of RegistroBitacora)()
            gvBitacora.DataBind()
            lblTotal.Text = ""
            Return
        End If

        Try
            Dim lista As List(Of RegistroBitacora) = BitacoraDAL.Listar(accion, idUsuario, desde, hasta)
            gvBitacora.DataSource = lista
            gvBitacora.DataBind()
            lblTotal.Text = "Registros encontrados: " & lista.Count.ToString()
        Catch ex As Exception
            RegistroErrores.Registrar(ex)
            MostrarMensaje("No se pudo consultar la bitácora. Intenta de nuevo más tarde.")
        End Try
    End Sub

    ' Convierte el texto de un <input type="date"> (yyyy-MM-dd) en fecha, nothing si esta vacio o es invalido
    Private Function ParsearFecha(ByVal texto As String) As DateTime?
        Dim f As DateTime
        If DateTime.TryParseExact(texto.Trim(), "yyyy-MM-dd",
                                  System.Globalization.CultureInfo.InvariantCulture,
                                  System.Globalization.DateTimeStyles.None, f) Then
            Return f
        End If
        Return Nothing
    End Function

    Protected Sub btnFiltrar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnFiltrar.Click
        gvBitacora.PageIndex = 0
        CargarBitacora()
    End Sub

    Protected Sub btnLimpiar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnLimpiar.Click
        ddlAccion.SelectedIndex = 0
        ddlUsuario.SelectedIndex = 0
        txtDesde.Text = ""
        txtHasta.Text = ""
        gvBitacora.PageIndex = 0
        CargarBitacora()
    End Sub

    Protected Sub gvBitacora_PageIndexChanging(ByVal sender As Object, ByVal e As GridViewPageEventArgs) Handles gvBitacora.PageIndexChanging
        gvBitacora.PageIndex = e.NewPageIndex
        CargarBitacora()
    End Sub

    ' Devuelve la clase de color de Bootstrap segun la accion
    Protected Function ClaseBadge(ByVal accion As String) As String
        Select Case accion
            Case "AGREGAR"
                Return "bg-success"
            Case "EDITAR"
                Return "bg-warning text-dark"
            Case "ELIMINAR"
                Return "bg-danger"
            Case Else
                Return "bg-secondary"
        End Select
    End Function

    Private Sub MostrarMensaje(ByVal mensaje As String)
        litMensaje.Text = Server.HtmlEncode(mensaje)
        pnlMensaje.Visible = True
    End Sub

End Class