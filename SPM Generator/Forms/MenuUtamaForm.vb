Imports System.Data.OleDb
Imports System.IO

Public Class MenuUtamaForm
    Dim conn As New OleDbConnection

    Dim data As DataSet
    Dim dt_ujs As DataTable
    Dim dt_list_do As DataTable
    Dim dt_hasil As DataTable
    Dim ds_hasil As DataSet
    Dim dt_list_truk_standby As DataTable
    Dim dt_optimalisasi As DataTable
    Dim list_kapasitas_truk As List(Of Integer)
    Dim list_kapasitas_truk_default As List(Of Integer)
    ''Dim max_qty As Integer = 230

    Private Sub BarButtonImport_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonImport.ItemClick
        OpenFileDialog1.Filter = "Excel files (*.xlsx)|*.xlsx|CSV Files (*.csv)|*.csv|XLS Files (*.xls)|*xls"
        If (OpenFileDialog1.ShowDialog(Me) = System.Windows.Forms.DialogResult.OK) Then
            GridView1.Columns.Clear()
            Dim fi As New FileInfo(OpenFileDialog1.FileName)
            Dim FileName As String = OpenFileDialog1.FileName
            Dim excel As String = fi.FullName
            conn = New OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + excel + ";Extended Properties=Excel 12.0;")
            Dim dta = New OleDbDataAdapter("Select * From [SPM$]", conn)
            Dim dts = New DataSet
            dta.Fill(dts, "[SPM$]")
            dt_list_do = New DataTable
            dt_list_do = dts.Tables(0)
            GridControl1.DataSource = dt_list_do
            'GridControl1.DataMember = "[SPM$]"
            data = New DataSet
            data = dts.Tables(0).DataSet
            conn.Close()
            GridView1.BestFitColumns()
        End If
    End Sub

    Private Sub BarButtonProses_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonProses.ItemClick

        If GridView1.RowCount = 0 Then
            MessageBox.Show("Harap Import data DO terlebih dahulu!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        If list_kapasitas_truk.Count = 0 Then
            MessageBox.Show("Harap masukan data Truck Standby terlebih dahulu!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        Dim paus As New ClassPaus
        paus.dt = dt_list_do
        paus.dt_ujs = dt_ujs
        paus.dt_list_truk_standby = dt_list_truk_standby
        paus.list_kapasitas_truk = list_kapasitas_truk
        'paus.list_kapasitas_truk_default = list_kapasitas_truk_default
        paus.banyak_paus = 200
        paus.maks_iterasi = 100
        dt_hasil = paus.proses
        dt_optimalisasi = paus.dt_optimalisasi
        ds_hasil = New DataSet
        ds_hasil.Tables.Add(dt_hasil)
        ds_hasil.Tables.Add(dt_optimalisasi)

        Dim childfm = New HasilForm
        dt_hasil.DefaultView.Sort = "PAUS ASC"
        childfm.GridControl1.DataSource = dt_hasil
        childfm.BarHeaderUjsMinimum.Caption = $"gbest = {paus.total_ujs_gbest}"
        childfm.GridView1.BestFitColumns()
        childfm.GridView1.Columns("PAUS").Visible = False
        childfm.Show()
    End Sub

    Private Sub MenuUtama_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        conn = Koneksi()
        Dim cmd As New OleDbCommand("SELECT * FROM m_ujs WHERE m_ujs.id IN ( SELECT MAX(id) FROM m_ujs GROUP BY asal, tujuan)", conn)
        Dim rd As OleDbDataReader
        Dim dt As New DataTable
        dt_ujs = New DataTable
        rd = cmd.ExecuteReader()
        dt_ujs.Load(rd)

        dt_list_truk_standby = New DataTable
        'conn = Koneksi()
        'cmd = New OleDbCommand("SELECT [Jenis Truk], [Max Qty] FROM m_tipetruck ", conn)
        'rd = cmd.ExecuteReader()
        'dt_list_truk_standby.Load(rd)
        dt_list_truk_standby.Columns.Add("Jenis Truk")
        dt_list_truk_standby.Columns.Add("Max Qty")
        dt_list_truk_standby.Columns.Add("Jumlah Standby")

        list_kapasitas_truk = New List(Of Integer)
        'list_kapasitas_truk_default = New List(Of Integer)
        'If dt_list_truk_standby.Rows.Count > 0 Then
        '    For i = 0 To dt_list_truk_standby.Rows.Count - 1
        '        Dim a As Integer = dt_list_truk_standby(i)("Max Qty")
        '        list_kapasitas_truk.Add(a)
        '        list_kapasitas_truk_default.Add(a)
        '    Next
        'End If

    End Sub

    'Private Sub BarButtonNormalisasi_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonNormalisasi.ItemClick
    '    Dim list_cust_sama As New List(Of Integer)

    '    For i = 0 To GridView1.RowCount - 1
    '        Dim cek = 0
    '        If list_cust_sama.Contains(i) Then
    '            cek = 1
    '        End If
    '        If cek = 0 Then
    '            For j = i + 1 To GridView1.RowCount - 1
    '                If list_cust_sama.Contains(j) Then
    '                    cek = 1
    '                End If
    '                If cek = 0 Then
    '                    If GridView1.GetRowCellValue(i, "DESA") = GridView1.GetRowCellValue(j, "DESA") And GridView1.GetRowCellValue(i, "KODE TOKO") = GridView1.GetRowCellValue(j, "KODE TOKO") Then
    '                        Dim total_qty As Integer = CInt(GridView1.GetRowCellValue(i, "QTY")) + CInt(GridView1.GetRowCellValue(j, "QTY"))
    '                        If total_qty <= 230 Then
    '                            dt_list_do.Rows(i)("NO") = GridView1.GetRowCellValue(i, "NO").ToString & " & " & GridView1.GetRowCellValue(j, "NO").ToString
    '                            dt_list_do.Rows(i)("ORDER DATE") = GridView1.GetRowCellValue(i, "ORDER DATE").ToString & " & " & GridView1.GetRowCellValue(j, "ORDER DATE").ToString
    '                            dt_list_do.Rows(i)("CEMENT") = GridView1.GetRowCellValue(i, "CEMENT") & "-" & GridView1.GetRowCellValue(j, "CEMENT")
    '                            dt_list_do.Rows(i)("SDO NO") = GridView1.GetRowCellValue(i, "SDO NO") & " & " & GridView1.GetRowCellValue(j, "SDO NO")
    '                            dt_list_do.Rows(i)("QTY") = total_qty
    '                            list_cust_sama.Add(j)
    '                        End If
    '                    End If
    '                End If
    '            Next
    '        End If
    '    Next

    '    For Each index As Integer In list_cust_sama
    '        If index >= 0 And index < dt_list_do.Rows.Count Then
    '            dt_list_do.Rows(index).Delete()
    '        End If
    '    Next
    '    dt_list_do.AcceptChanges()
    'End Sub

    'Private Sub BarButtonPermutasi_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonPermutasi.ItemClick
    '    Dim permutasi As New ClassPermutasi
    '    permutasi.dt = dt_list_do
    '    permutasi.dt_ujs = dt_ujs
    '    permutasi.max_qty = 230
    '    dt_hasil = permutasi.proses
    '    Dim childfm = New HasilForm
    '    childfm.GridControl1.DataSource = dt_hasil
    '    childfm.BarHeaderUjsMinimum.Caption = $"gbest = {permutasi.total_ujs_gbest}"
    '    childfm.ShowDialog()
    'End Sub

    Private Sub BarButtonTruckStandby_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonTruckStandby.ItemClick
        Dim childfm As New TrukStandbyForm(dt_list_truk_standby)
        childfm.ShowDialog()
        If childfm.DialogResult = DialogResult.OK Then
            dt_list_truk_standby = childfm.dt
            If childfm.list_kapasitas_truk.Count > 0 Then
                list_kapasitas_truk = childfm.list_kapasitas_truk
                list_kapasitas_truk_default = childfm.list_kapasitas_truk_default
            End If
        End If
    End Sub

End Class
