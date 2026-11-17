Public Class _Default
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            lblNombre.Text = Server.HtmlEncode(Autenticacion.ObtenerNombreCompleto())
            lblUsuarioActivo.Text = Server.HtmlEncode(Autenticacion.ObtenerNombreUsuario())

            Try
                lblTotalClientes.Text = ClienteDAL.Contar().ToString()
                lblTotalBitacora.Text = BitacoraDAL.Contar().ToString()
            Catch ex As Exception
                RegistroErrores.Registrar(ex)
                ' Si la base de datos no responde, el dashboard sigue cargando
                lblTotalClientes.Text = "N/D"
                lblTotalBitacora.Text = "N/D"
            End Try
        End If
    End Sub

End Class