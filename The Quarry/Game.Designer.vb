<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Game
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.tmrMovement = New System.Windows.Forms.Timer(Me.components)
        Me.lblMouseDir = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'tmrMovement
        '
        Me.tmrMovement.Enabled = True
        Me.tmrMovement.Interval = 1
        '
        'lblMouseDir
        '
        Me.lblMouseDir.AutoSize = True
        Me.lblMouseDir.Location = New System.Drawing.Point(12, 9)
        Me.lblMouseDir.Name = "lblMouseDir"
        Me.lblMouseDir.Size = New System.Drawing.Size(39, 13)
        Me.lblMouseDir.TabIndex = 0
        Me.lblMouseDir.Text = "Label1"
        '
        'Game
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(984, 961)
        Me.Controls.Add(Me.lblMouseDir)
        Me.Name = "Game"
        Me.Text = "The Quarry"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents tmrMovement As Timer
    Friend WithEvents lblMouseDir As Label
End Class
