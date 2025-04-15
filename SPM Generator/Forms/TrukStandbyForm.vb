Public Class TrukStandbyForm
    Public dt As DataTable
    Public list_kapasitas_truk As New List(Of Integer)
    Public list_kapasitas_truk_default As New List(Of Integer)
    Sub New(ByVal dt_list_truk_standby As DataTable)

        ' This call is required by the designer.
        dt = dt_list_truk_standby
        ' Add any initialization after the InitializeComponent() call.
        InitializeComponent()
        GridControl1.DataSource = dt
        GridView1.Columns(0).OptionsColumn.AllowEdit = False
        GridView1.Columns(1).OptionsColumn.AllowEdit = False

    End Sub

    Private Sub ButtonSimpan_Click(sender As Object, e As EventArgs) Handles ButtonSimpan.Click
        list_kapasitas_truk = New List(Of Integer)
        For i = 0 To GridView1.RowCount - 1
            If IsNumeric(GridView1.GetRowCellValue(i, "Jumlah Standby")) Then
                dt(i)("Jumlah Standby") = GridView1.GetRowCellValue(i, "Jumlah Standby")
                list_kapasitas_truk_default.Add(GridView1.GetRowCellValue(i, "Max Qty"))
                If GridView1.GetRowCellValue(i, "Jumlah Standby") > 0 Then
                    For j = 0 To CInt(GridView1.GetRowCellValue(i, "Jumlah Standby")) - 1
                        list_kapasitas_truk.Add(GridView1.GetRowCellValue(i, "Max Qty"))
                    Next
                End If
            Else
                If GridView1.GetRowCellValue(i, "Jumlah Standby") IsNot DBNull.Value Then
                    If GridView1.GetRowCellValue(i, "Jumlah Standby") <> "-" Then
                        MessageBox.Show($"Harap isi Jumlah Standby pada baris ke-{i + 1} dengan angka", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                        Exit Sub
                    End If
                Else
                    MessageBox.Show($"Harap isi Jumlah Standby pada baris ke-{i + 1} dengan angka", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    Exit Sub
                End If
            End If
        Next
        Me.DialogResult = DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Add_Click(sender As Object, e As EventArgs) Handles Add.Click
        If GridView1.RowCount > 0 Then
            For i = 0 To GridView1.RowCount - 1
                If GridView1.GetRowCellValue(i, "Jenis Truk") = ComboBoxJenisTruk.Text And GridView1.GetRowCellValue(i, "Max Qty") = ComboBoxMaxQty.Text Then
                    MessageBox.Show($"Data untuk Jenis Truk = {ComboBoxJenisTruk.Text} dan Max Qty = {ComboBoxMaxQty.Text} sudah ada", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    Exit Sub
                End If
            Next
        End If

        Dim dr As DataRow
        dr = dt.NewRow
        dr("Jenis Truk") = ComboBoxJenisTruk.Text
        dr("Max Qty") = ComboBoxMaxQty.Text
        dr("Jumlah Standby") = TextBox1.Text
        dt.Rows.InsertAt(dr, 0)
    End Sub

    Private Sub ButtonClear_Click(sender As Object, e As EventArgs) Handles ButtonClear.Click
        dt = New DataTable
        dt.Columns.Add("Jenis Truk")
        dt.Columns.Add("Max Qty")
        dt.Columns.Add("Jumlah Standby")
        GridControl1.DataSource = dt
        GridView1.RefreshData()
    End Sub
End Class
