<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class TrukStandbyForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.ButtonSimpan = New System.Windows.Forms.Button()
        Me.BehaviorManager1 = New DevExpress.Utils.Behaviors.BehaviorManager(Me.components)
        Me.GridControl1 = New DevExpress.XtraGrid.GridControl()
        Me.GridView1 = New DevExpress.XtraGrid.Views.Grid.GridView()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.ComboBoxJenisTruk = New System.Windows.Forms.ComboBox()
        Me.ComboBoxMaxQty = New System.Windows.Forms.ComboBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.Add = New System.Windows.Forms.Button()
        Me.ButtonClear = New System.Windows.Forms.Button()
        Me.Panel1.SuspendLayout()
        CType(Me.BehaviorManager1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GridControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel2.SuspendLayout()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.ButtonSimpan)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel1.Location = New System.Drawing.Point(0, 549)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(497, 60)
        Me.Panel1.TabIndex = 1
        '
        'ButtonSimpan
        '
        Me.ButtonSimpan.BackColor = System.Drawing.Color.Blue
        Me.ButtonSimpan.ForeColor = System.Drawing.Color.White
        Me.ButtonSimpan.Location = New System.Drawing.Point(368, 6)
        Me.ButtonSimpan.Name = "ButtonSimpan"
        Me.ButtonSimpan.Size = New System.Drawing.Size(106, 48)
        Me.ButtonSimpan.TabIndex = 0
        Me.ButtonSimpan.Text = "SIMPAN"
        Me.ButtonSimpan.UseVisualStyleBackColor = False
        '
        'GridControl1
        '
        Me.GridControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GridControl1.Location = New System.Drawing.Point(0, 214)
        Me.GridControl1.MainView = Me.GridView1
        Me.GridControl1.Name = "GridControl1"
        Me.GridControl1.Size = New System.Drawing.Size(497, 335)
        Me.GridControl1.TabIndex = 2
        Me.GridControl1.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.GridView1})
        '
        'GridView1
        '
        Me.GridView1.GridControl = Me.GridControl1
        Me.GridView1.Name = "GridView1"
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.ButtonClear)
        Me.Panel2.Controls.Add(Me.Add)
        Me.Panel2.Controls.Add(Me.TextBox1)
        Me.Panel2.Controls.Add(Me.ComboBoxMaxQty)
        Me.Panel2.Controls.Add(Me.ComboBoxJenisTruk)
        Me.Panel2.Controls.Add(Me.Label3)
        Me.Panel2.Controls.Add(Me.Label2)
        Me.Panel2.Controls.Add(Me.Label1)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel2.Location = New System.Drawing.Point(0, 0)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(497, 214)
        Me.Panel2.TabIndex = 3
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(26, 21)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(129, 20)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Jenis Truk           :"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(26, 61)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(130, 20)
        Me.Label2.TabIndex = 0
        Me.Label2.Text = "Max Qty               :"
        '
        'ComboBoxJenisTruk
        '
        Me.ComboBoxJenisTruk.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBoxJenisTruk.FormattingEnabled = True
        Me.ComboBoxJenisTruk.Items.AddRange(New Object() {"CDD", "TRONTON"})
        Me.ComboBoxJenisTruk.Location = New System.Drawing.Point(167, 18)
        Me.ComboBoxJenisTruk.Name = "ComboBoxJenisTruk"
        Me.ComboBoxJenisTruk.Size = New System.Drawing.Size(220, 28)
        Me.ComboBoxJenisTruk.TabIndex = 1
        '
        'ComboBoxMaxQty
        '
        Me.ComboBoxMaxQty.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBoxMaxQty.FormattingEnabled = True
        Me.ComboBoxMaxQty.Items.AddRange(New Object() {"230", "330", "430", "530", "630"})
        Me.ComboBoxMaxQty.Location = New System.Drawing.Point(167, 58)
        Me.ComboBoxMaxQty.Name = "ComboBoxMaxQty"
        Me.ComboBoxMaxQty.Size = New System.Drawing.Size(220, 28)
        Me.ComboBoxMaxQty.TabIndex = 1
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(26, 101)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(131, 20)
        Me.Label3.TabIndex = 0
        Me.Label3.Text = "Jumlah Standby :"
        '
        'TextBox1
        '
        Me.TextBox1.Location = New System.Drawing.Point(167, 101)
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(131, 26)
        Me.TextBox1.TabIndex = 2
        '
        'Add
        '
        Me.Add.BackColor = System.Drawing.SystemColors.GradientActiveCaption
        Me.Add.Location = New System.Drawing.Point(370, 152)
        Me.Add.Name = "Add"
        Me.Add.Size = New System.Drawing.Size(98, 44)
        Me.Add.TabIndex = 3
        Me.Add.Text = "Add"
        Me.Add.UseVisualStyleBackColor = False
        '
        'ButtonClear
        '
        Me.ButtonClear.BackColor = System.Drawing.Color.LemonChiffon
        Me.ButtonClear.Location = New System.Drawing.Point(262, 153)
        Me.ButtonClear.Name = "ButtonClear"
        Me.ButtonClear.Size = New System.Drawing.Size(98, 44)
        Me.ButtonClear.TabIndex = 3
        Me.ButtonClear.Text = "Clear"
        Me.ButtonClear.UseVisualStyleBackColor = False
        '
        'TrukStandbyForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(497, 609)
        Me.Controls.Add(Me.GridControl1)
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.Panel1)
        Me.Name = "TrukStandbyForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Truk Standby"
        Me.Panel1.ResumeLayout(False)
        CType(Me.BehaviorManager1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GridControl1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GridView1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Panel1 As Panel
    Friend WithEvents ButtonSimpan As Button
    Friend WithEvents BehaviorManager1 As DevExpress.Utils.Behaviors.BehaviorManager
    Friend WithEvents GridControl1 As DevExpress.XtraGrid.GridControl
    Friend WithEvents GridView1 As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents Panel2 As Panel
    Friend WithEvents ButtonClear As Button
    Friend WithEvents Add As Button
    Friend WithEvents TextBox1 As TextBox
    Friend WithEvents ComboBoxMaxQty As ComboBox
    Friend WithEvents ComboBoxJenisTruk As ComboBox
    Friend WithEvents Label3 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Label1 As Label
End Class
