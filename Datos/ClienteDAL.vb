Imports System.Data.SqlClient
Imports System.Text

Public Class ClienteDAL

    ' METODOS DE APOYO 

    Private Shared Function MapearCliente(ByVal dr As SqlDataReader) As Cliente
        Dim c As New Cliente()
        c.IDCliente = Convert.ToInt32(dr("IDCliente"))
        c.Nombre = dr("Nombre").ToString()
        c.Apellido = dr("Apellido").ToString()
        c.DUI = dr("DUI").ToString()
        c.Telefono = dr("Telefono").ToString()
        c.Correo = dr("Correo").ToString()
        c.Direccion = dr("Direccion").ToString()
        c.FechaRegistro = Convert.ToDateTime(dr("FechaRegistro"))
        Return c
    End Function

    Private Shared Function EscaparLike(ByVal texto As String) As String
        Return texto.Replace("[", "[[]").Replace("%", "[%]").Replace("_", "[_]")
    End Function

    ' Agrega al texto un cambio "Campo: 'antes' -> 'despues'" solo si el valor cambio
    Private Shared Sub AgregarCambio(ByVal sb As StringBuilder, ByVal campo As String,
                                     ByVal antes As String, ByVal despues As String)
        If Not String.Equals(antes, despues, StringComparison.Ordinal) Then
            If sb.Length > 0 Then sb.Append("; ")
            sb.Append(campo).Append(": '").Append(antes).Append("' -> '").Append(despues).Append("'")
        End If
    End Sub

    ' Compara el cliente anterior con el nuevo y describe que cambio
    Private Shared Function DescribirCambios(ByVal anterior As Cliente, ByVal nuevo As Cliente) As String
        Dim sb As New StringBuilder()
        AgregarCambio(sb, "Nombre", anterior.Nombre, nuevo.Nombre)
        AgregarCambio(sb, "Apellido", anterior.Apellido, nuevo.Apellido)
        AgregarCambio(sb, "DUI", anterior.DUI, nuevo.DUI)
        AgregarCambio(sb, "Teléfono", anterior.Telefono, nuevo.Telefono)
        AgregarCambio(sb, "Correo", anterior.Correo, nuevo.Correo)
        AgregarCambio(sb, "Dirección", anterior.Direccion, nuevo.Direccion)
        Return sb.ToString()
    End Function

    ' CONSULTAS

    ' Lista todos los clientes, si hay filtro, busca por nombre, apellido, nombre completo o DUI
    Public Shared Function Listar(ByVal filtro As String) As List(Of Cliente)
        Dim lista As New List(Of Cliente)()

        Dim sql As String = "SELECT IDCliente, Nombre, Apellido, DUI, Telefono, Correo, Direccion, FechaRegistro " &
                            "FROM dbo.Clientes"
        If filtro <> "" Then
            sql &= " WHERE Nombre LIKE @filtro OR Apellido LIKE @filtro " &
                   "OR (Nombre + ' ' + Apellido) LIKE @filtro OR DUI LIKE @filtro"
        End If
        sql &= " ORDER BY IDCliente DESC"

        Using cn As SqlConnection = Conexion.ObtenerConexion()
            cn.Open()
            Using cmd As New SqlCommand(sql, cn)
                If filtro <> "" Then
                    cmd.Parameters.AddWithValue("@filtro", "%" & EscaparLike(filtro) & "%")
                End If
                Using dr As SqlDataReader = cmd.ExecuteReader()
                    While dr.Read()
                        lista.Add(MapearCliente(dr))
                    End While
                End Using
            End Using
        End Using

        Return lista
    End Function

    ' Obtiene un cliente por su ID nothing si no existe
    Public Shared Function ObtenerPorId(ByVal idCliente As Integer) As Cliente
        Using cn As SqlConnection = Conexion.ObtenerConexion()
            cn.Open()
            Return ObtenerPorId(cn, Nothing, idCliente)
        End Using
    End Function

    ' Version interna usa una conexion  ya abierta
    Private Shared Function ObtenerPorId(ByVal cn As SqlConnection, ByVal tx As SqlTransaction,
                                         ByVal idCliente As Integer) As Cliente
        Dim sql As String = "SELECT IDCliente, Nombre, Apellido, DUI, Telefono, Correo, Direccion, FechaRegistro " &
                            "FROM dbo.Clientes WHERE IDCliente = @id"

        Using cmd As New SqlCommand(sql, cn, tx)
            cmd.Parameters.AddWithValue("@id", idCliente)
            Using dr As SqlDataReader = cmd.ExecuteReader()
                If dr.Read() Then
                    Return MapearCliente(dr)
                End If
            End Using
        End Using

        Return Nothing
    End Function

    ' Indica si ya existe otro cliente con ese DUI.
    Public Shared Function ExisteDUI(ByVal dui As String, ByVal idExcluir As Integer) As Boolean
        Dim sql As String = "SELECT COUNT(*) FROM dbo.Clientes WHERE DUI = @dui AND IDCliente <> @id"

        Using cn As SqlConnection = Conexion.ObtenerConexion()
            cn.Open()
            Using cmd As New SqlCommand(sql, cn)
                cmd.Parameters.AddWithValue("@dui", dui)
                cmd.Parameters.AddWithValue("@id", idExcluir)
                Return Convert.ToInt32(cmd.ExecuteScalar()) > 0
            End Using
        End Using
    End Function

    ' OPERACIONES CON BITACORA 

    ' Inserta un cliente, registra agregar en la bitacora y devuelve el ID generado
    Public Shared Function Insertar(ByVal c As Cliente, ByVal idUsuario As Integer,
                                    ByVal nombreUsuario As String) As Integer
        Dim nuevoId As Integer = 0

        Dim sql As String = "INSERT INTO dbo.Clientes (Nombre, Apellido, DUI, Telefono, Correo, Direccion) " &
                            "OUTPUT INSERTED.IDCliente " &
                            "VALUES (@nombre, @apellido, @dui, @telefono, @correo, @direccion)"

        Using cn As SqlConnection = Conexion.ObtenerConexion()
            cn.Open()
            Using tx As SqlTransaction = cn.BeginTransaction()
                Try
                    Using cmd As New SqlCommand(sql, cn, tx)
                        cmd.Parameters.AddWithValue("@nombre", c.Nombre)
                        cmd.Parameters.AddWithValue("@apellido", c.Apellido)
                        cmd.Parameters.AddWithValue("@dui", c.DUI)
                        cmd.Parameters.AddWithValue("@telefono", c.Telefono)
                        cmd.Parameters.AddWithValue("@correo", c.Correo)
                        cmd.Parameters.AddWithValue("@direccion", c.Direccion)
                        nuevoId = Convert.ToInt32(cmd.ExecuteScalar())
                    End Using

                    Dim descripcion As String = "Se agregó el cliente " & c.Nombre & " " & c.Apellido &
                                                " (DUI " & c.DUI & ")."
                    BitacoraDAL.Registrar(cn, tx, "AGREGAR", nuevoId, idUsuario, nombreUsuario, descripcion)

                    tx.Commit()
                Catch
                    tx.Rollback()
                    Throw
                End Try
            End Using
        End Using

        Return nuevoId
    End Function

    ' Actualiza un cliente y registra editar en la bitacora, devuelve truw si se modifico
    Public Shared Function Actualizar(ByVal c As Cliente, ByVal idUsuario As Integer,
                                      ByVal nombreUsuario As String) As Boolean
        Dim resultado As Boolean = False

        Dim sql As String = "UPDATE dbo.Clientes SET Nombre = @nombre, Apellido = @apellido, DUI = @dui, " &
                            "Telefono = @telefono, Correo = @correo, Direccion = @direccion " &
                            "WHERE IDCliente = @id"

        Using cn As SqlConnection = Conexion.ObtenerConexion()
            cn.Open()
            Using tx As SqlTransaction = cn.BeginTransaction()
                Try
                    ' Se lee el cliente ANTES del cambio para describir que se modifico
                    Dim anterior As Cliente = ObtenerPorId(cn, tx, c.IDCliente)

                    If anterior IsNot Nothing Then
                        Dim filas As Integer
                        Using cmd As New SqlCommand(sql, cn, tx)
                            cmd.Parameters.AddWithValue("@nombre", c.Nombre)
                            cmd.Parameters.AddWithValue("@apellido", c.Apellido)
                            cmd.Parameters.AddWithValue("@dui", c.DUI)
                            cmd.Parameters.AddWithValue("@telefono", c.Telefono)
                            cmd.Parameters.AddWithValue("@correo", c.Correo)
                            cmd.Parameters.AddWithValue("@direccion", c.Direccion)
                            cmd.Parameters.AddWithValue("@id", c.IDCliente)
                            filas = cmd.ExecuteNonQuery()
                        End Using

                        If filas > 0 Then
                            Dim cambios As String = DescribirCambios(anterior, c)
                            Dim descripcion As String
                            If cambios = "" Then
                                descripcion = "Se guardó el cliente " & anterior.Nombre & " " & anterior.Apellido &
                                              " sin cambios en sus datos."
                            Else
                                descripcion = "Se editó el cliente " & anterior.Nombre & " " & anterior.Apellido &
                                              ". Cambios -> " & cambios
                            End If

                            BitacoraDAL.Registrar(cn, tx, "EDITAR", c.IDCliente, idUsuario, nombreUsuario, descripcion)
                            resultado = True
                        End If
                    End If

                    If resultado Then
                        tx.Commit()
                    Else
                        tx.Rollback()
                    End If
                Catch
                    tx.Rollback()
                    Throw
                End Try
            End Using
        End Using

        Return resultado
    End Function

    ' Elimina un cliente y registra eliminarr en la bitacora, devuelve trur si se elimino.
    Public Shared Function Eliminar(ByVal idCliente As Integer, ByVal idUsuario As Integer,
                                    ByVal nombreUsuario As String) As Boolean
        Dim resultado As Boolean = False

        Using cn As SqlConnection = Conexion.ObtenerConexion()
            cn.Open()
            Using tx As SqlTransaction = cn.BeginTransaction()
                Try
                    ' Se leen los datos antes de borrar, para poder dejarlos en la descripcion
                    Dim anterior As Cliente = ObtenerPorId(cn, tx, idCliente)

                    If anterior IsNot Nothing Then
                        Dim filas As Integer
                        Using cmd As New SqlCommand("DELETE FROM dbo.Clientes WHERE IDCliente = @id", cn, tx)
                            cmd.Parameters.AddWithValue("@id", idCliente)
                            filas = cmd.ExecuteNonQuery()
                        End Using

                        If filas > 0 Then
                            Dim descripcion As String = "Se eliminó el cliente " & anterior.Nombre & " " &
                                                        anterior.Apellido & " (DUI " & anterior.DUI & ")."
                            BitacoraDAL.Registrar(cn, tx, "ELIMINAR", idCliente, idUsuario, nombreUsuario, descripcion)
                            resultado = True
                        End If
                    End If

                    If resultado Then
                        tx.Commit()
                    Else
                        tx.Rollback()
                    End If
                Catch
                    tx.Rollback()
                    Throw
                End Try
            End Using
        End Using

        Return resultado
    End Function

    Public Shared Function Contar() As Integer
        Using cn As SqlConnection = Conexion.ObtenerConexion()
            cn.Open()
            Using cmd As New SqlCommand("SELECT COUNT(*) FROM dbo.Clientes", cn)
                Return Convert.ToInt32(cmd.ExecuteScalar())
            End Using
        End Using
    End Function

End Class