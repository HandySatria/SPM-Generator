Public Class ClassPermutasi
    Public dt As DataTable
    Public dt_hasil As DataTable
    Public dt_ujs As DataTable
    Public max_qty As Integer
    Public total_ujs_gbest As Double = 0

    Public Function proses()
        Dim arr As New List(Of Integer)
        For i = 0 To dt.Rows.Count - 1
            arr.Add(i)
        Next
        Dim result As New List(Of List(Of Integer))

        Dim permutasi As New ClassPermutasi
        Dim pbest(dt.Rows.Count) As Double
        Dim total_ujs_pbest As Double

        permutasi.GeneratePermutations(arr.ToList(), New List(Of Integer), result)

        Dim total_ujs_per_rit(result.Count, dt.Rows.Count) As Double
        evaluasi_fungsi_tujuan(result, total_ujs_per_rit, pbest, total_ujs_pbest)

        Return dt_hasil
    End Function
    Sub GeneratePermutations(ByVal remaining As List(Of Integer), ByVal current As List(Of Integer), ByRef result As List(Of List(Of Integer)))
        If remaining.Count = 0 Then
            result.Add(New List(Of Integer)(current))
            Return
        End If

        For i = 0 To remaining.Count - 1
            Dim nextRemaining As List(Of Integer) = New List(Of Integer)(remaining)
            Dim nextCurrent As List(Of Integer) = New List(Of Integer)(current)

            nextCurrent.Add(nextRemaining(i))
            nextRemaining.RemoveAt(i)

            GeneratePermutations(nextRemaining, nextCurrent, result)
        Next
    End Sub
    Private Sub evaluasi_fungsi_tujuan(ByRef listArray As List(Of List(Of Integer)), ByRef total_ujs_per_rit(,) As Double, ByRef pbest() As Double, ByRef total_ujs_pbest As Double)
        Dim rute_ke, qty, actual_qty, index_awal, index_akhir As Integer
        ''Inisialisasi Parameter

        Dim gbest(dt.Rows.Count) As Double
        Dim gbest_urutan(dt.Rows.Count) As Double

        Dim jarak_per_rit(listArray.Count, dt.Rows.Count) As Double
        Dim jarak_per_rute(listArray.Count, dt.Rows.Count, dt.Rows.Count) As Double
        Dim ujs_per_rit(listArray.Count, dt.Rows.Count) As Double
        Dim ujs_per_rute(listArray.Count, dt.Rows.Count, dt.Rows.Count) As Double
        Dim total_ujs(listArray.Count) As Double
        Dim i = -1

        For Each arr In listArray
            i += 1
            rute_ke = 0
            qty = 0
            index_awal = 0
            index_akhir = 0
            total_ujs(i) = 0
            For j = 0 To dt.Rows.Count - 1
                qty += CInt(dt(arr(j))("QTY"))

                If qty > max_qty Or j = (dt.Rows.Count - 1) Then
                    rute_ke += 1
                    If j = (dt.Rows.Count - 1) And qty <= max_qty Then
                        index_akhir = j
                    Else
                        qty = qty - CInt(dt(arr(j))("QTY"))
                        index_akhir = j - 1
                    End If
                    actual_qty = qty
                    ujs_per_rit(i, rute_ke) = 0
                    jarak_per_rit(i, rute_ke) = 0

                    Dim dr As DataRow()
                    For k = index_awal To index_akhir
                        If k = index_awal Then
                            dr = dt_ujs.Select($"Asal = 'Pamekasan-Gudang Pamekasan-' and Tujuan = '{dt(arr(k))("DESA")}'")
                        Else
                            dr = dt_ujs.Select($"Asal = '{dt(arr(k - 1))("DESA")}' and Tujuan = '{dt(arr(k))("DESA")}'")
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
                        End If
                        qty = qty - CInt(dt(arr(k))("QTY"))
                    Next

                    ''kosongan kembali ke gudang
                    dr = dt_ujs.Select($"Asal = '{dt(arr(index_akhir))("DESA")}' and Tujuan = 'Pamekasan-Gudang Pamekasan-'")
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

                    If j < (dt.Rows.Count - 1) Then
                        qty = 0
                        index_awal = j
                        j = j - 1
                    End If
                End If
                total_ujs(i) += total_ujs_per_rit(i, rute_ke)
            Next
            If i = 0 Then
                total_ujs_pbest = total_ujs(i)
                For j = 0 To dt.Rows.Count - 1
                    pbest(j) = arr(j)
                Next
            Else
                If total_ujs_pbest < total_ujs(i) Then
                    total_ujs_pbest = total_ujs(i)
                    For j = 0 To dt.Rows.Count - 1
                        pbest(j) = arr(j)
                    Next
                End If
            End If
        Next
    End Sub

End Class
