
Imports System.IO
Public Class HasilForm

    Private Sub BarButtonGenerate_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonGenerate.ItemClick
        Dim folderPath As String = "D:\OUTPUT SPM\Export_" & Format(Now, "ddMMyyyy_hhmmss")
        If Not Directory.Exists(folderPath) Then
            Directory.CreateDirectory(folderPath)
        End If

        Dim index_awal As Integer = 0
        Dim index_akhir As Integer
        Dim rute_ke As Integer = 0
        For i = 0 To GridView1.RowCount - 1
            rute_ke = 0
            If GridView1.GetRowCellValue(i, "NO") = "" Then
                index_akhir = i

                Dim ExcelApp As New Object
                Dim ExcelBook As New Object
                Dim ExcelSheet1 As New Object

                ExcelApp = CreateObject("Excel.Application")
                ExcelBook = ExcelApp.WorkBooks.open(Application.StartupPath & $"\Template {index_akhir - index_awal} Trip.xlsx")

                ExcelSheet1 = ExcelBook.WorkSheets(1)
                Dim tanggal_order As String = ""
                Dim customer As String = ""
                Dim sdo_no As String = ""
                With ExcelSheet1
                    If (index_akhir - index_awal) > 1 Then
                        ''Multitrip

                        For j = index_awal To index_akhir - 1
                            If tanggal_order.Contains(GridView1.GetRowCellValue(j, "ORDER DATE")) Then
                            Else
                                If j = index_awal Then
                                    tanggal_order = GridView1.GetRowCellValue(j, "ORDER DATE")
                                Else
                                    tanggal_order = tanggal_order & " & " & GridView1.GetRowCellValue(j, "ORDER DATE")
                                End If
                            End If

                            If customer.Contains(GridView1.GetRowCellValue(j, "KODE TOKO")) Then
                            Else
                                If j = index_awal Then
                                    customer = GridView1.GetRowCellValue(j, "KODE TOKO") & "-" & GridView1.GetRowCellValue(j, "NAMA TOKO")
                                Else
                                    customer = customer & " & " & GridView1.GetRowCellValue(j, "KODE TOKO") & "-" & GridView1.GetRowCellValue(j, "NAMA TOKO")
                                End If
                            End If
                            If j = index_awal Then
                                sdo_no = GridView1.GetRowCellValue(j, "SDO NO")
                            Else
                                sdo_no = sdo_no & ", " & GridView1.GetRowCellValue(j, "SDO NO")
                            End If
                            .cells(21 + (rute_ke), 4) = GridView1.GetRowCellValue(j, "CEMENT")
                            .cells(21 + (rute_ke), 5) = GridView1.GetRowCellValue(j, "QTY")
                            .cells(21 + (rute_ke), 6) = GridView1.GetRowCellValue(j, "DESA")
                            .cells(21 + (rute_ke), 7) = GridView1.GetRowCellValue(j, "SDO NO")
                            rute_ke += 1
                        Next
                        .cells(21, 8) = GridView1.GetRowCellValue(index_awal, "SUM UJS")
                        .cells(21, 9) = GridView1.GetRowCellValue(index_awal, "SUM JARAK")
                        .cells(21 + (rute_ke), 8) = GridView1.GetRowCellValue(index_awal, "TAMBAHAN MULTITRIP")
                        .cells(21 + (rute_ke + 1), 8) = 10000
                        .cells(21 + (rute_ke + 2), 8) = GridView1.GetRowCellValue(index_awal, "TOTAL UJS")
                        .cells(21 + (rute_ke + 2), 9) = GridView1.GetRowCellValue(index_awal, "SUM JARAK")
                        .cells(10, 4) = tanggal_order
                        .cells(11, 4) = customer
                    Else
                        sdo_no = GridView1.GetRowCellValue(index_awal, "SDO NO")
                        ''Onetrip
                        .cells(10, 4) = GridView1.GetRowCellValue(index_awal, "ORDER DATE")
                        .cells(11, 4) = GridView1.GetRowCellValue(index_awal, "KODE TOKO") & "-" & GridView1.GetRowCellValue(index_awal, "NAMA TOKO")
                        .cells(21, 4) = GridView1.GetRowCellValue(index_awal, "CEMENT")
                        .cells(21, 5) = GridView1.GetRowCellValue(index_awal, "QTY")
                        .cells(21, 6) = GridView1.GetRowCellValue(index_awal, "DESA")
                        .cells(21, 7) = GridView1.GetRowCellValue(index_awal, "SDO NO")
                        .cells(21, 8) = GridView1.GetRowCellValue(index_awal, "SUM UJS")
                        .cells(21, 9) = GridView1.GetRowCellValue(index_awal, "SUM JARAK")
                        .cells(22, 8) = 10000
                        .cells(23, 8) = GridView1.GetRowCellValue(index_awal, "TOTAL UJS")
                        .cells(23, 9) = GridView1.GetRowCellValue(index_awal, "SUM JARAK")
                    End If

                    .cells(21 + (rute_ke + 6), 4) = Format(Date.Now, "dd-MMM-yyyy").ToString

                End With


                Dim filename As String = $"{GridView1.GetRowCellValue(index_awal, "RUTE")} RIMA PAMEKASAN {Format(Date.Now, "MM yyyy")} SDO {sdo_no}.xlsx"
                Dim outputPath As String = Path.Combine(folderPath, filename)
                ExcelBook.SaveAs(outputPath)
                ExcelApp.Visible = True
                ExcelSheet1 = Nothing
                ExcelBook = Nothing
                ExcelApp = Nothing

                index_awal = index_akhir + 1
            End If
        Next
    End Sub
End Class