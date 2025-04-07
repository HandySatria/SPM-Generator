Public Class ClassPaus
    Public dt As DataTable
    Public dt_hasil As DataTable
    Public dt_ujs As DataTable
    Public max_qty As Integer
    Public banyak_paus As Integer
    Public maks_iterasi As Integer
    Public total_ujs_gbest As Double = 0
    Dim beta As Integer = 0.5

    Public Function proses()
        ''Inisialisasi Parameter
        Dim alfa As Integer = 1
        Dim index_pbest As Integer
        Dim paus(banyak_paus, dt.Rows.Count), paus_urutan(banyak_paus, dt.Rows.Count) As Double
        Dim paus_terbaik(dt.Rows.Count), pbest As Double
        Dim gbest(dt.Rows.Count) As Double
        Dim gbest_urutan(dt.Rows.Count) As Double
        Dim p(banyak_paus) As Double
        Dim A(dt.Rows.Count), C(dt.Rows.Count) As Double
        Dim cek_A As Integer
        Dim bil_random(banyak_paus - 1, dt.Rows.Count - 1) As Double
        Dim jarak_per_rit(banyak_paus, dt.Rows.Count) As Double
        Dim jarak_per_rute(banyak_paus, dt.Rows.Count, dt.Rows.Count) As Double
        Dim ujs_per_rit(banyak_paus, dt.Rows.Count) As Double
        Dim ujs_per_rute(banyak_paus, dt.Rows.Count, dt.Rows.Count) As Double
        Dim total_ujs_per_rit(banyak_paus, dt.Rows.Count) As Double

        '' Generate populasi awal
        generate_bil_random(banyak_paus, paus)

        ''evaluasi fungsi tujuan
        evaluasi_fungsi_tujuan(banyak_paus, paus, paus_urutan, jarak_per_rute, jarak_per_rit, ujs_per_rute, ujs_per_rit, total_ujs_per_rit)

        '' menentukan nilai pbest
        menentukan_nilai_pbest(banyak_paus, paus, paus_urutan, total_ujs_per_rit, index_pbest, pbest, paus_terbaik, p, gbest, gbest_urutan, total_ujs_gbest)

        '' Tahap Migrasi
        MenuUtamaForm.ProgressBar1.Maximum = maks_iterasi
        For iterasi = 0 To maks_iterasi - 1
            MenuUtamaForm.ProgressBar1.Value += 1
            For i = 0 To banyak_paus - 1
                If p(i) < 0.5 Then
                    cek_A = menghitung_mutlak_A(iterasi, maks_iterasi, A, C)
                    If cek_A = 1 Then
                        mencari_mangsa(C, A, i, paus)
                    Else
                        mengitari_mangsa(A, C, gbest, i, paus)
                    End If
                Else
                    menyerang_mangsa_dengan_gelembung(i, gbest, paus)
                End If
            Next
            evaluasi_fungsi_tujuan(banyak_paus, paus, paus_urutan, jarak_per_rute, jarak_per_rit, ujs_per_rute, ujs_per_rit, total_ujs_per_rit)
            '' menentukan nilai pbest
            menentukan_nilai_pbest(banyak_paus, paus, paus_urutan, total_ujs_per_rit, index_pbest, pbest, paus_terbaik, p, gbest, gbest_urutan, total_ujs_gbest)
        Next

        kesimpulan(gbest_urutan, jarak_per_rute, jarak_per_rit, ujs_per_rute, ujs_per_rit, total_ujs_per_rit)
        MenuUtamaForm.ProgressBar1.Value = 0
        Return dt_hasil

    End Function

    Private Sub generate_bil_random(banyak_paus As Integer, ByRef bil_random(,) As Double)
        Randomize()
        For i = 0 To banyak_paus - 1
            For j = 0 To dt.Rows.Count - 1
                bil_random(i, j) = Rnd()
            Next
        Next
    End Sub

    Private Function mengurutkan_bil_random(banyak_paus As Integer, ByVal paus(,) As Double)
        Dim urutan(banyak_paus - 1, dt.Rows.Count - 1) As Double
        Dim paus_urutan(banyak_paus, dt.Rows.Count) As Double

        Dim temp As Double
        For i = 0 To banyak_paus - 1
            For j = 0 To dt.Rows.Count - 1
                urutan(i, j) = paus(i, j)
            Next
        Next

        For i = 0 To banyak_paus - 1
            For j = 0 To dt.Rows.Count - 1
                For k = 0 To dt.Rows.Count - 1
                    If urutan(i, j) < urutan(i, k) Then
                        temp = urutan(i, j)
                        urutan(i, j) = urutan(i, k)
                        urutan(i, k) = temp
                    End If
                Next
            Next
        Next

        For i = 0 To banyak_paus - 1
            For j = 0 To dt.Rows.Count - 1
                For k = 0 To dt.Rows.Count - 1
                    If paus(i, j) = urutan(i, k) Then
                        paus_urutan(i, j) = k
                    End If
                Next
            Next
        Next
        Console.WriteLine("(" & String.Join(",", paus_urutan) & ")")
        Return paus_urutan
    End Function

    Private Sub evaluasi_fungsi_tujuan(banyak_paus As Integer, ByVal paus(,) As Double, ByRef paus_urutan(,) As Double, jarak_per_rute(,,) As Double, jarak_per_rit(,) As Double, ujs_per_rute(,,) As Double, ujs_per_rit(,) As Double, ByRef total_ujs_per_rit(,) As Double)
        Dim rute_ke, qty, actual_qty, index_awal, index_akhir As Integer
        '' mengurutkan bilangan random

        paus_urutan = mengurutkan_bil_random(banyak_paus, paus)

        'dt_hasil = New DataTable
        'dt_hasil.Columns.Add("PAUS")
        'dt_hasil.Columns.Add("RUTE")
        'dt_hasil.Columns.Add("NO")
        'dt_hasil.Columns.Add("ORDER DATE")
        'dt_hasil.Columns.Add("KODE TOKO")
        'dt_hasil.Columns.Add("CEMENT")
        'dt_hasil.Columns.Add("SDO NO")
        'dt_hasil.Columns.Add("QTY")
        'dt_hasil.Columns.Add("DESA")
        'dt_hasil.Columns.Add("TOTAL QTY")
        'dt_hasil.Columns.Add("ASAL")
        'dt_hasil.Columns.Add("TUJUAN")
        'dt_hasil.Columns.Add("JARAK")
        'dt_hasil.Columns.Add("UJS")
        'dt_hasil.Columns.Add("SUM JARAK")
        'dt_hasil.Columns.Add("SUM UJS")
        'dt_hasil.Columns.Add("TAMBAHAN MULTITRIP")
        'dt_hasil.Columns.Add("TOTAL UJS")

        For i = 0 To banyak_paus - 1
            rute_ke = 0
            qty = 0
            index_awal = 0
            index_akhir = 0

            For j = 0 To dt.Rows.Count - 1
                qty += CInt(dt(paus_urutan(i, j))("QTY"))

                If qty > max_qty Or j = (dt.Rows.Count - 1) Then
                    rute_ke += 1
                    If j = (dt.Rows.Count - 1) And qty <= max_qty Then
                        index_akhir = j
                    Else
                        qty = qty - CInt(dt(paus_urutan(i, j))("QTY"))
                        index_akhir = j - 1
                    End If
                    actual_qty = qty
                    ujs_per_rit(i, rute_ke) = 0
                    jarak_per_rit(i, rute_ke) = 0

                    Dim dr As DataRow()

                    If (index_akhir - index_awal) > 1 Then
                        For k = index_awal To index_akhir
                            If k = index_awal Then
                                dr = dt_ujs.Select($"Asal = 'Pamekasan-Gudang Pamekasan-' and Tujuan = '{dt(paus_urutan(i, k))("DESA")}'")
                            Else
                                dr = dt_ujs.Select($"Asal = '{dt(paus_urutan(i, k - 1))("DESA")}' and Tujuan = '{dt(paus_urutan(i, k))("DESA")}'")
                            End If

                            If dr.Count > 0 Then
                                Dim kapasitas_ujs As Double

                                If qty > 200 Then
                                    kapasitas_ujs = 200
                                Else
                                    kapasitas_ujs = CInt(Math.Floor(qty / 25))
                                    If (kapasitas_ujs Mod 2) > 0 Then
                                        kapasitas_ujs += 1
                                    End If
                                    kapasitas_ujs = kapasitas_ujs * 25
                                End If

                                If qty >= 175 Then
                                    ujs_per_rute(i, rute_ke, k) = dr(0)("200")
                                    ujs_per_rit(i, rute_ke) += dr(0)("200")
                                ElseIf qty >= 125 Then
                                    ujs_per_rute(i, rute_ke, k) = dr(0)("150")
                                    ujs_per_rit(i, rute_ke) += dr(0)("150")
                                ElseIf qty >= 75 Then
                                    ujs_per_rute(i, rute_ke, k) = dr(0)("100")
                                    ujs_per_rit(i, rute_ke) += dr(0)("100")
                                ElseIf qty >= 25 Then
                                    ujs_per_rute(i, rute_ke, k) = dr(0)("50")
                                    ujs_per_rit(i, rute_ke) += dr(0)("50")
                                Else
                                    ujs_per_rute(i, rute_ke, k) = dr(0)("0")
                                    ujs_per_rit(i, rute_ke) += dr(0)("0")
                                End If
                                jarak_per_rute(i, rute_ke, k) = dr(0)("Jarak")
                                jarak_per_rit(i, rute_ke) += dr(0)("Jarak")
                            End If
                            qty = qty - CInt(dt(paus_urutan(i, k))("QTY"))
                        Next
                    Else
                        dr = dt_ujs.Select($"Asal = 'Pamekasan-Gudang Pamekasan-' and Tujuan = '{dt(paus_urutan(i, index_awal))("DESA")}'")
                        If dr.Count > 0 Then
                            ujs_per_rute(i, rute_ke, index_awal) = dr(0)("200")
                            ujs_per_rit(i, rute_ke) += dr(0)("200")
                            jarak_per_rute(i, rute_ke, index_awal) = dr(0)("Jarak")
                            jarak_per_rit(i, rute_ke) += dr(0)("Jarak")
                        End If
                        qty = qty - CInt(dt(paus_urutan(i, index_awal))("QTY"))
                    End If


                    ''kosongan kembali ke gudang
                    dr = dt_ujs.Select($"Asal = '{dt(paus_urutan(i, index_akhir))("DESA")}' and Tujuan = 'Pamekasan-Gudang Pamekasan-'")
                    If dr.Count > 0 Then
                        ujs_per_rute(i, rute_ke, j) = dr(0)("0")
                        ujs_per_rit(i, rute_ke) += dr(0)("0")
                        jarak_per_rute(i, rute_ke, j) = dr(0)("Jarak")
                        jarak_per_rit(i, rute_ke) += dr(0)("Jarak")
                    End If

                    Dim ujs_multitrip As Double = 0
                    Dim ujs_onetrip As Double = 0
                    Dim upah_multi_trip As Double = 0
                    Dim tambahan_multi_trip As Double = 0
                    Dim upah_tunggu As Double = 1.5 * 11000
                    Dim upah_kuli_pemerataan As Double = 10000

                    If (j - index_awal) > 1 Then
                        upah_multi_trip = (j - index_awal) * 2200
                        ujs_multitrip = Math.Ceiling((upah_multi_trip + upah_tunggu + upah_kuli_pemerataan + ujs_per_rit(i, rute_ke)) / 1000) * 1000
                        If dr.Count > 0 Then
                            ujs_onetrip = Math.Ceiling((upah_tunggu + upah_kuli_pemerataan + dr(0)("0") + dr(0)("200")) / 1000) * 1000
                        End If
                        tambahan_multi_trip = ujs_multitrip - ujs_onetrip
                        If tambahan_multi_trip > 0 Then
                            total_ujs_per_rit(i, rute_ke) = ujs_multitrip
                        Else
                            total_ujs_per_rit(i, rute_ke) = ujs_onetrip
                            tambahan_multi_trip = 0
                        End If
                    Else
                        total_ujs_per_rit(i, rute_ke) = Math.Ceiling((upah_tunggu + upah_kuli_pemerataan + ujs_per_rit(i, rute_ke)) / 1000) * 1000
                    End If

                    'Dim dr_hasil As DataRow
                    'For k = index_awal To index_akhir
                    '    dr_hasil = dt_hasil.NewRow
                    '    dr_hasil("PAUS") = i
                    '    dr_hasil("NO") = dt(paus_urutan(i, k))("NO")
                    '    dr_hasil("RUTE") = rute_ke
                    '    dr_hasil("ORDER DATE") = dt(paus_urutan(i, k))("ORDER DATE")
                    '    dr_hasil("KODE TOKO") = dt(paus_urutan(i, k))("KODE TOKO")
                    '    dr_hasil("CEMENT") = dt(paus_urutan(i, k))("CEMENT")
                    '    dr_hasil("SDO NO") = dt(paus_urutan(i, k))("SDO NO")
                    '    dr_hasil("QTY") = dt(paus_urutan(i, k))("QTY")
                    '    dr_hasil("DESA") = dt(paus_urutan(i, k))("DESA")
                    '    dr_hasil("TOTAL QTY") = actual_qty
                    '    dr_hasil("ASAL") = ""
                    '    dr_hasil("TUJUAN") = ""
                    '    dr_hasil("JARAK") = jarak_per_rute(i, rute_ke, k)
                    '    dr_hasil("UJS") = ujs_per_rute(i, rute_ke, k)
                    '    dr_hasil("SUM JARAK") = jarak_per_rit(i, rute_ke)
                    '    dr_hasil("SUM UJS") = ujs_per_rit(i, rute_ke)
                    '    dr_hasil("TAMBAHAN MULTITRIP") = tambahan_multi_trip
                    '    dr_hasil("TOTAL UJS") = total_ujs_per_rit(i, rute_ke)

                    '    dt_hasil.Rows.InsertAt(dr_hasil, 0)
                    'Next

                    '''kosongan kembali ke gudang
                    'dr_hasil = dt_hasil.NewRow
                    'dr_hasil("PAUS") = i
                    'dr_hasil("NO") = ""
                    'dr_hasil("RUTE") = rute_ke
                    'dr_hasil("ORDER DATE") = ""
                    'dr_hasil("KODE TOKO") = ""
                    'dr_hasil("CEMENT") = ""
                    'dr_hasil("SDO NO") = ""
                    'dr_hasil("QTY") = 0
                    'dr_hasil("DESA") = dt(paus_urutan(i, index_akhir))("DESA")
                    'dr_hasil("TOTAL QTY") = actual_qty
                    'dr_hasil("ASAL") = ""
                    'dr_hasil("TUJUAN") = ""
                    'dr_hasil("JARAK") = jarak_per_rute(i, rute_ke, j)
                    'dr_hasil("UJS") = ujs_per_rute(i, rute_ke, j)
                    'dr_hasil("SUM JARAK") = jarak_per_rit(i, rute_ke)
                    'dr_hasil("SUM UJS") = ujs_per_rit(i, rute_ke)
                    'dr_hasil("TAMBAHAN MULTITRIP") = tambahan_multi_trip
                    'dr_hasil("TOTAL UJS") = total_ujs_per_rit(i, rute_ke)
                    'dt_hasil.Rows.InsertAt(dr_hasil, 0)

                    If j < (dt.Rows.Count - 1) Then
                        qty = 0
                        index_awal = j
                        j = j - 1
                    End If
                End If
            Next
        Next
    End Sub

    Private Sub kesimpulan(ByRef gbest_urutan() As Double, ByRef jarak_per_rute(,,) As Double, ByRef jarak_per_rit(,) As Double, ujs_per_rute(,,) As Double, ujs_per_rit(,) As Double, total_ujs_per_rit(,) As Double)
        Dim rute_ke, qty, actual_qty, index_awal, index_akhir As Integer
        '' mengurutkan bilangan random


        dt_hasil = New DataTable
        dt_hasil.Columns.Add("PAUS")
        dt_hasil.Columns.Add("RUTE")
        dt_hasil.Columns.Add("NO")
        dt_hasil.Columns.Add("ORDER DATE")
        dt_hasil.Columns.Add("KODE TOKO")
        dt_hasil.Columns.Add("CEMENT")
        dt_hasil.Columns.Add("SDO NO")
        dt_hasil.Columns.Add("QTY")
        dt_hasil.Columns.Add("DESA")
        dt_hasil.Columns.Add("TOTAL QTY")
        dt_hasil.Columns.Add("ASAL")
        dt_hasil.Columns.Add("TUJUAN")
        dt_hasil.Columns.Add("JARAK")
        dt_hasil.Columns.Add("UJS")
        dt_hasil.Columns.Add("SUM JARAK")
        dt_hasil.Columns.Add("SUM UJS")
        dt_hasil.Columns.Add("TAMBAHAN MULTITRIP")
        dt_hasil.Columns.Add("TOTAL UJS")

        For i = 0 To 0
            rute_ke = 0
            qty = 0
            index_awal = 0
            index_akhir = 0

            For j = 0 To dt.Rows.Count - 1
                qty += CInt(dt(gbest_urutan(j))("QTY"))

                If qty > max_qty Or j = (dt.Rows.Count - 1) Then
                    rute_ke += 1
                    If j = (dt.Rows.Count - 1) And qty <= max_qty Then
                        index_akhir = j
                    Else
                        qty = qty - CInt(dt(gbest_urutan(j))("QTY"))
                        index_akhir = j - 1
                    End If
                    actual_qty = qty
                    ujs_per_rit(i, rute_ke) = 0
                    jarak_per_rit(i, rute_ke) = 0

                    Dim dr As DataRow()
                    For k = index_awal To index_akhir
                        If k = index_awal Then
                            dr = dt_ujs.Select($"Asal = 'Pamekasan-Gudang Pamekasan-' and Tujuan = '{dt(gbest_urutan(k))("DESA")}'")
                        Else
                            dr = dt_ujs.Select($"Asal = '{dt(gbest_urutan(k - 1))("DESA")}' and Tujuan = '{dt(gbest_urutan(k))("DESA")}'")
                        End If

                        If dr.Count > 0 Then
                            If qty >= 175 Then
                                ujs_per_rute(i, rute_ke, k) = dr(0)("200")
                                ujs_per_rit(i, rute_ke) += dr(0)("200")
                            ElseIf qty >= 125 Then
                                ujs_per_rute(i, rute_ke, k) = dr(0)("150")
                                ujs_per_rit(i, rute_ke) += dr(0)("150")
                            ElseIf qty >= 75 Then
                                ujs_per_rute(i, rute_ke, k) = dr(0)("100")
                                ujs_per_rit(i, rute_ke) += dr(0)("100")
                            ElseIf qty >= 25 Then
                                ujs_per_rute(i, rute_ke, k) = dr(0)("50")
                                ujs_per_rit(i, rute_ke) += dr(0)("50")
                            Else
                                ujs_per_rute(i, rute_ke, k) = dr(0)("0")
                                ujs_per_rit(i, rute_ke) += dr(0)("0")
                            End If
                            jarak_per_rute(i, rute_ke, k) = dr(0)("Jarak")
                            jarak_per_rit(i, rute_ke) += dr(0)("Jarak")
                        Else
                            ujs_per_rute(i, rute_ke, k) = 0
                            jarak_per_rute(i, rute_ke, k) = 0
                        End If
                        qty = qty - CInt(dt(gbest_urutan(k))("QTY"))
                    Next

                    ''kosongan kembali ke gudang
                    dr = dt_ujs.Select($"Asal = '{dt(gbest_urutan(index_akhir))("DESA")}' and Tujuan = 'Pamekasan-Gudang Pamekasan-'")
                    If dr.Count > 0 Then
                        ujs_per_rute(i, rute_ke, j) = dr(0)("0")
                        ujs_per_rit(i, rute_ke) += dr(0)("0")
                        jarak_per_rute(i, rute_ke, j) = dr(0)("Jarak")
                        jarak_per_rit(i, rute_ke) += dr(0)("Jarak")
                    End If

                    Dim ujs_multitrip As Double = 0
                    Dim ujs_onetrip As Double = 0
                    Dim upah_multi_trip As Double = 0
                    Dim tambahan_multi_trip As Double = 0
                    Dim upah_tunggu As Double = 1.5 * 11000
                    Dim upah_kuli_pemerataan As Double = 10000

                    If (j - index_awal) > 1 Then
                        upah_multi_trip = (j - index_awal) * 2200
                        ujs_multitrip = Math.Ceiling((upah_multi_trip + upah_tunggu + upah_kuli_pemerataan + ujs_per_rit(i, rute_ke)) / 1000) * 1000
                        If dr.Count > 0 Then
                            ujs_onetrip = Math.Ceiling((upah_tunggu + upah_kuli_pemerataan + dr(0)("0") + dr(0)("200")) / 1000) * 1000
                        End If
                        tambahan_multi_trip = ujs_multitrip - ujs_onetrip
                        If tambahan_multi_trip > 0 Then
                            total_ujs_per_rit(i, rute_ke) = ujs_multitrip
                        Else
                            total_ujs_per_rit(i, rute_ke) = ujs_onetrip
                            tambahan_multi_trip = 0
                        End If
                    Else
                        total_ujs_per_rit(i, rute_ke) = Math.Ceiling((upah_tunggu + upah_kuli_pemerataan + ujs_per_rit(i, rute_ke)) / 1000) * 1000
                    End If

                    Dim dr_hasil As DataRow
                    For k = index_awal To index_akhir
                        dr_hasil = dt_hasil.NewRow
                        dr_hasil("PAUS") = i
                        dr_hasil("NO") = dt(gbest_urutan(k))("NO")
                        dr_hasil("RUTE") = rute_ke
                        dr_hasil("ORDER DATE") = dt(gbest_urutan(k))("ORDER DATE")
                        dr_hasil("KODE TOKO") = dt(gbest_urutan(k))("KODE TOKO")
                        dr_hasil("CEMENT") = dt(gbest_urutan(k))("CEMENT")
                        dr_hasil("SDO NO") = dt(gbest_urutan(k))("SDO NO")
                        dr_hasil("QTY") = dt(gbest_urutan(k))("QTY")
                        dr_hasil("DESA") = dt(gbest_urutan(k))("DESA")
                        dr_hasil("TOTAL QTY") = actual_qty
                        dr_hasil("ASAL") = ""
                        dr_hasil("TUJUAN") = ""
                        dr_hasil("JARAK") = jarak_per_rute(i, rute_ke, k)
                        dr_hasil("UJS") = ujs_per_rute(i, rute_ke, k)
                        dr_hasil("SUM JARAK") = jarak_per_rit(i, rute_ke)
                        dr_hasil("SUM UJS") = ujs_per_rit(i, rute_ke)
                        dr_hasil("TAMBAHAN MULTITRIP") = tambahan_multi_trip
                        dr_hasil("TOTAL UJS") = total_ujs_per_rit(i, rute_ke)

                        dt_hasil.Rows.InsertAt(dr_hasil, 0)
                    Next

                    ''kosongan kembali ke gudang
                    dr_hasil = dt_hasil.NewRow
                    dr_hasil("PAUS") = i
                    dr_hasil("NO") = ""
                    dr_hasil("RUTE") = rute_ke
                    dr_hasil("ORDER DATE") = ""
                    dr_hasil("KODE TOKO") = ""
                    dr_hasil("CEMENT") = ""
                    dr_hasil("SDO NO") = ""
                    dr_hasil("QTY") = 0
                    dr_hasil("DESA") = dt(gbest_urutan(index_akhir))("DESA")
                    dr_hasil("TOTAL QTY") = actual_qty
                    dr_hasil("ASAL") = ""
                    dr_hasil("TUJUAN") = ""
                    dr_hasil("JARAK") = jarak_per_rute(i, rute_ke, j)
                    dr_hasil("UJS") = ujs_per_rute(i, rute_ke, j)
                    dr_hasil("SUM JARAK") = jarak_per_rit(i, rute_ke)
                    dr_hasil("SUM UJS") = ujs_per_rit(i, rute_ke)
                    dr_hasil("TAMBAHAN MULTITRIP") = tambahan_multi_trip
                    dr_hasil("TOTAL UJS") = total_ujs_per_rit(i, rute_ke)
                    dt_hasil.Rows.InsertAt(dr_hasil, 0)

                    If j < (dt.Rows.Count - 1) Then
                        qty = 0
                        index_awal = j
                        j = j - 1
                    End If
                End If
            Next
        Next
    End Sub

    Private Sub menentukan_nilai_pbest(banyak_paus As Integer, ByVal paus(,) As Double, ByVal paus_urutan(,) As Double, total_ujs_per_rit(,) As Double, ByRef index_pbest As Integer, ByRef pbest As Double, ByRef paus_terbaik() As Double, ByRef p() As Double, ByRef gbest() As Double, ByRef gbest_urutan() As Double, ByRef total_ujs_gbest As Double)
        Dim total_ujs As Double
        Dim p_terbaik As Double = 1
        Dim index_terbaik_p As Integer
        Dim cek_ft As Double
        Dim semua_fungsi_tujuan_sama As Boolean = True

        For i = 0 To banyak_paus - 1
            total_ujs = 0
            p(i) = Rnd()
            If p_terbaik > p(i) Then
                p_terbaik = p(i)
                index_terbaik_p = i
            End If

            For j = 0 To dt.Rows.Count - 1
                If total_ujs_per_rit(i, j + 1) > 0 Then
                    total_ujs += total_ujs_per_rit(i, j + 1)
                End If
            Next

            If i = 0 Then
                pbest = total_ujs
                index_pbest = i
                cek_ft = total_ujs
            Else
                If total_ujs < pbest Then
                    pbest = total_ujs
                    index_pbest = i
                End If
            End If
            If cek_ft <> total_ujs Then
                semua_fungsi_tujuan_sama = False
            End If
        Next

        If semua_fungsi_tujuan_sama Then
            index_pbest = index_terbaik_p
        End If
        For i = 0 To dt.Rows.Count - 1
            paus_terbaik(i) = paus_urutan(index_pbest, i)
        Next

        If total_ujs_gbest = 0 Then
            total_ujs_gbest = pbest
            For i = 0 To dt.Rows.Count - 1
                gbest(i) = paus(index_pbest, i)
                gbest_urutan(i) = paus_urutan(index_pbest, i)
            Next
        Else
            If pbest < total_ujs_gbest Then
                total_ujs_gbest = pbest
                For i = 0 To dt.Rows.Count - 1
                    gbest(i) = paus(index_pbest, i)
                    gbest_urutan(i) = paus_urutan(index_pbest, i)
                Next
            End If
        End If

    End Sub

    Private Function menghitung_mutlak_A(iterasi As Integer, maks_iterasi As Integer, ByRef A() As Double, ByRef C() As Double)
        Dim r, aa As Double
        For i = 0 To dt.Rows.Count - 1
            r = Rnd()
            aa = 2 - (iterasi * (2 / maks_iterasi))
            C(i) = Rnd()
            A(i) = (C(i) - 1) * aa
        Next
        For i = 0 To dt.Rows.Count - 1
            If Math.Abs(A(i)) >= 1 Then
                Return 1
            End If
        Next
        Return 0
    End Function

    Private Sub mencari_mangsa(ByVal C() As Double, ByVal A() As Double, ByVal pauske As Integer, ByRef paus(,) As Double)
        Dim d, xrand As Double
        For i = 0 To dt.Rows.Count - 1
            xrand = Rnd()
            d = Math.Abs(C(i) * xrand - paus(pauske, i))
            paus(pauske, i) = xrand - (A(i) * d)
        Next
    End Sub

    Private Sub mengitari_mangsa(ByVal A() As Double, ByVal C() As Double, ByVal gbest() As Double, ByVal pauske As Integer, ByRef paus(,) As Double)
        Dim d As Double
        For i = 0 To dt.Rows.Count - 1
            d = Math.Abs(C(i) * gbest(i) - paus(pauske, i))
            paus(pauske, i) = gbest(i) - A(i) * d
        Next
    End Sub

    Private Sub menyerang_mangsa_dengan_gelembung(ByVal pauske As Integer, ByVal gbest() As Double, ByRef paus(,) As Double)
        Dim phi As Double = 22 / 7
        Dim d As Double
        Dim L As Double = (Rnd() * 2)
        If L > 1 Then
            L = -1 * (L - 1)
        End If
        For i = 0 To dt.Rows.Count - 1
            d = Math.Abs(gbest(i) - paus(pauske, i))
            paus(pauske, i) = (d * Math.Exp(beta * L) * Math.Cos(2 * phi * L)) + gbest(i)
        Next
    End Sub

End Class
