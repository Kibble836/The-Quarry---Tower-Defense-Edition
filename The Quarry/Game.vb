Public Class Game

    'Private Variables



    Private Sub Game_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'Constants
        Const xMapSize As Integer = 1024
        Const yMapSize As Integer = 1024

        'Initializing form
        Me.ClientSize = New Size(xMapSize, yMapSize)
        Me.Name = "The Quarry"

        'Create Objects
        'Player
        Dim Player As New PictureBox
        With Player
            .Size = New Size(32, 32)
            .Name = "Player"
            .Visible = True
            .BackColor = Color.Red
            .Location = New Point(xMapSize / 2 - 16, yMapSize / 2 + 48)
            Controls.Add(Player)
        End With
        'Crystal
        Dim Crystal As New PictureBox
        With Crystal
            .Size = New Size(64, 32)
            .Name = "Crystal"
            .Location = New Point(xMapSize / 2 - 32, yMapSize / 2 - 16)
            .Image = My.Resources.Crystal
            .Visible = True
            Controls.Add(Crystal)
        End With

    End Sub
End Class
