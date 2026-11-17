Public Class PaginaError
    Inherits System.Web.UI.Page

    ' Muestra un mensaje generico segun el codigo, nunca muestra detalles tecnicos
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Select Case Request.QueryString("c")
                Case "404"
                    litTitulo.Text = "Página no encontrada"
                    litMensaje.Text = "La página que buscas no existe o fue movida."
                Case "403"
                    litTitulo.Text = "Acceso denegado"
                    litMensaje.Text = "No tienes permiso para ver este recurso."
                Case Else
                    litTitulo.Text = "Ocurrió un problema"
                    litMensaje.Text = "No pudimos completar tu solicitud. Intenta de nuevo y, si el problema continúa, contacta al administrador."
            End Select
        End If
    End Sub

End Class