
Public Class Global_asax
    Inherits HttpApplication

    Sub Application_Start(sender As Object, e As EventArgs)
        ' Se desencadena al iniciar la aplicación
    End Sub

    ' Se ejecuta cuando ocurre un error que ninguna pagina controlo
    Sub Application_Error(ByVal sender As Object, ByVal e As EventArgs)
        Dim ex As Exception = Server.GetLastError()

        If ex IsNot Nothing Then
            ' Las paginas inexistentes (404) no se registran: serian solo ruido
            Dim httpEx As HttpException = TryCast(ex, HttpException)
            If httpEx Is Nothing OrElse httpEx.GetHttpCode() <> 404 Then
                RegistroErrores.Registrar(ex.GetBaseException())
            End If
        End If
        ' No se limpia el error: Web.config redirige a PaginaError.aspx
    End Sub
End Class