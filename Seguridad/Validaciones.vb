Imports System.Text.RegularExpressions

Public Class Validaciones

    ' DUI 8 digitos, guion, 1 digito (ej. 01234567-8)
    Private Shared ReadOnly RegexDUI As New Regex("^[0-9]{8}-[0-9]$")

    ' Telefono 8 digitos, con o sin guion (ej. 7777-8888 o 77778888)
    Private Shared ReadOnly RegexTelefono As New Regex("^[0-9]{4}-?[0-9]{4}$")

    ' Correo test@test.test
    Private Shared ReadOnly RegexCorreo As New Regex("^[^@\s]+@[^@\s]+\.[^@\s]+$")

    ' Nombres y apellidos letras, espacios, guion y punto
    Private Shared ReadOnly RegexNombre As New Regex("^[A-Za-zÁÉÍÓÚÜÑáéíóúüñ\s'\.\-]+$")

    Public Shared Function EsDUIValido(ByVal dui As String) As Boolean
        Return RegexDUI.IsMatch(dui)
    End Function

    Public Shared Function EsTelefonoValido(ByVal telefono As String) As Boolean
        Return RegexTelefono.IsMatch(telefono)
    End Function

    Public Shared Function EsCorreoValido(ByVal correo As String) As Boolean
        Return RegexCorreo.IsMatch(correo)
    End Function

    ' formato ####-####
    Public Shared Function NormalizarTelefono(ByVal telefono As String) As String
        Dim soloDigitos As String = telefono.Replace("-", "")
        If soloDigitos.Length = 8 Then
            Return soloDigitos.Substring(0, 4) & "-" & soloDigitos.Substring(4, 4)
        End If
        Return telefono
    End Function

    ' devuelve la lista de errores (vacia si todo esta bien).
    Public Shared Function ValidarCliente(ByVal c As Cliente) As List(Of String)
        Dim errores As New List(Of String)()

        ' Nombre
        If String.IsNullOrWhiteSpace(c.Nombre) Then
            errores.Add("El nombre es obligatorio.")
        ElseIf c.Nombre.Length > 80 Then
            errores.Add("El nombre no puede superar los 80 caracteres.")
        ElseIf Not RegexNombre.IsMatch(c.Nombre) Then
            errores.Add("El nombre solo puede contener letras y espacios.")
        End If

        ' Apellido
        If String.IsNullOrWhiteSpace(c.Apellido) Then
            errores.Add("El apellido es obligatorio.")
        ElseIf c.Apellido.Length > 80 Then
            errores.Add("El apellido no puede superar los 80 caracteres.")
        ElseIf Not RegexNombre.IsMatch(c.Apellido) Then
            errores.Add("El apellido solo puede contener letras y espacios.")
        End If

        ' DUI
        If String.IsNullOrWhiteSpace(c.DUI) Then
            errores.Add("El DUI es obligatorio.")
        ElseIf Not EsDUIValido(c.DUI) Then
            errores.Add("El DUI debe tener el formato 00000000-0.")
        End If

        ' Telefono
        If String.IsNullOrWhiteSpace(c.Telefono) Then
            errores.Add("El teléfono es obligatorio.")
        ElseIf Not EsTelefonoValido(c.Telefono) Then
            errores.Add("El teléfono debe tener 8 dígitos (ej. 7777-8888).")
        End If

        ' Correo
        If String.IsNullOrWhiteSpace(c.Correo) Then
            errores.Add("El correo electrónico es obligatorio.")
        ElseIf c.Correo.Length > 100 Then
            errores.Add("El correo no puede superar los 100 caracteres.")
        ElseIf Not EsCorreoValido(c.Correo) Then
            errores.Add("El correo electrónico no tiene un formato válido.")
        End If

        ' Direccion
        If String.IsNullOrWhiteSpace(c.Direccion) Then
            errores.Add("La dirección es obligatoria.")
        ElseIf c.Direccion.Length > 200 Then
            errores.Add("La dirección no puede superar los 200 caracteres.")
        End If

        Return errores
    End Function

End Class