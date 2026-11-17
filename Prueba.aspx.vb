Imports System.Data.SqlClient

Public Class Prueba
    Inherits System.Web.UI.Page

    ' Prueba temporal: usa la clase Conexion y un modelo
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Try
                Dim u As New Usuario()

                Using cn As SqlConnection = Conexion.ObtenerConexion()
                    cn.Open()
                    Using cmd As New SqlCommand("SELECT IDUsuario, NombreUsuario, NombreCompleto, Activo FROM dbo.Usuarios WHERE NombreUsuario = @usuario", cn)
                        cmd.Parameters.AddWithValue("@usuario", "admin")
                        Using dr As SqlDataReader = cmd.ExecuteReader()
                            If dr.Read() Then
                                u.IDUsuario = Convert.ToInt32(dr("IDUsuario"))
                                u.NombreUsuario = dr("NombreUsuario").ToString()
                                u.NombreCompleto = dr("NombreCompleto").ToString()
                                u.Activo = Convert.ToBoolean(dr("Activo"))
                            End If
                        End Using
                    End Using
                End Using

                lblMensaje.Text = "Clase Conexion y modelo Usuario OK. Usuario leido: " & u.NombreUsuario & " (" & u.NombreCompleto & ")"
            Catch ex As Exception
                RegistroErrores.Registrar(ex)
                lblMensaje.Text = "Error: " & ex.Message
            End Try
        End If
    End Sub

End Class