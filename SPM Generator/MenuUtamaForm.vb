Imports System.Data.OleDb
Imports System.IO

Public Class MenuUtamaForm
    Dim conn As New OleDbConnection

    Dim data As DataSet
    Dim dt_ujs As DataTable
    Dim dt_list_do As DataTable
    Dim dt_hasil As DataTable
    Dim max_qty As Integer = 230

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
        End If
    End Sub


    Private Sub BarButtonProses_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonProses.ItemClick
        Dim paus As New ClassPaus
        paus.dt = dt_list_do
        paus.dt_ujs = dt_ujs
        paus.max_qty = 230
        paus.banyak_paus = 500
        paus.maks_iterasi = 100
        dt_hasil = paus.proses
        Dim childfm = New HasilForm
        childfm.GridControl1.DataSource = dt_hasil
        childfm.BarHeaderUjsMinimum.Caption = $"gbest = {paus.total_ujs_gbest}"
        childfm.ShowDialog()
    End Sub


    Private Sub MenuUtama_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        conn = Koneksi()
        Dim cmd As New OleDbCommand("select * from m_ujs", conn)
        Dim rd As OleDbDataReader
        Dim dt As New DataTable
        dt_ujs = New DataTable
        rd = cmd.ExecuteReader
        dt_ujs.Load(rd)
    End Sub

    Private Sub BarButtonNormalisasi_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonNormalisasi.ItemClick
        Dim list_cust_sama As New List(Of Integer)

        For i = 0 To GridView1.RowCount - 1
            Dim cek = 0
            If list_cust_sama.Contains(i) Then
                cek = 1
            End If
            If cek = 0 Then
                For j = i + 1 To GridView1.RowCount - 1
                    If list_cust_sama.Contains(j) Then
                        cek = 1
                    End If
                    If cek = 0 Then
                        If GridView1.GetRowCellValue(i, "DESA") = GridView1.GetRowCellValue(j, "DESA") And GridView1.GetRowCellValue(i, "KODE TOKO") = GridView1.GetRowCellValue(j, "KODE TOKO") Then
                            Dim total_qty As Integer = CInt(GridView1.GetRowCellValue(i, "QTY")) + CInt(GridView1.GetRowCellValue(j, "QTY"))
                            If total_qty <= 230 Then
                                dt_list_do.Rows(i)("NO") = GridView1.GetRowCellValue(i, "NO").ToString & " & " & GridView1.GetRowCellValue(j, "NO").ToString
                                dt_list_do.Rows(i)("ORDER DATE") = GridView1.GetRowCellValue(i, "ORDER DATE").ToString & " & " & GridView1.GetRowCellValue(j, "ORDER DATE").ToString
                                dt_list_do.Rows(i)("CEMENT") = GridView1.GetRowCellValue(i, "CEMENT") & "-" & GridView1.GetRowCellValue(j, "CEMENT")
                                dt_list_do.Rows(i)("SDO NO") = GridView1.GetRowCellValue(i, "SDO NO") & " & " & GridView1.GetRowCellValue(j, "SDO NO")
                                dt_list_do.Rows(i)("QTY") = total_qty
                                ''dt_list_do.Rows(j).Delete()
                                list_cust_sama.Add(j)
                            End If
                        End If
                    End If
                Next
            End If
        Next

        For Each index As Integer In list_cust_sama
            If index >= 0 And index < dt_list_do.Rows.Count Then
                dt_list_do.Rows(index).Delete()
            End If
        Next
        dt_list_do.AcceptChanges()

    End Sub

    Private Sub BarButtonPermutasi_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonPermutasi.ItemClick
        Dim permutasi As New ClassPermutasi
        permutasi.dt = dt_list_do
        permutasi.dt_ujs = dt_ujs
        permutasi.max_qty = 230
        dt_hasil = permutasi.proses
        Dim childfm = New HasilForm
        childfm.GridControl1.DataSource = dt_hasil
        childfm.BarHeaderUjsMinimum.Caption = $"gbest = {permutasi.total_ujs_gbest}"
        childfm.ShowDialog()
    End Sub
End Class