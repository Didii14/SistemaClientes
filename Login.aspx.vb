Public Class Login
    Inherits System.Web.UI.Page

    ' Si ya hay sesion activa, va directo al dashboard
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If Autenticacion.EstaAutenticado() Then
                Response.Redirect("Default.aspx")
            End If
        End If
    End Sub

    ' valida los datos y las credenciales
    Protected Sub btnIngresar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnIngresar.Click
        ' Validacion en el servidor por si el navegador omite la validacion del cliente
        Dim usuario As String = txtUsuario.Text.Trim()
        Dim contrasena As String = txtContrasena.Text

        If usuario = "" OrElse contrasena = "" Then
            MostrarError("Ingresa tu usuario y contraseña.")
            Return
        End If

        Try
            Dim u As Usuario = UsuarioDAL.ValidarLogin(usuario, contrasena)

            If u Is Nothing Then
                'no revela si el usuario existe o no
                MostrarError("Usuario o contraseña incorrectos.")
            Else
                Autenticacion.IniciarSesion(u)
                Response.Redirect("Default.aspx", False)
                Context.ApplicationInstance.CompleteRequest()
            End If

        Catch ex As Exception
            RegistroErrores.Registrar(ex)
            ' Ocultar detalles tecnicos al usuario
            MostrarError("No se pudo iniciar sesión. Intenta de nuevo más tarde.")
        End Try
    End Sub

    Private Sub MostrarError(ByVal mensaje As String)
        lblError.Text = mensaje
        pnlError.Visible = True
        txtContrasena.Text = ""
    End Sub

End Class