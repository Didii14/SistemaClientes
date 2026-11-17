Public Class Site
    Inherits System.Web.UI.MasterPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Response.Cache.SetCacheability(HttpCacheability.NoCache)
        Response.Cache.SetNoStore()
        Response.Cache.SetExpires(DateTime.UtcNow.AddDays(-1))

        If Not Autenticacion.EstaAutenticado() Then
            Response.Redirect("~/Login.aspx")
            Return
        End If

        If Not IsPostBack Then
            lblUsuario.Text = Server.HtmlEncode(Autenticacion.ObtenerNombreCompleto())
            MarcarMenuActivo()
        End If
    End Sub

    ' Resalta en el menu la pagina que esta viendo
    Private Sub MarcarMenuActivo()
        Dim pagina As String = System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToLower()

        Select Case pagina
            Case "default.aspx"
                lnkInicio.Attributes("class") = "nav-link active"
            Case "clientes.aspx"
                lnkClientes.Attributes("class") = "nav-link active"
            Case "bitacora.aspx"
                lnkBitacora.Attributes("class") = "nav-link active"
        End Select
    End Sub

    Protected Sub btnCerrarSesion_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCerrarSesion.Click
        Autenticacion.CerrarSesion()
        Response.Redirect("~/Login.aspx")
    End Sub

End Class