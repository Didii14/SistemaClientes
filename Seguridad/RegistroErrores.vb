Imports System.IO
Imports System.Web

Public Class RegistroErrores

    Private Shared ReadOnly candado As New Object()

    Public Shared Sub Registrar(ByVal ex As Exception)
        Try
            Dim ruta As String = HttpContext.Current.Server.MapPath("~/App_Data/errores.log")

            Dim url As String = ""
            If HttpContext.Current.Request IsNot Nothing Then
                url = HttpContext.Current.Request.RawUrl
            End If

            Dim usuario As String = "sin sesion"
            Try
                If HttpContext.Current.Session IsNot Nothing AndAlso Autenticacion.EstaAutenticado() Then
                    usuario = Autenticacion.ObtenerNombreUsuario()
                End If
            Catch
                ' La sesion puede no estar disponible al registrar ciertos errores
            End Try

            Dim texto As String = "[" & DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") & "] " &
                                  "Usuario: " & usuario & " | URL: " & url & Environment.NewLine &
                                  ex.ToString() & Environment.NewLine &
                                  New String("-"c, 60) & Environment.NewLine

            SyncLock candado
                File.AppendAllText(ruta, texto)
            End SyncLock
        Catch
            ' Si no se puede escribir el log, la aplicacion debe seguir funcionando
        End Try
    End Sub

End Class