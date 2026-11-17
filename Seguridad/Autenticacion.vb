Imports System.Security.Cryptography
Imports System.Text
Imports System.Web

Public Class Autenticacion

    ' Calcula SHA-256 de (Salt + Contrasena) y lo devuelve en hexadecimal MAYUSCULA (64 caractees)
    Public Shared Function CalcularHash(ByVal contrasena As String, ByVal salt As String) As String
        Using sha As SHA256 = SHA256.Create()
            Dim bytesEntrada As Byte() = Encoding.ASCII.GetBytes(salt & contrasena)
            Dim bytesHash As Byte() = sha.ComputeHash(bytesEntrada)

            Dim sb As New StringBuilder()
            For Each b As Byte In bytesHash
                sb.Append(b.ToString("X2"))
            Next
            Return sb.ToString()
        End Using
    End Function

    ' Genera un salt aleatorio de 32 caractere
    Public Shared Function GenerarSalt() As String
        Dim bytes(15) As Byte
        Using rng As RandomNumberGenerator = RandomNumberGenerator.Create()
            rng.GetBytes(bytes)
        End Using

        Dim sb As New StringBuilder()
        For Each b As Byte In bytes
            sb.Append(b.ToString("X2"))
        Next
        Return sb.ToString()
    End Function

    ' Guarda al usuario en la sesion despues de un login correcto
    Public Shared Sub IniciarSesion(ByVal u As Usuario)
        HttpContext.Current.Session("IDUsuario") = u.IDUsuario
        HttpContext.Current.Session("NombreUsuario") = u.NombreUsuario
        HttpContext.Current.Session("NombreCompleto") = u.NombreCompleto
    End Sub

    ' Cierra la sesion por completo y elimina la cookie de sesion del navegador
    Public Shared Sub CerrarSesion()
        HttpContext.Current.Session.Clear()
        HttpContext.Current.Session.Abandon()

        Dim cookie As New HttpCookie("ASP.NET_SessionId", "")
        cookie.Expires = DateTime.Now.AddYears(-1)
        cookie.HttpOnly = True
        HttpContext.Current.Response.Cookies.Add(cookie)
    End Sub

    Public Shared Function EstaAutenticado() As Boolean
        Return HttpContext.Current.Session("IDUsuario") IsNot Nothing
    End Function

    ' Devuelve el ID del usuario autenticado (0 si no hay sesion)
    Public Shared Function ObtenerIDUsuario() As Integer
        If HttpContext.Current.Session("IDUsuario") Is Nothing Then Return 0
        Return Convert.ToInt32(HttpContext.Current.Session("IDUsuario"))
    End Function

    ' Devuelve el nombre de usuario autenticado (vacio si no hay sesion)
    Public Shared Function ObtenerNombreUsuario() As String
        If HttpContext.Current.Session("NombreUsuario") Is Nothing Then Return ""
        Return HttpContext.Current.Session("NombreUsuario").ToString()
    End Function

    ' Devuelve el nombre completo del usuario autenticado (vacio si no hay sesion)
    Public Shared Function ObtenerNombreCompleto() As String
        If HttpContext.Current.Session("NombreCompleto") Is Nothing Then Return ""
        Return HttpContext.Current.Session("NombreCompleto").ToString()
    End Function

End Class