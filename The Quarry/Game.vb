Public Class Game

    'Private Variables



    Private Sub Game_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'Initializing form
        Me.ClientSize = New Size(1024, 1024)
        Me.Name = "The Quarry"

        'Create Objects
        'Player
        Dim Player As New PictureBox
        With Player
            .Size = New Size(32, 32)
            .Name = "Player"

        End With
        'Crystal
        Dim Crystal As New PictureBox
        With Crystal
            .Size = New Size(64, 32)
            .Name = "Crystal"
            .Location = New Point(512 - 32, 512 - 16)
            .BackColor = Color.Black
            .Visible = True
        End With
        Controls.Add(Crystal)
    End Sub
End Class
