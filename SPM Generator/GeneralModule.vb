Imports System.Data.OleDb

Module GeneralModule
    Dim da As OleDbDataAdapter
    Dim cmd As OleDbCommand
    Dim dr As OleDbDataReader
    Public Function Koneksi()
        Dim conn As New OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=db_spm_generator.accdb")
        conn.Open()
        Return conn
    End Function

End Module
