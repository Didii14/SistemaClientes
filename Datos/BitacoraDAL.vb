Imports System.Data.SqlClient

Public Class BitacoraDAL

    ' Registra una operacion en la bitacora. recibe la conexion y la transaccion abiertas por quien la llama (ClienteDAL),
    ' para que el cambio en Clientes y el registro en Bitacora se guarden juntos o no se guarden
    Public Shared Sub Registrar(ByVal cn As SqlConnection,
                                ByVal tx As SqlTransaction,
                                ByVal accion As String,
                                ByVal idCliente As Integer,
                                ByVal idUsuario As Integer,
                                ByVal nombreUsuario As String,
                                ByVal descripcion As String)

        Dim texto As String = If(descripcion, "")
        If texto.Length > 500 Then
            texto = texto.Substring(0, 497) & "..."
        End If

        Dim sql As String = "INSERT INTO dbo.Bitacora (Accion, IDCliente, IDUsuario, NombreUsuario, Descripcion) " &
                            "VALUES (@accion, @idCliente, @idUsuario, @nombreUsuario, @descripcion)"

        Using cmd As New SqlCommand(sql, cn, tx)
            cmd.Parameters.AddWithValue("@accion", accion)
            cmd.Parameters.AddWithValue("@idCliente", idCliente)
            cmd.Parameters.AddWithValue("@idUsuario", idUsuario)
            cmd.Parameters.AddWithValue("@nombreUsuario", nombreUsuario)
            cmd.Parameters.AddWithValue("@descripcion", texto)
            cmd.ExecuteNonQuery()
        End Using
    End Sub

    ' Consulta la bitacora aplicando filtros opcionales
    Public Shared Function Listar(ByVal accion As String,
                                  ByVal idUsuario As Integer,
                                  ByVal fechaDesde As DateTime?,
                                  ByVal fechaHasta As DateTime?) As List(Of RegistroBitacora)

        Dim lista As New List(Of RegistroBitacora)()

        Dim sql As String = "SELECT IDBitacora, Accion, IDCliente, FechaHora, IDUsuario, NombreUsuario, Descripcion " &
                            "FROM dbo.Bitacora WHERE 1 = 1"

        If accion <> "" Then sql &= " AND Accion = @accion"
        If idUsuario > 0 Then sql &= " AND IDUsuario = @idUsuario"
        If fechaDesde.HasValue Then sql &= " AND FechaHora >= @desde"
        If fechaHasta.HasValue Then sql &= " AND FechaHora < @hasta"

        sql &= " ORDER BY FechaHora DESC, IDBitacora DESC"

        Using cn As SqlConnection = Conexion.ObtenerConexion()
            cn.Open()
            Using cmd As New SqlCommand(sql, cn)
                If accion <> "" Then cmd.Parameters.AddWithValue("@accion", accion)
                If idUsuario > 0 Then cmd.Parameters.AddWithValue("@idUsuario", idUsuario)
                If fechaDesde.HasValue Then cmd.Parameters.AddWithValue("@desde", fechaDesde.Value.Date)
                ' Se suma un dia para que "hasta" incluya todo el dia seleccionado
                If fechaHasta.HasValue Then cmd.Parameters.AddWithValue("@hasta", fechaHasta.Value.Date.AddDays(1))

                Using dr As SqlDataReader = cmd.ExecuteReader()
                    While dr.Read()
                        Dim r As New RegistroBitacora()
                        r.IDBitacora = Convert.ToInt32(dr("IDBitacora"))
                        r.Accion = dr("Accion").ToString()
                        r.IDCliente = Convert.ToInt32(dr("IDCliente"))
                        r.FechaHora = Convert.ToDateTime(dr("FechaHora"))
                        r.IDUsuario = Convert.ToInt32(dr("IDUsuario"))
                        r.NombreUsuario = dr("NombreUsuario").ToString()
                        r.Descripcion = If(IsDBNull(dr("Descripcion")), "", dr("Descripcion").ToString())
                        lista.Add(r)
                    End While
                End Using
            End Using
        End Using

        Return lista
    End Function

    ' Cuenta todos los registros de la bitacora
    Public Shared Function Contar() As Integer
        Using cn As SqlConnection = Conexion.ObtenerConexion()
            cn.Open()
            Using cmd As New SqlCommand("SELECT COUNT(*) FROM dbo.Bitacora", cn)
                Return Convert.ToInt32(cmd.ExecuteScalar())
            End Using
        End Using
    End Function

End Class