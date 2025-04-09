Public Class TrukStandbyForm
    Dim dt As DataTable
    Sub New(ByRef dt_list_truk_standby As DataTable)

        ' This call is required by the designer.
        dt = dt_list_truk_standby
        ' Add any initialization after the InitializeComponent() call.
        InitializeComponent()
        GridControl1.DataSource = dt_list_truk_standby
        GridView1.Columns(0).OptionsColumn.AllowEdit = False
        GridView1.Columns(1).OptionsColumn.AllowEdit = False


    End Sub
    Private Sub ButtonSimpan_Click(sender As Object, e As EventArgs) Handles ButtonSimpan.Click
        For i = 0 To GridView1.RowCount - 1
            dt(i)("Jumlah Standby") = GridView1.GetRowCellValue(i, "Jumlah Standby")
        Next
        Me.Close()
    End Sub
End Class
