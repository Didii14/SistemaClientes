Imports System.Data.SqlClient

Public Class UsuarioDAL

    ' Valida las credenciales, devuelve el usuario si son correctas, o nothing si no
    Public Shared Function ValidarLogin(ByVal nombreUsuario As String, ByVal contrasena As String) As Usuario
        Dim sql As String = "SELECT IDUsuario, NombreUsuario, NombreCompleto, PasswordHash, Salt " &
                            "FROM dbo.Usuarios " &
                            "WHERE NombreUsuario = @usuario AND Activo = 1"

        Using cn As SqlConnection = Conexion.ObtenerConexion()
            cn.Open()
            Using cmd As New SqlCommand(sql, cn)
                cmd.Parameters.AddWithValue("@usuario", nombreUsuario)

                Using dr As SqlDataReader = cmd.ExecuteReader()
                    If dr.Read() Then
                        Dim hashGuardado As String = dr("PasswordHash").ToString()
                        Dim salt As String = dr("Salt").ToString()
                        Dim hashCalculado As String = Autenticacion.CalcularHash(contrasena, salt)

                        If String.Equals(hashGuardado, hashCalculado, StringComparison.OrdinalIgnoreCase) Then
                            Dim u As New Usuario()
                            u.IDUsuario = Convert.ToInt32(dr("IDUsuario"))
                            u.NombreUsuario = dr("NombreUsuario").ToString()
                            u.NombreCompleto = dr("NombreCompleto").ToString()
                            u.Activo = True
                            Return u
                        End If
                    End If
                End Using
            End Using
        End Using

        Return Nothing
    End Function

    ' Lista todos los usuarios
    Public Shared Function ListarUsuarios() As List(Of Usuario)
        Dim lista As New List(Of Usuario)()
        Dim sql As String = "SELECT IDUsuario, NombreUsuario, NombreCompleto, Activo " &
                            "FROM dbo.Usuarios ORDER BY NombreUsuario"

        Using cn As SqlConnection = Conexion.ObtenerConexion()
            cn.Open()
            Using cmd As New SqlCommand(sql, cn)
                Using dr As SqlDataReader = cmd.ExecuteReader()
                    While dr.Read()
                        Dim u As New Usuario()
                        u.IDUsuario = Convert.ToInt32(dr("IDUsuario"))
                        u.NombreUsuario = dr("NombreUsuario").ToString()
                        u.NombreCompleto = dr("NombreCompleto").ToString()
                        u.Activo = Convert.ToBoolean(dr("Activo"))
                        lista.Add(u)
                    End While
                End Using
            End Using
        End Using

        Return lista
    End Function

End Class